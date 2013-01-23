//using System;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
//using System.Windows.Data;

//namespace BindingToDynamicTypes
//{
//    public class DebugConverter : IValueConverter
//    {
//        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
//        {
//           // return value; //set breakpoint on this line
//            Metric metric = value as Metric;
//            return metric.Value;
//            //if (metric == null)
//            //    return value;
//            //else
//            //{
//            //    int asDouble;
//            //    if (Int32.TryParse(metric.Value, out asDouble))
//            //        return asDouble;
//            //    else
//            //        return metric.Value;
//            //}
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
