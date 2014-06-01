﻿using System.Collections.Generic;
using System.Linq;

namespace Catrobat.IDE.Core.Models.Formulas.FormulaTree
{
    public abstract partial class UnaryFormulaTree : BaseFormulaTree
    {
        public IFormulaTree Child { get; set; }

        #region Implements IFormulaTree

        public override IEnumerable<IFormulaTree> Children
        {
            get
            {
                return Enumerable.Repeat(Child, 1);
            }
        }

        #endregion

        #region Implements ICloneable

        public override object Clone()
        {
            var clone = (UnaryFormulaTree) base.Clone();
            clone.Child = Child == null ? null : (IFormulaTree) Child.Clone();
            return clone;
        }

        #endregion

        #region Overrides Equals

        public override bool Equals(object obj)
        {
            // auto-implemented by ReSharper
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UnaryFormulaTree) obj);
        }

        protected bool Equals(UnaryFormulaTree other)
        {
            // auto-implemented by ReSharper
            return Equals(Child, other.Child);
        }

        public override int GetHashCode()
        {
            // auto-implemented by ReSharper
            return (Child != null ? Child.GetHashCode() : 0);
        }

        #endregion
    }
}