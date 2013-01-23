using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace BindingToDynamicTypes
{
    public partial class DynGrid : UserControl
    {
        /// <summary>
        /// This XAML is in the code behind because the real scorecard needs to modify the "SingleMetric" to be the name of the current metric.
        /// </summary>
        string dataTemplateXaml = @"
<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>
            <StackPanel Orientation=""Horizontal"">
                <TextBlock Text='{Binding DisplayValue}' Foreground='{Binding Status}' />
            </StackPanel>
</DataTemplate>";

        public DynGrid()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DynGrid_Loaded);
        }

        void DynGrid_Loaded(object sender, RoutedEventArgs e)
        {
            CodeBehindHook.Columns.Add(new GridViewDataColumn() { DataMemberBinding = new Binding("Name") });
            CodeBehindHook.Columns.Add(createBoundColumn());
            CodeBehindHook.ItemsSource = buildDynamicType();
        }

        private List<dynamic> buildDynamicType()
        {
            List<Customer> stronglyTypedCustomers = buildStrongyTyped();
            var names = stronglyTypedCustomers.Select(e => e.Name).Distinct();
            dynamic metrics = new List<dynamic>();
            foreach (var name in names)
            {
                dynamic row = new RadDataRow();
                row["Name"] = name;
                row["SingleMetric"] = stronglyTypedCustomers.Where(e => e.Name == name).First().SingleMetric;
                row["SingleMetricFilter"] = stronglyTypedCustomers.Where(e => e.Name == name).First().SingleMetric.DoubleNullable; // ?? 0; // created explicitly for the filter
                row["DisplayValueB"] = stronglyTypedCustomers.Where(e => e.Name == name).First().SingleMetric.DisplayValue;

                metrics.Add(row);
            }
            return metrics;
        }

        private List<Customer> buildStrongyTyped()
        {
            List<Customer> customers = new List<Customer>();

            customers.Add(new Customer("Id", "Green", 0.0));
            customers.Add(new Customer("Blizzard", "Red", .86996));
            customers.Add(new Customer("ArenaNet", "Black", 1));
            customers.Add(new Customer("Rovio", "Green", null));

            return customers;
        }

        private CellContextColumn createBoundColumn()
        {
            string metricIdentifier = "SingleMetric";
            CellContextColumn column = new CellContextColumn(cell => metricIdentifier);

            //bind.Converter = new DebugConverter();

            // when you want to filter on formatted value
            //column.FilterMemberPath = "SingleMetric"; // see the tostring override in metric class. whatever you return from tostring becomes the values for the filter.                
            //column.ShowDistinctFilters = true;

            column.FilterMemberPath = "DisplayValueB";
            // when you want to filter of specific metric object value such as int
            column.FilterMemberType = typeof(double);
            column.FilterMemberPath = "SingleMetricFilter";
            column.ShowDistinctFilters = true;

            #region [ not necessary, just trying to repo secondary filter bug ]
            Style coloredHeader = new Style(typeof(Telerik.Windows.Controls.GridView.GridViewHeaderCell))
            {
                Setters = {
                        new Setter(Telerik.Windows.Controls.GridView.GridViewHeaderCell.FontWeightProperty, FontWeights.Light),
                        new Setter(Telerik.Windows.Controls.GridView.GridViewHeaderCell.BackgroundProperty, new SolidColorBrush(Colors.Gray)),
                    }
            };

            column.Header = "Metric";
            column.HeaderCellStyle = coloredHeader;
            column.UniqueName = metricIdentifier;
            column.IsReadOnly = true;
            column.Width = 142;
            #endregion [ not necessary, just trying to repo secondary filter bug ]

            DataTemplate dt = XamlReader.Load(dataTemplateXaml) as DataTemplate;
            column.CellTemplate = dt;
            return column;
        }
    }
}
