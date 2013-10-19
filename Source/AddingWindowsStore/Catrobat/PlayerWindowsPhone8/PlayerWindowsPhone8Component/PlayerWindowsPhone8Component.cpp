#include "pch.h"
#include "PlayerWindowsPhone8Component.h"
#include "Direct3DContentProvider.h"
#include "XMLParser.h"
#include "ProjectDaemon.h"
#include "ScriptHandler.h"
#include "WhenScript.h"
#include "lodepng.h"
#include "lodepng_util.h"
#include "DDSLoader.h"
#include "Interpreter.h"
#include "XMLParserFatalException.h"
#include "PlayerException.h"

#include <windows.system.threading.h>
#include <exception>
#include <windows.foundation.h>
#include <thread>
#include <math.h>

using namespace Windows::Foundation;
using namespace Windows::UI::Core;
using namespace Microsoft::WRL;
using namespace Windows::Phone::Graphics::Interop;
using namespace Windows::Phone::Input::Interop;
using namespace Windows::Graphics::Display;

namespace PhoneDirect3DXamlAppComponent
{

    Direct3DBackground::Direct3DBackground() :
        m_timer(ref new BasicTimer())
    {
        ProjectDaemon::Instance()->ReInit();
        m_initialized = false;
    }

    Direct3DBackground::~Direct3DBackground()
    {
        m_initialized = false;
    }

    IDrawingSurfaceBackgroundContentProvider^ Direct3DBackground::CreateContentProvider()
    {
        ComPtr<Direct3DContentProvider> provider = Make<Direct3DContentProvider>(this);
        return reinterpret_cast<IDrawingSurfaceBackgroundContentProvider^>(provider.Detach());
    }

    // IDrawingSurfaceManipulationHandler
    void Direct3DBackground::SetManipulationHost(DrawingSurfaceManipulationHost^ manipulationHost)
    {
        manipulationHost->PointerPressed +=
            ref new TypedEventHandler<DrawingSurfaceManipulationHost^, PointerEventArgs^>(this, &Direct3DBackground::OnPointerPressed);

        manipulationHost->PointerMoved +=
            ref new TypedEventHandler<DrawingSurfaceManipulationHost^, PointerEventArgs^>(this, &Direct3DBackground::OnPointerMoved);

        manipulationHost->PointerReleased +=
            ref new TypedEventHandler<DrawingSurfaceManipulationHost^, PointerEventArgs^>(this, &Direct3DBackground::OnPointerReleased);
    }

	// Event Handlers
	void Direct3DBackground::OnPointerPressed(DrawingSurfaceManipulationHost^ sender, PointerEventArgs^ args)
	{
		// Insert your code here.
		if (!ProjectDaemon::Instance()->FinishedLoading())
			return;
		Project* project = ProjectDaemon::Instance()->GetProject();
		ObjectList* objects = project->GetObjectList();
		if (objects == NULL)
		{
			return;
		}
		for (int i = objects->GetSize() - 1; i >= 0; i--)
		{
			/*sprites->getSprite(i)->GetCurrentLookData()->Texture()->GetDesc(&data);
			data.ViewDimension.Value*/

			try
			{
				Bounds bounds = objects->GetObject(i)->GetBounds();
				bounds.x += ProjectDaemon::Instance()->GetProject()->GetScreenWidth() / 2;
				bounds.y += ProjectDaemon::Instance()->GetProject()->GetScreenHeight() / 2;

				{
					float resolutionScaleFactor;
					switch (DisplayProperties::ResolutionScale) {
					case ResolutionScale::Scale100Percent:
						resolutionScaleFactor = 1.0f;
						break;
					case ResolutionScale::Scale150Percent:
						resolutionScaleFactor = 1.5f;
						break;
					case ResolutionScale::Scale160Percent:
						resolutionScaleFactor = 1.6f;
						break;
					}

					float actualX = args->CurrentPoint->Position.X;
					float actualY = args->CurrentPoint->Position.Y;

					double factorX = abs(ProjectDaemon::Instance()->GetProject()->GetScreenWidth() / (m_originalWindowsBounds.X / resolutionScaleFactor));
					double factorY = abs(ProjectDaemon::Instance()->GetProject()->GetScreenHeight() / (m_originalWindowsBounds.Y / resolutionScaleFactor));

					double normalizedX = factorX * actualX;
					double normalizedY = factorY * actualY;

					if (bounds.x <= normalizedX && bounds.y <= normalizedY && (bounds.x + bounds.width) >= normalizedX && (bounds.y + bounds.height) >= normalizedY)
					{
						for (int j = 0; j < objects->GetObject(i)->GetScriptListSize(); j++)
						{
							Script *script = objects->GetObject(i)->GetScript(j);
							if (script->GetType() == Script::TypeOfScript::WhenScript)
							{
								WhenScript *wScript = (WhenScript *) script;
								if (wScript->GetAction() == WhenScript::Action::Tapped)
								{
									wScript->Execute();
								}
							}

						}

						// One Hit is enough
						break;
					}
				}
			}
			catch (PlayerException *e)
			{
			}
			catch (Platform::Exception ^e)
			{
			}
		}

		/*HANDLE ExampleEvent = OpenEvent(EVENT_ALL_ACCESS, FALSE, TEXT("ExampleEvent"));
		SetEvent(ExampleEvent);*/
	}

    void Direct3DBackground::OnPointerMoved(DrawingSurfaceManipulationHost^ sender, PointerEventArgs^ args)
    {
        // Insert your code here.
    }

    void Direct3DBackground::OnPointerReleased(DrawingSurfaceManipulationHost^ sender, PointerEventArgs^ args)
    {
        // Insert your code here.
    }

    // Interface With Direct3DContentProvider
    HRESULT Direct3DBackground::Connect(_In_ IDrawingSurfaceRuntimeHostNative* host, _In_ ID3D11Device1* device)
    {
        // Initialize Renderer
        m_renderer = ref new Renderer();
        m_renderer->Initialize(device);

        // Initialize Sound
        SoundManager::Instance()->Initialize();

        //Initialize Project Renderer
		m_renderingErrorOccured = false;
		m_projectRenderer = ref new ProjectRenderer();
        ProjectDaemon::Instance()->SetupRenderer(device, m_projectRenderer);

        // Load Project
#ifdef _DEBUG

        //ProjectDaemon::Instance()->OpenProject("stoeckchen");
        ProjectDaemon::Instance()->OpenProject(ProjectName);
#else
        ProjectDaemon::Instance()->OpenProject(ProjectName);
#endif
        // Restart timer after renderer has finished initializing.
        m_timer->Reset();

        return S_OK;
    }

    void Direct3DBackground::Disconnect()
    {
        m_renderer = nullptr;
        m_projectRenderer = nullptr;
    }

    HRESULT Direct3DBackground::PrepareResources(_In_ const LARGE_INTEGER* presentTargetTime, _Inout_ DrawingSurfaceSizeF* desiredRenderTargetSize)
    {
        m_timer->Update();
        m_renderer->Update(m_timer->Total, m_timer->Delta);
        m_projectRenderer->Update(m_timer->Total, m_timer->Delta);

        // Save this for later
        if (!m_initialized)
        {
            m_originalWindowsBounds.X = desiredRenderTargetSize->width;
            m_originalWindowsBounds.Y = desiredRenderTargetSize->height;
            ProjectDaemon::Instance()->SetDesiredRenderTargetSize(desiredRenderTargetSize);
            m_initialized = true;
        }

        return S_OK;
    }

    HRESULT Direct3DBackground::Draw(_In_ ID3D11Device1* device, _In_ ID3D11DeviceContext1* context, _In_ ID3D11RenderTargetView* renderTargetView)
    {
        if (!ProjectDaemon::Instance()->FinishedLoading() || m_renderingErrorOccured)
        {
            // Render Loading Screen
            m_renderer->UpdateDevice(device, context, renderTargetView);
            m_renderer->Render();
        }
        else
        {
            // Render Project
			try
			{
				m_projectRenderer->UpdateDevice(device, context, renderTargetView);
			}
			catch (PlayerException *e)
			{
				m_renderingErrorOccured = true;
				ProjectDaemon::Instance()->AddDebug(L"Error Updating Device.");
			}
			catch (Platform::Exception^ e)
			{
				m_renderingErrorOccured = true;
				ProjectDaemon::Instance()->AddDebug(L"Error Updating Device.");
			}

			try
			{
				m_projectRenderer->Render();
			}
			catch (PlayerException *e)
			{
				m_renderingErrorOccured = true;
				ProjectDaemon::Instance()->AddDebug(L"Rendering not possible.");
			}
			catch (Platform::Exception^ e)
			{
				m_renderingErrorOccured = true;
				ProjectDaemon::Instance()->AddDebug(L"Rendering not possible.");
			}
        }

        RequestAdditionalFrame();

        return S_OK;
    }

}