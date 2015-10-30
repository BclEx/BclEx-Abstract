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
using System.ComponentModel;
using System.Globalization;
namespace System.Configuration
{
    /// <summary>
    /// CodecConverter
    /// </summary>
    public class CodecConverter<T, TSource> : ConfigurationConverterBase
    {
        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="ctx">The CTX.</param>
        /// <param name="ci">The ci.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data) { return ConfigurationManagerEx.Decode<T, TSource>((TSource)data, null); }

        /// <summary>
        /// Converts to.
        /// </summary>
        /// <param name="ctx">The CTX.</param>
        /// <param name="ci">The ci.</param>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci, object value, Type type) { return ConfigurationManagerEx.Encode<T, TSource>((TSource)value, null); }
    }
}