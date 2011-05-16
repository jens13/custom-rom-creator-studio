//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CrcStudio.Project
{
    public class ProjectTools
    {
        private readonly List<SignApkCertificate> _certificates = new List<SignApkCertificate>();

        private readonly Dictionary<ProjectToolType, List<string>> _tools =
            new Dictionary<ProjectToolType, List<string>>();

        public void Refresh(string toolsAndCertificateFolder)
        {
            Refresh(toolsAndCertificateFolder, toolsAndCertificateFolder);
        }

        public void Refresh(string toolsFolder, string certificateFolder)
        {
            _tools.Clear();

            FindToolVersions(toolsFolder, "apktool*.jar", ProjectToolType.ApkTool);
            FindToolVersions(toolsFolder, "smali*.jar", ProjectToolType.Smali);
            FindToolVersions(toolsFolder, "baksmali*.jar", ProjectToolType.Baksmali);
            FindToolVersions(toolsFolder, "signapk*.jar", ProjectToolType.SignApk);

            FindToolVersions(toolsFolder, "optipng*.exe", ProjectToolType.OptiPng);
            FindToolVersions(toolsFolder, "zipalign*.exe", ProjectToolType.ZipAlign);

            FindCertificates(certificateFolder);
        }

        private void FindCertificates(string certificateFolder)
        {
            var exts = new[] {".x509.pem", ".x509", ".pem"};
            foreach (string certKey in Directory.GetFiles(certificateFolder, "*.pk8", SearchOption.AllDirectories))
            {
                string cert = Path.Combine(Path.GetDirectoryName(certKey), Path.GetFileNameWithoutExtension(certKey));
                foreach (string ext in exts)
                {
                    if (File.Exists(cert + ext))
                    {
                        _certificates.Add(new SignApkCertificate(cert + ext, certKey));
                        break;
                    }
                }
            }
        }

        private void FindToolVersions(string toolsFolder, string searchPattern, ProjectToolType toolType)
        {
            if (_tools.ContainsKey(toolType)) return;
            List<string> tools = Directory.GetFiles(toolsFolder, searchPattern).OrderByDescending(x => x).ToList();
            if (tools.Count == 0) return;
            _tools.Add(toolType, tools);
        }

        public IEnumerable<string> GetToolVersions(ProjectToolType toolType)
        {
            if (!_tools.ContainsKey(toolType)) return new List<string>();
            return _tools[toolType].Select(Path.GetFileName);
        }

        public string GetTool(ProjectToolType toolType, string selectedVersion)
        {
            if (!_tools.ContainsKey(toolType)) return null;
            List<string> tools = _tools[toolType];
            if (tools.Count == 0) return null;
            if (string.IsNullOrWhiteSpace(selectedVersion)) return tools[0];
            string tool =
                tools.FirstOrDefault(
                    x => selectedVersion.Equals(Path.GetFileName(x), StringComparison.OrdinalIgnoreCase));
            return tool ?? tools[0];
        }

        public IEnumerable<SignApkCertificate> GetCertificates()
        {
            return _certificates;
        }

        public SignApkCertificate GetCertificate()
        {
            return GetCertificate(null);
        }

        public SignApkCertificate GetCertificate(string name)
        {
            if (_certificates.Count == 0) return null;
            if (string.IsNullOrWhiteSpace(name)) return _certificates[0];
            SignApkCertificate cert =
                _certificates.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return cert ?? _certificates[0];
        }
    }
}