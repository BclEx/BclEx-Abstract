using System.Configuration;

namespace Contoso.GenericBus.Config
{
    /// <summary>
    /// AssemblyElementCollection
    /// </summary>
    [ConfigurationCollection(typeof(AssemblyElement), CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class AssemblyElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new AssemblyElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement" /> to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            var assemblyElement = (AssemblyElement)element;
            return assemblyElement.Name;
        }

        /// <summary>
        /// Adds the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void Add(AssemblyElement assembly)
        {
            BaseAdd(assembly);
        }
    }
}