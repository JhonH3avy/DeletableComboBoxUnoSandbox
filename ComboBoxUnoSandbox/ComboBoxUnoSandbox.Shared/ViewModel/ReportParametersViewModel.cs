using ComboBoxUnoSandbox.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Popups;
using ComboBoxUnoSandbox.Shared.Helpers;
using ComboBoxUnoSandbox.Shared.Helpers.Linq;
#if __WASM__
using Uno.Extensions.Specialized;
#endif

namespace ComboBoxUnoSandbox.Shared.ViewModel
{
    class ReportParametersViewModel : BusyOkCancelViewModelBase
    {
        List<ReportParameterHolder> holders;
        IEnumerable<ReportParameterHolder> reportParameters;

        public ReportParametersViewModel()
        {
            Load();
        }

        void Load()
        {
            var parameters = new List<ReportParameterWcf>();
            parameters.Add(new ReportParameterWcf
            {
                Behaviorx = null,
                DefaultValue = null,
                DisplayMember = "LookupCompanyName",
                ItemsSource = "ClientAccountCompanies.TypedData",
                MultiValueField = false,
                Name = "restrictClientAccountCompanyKey",
                Nullable = false,
                Prompt = "Account",
                PromptUser = true,
                Type = HseParameterType.Integer,
                ValueMember = "Key"
            });
            parameters.Add(new ReportParameterWcf
            {
                Behaviorx = "restrictClientAccountCompanyKey ClientAccountRefCompanyKey",
                DefaultValue = null,
                DisplayMember = "Name",
                ItemsSource = "Divisions.TypedData",
                MultiValueField = false,
                Name = "divisionKey",
                Nullable = false,
                Prompt = "Division",
                PromptUser = true,
                Type = HseParameterType.Integer,
                ValueMember = "Key"
            });
            parameters.Add(new ReportParameterWcf
            {
                Behaviorx = "divisionKey DivisionKey",
                DefaultValue = null,
                DisplayMember = "Name",
                ItemsSource = "BusinessUnits.TypedData",
                MultiValueField = false,
                Name = "businessUnitKey",
                Nullable = false,
                Prompt = "Business Unit",
                PromptUser = false,
                Type = HseParameterType.Integer,
                ValueMember = "Key"
            });
            ProcessParameters(parameters);
        }

        void ProcessParameters(IEnumerable<ReportParameterWcf> parameters)
        {
            holders = new List<ReportParameterHolder>();
            foreach (var p in parameters)
            {
                holders.Add(new ReportParameterHolder(p, GetFormatter(p)));
            }

            SetParameters(holders);
        }

        void SetParameters(List<ReportParameterHolder> holders)
        {
            ReportParameters = holders.Where(VisibleParameter);
            OK(null);
        }

        bool VisibleParameter(ReportParameterHolder holder)
        {
            return true;
        }

        IValueFormatter GetFormatter(ReportParameterWcf reportParameterWcf)
        {
            if (reportParameterWcf.Type == HseParameterType.DateTime) return new DateValueFormatter();

            return new DefaultValueFormatter();
        }

        public override bool OK(object window)
        {
            return true;
        }

        bool IsValid()
        {
            return true;
        }

        public KeyValuePair<string, string>[] Presets { get; set; }

        public IEnumerable<ReportParameterHolder> ReportParameters
        {
            get { return reportParameters; }
            set
            {
                reportParameters = value;
                RaiseChanged();
            }
        }
    }
}
