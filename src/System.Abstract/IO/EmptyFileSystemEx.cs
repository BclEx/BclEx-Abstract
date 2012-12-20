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
using System.Abstract;
using System.Collections.Generic;
using System.Linq;
namespace System.IO
{
    /// <summary>
    /// EmptyFileSystemEx
    /// </summary>
    public class EmptyFileSystemEx : IFileSystemEx
    {
        /// <summary>
        /// Adds the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="stream">The stream.</param>
        public void AddFile(string path, Stream stream) { }
        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <param name="path">The path.</param>
        public Stream CreateFile(string path) { return Stream.Null; }
        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        public void DeleteDirectory(string path, bool recursive) { }
        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        public void DeleteFile(string path) { }
        /// <summary>
        /// Directories the exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public bool DirectoryExists(string path) { return false; }
        /// <summary>
        /// Files the exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public bool FileExists(string path) { return false; }
        /// <summary>
        /// Gets the created.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public DateTimeOffset GetCreationTimeUtc(string path) { return DateTimeOffset.MinValue; }
        /// <summary>
        /// Gets the directories.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public IEnumerable<string> GetDirectories(string path) { return Enumerable.Empty<string>(); }
        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns></returns>
        public IEnumerable<string> GetFiles(string path, string filter, bool recursive) { return Enumerable.Empty<string>(); }
        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public string GetFullPath(string path) { return path; }
        /// <summary>
        /// Gets the last accessed.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public DateTimeOffset GetLastAccessTimeUtc(string path) { return DateTimeOffset.MinValue; }
        /// <summary>
        /// Gets the last modified.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public DateTimeOffset GetLastWriteTimeUtc(string path) { return DateTimeOffset.MinValue; }
        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public Stream OpenRead(string path) { return Stream.Null; }
        /// <summary>
        /// Gets or sets the log.
        /// </summary>
        /// <value>
        /// The log.
        /// </value>
        public IServiceLog Log { get; set; }
        /// <summary>
        /// Gets the root.
        /// </summary>
        public string Root
        {
            get { return string.Empty; }
        }
    }
}

