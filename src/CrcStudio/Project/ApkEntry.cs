//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using CrcStudio.Zip;

namespace CrcStudio.Project
{
    public class ApkEntry
    {
        private readonly ZipEntry _zipEntry;
        private readonly int _index;
        private BitmapImage _image;
        private int _imageWidth;
        private int _imageHeight;
        private readonly string _fileSystemPath;

        public ApkEntry(ZipEntry zipEntry, int index, string resourceFolder)
        {
            _zipEntry = zipEntry;
            _fileSystemPath = Path.Combine(resourceFolder, zipEntry.Name.Replace('/', Path.DirectorySeparatorChar));
            _index = index;
            CreateImage();
        }

        private void CreateImage()
        {
            if ((Path.GetExtension(_zipEntry.Name) ?? "").ToUpperInvariant() != ".PNG") return;
            using (var stream = _zipEntry.GetStream())
            {
                bool resize = false;
                _image = new BitmapImage();
                _image.BeginInit();
                using (var bmp = new Bitmap(stream))
                {
                    _imageWidth = bmp.Width;
                    _imageHeight = bmp.Height;

                    if (_imageWidth > 100 || _imageHeight > 100)
                    {
                        if (_imageWidth > _imageHeight)
                        {
                            _image.DecodePixelWidth = 100;
                        }
                        else
                        {
                            _image.DecodePixelHeight = 100;
                        }
                    }
                }
                _image.CacheOption = BitmapCacheOption.OnLoad;
                _image.StreamSource = stream;
                _image.EndInit();
            }
        }

        public string FileSystemPath { get { return _fileSystemPath; } }
        public string Name { get { return _zipEntry.Name; } }
        public int Index { get { return _index; } }
        public long CompressedSize { get { return _zipEntry.CompressedSize; } }
        public long UncompressedSize { get { return _zipEntry.UncompressedSize; } }
        public long ExternalSize { get { return ExternalFileExist ? new FileInfo(_fileSystemPath).Length : 0; } }
        public string InternalCrc { get { return _zipEntry.Crc32.ToString("X8"); } }
        public string ExternalCrc { get { return ExternalFileExist ? Crc32Hash.CalculateHash(_fileSystemPath).ToString("X8") : ""; } }
        private bool ExternalFileExist { get { return File.Exists(_fileSystemPath); } }
        public string ImageSize { get { return _imageWidth > 0 ? _imageWidth + "x" + _imageHeight : ""; } }

        public BitmapImage Image { get { return _image; } }
    }
}