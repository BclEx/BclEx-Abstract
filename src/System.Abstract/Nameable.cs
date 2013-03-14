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
using System.Collections.Generic;
namespace System
{
    /// <summary>
    /// Nameable
    /// </summary>
    public static class Nameable
    {
        /// <summary>
        /// Compares the specified n1.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n1">The n1.</param>
        /// <param name="n2">The n2.</param>
        /// <returns></returns>
        public static int Compare<T>(Nameable<T> n1, Nameable<T> n2)
        {
            return Comparer<T>.Default.Compare(n1.Value, n2.Value);
        }

        /// <summary>
        /// Equalses the specified n1.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="n1">The n1.</param>
        /// <param name="n2">The n2.</param>
        /// <returns></returns>
        public static bool Equals<T>(Nameable<T> n1, Nameable<T> n2)
        {
            return EqualityComparer<T>.Default.Equals(n1.Value, n2.Value);
        }

        /// <summary>
        /// Gets the type of the underlying.
        /// </summary>
        /// <param name="nameableType">Type of the nameable.</param>
        /// <returns></returns>
        public static Type GetUnderlyingType(Type nameableType)
        {
            if (nameableType == null)
                throw new ArgumentNullException("nameableType");
            return (nameableType.IsGenericType && !nameableType.IsGenericTypeDefinition && object.ReferenceEquals(nameableType.GetGenericTypeDefinition(), typeof(Nameable<>)) ? nameableType.GetGenericArguments()[0] : null);
        }
    }
}
