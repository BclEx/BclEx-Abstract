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
using System.Security;
namespace System
{
    /// <summary>
    /// IConsoleEx
    /// </summary>
    public interface IConsoleEx
    {
        /// <summary>
        /// Confirms the specified description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        bool Confirm(string description);
        /// <summary>
        /// Prints the justified.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="text">The text.</param>
        void PrintJustified(int startIndex, string text);
        /// <summary>
        /// Prints the justified.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="text">The text.</param>
        /// <param name="maxWidth">Width of the max.</param>
        void PrintJustified(int startIndex, string text, int maxWidth);
        /// <summary>
        /// Reads the key.
        /// </summary>
        /// <returns></returns>
        ConsoleKeyInfo ReadKey();
        /// <summary>
        /// Reads the line.
        /// </summary>
        /// <returns></returns>
        string ReadLine();
        /// <summary>
        /// Reads the secure string.
        /// </summary>
        /// <param name="secureString">The secure string.</param>
        void ReadSecureString(SecureString secureString);
        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        void Write(object value);
        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        void Write(string value);
        /// <summary>
        /// Writes the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        void Write(string format, params object[] args);
        /// <summary>
        /// Writes the error.
        /// </summary>
        /// <param name="value">The value.</param>
        void WriteError(object value);
        /// <summary>
        /// Writes the error.
        /// </summary>
        /// <param name="value">The value.</param>
        void WriteError(string value);
        /// <summary>
        /// Writes the error.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        void WriteError(string format, params object[] args);
        /// <summary>
        /// Writes the line.
        /// </summary>
        void WriteLine();
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="value">The value.</param>
        void WriteLine(object value);
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="value">The value.</param>
        void WriteLine(string value);
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        void WriteLine(string format, params object[] args);
        /// <summary>
        /// Writes the warning.
        /// </summary>
        /// <param name="value">The value.</param>
        void WriteWarning(string value);
        /// <summary>
        /// Writes the warning.
        /// </summary>
        /// <param name="prependWarningText">if set to <c>true</c> [prepend warning text].</param>
        /// <param name="value">The value.</param>
        void WriteWarning(bool prependWarningText, string value);
        /// <summary>
        /// Writes the warning.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="args">The args.</param>
        void WriteWarning(string value, params object[] args);
        /// <summary>
        /// Writes the warning.
        /// </summary>
        /// <param name="prependWarningText">if set to <c>true</c> [prepend warning text].</param>
        /// <param name="value">The value.</param>
        /// <param name="args">The args.</param>
        void WriteWarning(bool prependWarningText, string value, params object[] args);
        /// <summary>
        /// Gets or sets the cursor left.
        /// </summary>
        /// <value>
        /// The cursor left.
        /// </value>
        int CursorLeft { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is non interactive.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is non interactive; otherwise, <c>false</c>.
        /// </value>
        bool IsNonInteractive { get; set; }
        /// <summary>
        /// Gets or sets the verbosity.
        /// </summary>
        /// <value>
        /// The verbosity.
        /// </value>
        ConsoleVerbosity Verbosity { get; set; }
        /// <summary>
        /// Gets or sets the width of the window.
        /// </summary>
        /// <value>
        /// The width of the window.
        /// </value>
        int WindowWidth { get; set; }
    }
}

