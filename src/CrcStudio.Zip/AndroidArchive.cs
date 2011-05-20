//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CrcStudio.Zip
{
    public class AndroidArchive : ZipFile
    {
        private readonly List<string> _storeFileTypes;
                                      //= new[] { ".OGG", ".MP3", ".PNG", ".JPG", ".JPEG", ".GIF", ".AVI", ".3GP", ".MP4", ".MPG", ".MPEG", ".ARSC" };

        public AndroidArchive(string file)
            : this(file, null)
        {
        }

        public AndroidArchive(string file, IEnumerable<string> extensionOfFilesToStore)
            : base(file)
        {
            _storeFileTypes =
                new List<string>(new[]
                                     {
                                         ".OGG", ".MP3", ".PNG", ".JPG", ".JPEG", ".GIF", ".AVI", ".3GP", ".MP4", ".MPG",
                                         ".MPEG", ".ARSC"
                                     });
            if (extensionOfFilesToStore != null)
            {
                _storeFileTypes.AddRange(
                    extensionOfFilesToStore.Select(x => x.ToUpperInvariant()).Where(
                        extension => !_storeFileTypes.Contains(extension)));
            }
            UseUtf8Encoding = false;
        }

        protected override void InternalFlush(bool closeStream)
        {
            _entries.OrderBy(x => x.Name);
            ChangePosition("classes.dex", 0);
            ChangePosition("AndroidManifest.xml", 0);
            ChangePosition("META-INF/CERT.RSA", 0);
            ChangePosition("META-INF/CERT.SF", 0);
            ChangePosition("META-INF/MANIFEST.MF", 0);
            ChangePosition("resources.arsc", -1);
            foreach (ZipEntry entry in _entries)
            {
                entry.ClearExtraField();
                bool dataDescriptor = (entry.CompressionMethod != 0);
                entry.UseDataDescriptor = dataDescriptor;
                entry.UseDataDescriptorSignature = dataDescriptor;
            }
            _entries[0].ExtraField = new byte[] {0xFE, 0xCA, 0, 0};
            _isChanged = true;
            base.InternalFlush(closeStream);
        }

        private void ChangePosition(string entryName, int newPosition)
        {
            int index = _entries.FindIndex(x => x.Name.Equals(entryName, StringComparison.OrdinalIgnoreCase));
            if (index == newPosition) return;
            if (index < 0) return;
            ZipEntry zipEntry = _entries[index];
            _entries.RemoveAt(index);
            if (newPosition < 0)
            {
                _entries.Add(zipEntry);
            }
            else
            {
                _entries.Insert(newPosition, zipEntry);
            }
        }

        public ZipEntry Add(string file, string entryName)
        {
            string extension = Path.GetExtension(file) ?? "";
            extension = extension.ToUpperInvariant();
            return Add(file, entryName,
                       _storeFileTypes.Contains(extension) ? CompressionType.Store : CompressionType.Deflate);
        }
        internal override void MergeZipFile(ZipFile archive, bool overwrite)
        {
            var manifest = overwrite ? _entries.FirstOrDefault(x => x.Name.Equals("AndroidManifest.xml", StringComparison.OrdinalIgnoreCase)) : null;
            base.MergeZipFile(archive, overwrite);
            if (manifest != null)
            {
                _entries.RemoveAll(x => x.Name.Equals("AndroidManifest.xml", StringComparison.OrdinalIgnoreCase));
                _entries.Add(manifest);
            }
        }
        public void ForceRearrangeEntries()
        {
            _isDirty = true;
            _isChanged = true;
        }
    }
}