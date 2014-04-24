﻿using System.Xml.Linq;

namespace Catrobat.IDE.Core.CatrobatObjects.Bricks
{
    public class NoteBrick : Brick
    {
        protected string _note;
        public string Note
        {
            get { return _note; }
            set
            {
                if (_note == value)
                {
                    return;
                }

                _note = value;
                RaisePropertyChanged();
            }
        }

        public NoteBrick() {}

        public NoteBrick(XElement xElement) : base(xElement) {}

        internal override void LoadFromXML(XElement xRoot)
        {
            if (xRoot.Element("note") != null)
            {
                _note = xRoot.Element("note").Value;
            }
        }

        internal override XElement CreateXML()
        {
            var xRoot = new XElement("noteBrick");

            if (_note != null)
            {
                xRoot.Add(new XElement("note")
                {
                    Value = _note
                });
            }
            //CreateCommonXML(xRoot);

            return xRoot;
        }

        public override DataObject Copy()
        {
            var newBrick = new NoteBrick();
            newBrick._note = _note;

            return newBrick;
        }

        public override bool Equals(DataObject other)
        {
            var otherBrick = other as NoteBrick;

            if (otherBrick == null)
                return false;

            if (Note != otherBrick.Note)
                return false;

            return true;
        }
    }
}