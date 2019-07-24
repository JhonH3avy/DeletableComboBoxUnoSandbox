using System;
using System.Collections.Generic;
using System.Text;

namespace ComboBoxUnoSandbox.Shared.Helpers
{
    [Bindable]
    public class ReferenceBinding
    {
        public static DependencyProperty ReferencePathProperty = DependencyProperty.RegisterAttached("ReferencePath",
            typeof(string), typeof(ReferenceBinding), new PropertyMetadata(null, ReferencePathChanged));

        static void ReferencePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var path = e.NewValue as string;
            var combo = d as ComboBox;
            if (path != null && combo != null)
            {
                var paramHolder = combo.DataContext as ReportParameterHolder;
                if (paramHolder != null)
                {
                    paramHolder.ItemsView = new CollectionViewSource
                    {
                        Source = GetRefDataItems(path, combo)
                    };
                    var en = paramHolder.ItemsView.Source as IEnumerable;
                    Debug.WriteLine("ReferencePathChanged set itemsview with source with " + en.Count() + " for " + paramHolder.Name + $" old {e.OldValue} and new {e.NewValue}");

                }
            }
        }

        static object GetRefDataItems(string path, ComboBox combo)
        {
            var binding = new Binding { Path = new PropertyPath(path), Source = RefDataSources.Instance };
            BindingOperations.SetBinding(combo, FrameworkElement.TagProperty, binding);
            return combo.GetValue(FrameworkElement.TagProperty);
        }

        public static string GetReferencePath(ComboBox element)
        {
            return (string)element.GetValue(ReferencePathProperty);
        }

        public static void SetReferencePath(ComboBox element, string value)
        {
            element.SetValue(ReferencePathProperty, value);
        }

        public static readonly DependencyProperty BehaviorProperty =
            DependencyProperty.RegisterAttached("Behavior", typeof(string), typeof(ReferenceBinding),
            new PropertyMetadata(default(string), BehaviorChanged));

        static void BehaviorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var combo = d as ComboBox;
            if (combo == null) return;

            Debug.WriteLine("adding behaviour");
            AddBehavior(combo);
        }

        static void AddBehavior(ComboBox combo)
        {
            var paramHolders = GetParamHolders(combo);
            if (paramHolders.IsNullOrEmpty()) return;
            var behaviorString = GetBehavior(combo);
            if (string.IsNullOrEmpty(behaviorString)) return;

            var parts = behaviorString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) return;

            if (parts[1].StartsWith("'"))
            {
                AddFixedFilter(parts, combo);
            }
            else
            {
                AddDrivenFilter(paramHolders, parts, combo);
            }
        }

        static void AddFixedFilter(string[] parts, ComboBox combo)
        {
            var paramHolder = combo.DataContext as ReportParameterHolder;
            if (paramHolder != null)
            {
                var fixedValue = parts[1].Replace("'", "");
                var filteredItems = paramHolder.ItemsView.View
                .Where(o =>
                {
                    var propInfo = o.GetType().GetProperty(parts[0]);
                    if (propInfo == null) return true;
                    var listValue = propInfo.GetValue(o, null);
                    if (listValue == null) return false;
                    return listValue.Equals(fixedValue);
                }).ToList();
                paramHolder.ItemsView.Source = filteredItems;  // TODO this is a bid dodgy , changing the paramHolder.ItemsView.Source
            }
        }


        static void AddDrivenFilter(IEnumerable<ReportParameterHolder> paramHolders, string[] parts, ComboBox combo)
        {
            var drivingParamHolder = paramHolders.FirstOrDefault(p => p.Name == parts[0]);
            var cvs = combo.ItemsSource as ICollectionView;
            // try to get original items from combo, but if they are not set, then hard code a path from the DataContext
            if (cvs == null)
            {
                if (combo.DataContext is ReportParameterHolder ph)
                {
                    cvs = ph.ItemsView.View;
                }
            }
            var beh = new ComboFilterBehavior
            {
                PropertyName = parts[1],
                MasterSource = cvs
            };
            BindingOperations.SetBinding(beh,
                                         ComboFilterBehavior.DrivingValueProperty,
                                         new Binding
                                         {
                                             Source = drivingParamHolder,
                                             Path = new PropertyPath("Value")
                                         });
            Interaction.GetBehaviors(combo).Add(beh);
        }

        public static void SetBehavior(UIElement element, string value)
        {
            element.SetValue(BehaviorProperty, value);
        }

        public static string GetBehavior(UIElement element)
        {
            return (string)element.GetValue(BehaviorProperty);
        }

        public static readonly DependencyProperty ParamHoldersProperty =
            DependencyProperty.RegisterAttached("ParamHolders", typeof(IEnumerable<ReportParameterHolder>), typeof(ReferenceBinding),
            new PropertyMetadata(default(IEnumerable<ReportParameterHolder>), ParamHoldersChanged));

        static void ParamHoldersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var combo = d as ComboBox;
            if (combo == null) return;

            AddBehavior(combo);
        }

        public static void SetParamHolders(UIElement element, IEnumerable<ReportParameterHolder> value)
        {
            element.SetValue(ParamHoldersProperty, value);
        }

        public static IEnumerable<ReportParameterHolder> GetParamHolders(UIElement element)
        {
            return (IEnumerable<ReportParameterHolder>)element.GetValue(ParamHoldersProperty);
        }
    }
}
