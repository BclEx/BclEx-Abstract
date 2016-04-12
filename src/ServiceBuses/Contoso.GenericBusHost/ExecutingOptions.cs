using CommandLine;
using System;
using System.Text;

namespace Contoso.GenericBusHost
{
    /// <summary>
    /// ExecutingOptions
    /// </summary>
    public class ExecutingOptions
    {
        /// <summary>
        /// The action
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Choose an action", ShortName = "action")]
        public Action Action;
        /// <summary>
        /// The assembly
        /// </summary>
        [Argument(ArgumentType.Required, HelpText = "Assembly to execute", ShortName = "asm")]
        public string Assembly;
        /// <summary>
        /// The configuration file
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Configuration file", ShortName = "config")]
        public string ConfigFile;
        /// <summary>
        /// The name
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Service name", ShortName = "name")]
        public string Name;
        /// <summary>
        /// The account
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "Account")]
        public string Account;
        /// <summary>
        /// The password
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Password for account used when installing service")]
        public string Password;
        /// <summary>
        /// The host
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "Host")]
        public string Host;
        /// <summary>
        /// The boot strapper
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "BootStrapper")]
        public string BootStrapper;
        /// <summary>
        /// The depends on
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "DependsOn")]
        public string DependsOn;

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(" /Action:").Append(Action)
                .Append(" /Name:\"")
                .Append(Name)
                .Append("\"");
            if (!string.IsNullOrEmpty(Host))
                sb.Append(" \"/Host:")
                    .Append(Host)
                    .Append("\"");
            if (!string.IsNullOrEmpty(BootStrapper))
                sb.Append(" \"/BootStrapper:")
                    .Append(BootStrapper)
                    .Append("\"");
            if (!string.IsNullOrEmpty(Assembly))
                sb.Append(" /Assembly:\"")
                    .Append(Assembly)
                    .Append("\"");
            if (!string.IsNullOrEmpty(ConfigFile))
                sb.Append(" /ConfigFile:\"")
                    .Append(ConfigFile)
                    .Append("\"");
            return sb.ToString();
        }

        /// <summary>
        /// Gets the depend on service.
        /// </summary>
        /// <value>
        /// The depend on service.
        /// </value>
        public string[] DependOnService
        {
            get { return (string.IsNullOrEmpty(DependsOn) ? null : DependsOn.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)); }
        }
    }
}
