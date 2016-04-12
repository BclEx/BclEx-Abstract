using System.Configuration;

namespace Contoso.GenericBus.Config
{
    /// <summary>
    /// BusConfigurationSection
    /// </summary>
    public class BusConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusConfigurationSection"/> class.
        /// </summary>
        public BusConfigurationSection()
        {
            SetupDefaults();
        }

        /// <summary>
        /// Gets or sets the assemblies.
        /// </summary>
        /// <value>
        /// The assemblies.
        /// </value>
        [ConfigurationProperty("assemblies")]
        public AssemblyElementCollection Assemblies
        {
            get { return this["assemblies"] as AssemblyElementCollection; }
            set { this["assemblies"] = value; }
        }

        private void SetupDefaults()
        {
            Properties.Add(new ConfigurationProperty("assemblies", typeof(AssemblyElementCollection), null));
        }
    }
}
