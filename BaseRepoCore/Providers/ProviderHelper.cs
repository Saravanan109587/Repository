using System;

namespace BaseRepoCore
{
    public class ProviderHelper
    {
        public static IProvider GetProvider(string providerName)
        {
            if (string.Equals(providerName, "System.Data.SqlClient", StringComparison.InvariantCultureIgnoreCase))
                return new SqlServerProvider();

            return null;
        }
    }
}