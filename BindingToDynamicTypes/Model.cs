﻿using System;
using System.Net;
using System.Linq;
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
        public decimal? FilterValue { get; set; }

        public Metric(string metricName, double? dbl, string dataStateColor)
        {
            MetricName = metricName;
            DataStateColor = dataStateColor;
            DoubleNullable = dbl;
            DisplayValue = dbl.HasValue ? dbl.Value.ToString("P3") : ""; //Display value as percentage
            FilterValue = parseStringDigits(DisplayValue);
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", DataStateColor, DisplayValue);
        }

        /// <summary>
        /// strips non-digits from a string then converts it to a decimal.
        /// </summary>
        private decimal? parseStringDigits(string str)
        {
            char[] validChars = str.Where(c => (char.IsDigit(c) || c == '.')).ToArray();
            str = new string(validChars);

            decimal output;
            if (decimal.TryParse(str, out output))
                return output;
            else
                return null;
        }
    }
}