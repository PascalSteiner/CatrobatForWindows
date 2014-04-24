﻿using Catrobat.IDE.Core.Formulas;
using Catrobat.IDE.Core.Models.Formulas.FormulaTree;
using Catrobat.IDE.Core.UI.PortableUI;
using System;
using System.Globalization;

namespace Catrobat.IDE.Core.UI.Converters
{
    public class FormulaStringConverter : IPortableValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return FormulaSerializer.Serialize((IFormulaTree) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // not supported
            return null;
        }
    }
}