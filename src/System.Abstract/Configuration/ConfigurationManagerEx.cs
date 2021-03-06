#region License
/*
The MIT License

Copyright (c) 2008 Sky Morey

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion
using System.Collections.Specialized;
namespace System.Configuration
{
    /// <summary>
    /// ConfigurationManagerEx
    /// </summary>
    public static class ConfigurationManagerEx
    {
        // Summary:
        //     Gets the System.Configuration.AppSettingsSection data for the current application's
        //     default configuration.
        //
        // Returns:
        //     Returns a System.Collections.Specialized.NameValueCollection object that
        //     contains the contents of the System.Configuration.AppSettingsSection object
        //     for the current application's default configuration.
        //
        // Exceptions:
        //   System.Configuration.ConfigurationErrorsException:
        //     Could not retrieve a System.Collections.Specialized.NameValueCollection object
        //     with the application settings data.
        /// <summary>
        /// Gets the app settings.
        /// </summary>
        public static NameValueCollection AppSettings
        {
            get { return ConfigurationManager.AppSettings; }
        }

        // Summary:
        //     Gets the System.Configuration.ConnectionStringsSection data for the current
        //     application's default configuration.
        //
        // Returns:
        //     Returns a System.Configuration.ConnectionStringSettingsCollection object
        //     that contains the contents of the System.Configuration.ConnectionStringsSection
        //     object for the current application's default configuration.
        //
        // Exceptions:
        //   System.Configuration.ConfigurationErrorsException:
        //     Could not retrieve a System.Configuration.ConnectionStringSettingsCollection
        //     object.
        /// <summary>
        /// Gets the connection strings.
        /// </summary>
        public static ConnectionStringSettingsCollection ConnectionStrings
        {
            get { return ConfigurationManager.ConnectionStrings; }
        }

        // Summary:
        //     Retrieves a specified configuration section for the current application's
        //     default configuration.
        //
        // Parameters:
        //   sectionName:
        //     The configuration section path and name.
        //
        // Returns:
        //     The specified System.Configuration.ConfigurationSection object, or null if
        //     the section does not exist.
        //
        // Exceptions:
        //   System.Configuration.ConfigurationErrorsException:
        //     A configuration file could not be loaded.
        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static T GetSection<T>(string sectionName)
            where T : class
        {
            var section = (T)ConfigurationManager.GetSection(sectionName);
            if (section == null)
                throw new InvalidOperationException(string.Format(Local.UndefinedItemAB, "Configuration::Section", sectionName));
            return section;
        }

        /// <summary>
        /// Gets the section.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="throwIfMissing">if set to <c>true</c> [throw if missing].</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static T GetSection<T>(string sectionName, bool throwIfMissing)
            where T : class, new()
        {
            var section = (T)ConfigurationManager.GetSection(sectionName);
            if (throwIfMissing)
            {
                if (section == null)
                    throw new InvalidOperationException(string.Format(Local.UndefinedItemAB, "Configuration::Section", sectionName));
                return section;
            }
            return (section != null ? section : new T());
        }

        #region Codec

        /// <summary>
        /// Encodes the specified tag.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static T Encode<T, TSource>(TSource value, object tag = null)
        {
            var codec = GetCodec<T, TSource>();
            if (codec == null)
                throw new InvalidOperationException(string.Format("No registration found for {0}. Please register one.", typeof(T).Name));
            return codec.Encode(value, tag);
        }

        /// <summary>
        /// Decodes the specified tag.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static T Decode<T, TSource>(TSource value, object tag = null)
        {
            var codec = GetCodec<T, TSource>();
            if (codec == null)
                throw new InvalidOperationException(string.Format("No registration found for {0}. Please register one.", typeof(T).Name));
            return codec.Decode(value, tag);
        }

        /// <summary>
        /// Gets the codec.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <returns></returns>
        public static ICodec<T, TSource> GetCodec<T, TSource>() { return Registration<T, TSource>.Codec; }

        /// <summary>
        /// Sets the codec.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="codec">The codec.</param>
        public static void SetCodec<T, TSource>(ICodec<T, TSource> codec) { Registration<T, TSource>.Codec = codec; }

        internal static class Registration<T, TSource>
        {
            public static ICodec<T, TSource> Codec { get; set; }
        }

        #endregion
    }
}
