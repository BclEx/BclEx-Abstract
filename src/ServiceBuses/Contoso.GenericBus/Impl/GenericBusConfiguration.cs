using Contoso.GenericBus.Config;
using System.Configuration;

namespace Contoso.GenericBus.Impl
{
    /// <summary>
    /// GenericBusConfiguration
    /// </summary>
    public class GenericBusConfiguration : AbstractGenericBusConfiguration
    {
        /// <summary>
        /// Reads the bus configuration.
        /// </summary>
        protected override void ReadBusConfiguration()
        {
            base.ReadBusConfiguration();
        }

        /// <summary>
        /// Applies the configuration.
        /// </summary>
        protected override void ApplyConfiguration()
        {
            var assemblies = ConfigurationSection.Assemblies;
            if (assemblies != null)
                foreach (AssemblyElement assembly in assemblies)
                    ScanAssemblies.Add(assembly.Assembly);
        }

        /// <summary>
        /// Configures this instance.
        /// </summary>
        public override void Configure()
        {
            base.Configure();
            Builder.RegisterBus();
        }
    }
}
