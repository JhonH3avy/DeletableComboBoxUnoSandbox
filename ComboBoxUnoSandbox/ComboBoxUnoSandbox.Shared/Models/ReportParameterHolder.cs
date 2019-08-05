using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Windows.UI.Xaml.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ComboBoxUnoSandbox.Shared.ViewModel;
using System.ComponentModel;
using System.Collections;
using System.Collections.ObjectModel;
#if !__WASM__
using System.ComponentModel.DataAnnotations;
#endif
using ComboBoxUnoSandbox.Shared.Helpers;
using ComboBoxUnoSandbox.Shared.Helpers.Json;
using ComboBoxUnoSandbox.Shared.Helpers.Linq;
using ValidationResult = ComboBoxUnoSandbox.Shared.Helpers.ValidationResult;

namespace ComboBoxUnoSandbox.Shared.Models
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ReportParameterHolder : ViewModelBase, INotifyDataErrorInfo
    {
        readonly ReportParameterWcf parameter;
        readonly IValueFormatter formatter;

        public ReportParameterHolder(ReportParameterWcf parameter, IValueFormatter formatter)
        {
            this.parameter = parameter;
            this.formatter = formatter;
            if (parameter.ItemsSource != null && parameter.ItemsSource.Contains("{"))
            {
                var items = JsonConvert.DeserializeObject<List<CodeName>>(parameter.ItemsSource, new JsonSerializerSettings
                {
                    ContractResolver = new CodeNameContractResolver(parameter),
                    Error = CodeNameErrorHandler,
                    MissingMemberHandling = MissingMemberHandling.Error,

                });
                if (parameter.ValueMember == parameter.DisplayMember)
                {
                    items.Run(i => i.name = i.code);
                }
                ItemsView = new CollectionViewSource { Source = items };
                parameter.ValueMember = "code";
                parameter.DisplayMember = "name";
            }
            if (parameter.MultiValueField)
            {
                Value = new ObservableCollection<object>();
            }
            ConvertAndSetDefault();
        }

        void CodeNameErrorHandler(object sender, ErrorEventArgs e)
        {

        }

        void ConvertAndSetDefault()
        {
            TypeCode code = ParameterTypeToTypeCode(ParameterType);
            if (!string.IsNullOrEmpty(parameter.DefaultValue))
            {
                try
                {
                    Value = Convert.ChangeType(parameter.DefaultValue, code, CultureInfo.CurrentCulture);
                }
                catch (Exception e)
                {
                    //                    ErrorNotifier.NotifyError("Report parameter holder error", new Exception(string.Format("Could not convert default value {0} to type {1}", parameter.DefaultValue, code), e), 
                    //                        WebContext.Current, ErrorNotifier.MailWcfServiceClient);
                }
            }
            // always set bools to false when they are mandatory
            if (code == TypeCode.Boolean && Value == null && !parameter.Nullable) Value = false;
        }

        static TypeCode ParameterTypeToTypeCode(HseParameterType type)
        {
            switch (type)
            {
                case HseParameterType.Integer: return TypeCode.Int32;
                case HseParameterType.DateTime: return TypeCode.DateTime;
                case HseParameterType.Float: return TypeCode.Double;
                case HseParameterType.Boolean: return TypeCode.Boolean;
                case HseParameterType.String: return TypeCode.String;
                default:
                    throw new ArgumentException("unsupported report parameter type " + type);
            }
        }

        public string Prompt
        {
            get { return parameter.Prompt; }
        }

        public string Name
        {
            get { return parameter.Name; }
        }

        public string ItemsSource
        {
            get { return parameter.ItemsSource; }
        }

        public string Behavior
        {
            get { return parameter.Behaviorx; }
        }

        public string ValueMember
        {
            get { return parameter.ValueMember; }
        }

        public string DisplayMember
        {
            get { return parameter.DisplayMember; }
        }

        public bool MultiValue
        {
            get { return parameter.MultiValueField; }
        }

        public HseParameterType ParameterType
        {
            get { return parameter.Type; }
        }

        public bool Visible { get; set; }
        public bool PromptUser => parameter.PromptUser;

        public CollectionViewSource ItemsView
        {
            get { return itemsView; }
            set { SetAndRaiseChanged(ref itemsView, value); }
        }

        object val;
        public object Value
        {
            get { return val; }
            set
            {
                SetAndRaiseChanged(ref val, value);
                Validate();
            }
        }

        public bool HasValue
        {
            get { return val != null; }
        }

        public bool Nullable { get { return parameter.Nullable; } }


        public bool Validate()
        {
            errors.Clear();
            InvokeErrorsChanged(new DataErrorsChangedEventArgs("Value"));
            if (!Nullable && !HasValue)
            {
                errors.Add("Value", new List<ValidationResult>
                {
                    new ValidationResult("Value is mandatory", new [] {"Value"})
                });
                InvokeErrorsChanged(new DataErrorsChangedEventArgs("Value"));
            }
            if (HasValue && ParameterType == HseParameterType.DateTime
               && Value is DateTime
               && (((DateTime)Value) < DateTimeEx.MinSqlDate || ((DateTime)Value) > new DateTime(2099, 1, 1)))
            {
                errors.Add("Value", new List<ValidationResult>
                {
                    new ValidationResult("Date must be greater than 1 Jan 1900 and less than 1 Jan 2099", new [] {"Value"})
                });
                InvokeErrorsChanged(new DataErrorsChangedEventArgs("Value"));
            }
            return HasErrors;
        }

        readonly Dictionary<String, List<ValidationResult>> errors = new Dictionary<string, List<ValidationResult>>();
        CollectionViewSource itemsView;

        public IEnumerable GetErrors(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName) ||
               !errors.ContainsKey(propertyName)) return null;
            return errors[propertyName];
        }

        public bool HasErrors
        {
            get
            {
                return errors.Count > 0;
            }
        }

        public string FormatForQueryString()
        {
            return FormatForQueryString(Value);
        }

        string FormatForQueryString(object v)
        {
            return formatter.Format(v);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public void InvokeErrorsChanged(DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }

        public void AddToUriQuery(UriBuilder builder)
        {
            if (MultiValue)
            {
                var enumerable = Value as IEnumerable<object>;
                if (enumerable != null && enumerable.Any())
                {
                    //  Value will be the SelectedItems from the combo, i.e the entities not the values
                    var pi = enumerable.First().GetType().GetProperty(ValueMember);
                    builder.AddQueryParams(Name, enumerable.Select(v => FormatForQueryString(pi.GetValue(v, null))));
                }
            }
            else builder.SetQueryParam(Name, FormatForQueryString());
        }
    }

    class CodeNameContractResolver : DefaultContractResolver
    {
        readonly ReportParameterWcf parameter;

        public CodeNameContractResolver(ReportParameterWcf parameter)
        {
            this.parameter = parameter;
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            if (propertyName.EndsWith("code", StringComparison.OrdinalIgnoreCase)) return parameter.ValueMember;
            if (parameter.ValueMember == parameter.DisplayMember && !propertyName.EndsWith("code")) return parameter.ValueMember + "_";
            return parameter.DisplayMember;
        }
    }
}
