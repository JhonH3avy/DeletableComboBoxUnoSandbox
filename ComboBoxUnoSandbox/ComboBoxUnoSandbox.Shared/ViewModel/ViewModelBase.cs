using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ComboBoxUnoSandbox.Shared.Helpers.Extensions;
using ComboBoxUnoSandbox.Shared.Models.EventArgs;
#if __WASM__
using ComboBoxUnoSandbox.Wasm.Annotations;
#endif

namespace ComboBoxUnoSandbox.Shared.ViewModel
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        public void SetAndRaiseChanged<T>(ref T backingProperty, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (!AreEqual(backingProperty, newValue))
            {
                backingProperty = newValue;
                OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            }
        }

        public void ValidateSetAndRaiseChanged<T>(ref T backingProperty, T newValue, Expression<Func<T>> propertySelector)
        {
            throw new NotImplementedException();
            //            if (!AreEqual(backingProperty, newValue))
            //            {
            ////                var validationResult = new List<ValidationResult>();
            //                var memberName = ExpressionMemberName(propertySelector);
            ////                if (!Validator.TryValidateProperty(newValue, new ValidationContext(this, null, null) { MemberName = memberName }, validationResult))
            ////                {
            ////                    SetErrors(memberName, validationResult);
            ////                    return;
            ////                }
            //                backingProperty = newValue;
            //				RaiseChanged(propertySelector);
            //                ClearErrors(memberName);
            //            }
        }

        protected virtual void ClearErrors(string memberName)
        {
        }

        //        protected virtual void SetErrors(string member, List<ValidationResult> validationResult)
        //	    {
        //	    }

        public static bool AreEqual(object backingProperty, object newValue)
        {
            return backingProperty == newValue || (backingProperty == null && newValue == null);
        }

#if __WASM__
        [NotifyPropertyChangedInvocator]
#endif
        public void RaiseChanged([CallerMemberName] string propertyName = "no caller")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected string ExpressionMemberName<TValue>(Expression<Func<TValue>> propertySelector)
        {
            var memberExpression = propertySelector.Body as MemberExpression;
            if (memberExpression != null) return memberExpression.Member.Name;
            return null;
        }

        protected string ExpressionMemberName<T, TReturn>(Expression<Func<T, TReturn>> expression)
        {
            var body = (MemberExpression)expression.Body;
            return body.Member.Name;
        }

        public bool ChangedNameEquals<T, TReturn>(PropertyChangedEventArgs propertyChangedEventArgs, Expression<Func<T, TReturn>> expression)
        {
            return ChangedNameEquals(propertyChangedEventArgs.PropertyName, expression);
        }

        public bool ChangedNameEquals<T, TReturn>(EntityChangedEventArgs args, Expression<Func<T, TReturn>> expression)
        {
            return ChangedNameEquals(args.PropertyName, expression);
        }

        public bool ChangedNameEquals(EntityChangedEventArgs args, string propertyName)
        {
            return propertyName.EqualsIc(args.PropertyName);
        }

        public bool ChangedNameEquals<T, TReturn>(string changedPropertyName, Expression<Func<T, TReturn>> expression)
        {
            return ExpressionMemberName(expression).Equals(changedPropertyName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
