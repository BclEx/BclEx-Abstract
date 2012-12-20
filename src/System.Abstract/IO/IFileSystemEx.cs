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
namespace System.IO
{
    /// <summary>
    /// IFileSystemEx
    /// </summary>
    public interface IFileSystemEx
    {
        /// <summary>
        /// Adds the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="stream">The stream.</param>
        void AddFile(string path, Stream stream);
        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        Stream CreateFile(string path);
        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        void DeleteDirectory(string path, bool recursive);
        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        void DeleteFile(string path);
        /// <summary>
        /// Directories the exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        bool DirectoryExists(string path);
        /// <summary>
        /// Files the exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        bool FileExists(string path);
        /// <summary>
        /// Gets the created.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        DateTimeOffset GetCreationTimeUtc(string path);
        /// <summary>
        /// Gets the directories.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        IEnumerable<string> GetDirectories(string path);
        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns></returns>
        IEnumerable<string> GetFiles(string path, string filter, bool recursive);
        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        string GetFullPath(string path);
        /// <summary>
        /// Gets the last accessed.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        DateTimeOffset GetLastAccessTimeUtc(string path);
        /// <summary>
        /// Gets the last modified.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        DateTimeOffset GetLastWriteTimeUtc(string path);
        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        Stream OpenRead(string path);
        /// <summary>
        /// Gets or sets the log.
        /// </summary>
        /// <value>
        /// The log.
        /// </value>
        IServiceLog Log { get; set; }
        /// <summary>
        /// Gets the root.
        /// </summary>
        string Root { get; }
    }
}

