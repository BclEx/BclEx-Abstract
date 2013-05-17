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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Security;
namespace System
{
    /// <summary>
    /// ConsoleEx
    /// </summary>
    public class ConsoleEx : IConsoleEx
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleEx"/> class.
        /// </summary>
        public ConsoleEx()
        {
            ResourceType = typeof(Local);
        }

        /// <summary>
        /// Confirms the specified description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public bool Confirm(string description)
        {
            if (IsNonInteractive)
                return true;
            var currentColor = System.Console.ForegroundColor;
            try
            {
                System.Console.ForegroundColor = ConsoleColor.Yellow;
                System.Console.Write(string.Format(CultureInfo.CurrentCulture, ResourceManagerEx.GetString(ResourceType, "ConsoleConfirmMessage"), description));
                return System.Console.ReadLine().StartsWith(ResourceManagerEx.GetString(ResourceType, "ConsoleConfirmMessageAccept"), StringComparison.OrdinalIgnoreCase);
            }
            finally { System.Console.ForegroundColor = currentColor; }
        }

        private void EnsureInteractive()
        {
            if (IsNonInteractive)
                throw new InvalidOperationException(ResourceManagerEx.GetString(ResourceType, "ConsoleCannotPromptForInput_Error"));
        }

        //public void Log(MessageLevel level, string message, params object[] args)
        //{
        //    switch (level)
        //    {
        //        case MessageLevel.Info: WriteLine(message, args); return;
        //        case MessageLevel.Warning: WriteWarning(message, args); return;
        //        case MessageLevel.Debug: WriteColor(Out, ConsoleColor.Gray, message, args); return;
        //    }
        //}

        /// <summary>
        /// Prints the justified.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="text">The text.</param>
        public void PrintJustified(int startIndex, string text) { PrintJustified(startIndex, text, WindowWidth); }
        /// <summary>
        /// Prints the justified.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="text">The text.</param>
        /// <param name="maxWidth">Width of the max.</param>
        public void PrintJustified(int startIndex, string text, int maxWidth)
        {
            if (maxWidth > startIndex)
                maxWidth = (maxWidth - startIndex) - 1;
            while (text.Length > 0)
            {
                text = text.TrimStart(new char[0]);
                var length = Math.Min(text.Length, maxWidth);
                var content = text.Substring(0, length);
                var totalWidth = startIndex + length - CursorLeft;
                Out.WriteLine(content.PadLeft(totalWidth));
                text = text.Substring(content.Length);
            }
        }

        /// <summary>
        /// Reads the key.
        /// </summary>
        /// <returns></returns>
        public ConsoleKeyInfo ReadKey()
        {
            EnsureInteractive();
            return System.Console.ReadKey(true);
        }

        /// <summary>
        /// Reads the line.
        /// </summary>
        /// <returns></returns>
        public string ReadLine()
        {
            EnsureInteractive();
            return System.Console.ReadLine();
        }

        /// <summary>
        /// Reads the secure string.
        /// </summary>
        /// <param name="secureString">The secure string.</param>
        public void ReadSecureString(SecureString secureString)
        {
            EnsureInteractive();
            try { ReadSecureStringFromConsole(secureString); }
            catch (InvalidOperationException) { foreach (char ch in this.ReadLine()) secureString.AppendChar(ch); }
            secureString.MakeReadOnly();
        }

        private static void ReadSecureStringFromConsole(SecureString secureString)
        {
            for (ConsoleKeyInfo keyInfo = System.Console.ReadKey(true); keyInfo.Key != ConsoleKey.Enter; keyInfo = System.Console.ReadKey(true))
            {
                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (secureString.Length >= 1)
                    {
                        System.Console.SetCursorPosition(System.Console.CursorLeft - 1, System.Console.CursorTop);
                        System.Console.Write(' ');
                        System.Console.SetCursorPosition(System.Console.CursorLeft - 1, System.Console.CursorTop);
                        secureString.RemoveAt(secureString.Length - 1);
                    }
                }
                else
                {
                    secureString.AppendChar(keyInfo.KeyChar);
                    System.Console.Write('*');
                }
            }
            System.Console.WriteLine();
        }

        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Write(object value) { Out.Write(value); }
        /// <summary>
        /// Writes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Write(string value) { Out.Write(value); }
        /// <summary>
        /// Writes the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void Write(string format, params object[] args)
        {
            if (args == null || !args.Any())
                Out.Write(format);
            else
                Out.Write(format, args);
        }
        private static void WriteColor(TextWriter writer, ConsoleColor color, string value, params object[] args)
        {
            var currentColor = System.Console.ForegroundColor;
            try
            {
                System.Console.ForegroundColor = color;
                if (args == null || !args.Any())
                    writer.WriteLine(value);
                else
                    writer.WriteLine(value, args);
            }
            finally { System.Console.ForegroundColor = currentColor; }
        }

        /// <summary>
        /// Writes the error.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteError(object value) { WriteError(value.ToString()); }
        /// <summary>
        /// Writes the error.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteError(string value) { WriteError(value, new object[0]); }
        /// <summary>
        /// Writes the error.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void WriteError(string format, params object[] args) { WriteColor(System.Console.Error, ConsoleColor.Red, format, args); }
        /// <summary>
        /// Writes the line.
        /// </summary>
        public void WriteLine() { Out.WriteLine(); }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteLine(object value) { Out.WriteLine(value); }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteLine(string value) { Out.WriteLine(value); }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void WriteLine(string format, params object[] args) { Out.WriteLine(format, args); }
        /// <summary>
        /// Writes the warning.
        /// </summary>
        /// <param name="value">The value.</param>
        public void WriteWarning(string value) { WriteWarning(true, value, new object[0]); }
        /// <summary>
        /// Writes the warning.
        /// </summary>
        /// <param name="prependWarningText">if set to <c>true</c> [prepend warning text].</param>
        /// <param name="value">The value.</param>
        public void WriteWarning(bool prependWarningText, string value) { WriteWarning(prependWarningText, value, new object[0]); }
        /// <summary>
        /// Writes the warning.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="args">The args.</param>
        public void WriteWarning(string value, params object[] args) { WriteWarning(true, value, args); }
        /// <summary>
        /// Writes the warning.
        /// </summary>
        /// <param name="prependWarningText">if set to <c>true</c> [prepend warning text].</param>
        /// <param name="value">The value.</param>
        /// <param name="args">The args.</param>
        public void WriteWarning(bool prependWarningText, string value, params object[] args)
        {
            var text = (prependWarningText ? string.Format(CultureInfo.CurrentCulture, ResourceManagerEx.GetString(ResourceType, "ConsoleCommandLine_Warning"), value) : value);
            WriteColor(Out, ConsoleColor.Yellow, text, args);
        }

        /// <summary>
        /// Gets or sets the cursor left.
        /// </summary>
        /// <value>
        /// The cursor left.
        /// </value>
        public int CursorLeft
        {
            get
            {
                try { return System.Console.CursorLeft; }
                catch (IOException) { return 0; }
            }
            set { System.Console.CursorLeft = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is non interactive.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is non interactive; otherwise, <c>false</c>.
        /// </value>
        public bool IsNonInteractive { get; set; }

        private TextWriter Out
        {
            get
            {
                if (Verbosity != ConsoleVerbosity.Quiet)
                    return System.Console.Out;
                return TextWriter.Null;
            }
        }

        /// <summary>
        /// Gets the type of the resource.
        /// </summary>
        /// <value>
        /// The type of the resource.
        /// </value>
        public Type ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the verbosity.
        /// </summary>
        /// <value>
        /// The verbosity.
        /// </value>
        public ConsoleVerbosity Verbosity { get; set; }

        /// <summary>
        /// Gets or sets the width of the window.
        /// </summary>
        /// <value>
        /// The width of the window.
        /// </value>
        public int WindowWidth
        {
            get
            {
                try { return System.Console.WindowWidth; }
                catch (IOException) { return 60; }
            }
            set { System.Console.WindowWidth = value; }
        }
    }
}

