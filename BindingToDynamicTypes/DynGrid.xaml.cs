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
using System.Collections.ObjectModel;

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
                <TextBlock Text='{Binding DisplayValue}' Foreground='{Binding DataStateColor}' />
            </StackPanel>
</DataTemplate>";

        List<string> colName = new List<string> { "Sales", "Share" };
        public DynGrid()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DynGrid_Loaded);
        }

        void DynGrid_Loaded(object sender, RoutedEventArgs e)
        {
            CodeBehindHook.Columns.Add(new GridViewDataColumn() { DataMemberBinding = new Binding("Name") });
            CodeBehindHook.Columns.Add(createBoundColumn(colName[0]));
            CodeBehindHook.Columns.Add(createBoundColumn(colName[1]));
            CodeBehindHook.ItemsSource = buildDynamicType();
        }

        private ObservableCollection<dynamic> buildDynamicType()
        {
            List<Customer> stronglyTypedCustomers = buildStrongyTyped();
            var rowTitles = stronglyTypedCustomers.Select(e => e.Name).Distinct();
            string metricIdentifier;
            dynamic metrics = new ObservableCollection<dynamic>();
            foreach (var title in rowTitles)
            {
                dynamic currentRow = new RadDataRow();
                currentRow["Name"] = title;

                Customer rowContents = stronglyTypedCustomers.Where(e => e.Name == title).First();
                foreach (var metric in rowContents.Metrics)
                {
                    metricIdentifier = metric.MetricName;
                    currentRow[metricIdentifier] = metric;
                    currentRow["Filter" + metricIdentifier] = (double)(metric.DoubleNullable ?? 0); // created explicitly for the filter
                    currentRow["Display" + metricIdentifier] = metric.DisplayValue;
                }

                metrics.Add(currentRow);
            }
            return metrics;
        }

        private List<Customer> buildStrongyTyped()
        {
            List<Customer> customers = new List<Customer>();

            //Each column needs to have a value, otherwise every row beneath can't be filtered on that column
            customers.Add(new Customer("Id", new List<Metric>() { 
                new Metric(colName[0], 0.0, "Green"),
            }));
            customers.Add(new Customer("Blizzard", new List<Metric>() {
                new Metric(colName[1], 1, "Green")
            }));
            customers.Add(new Customer("ArenaNet", new List<Metric>() {
                new Metric(colName[0], null, "Red"), new Metric(colName[1], 1, "Black"),
            }));
            customers.Add(new Customer("Rovio", new List<Metric>() {
                new Metric(colName[0], .86996, "Orange"), new Metric(colName[1], 0.0, "Red")
            }));

            return customers;
        }

        private CellContextColumn createBoundColumn(string metricIdentifier)
        {
            CellContextColumn column = new CellContextColumn(cell => metricIdentifier);

            //bind.Converter = new DebugConverter();

            // when you want to filter on formatted value
            //column.FilterMemberPath = "SingleMetric"; // see the tostring override in metric class. whatever you return from tostring becomes the values for the filter.                
            //column.ShowDistinctFilters = true;

            column.FilterMemberPath = "Display" + metricIdentifier;
            // when you want to filter of specific metric object value such as int
            column.FilterMemberType = typeof(double);
            column.FilterMemberPath = "Filter" + metricIdentifier;
            column.ShowDistinctFilters = true;

            #region [ not necessary, just trying to repo secondary filter bug ]
            Style coloredHeader = new Style(typeof(Telerik.Windows.Controls.GridView.GridViewHeaderCell))
            {
                Setters = {
                        new Setter(Telerik.Windows.Controls.GridView.GridViewHeaderCell.FontWeightProperty, FontWeights.Light),
                        new Setter(Telerik.Windows.Controls.GridView.GridViewHeaderCell.BackgroundProperty, new SolidColorBrush(Colors.Gray)),
                    }
            };

            column.Header = metricIdentifier;
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
