using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace testWpf.MVVM.Converters
{
    public class ConverterFont
    {
    }

    #region FONTS
    [ValueConversion(typeof(double), typeof(double))]
    class ConvTitle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // here you can use the parameter that you can give in here via setting , ConverterParameter='something'} or use any nice login with the VisualTreeHelper to make a better return value, or maybe even just hardcode some max values if you like
            var maxWidth = SystemParameters.PrimaryScreenWidth;
            var width = (double)value;
            var widthNormaliz = (width - 520) / ((maxWidth - 250) - 520);
            var fontSize = ((50 - 20) * widthNormaliz) + 20;
            return fontSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    [ValueConversion(typeof(double), typeof(double))]
    class ConvDesc : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // here you can use the parameter that you can give in here via setting , ConverterParameter='something'} or use any nice login with the VisualTreeHelper to make a better return value, or maybe even just hardcode some max values if you like
            //System.Windows.SystemParameters.WorkArea.Width

            var maxWidth = SystemParameters.PrimaryScreenWidth;
            var width = (double)value;
            var widthNormaliz = (width - 520) / ((maxWidth - 250) - 520);
            var fontSize = ((20 - 10) * widthNormaliz) + 10;
            return fontSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    [ValueConversion(typeof(double), typeof(double))]
    class ConvOrig : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // here you can use the parameter that you can give in here via setting , ConverterParameter='something'} or use any nice login with the VisualTreeHelper to make a better return value, or maybe even just hardcode some max values if you like
            var maxWidth = SystemParameters.PrimaryScreenWidth;
            var width = (double)value;
            var widthNormaliz = (width - 520) / ((maxWidth - 250) - 520);
            var fontSize = ((36 - 16) * widthNormaliz) + 16;
            return fontSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region TEMPLATES

    class ConvTitleTemplate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // here you can use the parameter that you can give in here via setting , ConverterParameter='something'} or use any nice login with the VisualTreeHelper to make a better return value, or maybe even just hardcode some max values if you like
            var maxWidth = SystemParameters.PrimaryScreenWidth;
            var width = (double)value;
            var widthNormaliz = (width - 520) / ((maxWidth - 250) - 520);
            var TemplateSize = ((500 - 150) * widthNormaliz) + 150;
            return TemplateSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    class ConvDescTemplate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // here you can use the parameter that you can give in here via setting , ConverterParameter='something'} or use any nice login with the VisualTreeHelper to make a better return value, or maybe even just hardcode some max values if you like
            var maxWidth = SystemParameters.PrimaryScreenWidth;
            var width = (double)value;
            var widthNormaliz = (width - 520) / ((maxWidth - 250) - 520);
            var TemplateSize = ((300 - 100) * widthNormaliz) + 100;
            return TemplateSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class ConvOrigTemplate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // here you can use the parameter that you can give in here via setting , ConverterParameter='something'} or use any nice login with the VisualTreeHelper to make a better return value, or maybe even just hardcode some max values if you like
            var maxWidth = SystemParameters.PrimaryScreenWidth;
            var width = (double)value;
            var widthNormaliz = (width - 520) / ((maxWidth - 250) - 520);
            var TemplateSize = ((500 - 150) * widthNormaliz) + 150;
            return TemplateSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    #endregion

    #region PIC
    class ConvPicWidth : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // here you can use the parameter that you can give in here via setting , ConverterParameter='something'} or use any nice login with the VisualTreeHelper to make a better return value, or maybe even just hardcode some max values if you like
            var maxWidth = SystemParameters.PrimaryScreenWidth;
            var width = (double)value;
            var widthNormaliz = (width - 520) / ((maxWidth - 250) - 520);
            var picSize = ((260 - 110) * widthNormaliz) + 110;
            Console.WriteLine($"width {picSize}");
            return picSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    class ConvPicHeight : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // here you can use the parameter that you can give in here via setting , ConverterParameter='something'} or use any nice login with the VisualTreeHelper to make a better return value, or maybe even just hardcode some max values if you like
            var maxWidth = SystemParameters.PrimaryScreenWidth;
            var width = (double)value;
            var widthNormaliz = (width - 520) / ((maxWidth - 250) - 520);
            var picSize = ((260 - 110) * widthNormaliz) + 110;
            Console.WriteLine($"h {picSize*4/3}");
            return picSize*4/3;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
