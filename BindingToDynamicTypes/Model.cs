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
using System.Collections.Generic;

namespace BindingToDynamicTypes
{
    public class Customer
    {
        public Customer(string name, List<Metric> metrics)
        {
            Name = name;
            Metrics = metrics;
        }
        public string Name { get; set; }
        public List<Metric> Metrics { get; set; }
    }

    public class Metric
    {
        public string MetricName { get; set; }
        public double? DoubleNullable { get; set; }
        public string DataStateColor { get; set; }
        public string DisplayValue { get; set; }

        public Metric(string metricName, double? dbl, string dataStateColor)
        {
            MetricName = metricName;
            DataStateColor = dataStateColor;
            DoubleNullable = dbl;
            DisplayValue = dbl.HasValue ? dbl.Value.ToString("P2") : ""; //Display value as percentage
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", DataStateColor, DisplayValue);
        }
    }
}