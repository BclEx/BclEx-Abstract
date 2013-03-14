﻿#region License
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
namespace System
{
    /// <summary>
    /// Nameable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Nameable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Nameable&lt;T&gt;"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Nameable(T value)
        {
            Value = value;
            Name = null;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Nameable&lt;T&gt;"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        public Nameable(T value, string name)
        {
            Value = value;
            Name = name;
        }

        /// <summary>
        /// Value
        /// </summary>
        public T Value;
        /// <summary>
        /// Name
        /// </summary>
        public string Name;

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) { return Value.Equals(obj); }
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() { return Value.ToString(); }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() { return Value.GetHashCode(); }
        /// <summary>
        /// Performs an implicit conversion from T to <see cref="System.Nameable&lt;T&gt;"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Nameable<T>(T value) { return new Nameable<T>(value); }
        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Nameable&lt;T&gt;"/> to T.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator T(Nameable<T> value) { return value.Value; }
    }
}
