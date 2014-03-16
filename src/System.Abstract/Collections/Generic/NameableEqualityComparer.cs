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
namespace System.Collections.Generic
{
    /// <summary>
    /// NameableEqualityComparer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class NameableEqualityComparer<T> : EqualityComparer<Nameable<T>>
        where T : IEquatable<T>
    {
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var comparer = (obj as NameableEqualityComparer<T>);
            return (comparer != null);
        }

        /// <summary>
        /// Equalses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public override bool Equals(Nameable<T> x, Nameable<T> y)
        {
            return x.Value.Equals(y.Value);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetType().Name.GetHashCode();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode(Nameable<T> obj)
        {
            return obj.GetHashCode();
        }

        //internal int IndexOf(Nameable<T>[] array, Nameable<T> value, int startIndex, int count)
        //{
        //    var num = startIndex + count;
        //    for (var j = startIndex; j < num; j++)
        //        if (array[j].Value.Equals(value.Value))
        //            return j;
        //    return -1;
        //}

        //internal int LastIndexOf(Nameable<T>[] array, Nameable<T> value, int startIndex, int count)
        //{
        //    var num = (startIndex - count) + 1;
        //    for (var j = startIndex; j >= num; j--)
        //        if (array[j].Value.Equals(value.Value))
        //            return j;
        //    return -1;
        //}
    }
}
