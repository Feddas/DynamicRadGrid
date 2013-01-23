using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BindingToDynamicTypes
{
    public class Customer
    {
        public Customer(string name, string metricStatus, double? dbl)
        {
            Name = name;
            SingleMetric = new Metric() {
                Status = metricStatus,
                DoubleNullable = dbl,
                DisplayValue = dbl.HasValue ? dbl.Value.ToString("P2") : "" //Display value as percentage
            };
        }
        public string Name { get; set; }
        public Metric SingleMetric { get; set; }
    }

    public class Metric
    {
        public double? DoubleNullable { get; set; }
        public string Status { get; set; }
        public string DisplayValue { get; set; }

        public override string ToString()
        {
            return string.Format("{0}{1}", Status, DisplayValue);
        }
    }
}