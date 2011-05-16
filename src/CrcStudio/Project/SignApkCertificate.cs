//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.IO;

namespace CrcStudio.Project
{
    public class SignApkCertificate
    {
        public SignApkCertificate(string certificateFile, string certificateKeyFile)
        {
            CertificateFile = certificateFile;
            CertificateKeyFile = certificateKeyFile;
            Name = Path.GetFileNameWithoutExtension(certificateKeyFile);
        }

        public string Name { get; private set; }

        public string CertificateKeyFile { get; private set; }

        public string CertificateFile { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}