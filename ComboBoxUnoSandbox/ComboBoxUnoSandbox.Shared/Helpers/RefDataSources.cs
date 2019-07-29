using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace ComboBoxUnoSandbox.Shared.Helpers
{
    [Bindable]
    public class RefDataSources
    {
        public string ProductCode = "CMS";
        public bool IsCmsLite { get { return ProductCode == "CMSLite"; } }
        public bool IsCms { get { return "CMS".Equals(ProductCode, StringComparison.OrdinalIgnoreCase); } }

        public static RefDataSources Instance = new RefDataSources();

        public static void Clear()
        {
            var productCode = Instance.ProductCode;
            Instance = new RefDataSources();
            Instance.ProductCode = productCode;
        }

        public IEnumerable<string> ClientAccountCompanies => new[] { "Client1", "Client2", "Client3" };

        public IEnumerable<string> BusinessUnits => new[] {"Business1", "Business2", "Business3"};

        public IEnumerable<string> ClaimSubtypes => new[] { "ClaimSubtypes1", "ClaimSubtypes2", "ClaimSubtypes3" };


        public IEnumerable<string> Companies => new[] { "Companies1", "Companies2", "Companies3" };


        public IEnumerable<string> Contracts => new[] { "Contracts1", "Contracts2", "Contracts3" };


        public IEnumerable<string>  Divisions => new[] { "Divisions1", "Divisions2", "Divisions3" };


        public IEnumerable<string> ClientAccountUser => new[] { "ClientAccountUser1", "ClientAccountUser2", "ClientAccountUser3" };


        public IEnumerable<string> ClientAccountUsers => new[] { "ClientAccountUsers1", "ClientAccountUsers2", "ClientAccountUsers3" };

    }
}
