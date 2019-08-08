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

        public IEnumerable<ComboBoxEntity> ClientAccountCompanies => new[] { new ComboBoxEntity{ Name = "Client1", Value = "Client1" },
            new ComboBoxEntity{ Name = "Client2", Value = "Client2" },
            new ComboBoxEntity{ Name = "Client3", Value = "Client2" }
        };

        public IEnumerable<ComboBoxEntity> BusinessUnits => new[] {new ComboBoxEntity{ Name = "BusinessUnit1", Value = "BusinessUnit1" },
            new ComboBoxEntity{ Name = "BusinessUnit2", Value = "BusinessUnit2" },
            new ComboBoxEntity{ Name = "BusinessUnit3", Value = "BusinessUnit3" }

        };

        public IEnumerable<ComboBoxEntity> ClaimSubtypes => new[] {new ComboBoxEntity{ Name = "ClaimSubtype1", Value = "ClaimSubtype1" },
            new ComboBoxEntity{ Name = "ClaimSubtype2", Value = "ClaimSubtype2" },
            new ComboBoxEntity{ Name = "ClaimSubtype3", Value = "ClaimSubtype3" }

        };


        public IEnumerable<ComboBoxEntity> Companies => new[] { new ComboBoxEntity{ Name = "Company1", Value = "Company1" },
            new ComboBoxEntity{ Name = "Company2", Value = "Company2" },
            new ComboBoxEntity{ Name = "Company3", Value = "Company3" }
        };


        public IEnumerable<ComboBoxEntity> Contracts => new[] { new ComboBoxEntity{ Name = "Contract1", Value = "Contract1" },
            new ComboBoxEntity{ Name = "Contract2", Value = "Contract2" },
            new ComboBoxEntity{ Name = "Contract3", Value = "Contract3" }
        };


        public IEnumerable<ComboBoxEntity>  Divisions => new[] { new ComboBoxEntity{ Name = "Division1", Value = "Division1" },
            new ComboBoxEntity{ Name = "Division2", Value = "Division2" },
            new ComboBoxEntity{ Name = "Division3", Value = "Division3" }
        };


        public IEnumerable<ComboBoxEntity> ClientAccountUser => new[] { new ComboBoxEntity{ Name = "ClientAccountUser1", Value = "ClientAccountUser1" },
            new ComboBoxEntity{ Name = "ClientAccountUser2", Value = "ClientAccountUser2" },
            new ComboBoxEntity{ Name = "ClientAccountUser3", Value = "ClientAccountUser3" }
        };


        public IEnumerable<ComboBoxEntity> ClientAccountUsers => new[] { new ComboBoxEntity{ Name = "ClientAccountUsers1", Value = "ClientAccountUsers1" },
            new ComboBoxEntity{ Name = "ClientAccountUsers2", Value = "ClientAccountUsers2" },
            new ComboBoxEntity{ Name = "ClientAccountUsers3", Value = "ClientAccountUsers3" }
        };

    }
}
