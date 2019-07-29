using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ComboBoxUnoSandbox.Shared.Models
{
    public class ReportModel
    {

        public ReportModel()
        {
            Load();
        }

        void Load()
        {
            var reports = new List<ReportEntity>();
            reports.Add(new ReportEntity
            {
                Code = "Report1",
                Key = 1,
                Name = "Report1",
                ReportPath = "ReportPath1",
                RoleCode = "RoleCode1",
                ShowInReportMenu = true,
                Trdx = new byte[0]
            });
            Reports = reports;
        }

        public IEnumerable<ReportEntity> Reports { get; set; }


    }
}
