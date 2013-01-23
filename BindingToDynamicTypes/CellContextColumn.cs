using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using System.Collections.Generic;

namespace BindingToDynamicTypes
{
    public class CellContextColumn : GridViewBoundColumnBase
    {
        Func<GridViewCell, string> buildIdentifierFromCell;

        public CellContextColumn(Func<GridViewCell, string> buildIdentifierFromCell)
        {
            this.buildIdentifierFromCell = buildIdentifierFromCell;
        }

        public override System.Windows.FrameworkElement CreateCellElement(GridViewCell cell, object dataItem)
        {
            var element = base.CreateCellElement(cell, dataItem);
            if (buildIdentifierFromCell == null)
                return element;

            string metricIdentifier = buildIdentifierFromCell(cell);
            IEnumerable<string> members = (dataItem as RadDataRow).GetDynamicMemberNames();
            if (members.Contains(metricIdentifier))
            {
                Metric metric = (dataItem as RadDataRow)[metricIdentifier] as Metric;
                element.DataContext = metric;
            }
            else
            {
                element.DataContext = new Metric() { Value = 99, Status = "none", DoubleNullable = 0.123 };
            }
            return element;
        }
    }
}
