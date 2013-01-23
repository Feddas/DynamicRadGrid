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
using System.Dynamic;
using System.ComponentModel;
using System.Collections.Generic;

namespace BindingToDynamicTypes
{
    /// <summary>
    /// Allows a DataRow to have a dynamic number of columns.
    /// copied from http://blogs.telerik.com/vladimirenchev/posts/11-09-28/dynamic-binding-for-your-silverlight-applications.aspx
    /// </summary>
    public class RadDataRow : DynamicObject, INotifyPropertyChanged
    {
        IDictionary<string, object> data;

        public RadDataRow()
        {
            data = new Dictionary<string, object>();
        }

        public RadDataRow(IDictionary<string, object> source)
        {
            data = source;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return data.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this[binder.Name] = value;

            return true;
        }

        public object this[string columnName]
        {
            get
            {
                if (data.ContainsKey(columnName))
                {
                    return data[columnName];
                }

                return null;
            }
            set
            {
                if (!data.ContainsKey(columnName))
                {
                    data.Add(columnName, value);

                    OnPropertyChanged(columnName);
                }
                else
                {
                    if (data[columnName] != value)
                    {
                        data[columnName] = value;

                        OnPropertyChanged(columnName);
                    }
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
