using System;
using System.Configuration;

namespace Store_API.Adapters.Configuration
{
    class StoreServerConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("address")]
        public StoreUriSpecification Address
        {
            get { return this["address"] as StoreUriSpecification; }
            set { this["address"] = value; }
        }

        public static StoreServerConfiguration GetConfiguration()
        {
            var configuration =
                ConfigurationManager.GetSection("storeServer") as StoreServerConfiguration;

            if (configuration != null)
                return configuration;

            return new StoreServerConfiguration();
        }
    }

    public class StoreUriSpecification : ConfigurationElement
    {
        [ConfigurationProperty("uri", DefaultValue = "http://localhost:3300/", IsRequired = true)]
        public Uri Uri
        {
            get { return (Uri)this["uri"]; }
            set { this["uri"] = value; }
        }
    }
}
