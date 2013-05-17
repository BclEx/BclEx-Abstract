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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Abstract;
namespace System.IO
{
    /// <summary>
    /// SystemFileSystemEx
    /// </summary>
    public class SystemFileSystemEx : IFileSystemEx
    {
        private IServiceLog _log;
        private readonly string _root;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemFileSystemEx"/> class.
        /// </summary>
        /// <param name="root">The root.</param>
        public SystemFileSystemEx(string root)
        {
            if (string.IsNullOrEmpty(root))
                throw new ArgumentException("root");
            _root = root;
        }

        /// <summary>
        /// Adds the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="stream">The stream.</param>
        public virtual void AddFile(string path, Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            EnsureDirectory(Path.GetDirectoryName(path));
            using (var s = File.Create(GetFullPath(path)))
                stream.CopyTo(s);
            var directoryName = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directoryName))
                Log.DebugFormat(Local.AddedFileToFolderAB, Path.GetFileName(path), directoryName);
            else
                Log.DebugFormat(Local.AddedFileA, Path.GetFileName(path));
        }

        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public virtual Stream CreateFile(string path)
        {
            path = GetFullPath(path);
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            return File.Create(path);
        }

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        public virtual void DeleteDirectory(string path) { DeleteDirectory(path, false); }
        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        public virtual void DeleteDirectory(string path, bool recursive)
        {
            if (DirectoryExists(path))
            {
                try
                {
                    path = GetFullPath(path);
                    Directory.Delete(path, recursive);
                    // The directory is not guranteed to be gone since there could be other open handles. Wait, up to half a second, until the directory is gone.
                    for (int i = 0; Directory.Exists(path) && i < 5; ++i)
                        System.Threading.Thread.Sleep(100);
                    Log.DebugFormat(Local.RemovedFolderA, path);
                }
                catch (DirectoryNotFoundException) { }
            }
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        public virtual void DeleteFile(string path)
        {
            if (!FileExists(path))
                return;
            try
            {
                path = GetFullPath(path);
                File.Delete(path);
                var directoryPath = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directoryPath))
                    Log.DebugFormat(Local.RemovedFileFromFolderAB, Path.GetFileName(path), directoryPath);
                else
                    Log.DebugFormat(Local.RemovedFileA, Path.GetFileName(path));
            }
            catch (FileNotFoundException) { }
        }

        /// <summary>
        /// Directories the exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public virtual bool DirectoryExists(string path)
        {
            path = GetFullPath(path);
            return Directory.Exists(path);
        }

        /// <summary>
        /// Ensures the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        protected virtual void EnsureDirectory(string path)
        {
            path = GetFullPath(path);
            Directory.CreateDirectory(path);
        }

        private static string EnsureTrailingSlash(string path)
        {
            if (!path.EndsWith(@"\", StringComparison.Ordinal))
                path = path + @"\";
            return path;
        }

        /// <summary>
        /// Files the exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public virtual bool FileExists(string path)
        {
            path = GetFullPath(path);
            return File.Exists(path);
        }

        /// <summary>
        /// Gets the creation time UTC.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public DateTimeOffset GetCreationTimeUtc(string path)
        {
            path = GetFullPath(path);
            return (File.Exists(path) ? File.GetCreationTimeUtc(path) : Directory.GetCreationTimeUtc(path));
        }

        /// <summary>
        /// Gets the directories.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public virtual IEnumerable<string> GetDirectories(string path)
        {
            try
            {
                path = EnsureTrailingSlash(GetFullPath(path));
                if (!Directory.Exists(path))
                    return Enumerable.Empty<string>();
#if !CLR4
                return DirectoryEx.EnumerateDirectories(path).Select<string, string>(MakeRelativePath);
#else
                return Directory.EnumerateDirectories(path).Select<string, string>(MakeRelativePath);
#endif
            }
            catch (UnauthorizedAccessException) { }
            catch (DirectoryNotFoundException) { }
            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns></returns>
        public virtual IEnumerable<string> GetFiles(string path, bool recursive) { return GetFiles(path, null, recursive); }
        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns></returns>
        public virtual IEnumerable<string> GetFiles(string path, string filter, bool recursive)
        {
            path = EnsureTrailingSlash(GetFullPath(path));
            if (string.IsNullOrEmpty(filter))
                filter = "*.*";
            try
            {
                if (!Directory.Exists(path))
                    return Enumerable.Empty<string>();
#if !CLR4
                return DirectoryEx.EnumerateFiles(path, filter, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Select(MakeRelativePath);
#else
                return Directory.EnumerateFiles(path, filter, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Select(MakeRelativePath);
#endif
            }
            catch (UnauthorizedAccessException) { }
            catch (DirectoryNotFoundException) { }
            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public virtual string GetFullPath(string path) { return (string.IsNullOrEmpty(path) ? Root : Path.Combine(Root, path)); }

        /// <summary>
        /// Gets the last access time UTC.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public DateTimeOffset GetLastAccessTimeUtc(string path)
        {
            path = GetFullPath(path);
            return (File.Exists(path) ? File.GetLastAccessTimeUtc(path) : Directory.GetLastAccessTimeUtc(path));
        }

        /// <summary>
        /// Gets the last write time UTC.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public virtual DateTimeOffset GetLastWriteTimeUtc(string path)
        {
            path = GetFullPath(path);
            return (File.Exists(path) ? File.GetLastWriteTimeUtc(path) : Directory.GetLastWriteTimeUtc(path));
        }

        /// <summary>
        /// Makes the relative path.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <returns></returns>
        protected string MakeRelativePath(string fullPath) { return fullPath.Substring(Root.Length).TrimStart(Path.DirectorySeparatorChar); }

        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public virtual Stream OpenRead(string path)
        {
            path = GetFullPath(path);
            return File.OpenRead(path);
        }

        /// <summary>
        /// Gets or sets the log.
        /// </summary>
        /// <value>
        /// The log.
        /// </value>
        public IServiceLog Log
        {
            get { return (_log == null ? ServiceLogManager.Empty : _log); }
            set { _log = value; }
        }

        /// <summary>
        /// Gets the root.
        /// </summary>
        public string Root
        {
            get { return _root; }
        }
    }
}

