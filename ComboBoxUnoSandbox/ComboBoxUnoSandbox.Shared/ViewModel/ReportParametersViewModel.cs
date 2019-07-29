using ComboBoxUnoSandbox.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Popups;
using ComboBoxUnoSandbox.Shared.Helpers;
using ComboBoxUnoSandbox.Shared.Helpers.Linq;
using Uno.Extensions.Specialized;

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
                Behaviorx = "Behaviour1",
                DefaultValue = "DefaultValue1",
                DisplayMember = "DisplayMember1",
                ItemsSource = "ItemsSource1",
                MultiValueField = false,
                Name = "ClientAccountCompanies",
                Nullable = false,
                Prompt = "Prompt1",
                PromptUser = false,
                Type = HseParameterType.String,
                ValueMember = "ValueMember1"
            });
            parameters.Add(new ReportParameterWcf
            {
                Behaviorx = "Behaviour2",
                DefaultValue = "DefaultValue2",
                DisplayMember = "DisplayMember2",
                ItemsSource = "ItemsSource2",
                MultiValueField = false,
                Name = "ClaimSubtypes",
                Nullable = false,
                Prompt = "Prompt2",
                PromptUser = false,
                Type = HseParameterType.String,
                ValueMember = "ValueMember2"
            });
            parameters.Add(new ReportParameterWcf
            {
                Behaviorx = "Behaviour3",
                DefaultValue = "DefaultValue3",
                DisplayMember = "DisplayMember3",
                ItemsSource = "ItemsSource3",
                MultiValueField = false,
                Name = "Divisions",
                Nullable = false,
                Prompt = "Prompt3",
                PromptUser = false,
                Type = HseParameterType.String,
                ValueMember = "ValueMember3"
            });
            parameters.Add(new ReportParameterWcf
            {
                Behaviorx = "Behaviour4",
                DefaultValue = "DefaultValue4",
                DisplayMember = "DisplayMember4",
                ItemsSource = "ItemsSource4",
                MultiValueField = false,
                Name = "BusinessUnits",
                Nullable = false,
                Prompt = "Prompt4",
                PromptUser = false,
                Type = HseParameterType.String,
                ValueMember = "ValueMember4"
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
            //var client = RefDataSources.Instance.ClientAccountCompanies;
            //var subTypes = RefDataSources.Instance.ClaimSubtypes;
            //var divisions = RefDataSources.Instance.Divisions;
            //var business = RefDataSources.Instance.BusinessUnits;
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
