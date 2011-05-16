//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace CrcStudio.Zip
{
    public class ZipFile : IDisposable
    {
        public const uint CentralEndRecordSignature = 0x06054b50;

        private readonly string _archiveFile;
        protected readonly List<ZipEntry> _entries = new List<ZipEntry>();
        private Stream _archiveStream;
        private uint _centralDirectoryOffset;
        private uint _centralDirectorySize;
        private bool _disposed;
        private byte[] _fileComment = new byte[0];
        protected bool _isChanged;

        protected bool _isDirty;
        protected bool _isNew;
        private ushort _numberOfThisDisk;
        private ushort _numberOfThisDiskFromCentralDir;
        private ushort _totalEntriesInCentralDir;
        private ushort _totalEntriesInCentralDirOnThisDisk;
        private int _useDataDescriptor = -1;
        private int _useDataDescriptorSignature = -1;
        private int _useUtf8Encoding = -1;

        public ZipFile(string file, FileAccess access = FileAccess.ReadWrite)
        {
            _archiveFile = file;
            var fileInfo = new FileInfo(file);
            if (fileInfo.Exists && fileInfo.Length < 22) throw new IOException("File is to small to be a zip archive");
            _isNew = !fileInfo.Exists;
            if (access == FileAccess.Read)
            {
                _archiveStream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            else
            {
                _archiveStream = File.Open(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            }
            if (!_isNew)
            {
                FindCentralEndRecord();
                ReadCentralDirectory();
                AllowReplacingEntries = true;
            }
        }

        public Stream BaseStream { get { return _archiveStream; } }
        public IEnumerable<ZipEntry> Entries { get { return _entries; } }
        private bool _allowReplacingEntries;
        public bool AllowReplacingEntries 
        { 
            get { return _allowReplacingEntries; } 
            set
            { 
                if (_allowReplacingEntries == value) return;
                if (_isNew && _entries.Count > 0) throw new Exception("Allow overwriting can not be changed after entries has been added");
                _allowReplacingEntries = value; 
            } 
        }

        public bool UseDataDescriptorSignature
        {
            get
            {
                if (_useDataDescriptorSignature == -1 && _entries.Count > 0)
                {
                    return _entries[0].UseDataDescriptorSignature;
                }
                return _useDataDescriptorSignature == 1;
            }
            set { _useDataDescriptorSignature = value ? 1 : 0; }
        }

        public bool UseUtf8Encoding
        {
            get
            {
                if (_useUtf8Encoding == -1)
                {
                    if (_entries.Count > 0)
                    {
                        return _entries[0].UseUtf8Encoding;
                    }
                    return true;
                }
                return _useUtf8Encoding == 1;
            }
            set { _useUtf8Encoding = value ? 1 : 0; }
        }

        public bool UseDataDescriptor
        {
            get
            {
                if (_useDataDescriptor == -1 && _entries.Count > 0)
                {
                    return _entries[0].UseDataDescriptor;
                }
                return _useDataDescriptor == 1;
            }
            set { _useDataDescriptor = value ? 1 : 0; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion

        public ZipEntry Find(string entryName)
        {
            string name = ZipEntry.NormalizeName(entryName);
            return _entries.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public ZipEntry Add(string fileName, CompressionType compressionType)
        {
            string entryName = Path.GetFileName(fileName);
            if (entryName == null)
                throw new ArgumentException(string.Format("{0} is not a file", fileName), "fileName");
            return Add(fileName, entryName, compressionType);
        }

        public ZipEntry Add(string fileName, string entryName, CompressionType compressionType)
        {
            string name = ZipEntry.NormalizeName(entryName);
            ZipEntry zipEntry = Find(name);
            if (zipEntry != null)
            {
                if (!AllowReplacingEntries) throw new Exception(string.Format("Entry {0} already exists.", entryName));
                zipEntry.SetNewFile(fileName, compressionType);
                _isChanged = true;
            }
            else
            {
                zipEntry = new ZipEntry(fileName, name, compressionType);
                zipEntry.UseUtf8Encoding = UseUtf8Encoding;
                zipEntry.UseDataDescriptor = UseDataDescriptor;
                zipEntry.UseDataDescriptorSignature = UseDataDescriptorSignature;
                _entries.Add(zipEntry);
                if (_isNew && !AllowReplacingEntries) zipEntry.WriteEntry(_archiveStream);
            }
            _isDirty = true;
            return zipEntry;
        }

        public ZipEntry Add(Stream fileStream, string entryName, CompressionType compressionType)
        {
            string name = ZipEntry.NormalizeName(entryName);
            ZipEntry zipEntry = Find(name);
            if (zipEntry != null)
            {
                if (!AllowReplacingEntries) throw new Exception(string.Format("Entry {0} already exists.", entryName));
                zipEntry.SetNewStream(fileStream, compressionType);
                _isChanged = true;
            }
            else
            {
                zipEntry = new ZipEntry(fileStream, name, compressionType);
                zipEntry.UseUtf8Encoding = UseUtf8Encoding;
                zipEntry.UseDataDescriptor = UseDataDescriptor;
                zipEntry.UseDataDescriptorSignature = UseDataDescriptorSignature;
                _entries.Add(zipEntry);
                if (_isNew && !AllowReplacingEntries) zipEntry.WriteEntry(_archiveStream);
            }
            _isDirty = true;
            return zipEntry;
        }

        public void Delete(string entryName)
        {
            if (_isNew) throw new Exception("Can not delete from new archive");
            ZipEntry zipEntry = Find(ZipEntry.NormalizeName(entryName));
            if (zipEntry != null)
            {
                _entries.Remove(zipEntry);
                _isChanged = true;
                _isDirty = true;
            }
        }

        public static bool Contains(string archive, string entryName)
        {
            using (var zf = new ZipFile(archive, FileAccess.Read))
            {
                return (zf.Find(ZipEntry.NormalizeName(entryName)) != null);
            }
        }

        public static void ExtractAll(string archive, string folder, bool overwrite)
        {
            using (var zf = new ZipFile(archive, FileAccess.Read))
            {
                zf.ExtractAll(folder, overwrite);
            }
        }

        public void ExtractAll(string folder, bool overwrite)
        {
            Extract(_entries, folder, overwrite);
        }

        public void Extract(IEnumerable<ZipEntry> entries, string folder, bool overwrite)
        {
            if (Directory.Exists(folder) && !overwrite)
                throw new IOException(string.Format("Folder {0} already exists", folder));
            foreach (ZipEntry entry in entries)
            {
                string file = Path.Combine(folder, entry.Name.Replace('/', Path.DirectorySeparatorChar));
                Extract(entry, file, overwrite);
            }
        }

        public void Extract(ZipEntry entry, string path, bool overwrite)
        {
            if (entry.Name.EndsWith("/"))
            {
                Directory.CreateDirectory(path);
                return;
            }
            if (Directory.Exists(path) && !overwrite)
                throw new IOException(string.Format("File {0} already exists", path));
            string directoryName = Path.GetDirectoryName(path);
            if (directoryName != null) Directory.CreateDirectory(directoryName);
            using (FileStream stream = File.Create(path))
            {
                entry.Extract(stream);
            }
            File.SetCreationTime(path, entry.LastModified);
            File.SetLastWriteTime(path, entry.LastModified);
        }

        private void FindCentralEndRecord()
        {
            _archiveStream.Seek(-17, SeekOrigin.End);
            uint signature = 0;
            while (_archiveStream.Position > 5 && signature != CentralEndRecordSignature)
            {
                _archiveStream.Seek(-5, SeekOrigin.Current);
                signature = _archiveStream.ReadUInt();
            }
            ReadCentralEndRecord();
        }

        private void ReadCentralEndRecord()
        {
            // end of central dir signature    4 bytes  (0x06054b50)
            // number of this disk             2 bytes
            // number of the disk with the
            // start of the central directory  2 bytes
            // total number of entries in the
            // central directory on this disk  2 bytes
            // total number of entries in
            // the central directory           2 bytes
            // size of the central directory   4 bytes
            // offset of start of central
            // directory with respect to
            // the starting disk number        4 bytes
            // .ZIP file comment length        2 bytes
            // .ZIP file comment       (variable size)

            _numberOfThisDisk = _archiveStream.ReadUShort();
            _numberOfThisDiskFromCentralDir = _archiveStream.ReadUShort();
            _totalEntriesInCentralDirOnThisDisk = _archiveStream.ReadUShort();
            _totalEntriesInCentralDir = _archiveStream.ReadUShort();
            _centralDirectorySize = _archiveStream.ReadUInt();
            _centralDirectoryOffset = _archiveStream.ReadUInt();
            ushort fileCommentLength = _archiveStream.ReadUShort();

            _fileComment = new byte[fileCommentLength];
            if (fileCommentLength > 0) _archiveStream.Read(_fileComment, 0, fileCommentLength);
        }

        private void ReadCentralDirectory()
        {
            _archiveStream.Seek(_centralDirectoryOffset, SeekOrigin.Begin);
            while (_archiveStream.ReadUInt() == ZipEntry.CentralHeaderSignature)
            {
                _entries.Add(new ZipEntry(_archiveStream));
            }
        }

        private void WriteCentralEndRecord(Stream stream)
        {
            stream.WriteUInt(CentralEndRecordSignature);
            stream.WriteUShort(_numberOfThisDisk);
            stream.WriteUShort(_numberOfThisDiskFromCentralDir);
            stream.WriteUShort(_totalEntriesInCentralDirOnThisDisk);
            stream.WriteUShort(_totalEntriesInCentralDir);
            stream.WriteUInt(_centralDirectorySize);
            stream.WriteUInt(_centralDirectoryOffset);
            stream.WriteUShort((ushort) _fileComment.Length);
            stream.Write(_fileComment, 0, _fileComment.Length);
            stream.Flush();
        }

        public void WriteToFile(string file)
        {
            using (FileStream stream = File.Open(file, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                foreach (ZipEntry entry in _entries)
                {
                    entry.WriteEntry(stream);
                }
                WriteCentralDirectory(stream);
                WriteCentralEndRecord(stream);
            }
        }

        private void WriteCentralDirectory(Stream stream)
        {
            _centralDirectoryOffset = (uint) stream.Position;
            foreach (ZipEntry entry in _entries)
            {
                entry.WriteCentralRecord(stream);
            }
            _centralDirectorySize = (uint) (stream.Position - _centralDirectoryOffset);
            _totalEntriesInCentralDir = (ushort) _entries.Count;
            _totalEntriesInCentralDirOnThisDisk = _totalEntriesInCentralDir;
        }

        public void Flush()
        {
            InternalFlush(false);
        }

        protected virtual void InternalFlush(bool closeStream)
        {
            if (_isNew)
            {
                if (AllowReplacingEntries)
                {
                    _archiveStream.Position = 0;
                    foreach (ZipEntry entry in _entries)
                    {
                        entry.WriteEntry(_archiveStream);
                    }
                }
                WriteCentralDirectory(_archiveStream);
                WriteCentralEndRecord(_archiveStream);
                if (closeStream)
                {
                    _archiveStream.Close();
                    _archiveStream = null;
                }
                else
                {
                    _archiveStream.Position = 0;
                }
            }
            else if (!_isChanged)
            {
                _archiveStream.Seek(_centralDirectoryOffset, SeekOrigin.Begin);
                foreach (ZipEntry entry in _entries.Where(x => x.IsDirty))
                {
                    entry.WriteEntry(_archiveStream);
                }
                WriteCentralDirectory(_archiveStream);
                WriteCentralEndRecord(_archiveStream);
                if (closeStream)
                {
                    _archiveStream.Close();
                    _archiveStream = null;
                }
                else
                {
                    _archiveStream.Position = 0;
                }
            }
            else
            {
                string tempFile = Path.Combine(Path.GetDirectoryName(_archiveFile) ?? Path.GetTempPath(),
                                               Guid.NewGuid() + ".zipfile");
                WriteToFile(tempFile);
                _archiveStream.Close();
                _archiveStream = null;
                try
                {
                    DeleteFile(_archiveFile);
                }
                catch (IOException)
                {
                    DeleteFile(tempFile);
                    throw new Exception("Could not update zip file");
                }
                File.Move(tempFile, _archiveFile);
                if (!closeStream)
                {
                    _archiveStream = File.Open(_archiveFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                }
            }
            _isNew = false;
            _isChanged = false;
            _isDirty = false;
        }

        public static void DeleteFile(string file)
        {
            if (file == null) throw new ArgumentNullException("file");
            int attempts = 0;
            while (File.Exists(file) && attempts < 10)
            {
                try
                {
                    attempts++;
                    if (File.Exists(file)) File.Delete(file);
                }
                catch (IOException)
                {
                    Thread.Sleep(0);
                }
            }
        }

        public void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // get rid of managed resources
            }
            if (_disposed) return;
            if (_archiveStream == null) return;
            if (_isDirty)
            {
                InternalFlush(true);
            }
            else
            {
                _archiveStream.Close();
                _archiveStream = null;
            }

            _disposed = true;
        }


        ~ZipFile()
        {
            Dispose(false);
        }
    }
}