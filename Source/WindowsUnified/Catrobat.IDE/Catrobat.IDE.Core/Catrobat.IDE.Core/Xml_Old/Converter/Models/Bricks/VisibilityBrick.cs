﻿using Catrobat.IDE.Core.Xml.XmlObjects.Bricks;
using Catrobat.IDE.Core.Xml.XmlObjects.Bricks.Properties;
using Context = Catrobat.IDE.Core.Xml.Converter.XmlProgramConverter.ConvertBackContext;

// ReSharper disable once CheckNamespace
namespace Catrobat.IDE.Core.Models.Bricks
{
    partial class VisibilityBrick
    {
    }

    partial class ShowBrick
    {
        protected internal override XmlBrick ToXmlObject2(Context context)
        {
            return new XmlShowBrick();
        }
    }

    partial class HideBrick
    {
        protected internal override XmlBrick ToXmlObject2(Context context)
        {
            return new XmlHideBrick();
        }
    }
}