using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;

namespace System.Resources
{
    /// <summary>
    /// ResourceManagerEx
    /// </summary>
    public static class ResourceManagerEx
    {
        private static readonly Dictionary<Type, ResourceManager> _cachedResourceManagers = new Dictionary<Type, ResourceManager>();

        /// <summary>
        /// Gets the string or default.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="names">The names.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string GetStringOrDefault(Type resourceType, string names, string defaultValue) { return (resourceType != null && !string.IsNullOrEmpty(names) ? GetString(resourceType, names) : defaultValue); }
        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="names">The names.</param>
        /// <returns></returns>
        public static string GetString(Type resourceType, string names)
        {
            if (string.IsNullOrEmpty(names))
                throw new ArgumentNullException("resourceNames");
            if (resourceType == null)
                throw new ArgumentNullException("resourceType");
            ResourceManager resourceManager;
            if (!_cachedResourceManagers.TryGetValue(resourceType, out resourceManager))
            {
                var property = resourceType.GetProperty("ResourceManager", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
                if (property == null || property.GetGetMethod(true) != null)
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Local.ResourceTypeDoesNotHaveProperty, resourceType, "ResourceManager"));
                if (property.PropertyType != typeof(ResourceManager))
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Local.ResourcePropertyIncorrectType, names, resourceType));
                resourceManager = (ResourceManager)property.GetGetMethod(true).Invoke(null, null);
            }
            var b = new StringBuilder();
            var culture = Thread.CurrentThread.CurrentUICulture.ThreeLetterISOLanguageName;
            foreach (string resourceName in names.Split(';'))
            {
                var value = (resourceManager.GetString(resourceName + '_' + culture) ?? resourceManager.GetString(resourceName));
                if (string.IsNullOrEmpty(value))
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Local.ResourceTypeDoesNotHaveProperty, resourceType, resourceName));
                if (b.Length > 0)
                    b.AppendLine();
                b.Append(value);
            }
            return b.ToString();
        }
    }
}

