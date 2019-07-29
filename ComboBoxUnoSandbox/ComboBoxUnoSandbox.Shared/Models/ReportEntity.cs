using System;
using System.Collections.Generic;
using System.Text;

namespace ComboBoxUnoSandbox.Shared.Models
{
    public class ReportEntity
    {

        public string Code { get; set; }

		public int Key { get; set; }

		public string Name { get; set; }

		public string ReportPath { get; set; }

		public string RoleCode { get; set; }

		public bool ShowInReportMenu { get; set; }

		public byte[] Trdx { get; set; }
    }
}
