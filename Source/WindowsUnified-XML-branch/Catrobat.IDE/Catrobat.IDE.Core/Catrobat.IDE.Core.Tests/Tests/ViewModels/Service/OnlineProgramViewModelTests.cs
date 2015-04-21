﻿using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Catrobat.IDE.Core.Services;
using Catrobat.IDE.Core.Tests.Services;
using Catrobat.IDE.Core.Tests.Services.Common;
using Catrobat.IDE.Core.Resources;
using Catrobat.IDE.Core.ViewModels;
using Catrobat.IDE.Core.ViewModels.Main;
using Catrobat.IDE.Core.ViewModels.Service;
using Catrobat.IDE.Core.CatrobatObjects;
using System.Globalization;
using Catrobat.IDE.Core.Tests.SampleData;

namespace Catrobat.IDE.Core.Tests.Tests.ViewModels.Service
{
    [TestClass]
    public class OnlineProgramViewModelTests
    {
        private bool _downloadStarted = false;

        [ClassInitialize]
        public static void TestClassInitialize(TestContext testContext)
        {
            ServiceLocator.NavigationService = new NavigationServiceTest();
            ServiceLocator.UnRegisterAll();
            ServiceLocator.Register<DispatcherServiceTest>(TypeCreationMode.Lazy);
            ServiceLocator.Register<NotificationServiceTest>(TypeCreationMode.Lazy);
            ServiceLocator.Register<WebCommunicationTest>(TypeCreationMode.Normal);
            ServiceLocator.Register<CultureServiceTest>(TypeCreationMode.Lazy);
            ServiceLocator.CultureService.SetCulture(new CultureInfo("en"));
        }

        [TestMethod, TestCategory("ViewModels.Service")]
        public void NavigateToTest()
        {
            var onlineProjectHeader = SampleLoader.GetSampleOnlineProjectHeader();
            var viewModel = new OnlineProgramViewModel();
            var messageContext = new GenericMessage<OnlineProgramHeader>(onlineProjectHeader);
            Messenger.Default.Send(messageContext, ViewModelMessagingToken.SelectedOnlineProgramChangedListener);

            viewModel.NavigateTo();

            Assert.AreEqual("07/26/2014 13:54:08 by:", viewModel.UploadedLabelText);
            Assert.AreEqual("(version: 0.9.9)", viewModel.VersionLabelText);
            Assert.AreEqual("2 views", viewModel.ViewsLabelText);
            Assert.AreEqual("5 downloads", viewModel.DownloadsLabelText);
            Assert.IsTrue(viewModel.ButtonDownloadIsEnabled);
        }

        [TestMethod, TestCategory("ViewModels.Service"), TestCategory("ExcludeGated")]
        public void DownloadActionTest()
        {
            //TODO to be tested
            Assert.AreEqual(0, "test async-command and ProgramImporterService");
            Messenger.Default.Register<GenericMessage<string>>(this,
               ViewModelMessagingToken.DownloadProgramStartedListener, DownloadProgramStartedMessageAction);

            var navigationService = (NavigationServiceTest)ServiceLocator.NavigationService;
            navigationService.PageStackCount = 1;
            navigationService.CurrentNavigationType = NavigationServiceTest.NavigationType.Initial;
            navigationService.CurrentView = typeof(OnlineProgramViewModel);

            var viewModel = new OnlineProgramViewModel();
            //viewModel.DownloadCommand.Execute(null);

            Assert.AreEqual(NavigationServiceTest.NavigationType.NavigateTo, navigationService.CurrentNavigationType);
            Assert.AreEqual(typeof(OnlineProgramViewModel), navigationService.CurrentView);
            Assert.AreEqual(1, navigationService.PageStackCount);
        }

        [TestMethod, TestCategory("ViewModels.Service")]
        public void ReportActionTest()
        {
            var navigationService = (NavigationServiceTest)ServiceLocator.NavigationService;
            navigationService.PageStackCount = 1;
            navigationService.CurrentNavigationType = NavigationServiceTest.NavigationType.Initial;
            navigationService.CurrentView = typeof(OnlineProgramViewModel);

            var viewModel = new OnlineProgramViewModel();
            viewModel.ReportCommand.Execute(null);

            Assert.AreEqual(NavigationServiceTest.NavigationType.NavigateTo, navigationService.CurrentNavigationType);
            Assert.AreEqual(typeof(OnlineProgramReportViewModel), navigationService.CurrentView);
            Assert.AreEqual(2, navigationService.PageStackCount);
        }

        [TestMethod, TestCategory("ViewModels.Service")]
        public void LicenseActionTest()
        {
            var navigationService = (NavigationServiceTest)ServiceLocator.NavigationService;
            navigationService.PageStackCount = 1;
            navigationService.CurrentNavigationType = NavigationServiceTest.NavigationType.Initial;
            navigationService.CurrentView = typeof(OnlineProgramViewModel);

            var viewModel = new OnlineProgramViewModel();
            viewModel.LicenseCommand.Execute(null);

            Assert.AreEqual(NavigationServiceTest.NavigationType.NavigateToWebPage, navigationService.CurrentNavigationType);
            Assert.AreEqual(ApplicationResources.PROJECT_LICENSE_URL, navigationService.CurrentView);
            Assert.AreEqual(1, navigationService.PageStackCount);
        }

        [TestMethod, TestCategory("ViewModels.Service")]
        public void GoBackActionTest()
        {
            var navigationService = (NavigationServiceTest)ServiceLocator.NavigationService;
            navigationService.PageStackCount = 1;
            navigationService.CurrentNavigationType = NavigationServiceTest.NavigationType.Initial;
            navigationService.CurrentView = typeof(OnlineProgramViewModel);

            var viewModel = new OnlineProgramViewModel();
            viewModel.GoBackCommand.Execute(null);

            Assert.AreEqual(NavigationServiceTest.NavigationType.NavigateBack, navigationService.CurrentNavigationType);
            Assert.AreEqual(null, navigationService.CurrentView);
            Assert.AreEqual(0, navigationService.PageStackCount);
        }

        #region MessageActions
        private void DownloadProgramStartedMessageAction(GenericMessage<string> message)
        {
            _downloadStarted = true;
        }
        #endregion
    }
}