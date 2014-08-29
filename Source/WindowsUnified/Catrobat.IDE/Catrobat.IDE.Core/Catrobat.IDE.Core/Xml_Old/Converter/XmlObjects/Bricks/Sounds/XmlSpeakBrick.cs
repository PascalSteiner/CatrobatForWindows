﻿using Catrobat.IDE.Core.Models.Bricks;
using Context = Catrobat.IDE.Core.Xml.Converter.XmlProgramConverter.ConvertContext;

// ReSharper disable once CheckNamespace
namespace Catrobat.IDE.Core.Xml.XmlObjects.Bricks.Sounds
{
    partial class XmlSpeakBrick
    {
        protected override Brick ToModel2(Context context)
        {
            return new SpeakBrick
            {
                Value = Text
            };
        }
    }
}