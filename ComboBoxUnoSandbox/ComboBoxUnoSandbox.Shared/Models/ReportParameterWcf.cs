using ComboBoxUnoSandbox.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComboBoxUnoSandbox.Shared.Models
{
    public partial class ReportParameterWcf : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string BehaviorxField;

        private string DefaultValueField;

        private string DisplayMemberField;

        private string ItemsSourceField;

        private bool MultiValueFieldField;

        private string NameField;

        private bool NullableField;

        private string PromptField;

        private bool PromptUserField;

        private HseParameterType TypeField;

        private string ValueMemberField;

        public string Behaviorx
        {
            get
            {
                return this.BehaviorxField;
            }
            set
            {
                if ((object.ReferenceEquals(this.BehaviorxField, value) != true))
                {
                    this.BehaviorxField = value;
                    this.RaisePropertyChanged("Behaviorx");
                }
            }
        }

        public string DefaultValue
        {
            get
            {
                return this.DefaultValueField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DefaultValueField, value) != true))
                {
                    this.DefaultValueField = value;
                    this.RaisePropertyChanged("DefaultValue");
                }
            }
        }

        public string DisplayMember
        {
            get
            {
                return this.DisplayMemberField;
            }
            set
            {
                if ((object.ReferenceEquals(this.DisplayMemberField, value) != true))
                {
                    this.DisplayMemberField = value;
                    this.RaisePropertyChanged("DisplayMember");
                }
            }
        }

        public string ItemsSource
        {
            get
            {
                return this.ItemsSourceField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ItemsSourceField, value) != true))
                {
                    this.ItemsSourceField = value;
                    this.RaisePropertyChanged("ItemsSource");
                }
            }
        }

        public bool MultiValueField
        {
            get
            {
                return this.MultiValueFieldField;
            }
            set
            {
                if ((this.MultiValueFieldField.Equals(value) != true))
                {
                    this.MultiValueFieldField = value;
                    this.RaisePropertyChanged("MultiValueField");
                }
            }
        }

        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                if ((object.ReferenceEquals(this.NameField, value) != true))
                {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }

        public bool Nullable
        {
            get
            {
                return this.NullableField;
            }
            set
            {
                if ((this.NullableField.Equals(value) != true))
                {
                    this.NullableField = value;
                    this.RaisePropertyChanged("Nullable");
                }
            }
        }

        public string Prompt
        {
            get
            {
                return this.PromptField;
            }
            set
            {
                if ((object.ReferenceEquals(this.PromptField, value) != true))
                {
                    this.PromptField = value;
                    this.RaisePropertyChanged("Prompt");
                }
            }
        }

        public bool PromptUser
        {
            get
            {
                return this.PromptUserField;
            }
            set
            {
                if ((this.PromptUserField.Equals(value) != true))
                {
                    this.PromptUserField = value;
                    this.RaisePropertyChanged("PromptUser");
                }
            }
        }

        public HseParameterType Type
        {
            get
            {
                return this.TypeField;
            }
            set
            {
                if ((this.TypeField.Equals(value) != true))
                {
                    this.TypeField = value;
                    this.RaisePropertyChanged("Type");
                }
            }
        }

        public string ValueMember
        {
            get
            {
                return this.ValueMemberField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ValueMemberField, value) != true))
                {
                    this.ValueMemberField = value;
                    this.RaisePropertyChanged("ValueMember");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
