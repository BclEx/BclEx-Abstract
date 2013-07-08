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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Security.Permissions;
namespace System
{
    /// <summary>
    /// NameableConverter
    /// </summary>
    [HostProtection(SecurityAction.LinkDemand, SharedState = true)]
    public class NameableConverter : TypeConverter
    {
        private Type _nameableType;
        private Type _simpleType;
        private TypeConverter _simpleTypeConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="NameableConverter"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <exception cref="System.ArgumentException">NameableConverterBadCtorArg;type</exception>
        public NameableConverter(Type type)
        {
            _nameableType = type;
            _simpleType = Nullable.GetUnderlyingType(type);
            if (_simpleType == null)
                throw new ArgumentException("NameableConverterBadCtorArg", "type");
            _simpleTypeConverter = TypeDescriptor.GetConverter(_simpleType);
        }

        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you want to convert from.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == _simpleType)
                return true;
            if (_simpleTypeConverter != null)
                return _simpleTypeConverter.CanConvertFrom(context, sourceType);
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Returns whether this converter can convert the object to the specified type, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you want to convert to.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == _simpleType)
                return true;
            if (destinationType == typeof(InstanceDescriptor))
                return true;
            if (_simpleTypeConverter != null)
                return _simpleTypeConverter.CanConvertTo(context, destinationType);
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that represents the converted value.
        /// </returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null || value.GetType() == _simpleType)
                return value;
            if (value is string && string.IsNullOrEmpty(value as string))
                return null;
            if (_simpleTypeConverter != null)
                return _simpleTypeConverter.ConvertFrom(context, culture, value);
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If null is passed, the current culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <param name="destinationType">The <see cref="T:System.Type" /> to convert the <paramref name="value" /> parameter to.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that represents the converted value.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">destinationType</exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException("destinationType");
            if (destinationType == _simpleType && _nameableType.IsInstanceOfType(value))
                return value;
            if (destinationType == typeof(InstanceDescriptor))
                return new InstanceDescriptor(_nameableType.GetConstructor(new Type[] { _simpleType }), new object[] { value }, true);
            if (value == null)
            {
                if (destinationType == typeof(string))
                    return string.Empty;
            }
            else if (_simpleTypeConverter != null)
                return _simpleTypeConverter.ConvertTo(context, culture, value, destinationType);
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Creates an instance of the type that this <see cref="T:System.ComponentModel.TypeConverter" /> is associated with, using the specified context, given a set of property values for the object.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="propertyValues">An <see cref="T:System.Collections.IDictionary" /> of new property values.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> representing the given <see cref="T:System.Collections.IDictionary" />, or null if the object cannot be created. This method always returns null.
        /// </returns>
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (_simpleTypeConverter != null)
                return _simpleTypeConverter.CreateInstance(context, propertyValues);
            return base.CreateInstance(context, propertyValues);
        }

        /// <summary>
        /// Returns whether changing a value on this object requires a call to <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)" /> to create a new value, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <returns>
        /// true if changing a property on this object requires a call to <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)" /> to create a new value; otherwise, false.
        /// </returns>
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            if (_simpleTypeConverter != null)
                return _simpleTypeConverter.GetCreateInstanceSupported(context);
            return base.GetCreateInstanceSupported(context);
        }

        /// <summary>
        /// Returns a collection of properties for the type of array specified by the value parameter, using the specified context and attributes.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="value">An <see cref="T:System.Object" /> that specifies the type of array for which to get properties.</param>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that is used as a filter.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> with the properties that are exposed for this data type, or null if there are no properties.
        /// </returns>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            if (_simpleTypeConverter != null)
            {
                object obj2 = value;
                return _simpleTypeConverter.GetProperties(context, obj2, attributes);
            }
            return base.GetProperties(context, value, attributes);
        }

        /// <summary>
        /// Returns whether this object supports properties, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <returns>
        /// true if <see cref="M:System.ComponentModel.TypeConverter.GetProperties(System.Object)" /> should be called to find the properties of this object; otherwise, false.
        /// </returns>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            if (_simpleTypeConverter != null)
                return _simpleTypeConverter.GetPropertiesSupported(context);
            return base.GetPropertiesSupported(context);
        }

        /// <summary>
        /// Returns a collection of standard values for the data type this type converter is designed for when provided with a format context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context that can be used to extract additional information about the environment from which this converter is invoked. This parameter or properties of this parameter can be null.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> that holds a standard set of valid values, or null if the data type does not support a standard set of values.
        /// </returns>
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (_simpleTypeConverter != null)
            {
                var standardValues = _simpleTypeConverter.GetStandardValues(context);
                if (GetStandardValuesSupported(context) && standardValues != null)
                {
                    var values = new object[standardValues.Count + 1];
                    int i = 0;
                    values[i++] = null;
                    foreach (object obj2 in standardValues)
                        values[i++] = obj2;
                    return new TypeConverter.StandardValuesCollection(values);
                }
            }
            return base.GetStandardValues(context);
        }

        /// <summary>
        /// Returns whether the collection of standard values returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues" /> is an exclusive list of possible values, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <returns>
        /// true if the <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues" /> is an exhaustive list of possible values; false if other values are possible.
        /// </returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            if (_simpleTypeConverter != null)
                return _simpleTypeConverter.GetStandardValuesExclusive(context);
            return base.GetStandardValuesExclusive(context);
        }

        /// <summary>
        /// Returns whether this object supports a standard set of values that can be picked from a list, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <returns>
        /// true if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues" /> should be called to find a common set of values the object supports; otherwise, false.
        /// </returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            if (_simpleTypeConverter != null)
                return _simpleTypeConverter.GetStandardValuesSupported(context);
            return base.GetStandardValuesSupported(context);
        }

        /// <summary>
        /// Returns whether the given value object is valid for this type and for the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to test for validity.</param>
        /// <returns>
        /// true if the specified value is valid for this object; otherwise, false.
        /// </returns>
        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            if (_simpleTypeConverter == null)
                return base.IsValid(context, value);
            object obj2 = value;
            return (obj2 == null || _simpleTypeConverter.IsValid(context, obj2));
        }

        /// <summary>
        /// Gets the type of the nameable.
        /// </summary>
        /// <value>
        /// The type of the nameable.
        /// </value>
        public Type NameableType
        {
            get { return _nameableType; }
        }

        /// <summary>
        /// Gets the type of the underlying.
        /// </summary>
        /// <value>
        /// The type of the underlying.
        /// </value>
        public Type UnderlyingType
        {
            get { return _simpleType; }
        }

        /// <summary>
        /// Gets the underlying type converter.
        /// </summary>
        /// <value>
        /// The underlying type converter.
        /// </value>
        public TypeConverter UnderlyingTypeConverter
        {
            get { return _simpleTypeConverter; }
        }
    }
}
