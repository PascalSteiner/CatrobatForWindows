﻿using Catrobat.Core.Objects.Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Catrobat.TestsCommon.Misc
{
    public static class FormulaComparer
    {
        public static void CompareFormulas(FormulaTree expectedFromula, FormulaTree actualFormula)
        {
            if (expectedFromula == null && actualFormula == null)
                return;

            Assert.IsNotNull(expectedFromula);
            Assert.IsNotNull(actualFormula);

            Assert.AreEqual(expectedFromula.VariableType.ToLowerInvariant(),
                actualFormula.VariableType.ToLowerInvariant());

            Assert.AreEqual(expectedFromula.VariableValue.ToLowerInvariant(),
                actualFormula.VariableValue.ToLowerInvariant());

            CompareFormulas(expectedFromula.LeftChild, actualFormula.LeftChild);
            CompareFormulas(expectedFromula.RightChild, actualFormula.RightChild);
        }
    }
}