using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using System.Collections;
using ComboBoxUnoSandbox.Shared.Helpers.Linq;

namespace ComboBoxUnoSandbox.Shared.Controls
{
    public class ComboFilterBehavior : Behavior<ComboBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            FilterSource(DrivingValue);
            //            if (AssociatedObject.ItemsSource == null) AssociatedObject.ItemsSource = collectionView;
            if (ComboFilterOperation == ComboFilterOperations.Unique)
            {
                AssociatedObject.DropDownOpened += DropDownOpened;
            }
        }

        void DropDownOpened(object sender, object o1)
        {
            FilteredSource = ((IEnumerable<object>)MasterSource).Where(o =>
            {
                var propInfo = o.GetType().GetProperty(PropertyName);
                if (propInfo == null) return true;
                var existing = DrivingValue as IEnumerable;
                var thisValue = AssociatedObject.SelectedValue;
                var listValue = propInfo.GetValue(o, null);
                if (listValue == null) return false;
                return listValue.Equals(thisValue) || existing.Cast<object>().None(listValue.Equals);
            });
            if (AssociatedObject != null) AssociatedObject.ItemsSource = FilteredSource;
        }

        public object FilteredSource
        {
            get { return GetValue(FilteredSourceProperty); }
            set { SetValue(FilteredSourceProperty, value); }
        }

        public static readonly DependencyProperty FilteredSourceProperty =
            DependencyProperty.Register("FilteredSource", typeof(IEnumerable), typeof(ComboFilterBehavior),
                new PropertyMetadata(null));

        public object MasterSource
        {
            get { return GetValue(MasterSourceProperty); }
            set { SetValue(MasterSourceProperty, value); }
        }

        public static readonly DependencyProperty MasterSourceProperty =
            DependencyProperty.Register("MasterSource", typeof(object), typeof(ComboFilterBehavior),
            new PropertyMetadata(null, MasterSourceChanged));

        static void MasterSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var b = d as ComboFilterBehavior;
            if (b != null)
            {
                b.FilterSource(b.DrivingValue);
            }
        }

        public object DrivingValue
        {
            get { return GetValue(DrivingValueProperty); }
            set { SetValue(DrivingValueProperty, value); }
        }

        public static readonly DependencyProperty DrivingValueProperty =
            DependencyProperty.Register("DrivingValue", typeof(object), typeof(ComboFilterBehavior), new PropertyMetadata(null, DrivingValueChanged));


        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(string), typeof(ComboFilterBehavior), null);

        static void DrivingValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var b = d as ComboFilterBehavior;
            if (b != null)
            {
                b.FilterSource(e.NewValue);
            }
        }

        void FilterSource(object newValue)
        {
            // for Unique filter on the drop down open event
            if (ComboFilterOperation == ComboFilterOperations.Unique)
            {
                return;
            }
            if (MasterSource == null || string.IsNullOrEmpty(PropertyName)) return;
            var items = newValue == null ? new List<object>() : ((IEnumerable<object>)MasterSource).Where(i =>
            {
                var propInfo = i.GetType().GetProperty(PropertyName);
                if (propInfo == null) return true;
                return newValue.Equals(propInfo.GetValue(i, null));
            }).ToList();
            FilteredSource = items;
            if (AssociatedObject != null) AssociatedObject.ItemsSource = FilteredSource;
        }

        public static readonly DependencyProperty ComboFilterOperationProperty = DependencyProperty.Register(
            "ComboFilterOperation", typeof(ComboFilterOperations?), typeof(ComboFilterBehavior), new PropertyMetadata(default(ComboFilterOperations?)));



        public ComboFilterOperations? ComboFilterOperation
        {
            get { return (ComboFilterOperations?)GetValue(ComboFilterOperationProperty); }
            set { SetValue(ComboFilterOperationProperty, value); }
        }
    }

    public enum ComboFilterOperations
    {
        /// <summary>
        /// Means the current value and other values in the source list that are not in the DrivingValue are allowed.
        /// </summary>
        Unique
    }
}
