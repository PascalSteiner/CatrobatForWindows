﻿using System;
using System.Xml.Linq;
using Catrobat.IDE.Core.Xml.XmlObjects.Formulas;
using Catrobat_Player.NativeComponent;

namespace Catrobat.IDE.Core.Xml.XmlObjects.Bricks.Properties
{
    public partial class XmlTurnLeftBrick : XmlBrick, ITurnLeftBrick
    {
        #region NativeInterface
        public IFormulaTree Rotation
        {
            get
            {
                return Degrees == null ? null : Degrees.FormulaTree;
            }
            set { }
        }

        #endregion

        public XmlFormula Degrees { get; set; }

        public XmlTurnLeftBrick() { }

        public XmlTurnLeftBrick(XElement xElement) : base(xElement) { }

        internal override void LoadFromXml(XElement xRoot)
        {
            if (xRoot != null)
            {
                Degrees = new XmlFormula(xRoot, XmlConstants.TurnLeftDegrees);
            }
        }

        internal override XElement CreateXml()
        {
            var xRoot = new XElement(XmlConstants.Brick);
            xRoot.SetAttributeValue(XmlConstants.Type, XmlConstants.XmlTurnLeftBrickType);

            var xElement = Degrees.CreateXml();
            xElement.SetAttributeValue(XmlConstants.Category, XmlConstants.TurnLeftDegrees);

            var xFormulalist = new XElement(XmlConstants.FormulaList);
            xFormulalist.Add(xElement);

            xRoot.Add(xFormulalist);

            return xRoot;
        }

        public override void LoadReference()
        {
            if (Degrees != null)
                Degrees.LoadReference();
        }
    }
}
