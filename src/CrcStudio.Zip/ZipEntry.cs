//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace CrcStudio.Zip
{
    public class ZipEntry
    {
        private const int WriteBufferSize = 4096;
        public const uint CentralHeaderSignature = 0x02014b50;
        public const uint LocalHeaderSignature = 0x04034b50;
        public const uint DataDescriptorSignature = 0x08074b50;
        private string _comment = "";
        private uint _compressedSize;
        private ushort _compressionMethod;
        private uint _crc32;

        private long _dataOffset;
        private ushort _diskNumberStart;
        private uint _externalAttributes;
        private byte[] _extraField = new byte[0];
        private string _fileName;
        private Stream _fileStream;
        private ushort _generalPurposeBitFlag;
        private ushort _internalAttributes;
        private DateTime _lastModified = DateTime.Now;

        private uint _localCompressedSize;
        private ushort _localCompressionMethod;
        private uint _localCrc32;
        private byte[] _localExtraField = new byte[0];
        private ushort _localGeneralPurposeBitFlag;
        private uint _localHeaderOffset;
        private bool _localHeaderRead;
        private DateTime _localLastModified = DateTime.MinValue;
        private string _localName = "";
        private uint _localUncompressedSize;
        private ushort _localVersionNeededToExtract;
        private string _name;
        private Stream _originalStream;
        private uint _uncompressedSize;
        private ushort _versionMadeBy = 10;
        private ushort _versionNeededToExtract = 10;

        internal ZipEntry(string fileName, string entryName, CompressionType compressionType)
            : this(entryName, compressionType)
        {
            _fileName = fileName;
            var fi = new FileInfo(fileName);
            if (!fi.Exists) throw new FileNotFoundException("File not found", fileName);
            _lastModified = fi.LastWriteTime;
            IsDirty = true;
        }

        internal ZipEntry(Stream fileStream, string entryName, CompressionType compressionType)
            : this(entryName, compressionType)
        {
            if (!fileStream.CanRead) throw new IOException("Can not read from file stream");
            _fileStream = fileStream;
            IsDirty = true;
        }

        private ZipEntry(string entryName, CompressionType compressionType)
        {
            _name = NormalizeName(entryName);
            _compressionMethod = (ushort) compressionType;
            if (compressionType == CompressionType.Deflate)
            {
                _versionMadeBy = 20;
                _versionNeededToExtract = 20;
            }
            IsDirty = true;
        }

        internal ZipEntry(Stream stream)
        {
            _originalStream = stream;
            ReadCentralHeader();
        }

        public byte[] LocalExtraField { get { return _localExtraField; } }

        public string Comment { get { return _comment; } set { _comment = value; } }

        public byte[] ExtraField { get { return (_localExtraField.Length > 0 && _extraField.Length == 0) ? _localExtraField : _extraField; } set { _extraField = value; } }

        public string Name { get { return _name; } }

        public uint LocalHeaderOffset { get { return _localHeaderOffset; } }

        public uint ExternalAttributes { get { return _externalAttributes; } }

        public ushort InternalAttributes { get { return _internalAttributes; } }

        public ushort DiskNumberStart { get { return _diskNumberStart; } }

        public uint UncompressedSize { get { return _uncompressedSize; } }

        public uint CompressedSize { get { return _compressedSize; } }

        public uint Crc32 { get { return _crc32; } }

        public DateTime LastModified { get { return _lastModified; } set { _lastModified = value; } }

        public ushort CompressionMethod { get { return _compressionMethod; } }

        public ushort GeneralPurposeBitFlag { get { return _generalPurposeBitFlag; } }

        public ushort VersionNeededToExtract { get { return _versionNeededToExtract; } }

        public ushort VersionMadeBy { get { return _versionMadeBy; } }

        public bool UseUtf8Encoding
        {
            get { return (GeneralPurposeBitFlag & GeneralPurposeType.UseUtf8Encoding) == GeneralPurposeType.UseUtf8Encoding; }
            set
            {
                if (value)
                {
                    _generalPurposeBitFlag = (ushort) (_generalPurposeBitFlag | GeneralPurposeType.UseUtf8Encoding);
                }
                else
                {
                    _generalPurposeBitFlag = (ushort) (_generalPurposeBitFlag & ~GeneralPurposeType.UseUtf8Encoding);
                }
            }
        }

        public bool UseDataDescriptor
        {
            get
            {
                return (GeneralPurposeBitFlag & GeneralPurposeType.UseDataDescriptor) ==
                       GeneralPurposeType.UseDataDescriptor;
            }
            set
            {
                if (value)
                {
                    _generalPurposeBitFlag = (ushort) (_generalPurposeBitFlag | GeneralPurposeType.UseDataDescriptor);
                }
                else
                {
                    _generalPurposeBitFlag = (ushort) (_generalPurposeBitFlag & ~GeneralPurposeType.UseDataDescriptor);
                }
            }
        }

        public Encoding Encoding
        {
            get
            {
                if (UseUtf8Encoding) return Encoding.UTF8;
                return Encoding.GetEncoding(437);
            }
        }

        public bool UseDataDescriptorSignature { get; set; }

        public bool IsDirty { get; private set; }

        internal void SetNewFile(string fileName, CompressionType compressionType)
        {
            var fi = new FileInfo(fileName);
            if (!fi.Exists) throw new FileNotFoundException("File not found", fileName);
            if (_fileStream != null)
            {
                _fileStream.Close();
                _fileStream = null;
            }
            _fileName = fileName;
            _lastModified = fi.LastWriteTime;
            _originalStream = null;
            IsDirty = true;
        }

        internal void SetNewStream(Stream fileStream, CompressionType compressionType)
        {
            if (!fileStream.CanRead) throw new IOException("Can not read from file stream");
            if (_fileStream != null)
            {
                _fileStream.Close();
                _fileStream = null;
            }
            _fileStream = fileStream;
            _originalStream = null;
            IsDirty = true;
        }

        public void ClearExtraField()
        {
            _extraField = new byte[0];
            _localExtraField = new byte[0];
        }

        public static string NormalizeName(string entryName)
        {
            string name = entryName.Replace('\\', '/');

            int position = name.IndexOf(':');
            if (position > -1) name = name.Substring(position);

            return name.Trim('/');
        }

        private void ReadCentralHeader()
        {
            // central file header signature   4 bytes  (0x02014b50)
            // version made by                 2 bytes
            // version needed to extract       2 bytes
            // general purpose bit flag        2 bytes
            // compression method              2 bytes
            // last mod file time              2 bytes
            // last mod file date              2 bytes
            // crc-32                          4 bytes
            // compressed size                 4 bytes
            // uncompressed size               4 bytes
            // file name length                2 bytes
            // extra field length              2 bytes
            // file comment length             2 bytes
            // disk number start               2 bytes
            // internal file attributes        2 bytes
            // external file attributes        4 bytes
            // relative offset of local header 4 bytes

            //file name (variable size)
            //extra field (variable size)
            //file comment (variable size)
            _versionMadeBy = _originalStream.ReadUShort();
            _versionNeededToExtract = _originalStream.ReadUShort();
            _generalPurposeBitFlag = _originalStream.ReadUShort();
            _compressionMethod = _originalStream.ReadUShort();
            _lastModified = _originalStream.ReadDateTime();
            _crc32 = _originalStream.ReadUInt();
            _compressedSize = _originalStream.ReadUInt();
            _uncompressedSize = _originalStream.ReadUInt();
            ushort fileNameLength = _originalStream.ReadUShort();
            ushort extraFieldLength = _originalStream.ReadUShort();
            ushort fileCommentLength = _originalStream.ReadUShort();
            _diskNumberStart = _originalStream.ReadUShort();
            _internalAttributes = _originalStream.ReadUShort();
            _externalAttributes = _originalStream.ReadUInt();
            _localHeaderOffset = _originalStream.ReadUInt();

            _name = (fileNameLength == 0) ? "" : _originalStream.ReadString(fileNameLength, Encoding);
            _extraField = new byte[extraFieldLength];
            if (extraFieldLength > 0) _originalStream.Read(_extraField, 0, extraFieldLength);
            _comment = (fileCommentLength == 0) ? "" : _originalStream.ReadString(fileCommentLength, Encoding);
            _localExtraField = new byte[0];
        }

        public void ValidateLocalHeader()
        {
            if (!_localHeaderRead) ReadLocalHeader();
            if (_localVersionNeededToExtract != VersionNeededToExtract)
                throw new Exception(string.Format("VersionNeededToExtract local header missmatch for entry {0}", Name));
            if (_localGeneralPurposeBitFlag != GeneralPurposeBitFlag)
                throw new Exception(string.Format("GeneralPurposeBitFlag local header missmatch for entry {0}", Name));
            if (_localCompressionMethod != CompressionMethod)
                throw new Exception(string.Format("CompressionMethod local header missmatch for entry {0}", Name));
            if (_localLastModified != LastModified)
                throw new Exception(string.Format("LastModified local header missmatch for entry {0}", Name));
            if (_localCrc32 != Crc32)
                throw new Exception(string.Format("Crc32 local header missmatch for entry {0}", Name));
            if (_localCompressedSize != CompressedSize)
                throw new Exception(string.Format("CompressedSize local header missmatch for entry {0}", Name));
            if (_localUncompressedSize != UncompressedSize)
                throw new Exception(string.Format("UncompressedSize local header missmatch for entry {0}", Name));
            if (_localName != Name)
                throw new Exception(string.Format("Name local header missmatch for entry {0}", Name));
            //            if (Convert.ToBase64String(_localExtraField) != Convert.ToBase64String(ExtraField)) throw new Exception(string.Format("ExtraField local header missmatch for entry {0}", Name));
        }

        private void ReadLocalHeader()
        {
            if (_localHeaderRead) return;
            long streamPos = _originalStream.Position;
            _originalStream.Seek(LocalHeaderOffset + 4, SeekOrigin.Begin);

            // local file header signature     4 bytes  (0x04034b50)
            // version needed to extract       2 bytes
            // general purpose bit flag        2 bytes
            // compression method              2 bytes
            // last mod file time              2 bytes
            // last mod file date              2 bytes
            // crc-32                          4 bytes
            // compressed size                 4 bytes
            // uncompressed size               4 bytes
            // file name length                2 bytes
            // extra field length              2 bytes

            // file name (variable size)
            // extra field (variable size)

            Encoding encoding = Encoding.GetEncoding(437);
            _localVersionNeededToExtract = _originalStream.ReadUShort();
            _localGeneralPurposeBitFlag = _originalStream.ReadUShort();
            _localCompressionMethod = _originalStream.ReadUShort();
            _localLastModified = _originalStream.ReadDateTime();
            _localCrc32 = _originalStream.ReadUInt();
            _localCompressedSize = _originalStream.ReadUInt();
            _localUncompressedSize = _originalStream.ReadUInt();
            ushort fileNameLength = _originalStream.ReadUShort();
            ushort extraFieldLength = _originalStream.ReadUShort();

            if ((_localGeneralPurposeBitFlag & 0x0800) == 0x0800) encoding = Encoding.UTF8;
            _localName = (fileNameLength == 0) ? "" : _originalStream.ReadString(fileNameLength, encoding);
            _localExtraField = new byte[extraFieldLength];
            if (extraFieldLength > 0) _originalStream.Read(_localExtraField, 0, extraFieldLength);

            _dataOffset = _originalStream.Position;

            if (UseDataDescriptor)
            {
                _originalStream.Seek(CompressedSize, SeekOrigin.Current);
                uint crc = _originalStream.ReadUInt();
                if (crc == DataDescriptorSignature)
                {
                    UseDataDescriptorSignature = true;
                    crc = _originalStream.ReadUInt();
                }
                _localCrc32 = crc;
                _localCompressedSize = _originalStream.ReadUInt();
                _localUncompressedSize = _originalStream.ReadUInt();
            }
            _originalStream.Position = streamPos;
            _localHeaderRead = true;
        }

        public void WriteCentralRecord(Stream stream)
        {
            stream.WriteUInt(CentralHeaderSignature);
            stream.WriteUShort(VersionMadeBy);
            stream.WriteUShort(VersionNeededToExtract);
            stream.WriteUShort(GeneralPurposeBitFlag);
            stream.WriteUShort(CompressionMethod);
            stream.WriteDateTime(LastModified);
            stream.WriteUInt(Crc32);
            stream.WriteUInt(CompressedSize);
            stream.WriteUInt(UncompressedSize);
            byte[] fileName = Encoding.GetBytes(Name);
            stream.WriteUShort((ushort) fileName.Length);
            stream.WriteUShort((ushort) ExtraField.Length);
            byte[] fileComment = Encoding.GetBytes(Comment);
            stream.WriteUShort((ushort) fileComment.Length);
            stream.WriteUShort(DiskNumberStart);
            stream.WriteUShort(InternalAttributes);
            stream.WriteUInt(ExternalAttributes);
            stream.WriteUInt(LocalHeaderOffset);
            stream.Write(fileName, 0, fileName.Length);
            stream.Write(ExtraField, 0, ExtraField.Length);
            stream.Write(fileComment, 0, fileComment.Length);
            stream.Flush();
        }

        public void WriteEntry(Stream stream)
        {
            if (_originalStream != null) ReadLocalHeader();
            _localHeaderOffset = (uint) stream.Position;
            stream.WriteUInt(LocalHeaderSignature);
            stream.WriteUShort(VersionNeededToExtract);
            stream.WriteUShort(GeneralPurposeBitFlag);
            stream.WriteUShort(CompressionMethod);
            stream.WriteDateTime(LastModified);
            stream.WriteUInt(Crc32);
            stream.WriteUInt(CompressedSize);
            stream.WriteUInt(UncompressedSize);
            byte[] fileName = Encoding.GetBytes(Name);
            stream.WriteUShort((ushort) fileName.Length);
            stream.WriteUShort((ushort) ExtraField.Length);
            stream.Write(fileName, 0, fileName.Length);
            stream.Write(ExtraField, 0, ExtraField.Length);

            var dataOffset = stream.Position;
            WriteData(stream);
            stream.Flush();

            if (UncompressedSize == 0)
            {
                _compressionMethod = (ushort)CompressionType.Store;
            }
            else if (CompressedSize > UncompressedSize)
            {
                stream.Position = dataOffset;
                if (_fileStream != null) _fileStream.Position = 0;
                _compressionMethod = (ushort) CompressionType.Store;
                WriteData(stream);
                stream.Flush();
            }

            if (UseDataDescriptor)
            {
                if (UseDataDescriptorSignature)
                {
                    stream.WriteUInt(DataDescriptorSignature);
                }
                stream.WriteUInt(Crc32);
                stream.WriteUInt(CompressedSize);
                stream.WriteUInt(UncompressedSize);
                stream.Flush();
            }
            long streamPos = stream.Position;
            stream.Position = LocalHeaderOffset + 8;
            stream.WriteUShort(CompressionMethod);
            stream.WriteDateTime(LastModified);
            stream.WriteUInt(Crc32);
            stream.WriteUInt(CompressedSize);
            stream.WriteUInt(UncompressedSize);
            stream.Position = streamPos;
            stream.Flush();
            IsDirty = false;
        }

        private void WriteData(Stream stream)
        {
            if (_originalStream != null)
            {
                WriteOriginalData(stream);
                return;
            }
            bool closeFileStream = false;
            if (_fileStream == null)
            {
                _fileStream = File.OpenRead(_fileName);
                closeFileStream = true;
            }
            long startPosition = stream.Position;
            _uncompressedSize = 0;
            _compressedSize = 0;
            _crc32 = Crc32Hash.DefaultSeed;
            Stream outStream = stream;
            if (CompressionMethod == (uint) CompressionType.Deflate)
            {
                outStream = new DeflateStream(stream, CompressionMode.Compress, true);
            }

            int bytesRead;
            var buffer = new byte[WriteBufferSize];
            do
            {
                bytesRead = _fileStream.Read(buffer, 0, WriteBufferSize);
                if (bytesRead <= 0) break;
                _uncompressedSize += (uint) bytesRead;
                outStream.Write(buffer, 0, bytesRead);
                _crc32 = Crc32Hash.CalculateHash(_crc32, buffer, bytesRead);
            } while (bytesRead == WriteBufferSize);
            _crc32 ^= Crc32Hash.DefaultSeed;
            outStream.Flush();

            if (CompressionMethod == (uint) CompressionType.Deflate)
            {
                outStream.Close();
            }
            _compressedSize = (uint) (stream.Position - startPosition);
            if (closeFileStream)
            {
                _fileStream.Close();
                _fileStream = null;
            }
        }

        private void WriteOriginalData(Stream outStream)
        {
            long streamPos = _originalStream.Position;
            _originalStream.Position = _dataOffset;
            var buffer = new byte[WriteBufferSize];
            long size = CompressedSize;
            while (size > 0)
            {
                var length = (int) Math.Min(size, buffer.Length);
                int bytesRead = _originalStream.Read(buffer, 0, length);
                if (bytesRead <= 0) break;
                size -= bytesRead;
                outStream.Write(buffer, 0, bytesRead);
            }
            outStream.Flush();
            _originalStream.Position = streamPos;
        }

        internal void Extract(Stream stream)
        {
            if (_originalStream == null)
                throw new Exception(string.Format("{0} entry has not been written to archive.", Name));
            ReadLocalHeader();
            if (_localUncompressedSize == 0) return;
            long streamPos = _originalStream.Position;
            _originalStream.Position = _dataOffset;
            Stream readStream = _originalStream;
            if (CompressionMethod != 0)
            {
                readStream = new DeflateStream(_originalStream, CompressionMode.Decompress, true);
            }
            var buffer = new byte[WriteBufferSize];
            long size = _localUncompressedSize;
            while (size > 0)
            {
                var length = (int) Math.Min(size, buffer.Length);
                int bytesRead = readStream.Read(buffer, 0, length);
                if (bytesRead <= 0) break;
                size -= bytesRead;
                stream.Write(buffer, 0, bytesRead);
            }
            stream.Flush();

            if (CompressionMethod != 0)
            {
                readStream.Dispose();
            }
            _originalStream.Position = streamPos;
        }

        public Stream GetStream()
        {
            var memoryStream = new MemoryStream();
            Extract(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}