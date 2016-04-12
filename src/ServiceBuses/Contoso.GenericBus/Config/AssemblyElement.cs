using System.Configuration;
using System.ComponentModel;
using System.Reflection;

namespace Contoso.GenericBus.Config
{
    /// <summary>
    /// AssemblyElement
    /// </summary>
    public class AssemblyElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [ConfigurationProperty("name")]
        public string Name
        {
            get { string name; return (!string.IsNullOrEmpty(name = (string)this["name"]) ? name : (Assembly != null ? Assembly.FullName : null)); }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the assembly.
        /// </summary>
        /// <value>
        /// The assembly.
        /// </value>
        [ConfigurationProperty("assembly", IsRequired = true)]
        [TypeConverter(typeof(AssemblyNameConverter))]
        public Assembly Assembly
        {
            get { return (Assembly)this["assembly"]; }
            set { base["assembly"] = value; }
        }
    }
}