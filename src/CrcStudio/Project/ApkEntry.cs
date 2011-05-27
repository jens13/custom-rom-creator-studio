//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using CrcStudio.Utility;
using CrcStudio.Zip;

namespace CrcStudio.Project
{
    public class ApkEntry
    {
        private readonly ZipEntry _zipEntry;
        private readonly int _index;
        private BitmapImage _internalImage;
        private int _internalImageWidth;
        private int _internalImageHeight;
        private BitmapImage _externalImage;
        private int _externalImageWidth;
        private int _externalImageHeight;
        private readonly string _fileSystemPath;
        private readonly string _resourceFolder;
        private FileInfo _fileInfo;
        private string _shortFilePath;

        public ApkEntry(string fileSystemPath, string resourceFolder)
        {
            _fileSystemPath = fileSystemPath;
            _resourceFolder = resourceFolder;
            _shortFilePath = FileUtility.ShortFilePath(_fileSystemPath, 100);
            _fileInfo = new FileInfo(_fileSystemPath);
            CreateExternalImage();
        }

        public ApkEntry(ZipEntry zipEntry, int index, string resourceFolder)
        {
            _zipEntry = zipEntry;
            _resourceFolder = resourceFolder;
            _fileSystemPath = Path.Combine(_resourceFolder, zipEntry.Name.Replace('/', Path.DirectorySeparatorChar));
            _shortFilePath = FileUtility.ShortFilePath(_fileSystemPath, 70);
            _fileInfo = new FileInfo(_fileSystemPath);
            _index = index;
            CreateInternalImage();
            CreateExternalImage();
        }

        private void CreateInternalImage()
        {
            if ((Path.GetExtension(_zipEntry.Name) ?? "").ToUpperInvariant() != ".PNG") return;
            using (var stream = _zipEntry.GetStream())
            {
                _internalImage = new BitmapImage();
                _internalImage.BeginInit();
                using (var bmp = new Bitmap(stream))
                {
                    _internalImageWidth = bmp.Width;
                    _internalImageHeight = bmp.Height;

                    if (_internalImageWidth > 100 || _internalImageHeight > 100)
                    {
                        if (_internalImageWidth > _internalImageHeight)
                        {
                            _internalImage.DecodePixelWidth = 100;
                        }
                        else
                        {
                            _internalImage.DecodePixelHeight = 100;
                        }
                    }
                }
                _internalImage.CacheOption = BitmapCacheOption.OnLoad;
                _internalImage.StreamSource = stream;
                _internalImage.EndInit();
            }
        }

        private void CreateExternalImage()
        {
            if (!ExternalFileExists) return;
            if ((Path.GetExtension(_fileSystemPath) ?? "").ToUpperInvariant() != ".PNG") return;
            using (var stream = File.OpenRead(_fileSystemPath))
            {
                _externalImage = new BitmapImage();
                _externalImage.BeginInit();
                using (var bmp = new Bitmap(stream))
                {
                    _externalImageWidth = bmp.Width;
                    _externalImageHeight = bmp.Height;

                    if (_externalImageWidth > 100 || _externalImageHeight > 100)
                    {
                        if (_externalImageWidth > _externalImageHeight)
                        {
                            _externalImage.DecodePixelWidth = 100;
                        }
                        else
                        {
                            _externalImage.DecodePixelHeight = 100;
                        }
                    }
                }
                _externalImage.CacheOption = BitmapCacheOption.OnLoad;
                _externalImage.StreamSource = stream;
                _externalImage.EndInit();
            }
        }

        public string RelativePath
        {
            get
            {
                return ZipEntryExists
                           ? _zipEntry.Name
                           : FolderUtility.GetRelativePath(_resourceFolder, _fileSystemPath).Replace(
                               Path.DirectorySeparatorChar, '/').TrimStart('/');
            }
        }
        public string FileSystemPath { get { return _fileSystemPath; } }
        public string Name { get { return ZipEntryExists ? _zipEntry.Name : ""; } }
        public string ExternalName { get { return ExternalFileExists ? _shortFilePath : ""; } }
        public string Index { get { return _index.ToString(); } }
        public string CompressedSize { get { return ZipEntryExists ? _zipEntry.CompressedSize.ToString() : ""; } }
        public string UncompressedSize { get { return ZipEntryExists ? _zipEntry.UncompressedSize.ToString() : ""; } }
        public string ExternalSize { get { return ExternalFileExists ? ExternalFile.Length.ToString() : ""; } }
        public string InternalCrc { get { return ZipEntryExists ? _zipEntry.Crc32.ToString("X8") : ""; } }
        public string ExternalCrc { get { return ExternalFileExists ? Crc32Hash.CalculateHash(_fileSystemPath).ToString("X8") : ""; } }
        public string InternalImageSize { get { return _internalImageWidth > 0 ? _internalImageWidth + "x" + _internalImageHeight : ""; } }
        public string ExternalImageSize { get { return _externalImageWidth > 0 ? _externalImageWidth + "x" + _internalImageHeight : ""; } }

        public string InternalModifiedDate { get { return ZipEntryExists ? _zipEntry.LastModified.ToShortDateString() : ""; } }
        public string ExternalModifiedDate { get { return ExternalFileExists ? ExternalFile.LastWriteTime.ToShortDateString() : ""; } }
        public string ExternalCreatedDate { get { return ExternalFileExists ? ExternalFile.CreationTime.ToShortDateString() : ""; } }

        public BitmapImage InternalImage { get { return _internalImage; } }
        public BitmapImage ExternalImage { get { return _externalImage; } }

        private FileInfo ExternalFile { get { return _fileInfo; } }
        private bool ExternalFileExists { get { return _fileInfo.Exists; } }
        private bool ZipEntryExists { get { return (_zipEntry != null); } }
    }
}