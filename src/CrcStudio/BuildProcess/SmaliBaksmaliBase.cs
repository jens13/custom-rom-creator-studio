//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CrcStudio.Messages;
using CrcStudio.Project;
using CrcStudio.Utility;
using CrcStudio.Zip;

namespace CrcStudio.BuildProcess
{
    public abstract class SmaliBaksmaliBase
    {
        protected readonly string _baksmaliFile;
        protected readonly bool _canDecompile;
        protected readonly bool _canRecompile;
        protected readonly string _javaFile;
        private readonly Dictionary<CrcsProject, string> _pathToDependencies = new Dictionary<CrcsProject, string>();
        protected readonly string _smaliFile;

        protected SmaliBaksmaliBase(SolutionProperties properties)
        {
            _javaFile = CrcsSettings.Current.JavaFile;
            _baksmaliFile = properties.BaksmaliFile;
            _canDecompile = properties.CanDecompile;
            _smaliFile = properties.SmaliFile;
            _canRecompile = properties.CanRecompile;
        }

        protected void Decompile(string name, string classesFile, string outputFolder, bool ignoreDecompileErrors,
                                 CrcsProject project)
        {
            var ep = new ExecuteProgram();

            string locationOfDependencies = GetLocationOfDependencies(project);

            bool decompiled = false;
            while (!decompiled)
            {
                string additionalDependencies = project.GetAdditionalDependencies(name).Aggregate("",
                                                                                                  (current, dependency)
                                                                                                  =>
                                                                                                  current +
                                                                                                  (":" + dependency));
                if (additionalDependencies.Length > 0) additionalDependencies = " -c " + additionalDependencies;

                var arguments = new StringBuilder();
                arguments.Append("-Xmx512m -jar");

                arguments.Append(" ").Append(_baksmaliFile);
                arguments.Append(locationOfDependencies);
                arguments.Append(" -o \"").Append(outputFolder).Append("\"");
                if (ignoreDecompileErrors) arguments.Append(" -I");
                arguments.Append(" -l -s");
                arguments.Append(additionalDependencies);
                if ((Path.GetExtension(classesFile) ?? "").ToUpperInvariant() == ".ODEX") arguments.Append(" -x");
                arguments.Append(" \"").Append(classesFile).Append("\"");
                if (ep.Execute(_javaFile, arguments.ToString(), Path.GetDirectoryName(classesFile)) == 0)
                {
                    if (ignoreDecompileErrors)
                    {
                        MessageEngine.AddInformation(this, ep.Output);
                    }
                    decompiled = true;
                }
                else
                {
                    if (!FindMissingDependency(name, project, ep.Output))
                    {
                        throw ep.CreateException(Path.GetFileName(_baksmaliFile));
                    }
                }
            }
        }

        protected bool FindMissingDependency(string name, CrcsProject project, string baksmaliOutput)
        {
            string[] bootClassPathArray = GetBootClassPath(baksmaliOutput);
            if (bootClassPathArray.Length == 0) return false;
            if (bootClassPathArray[0].Equals("junit", StringComparison.OrdinalIgnoreCase) &&
                project.LocationsOfDependencies.FirstOrDefault(x => File.Exists(Path.Combine(x, "core-junit.jar"))) !=
                null)
            {
                return project.AddDependency(name, "core-junit.jar");
            }

            string missingClassName = bootClassPathArray[bootClassPathArray.Length - 1];
            if (FindBootClassPathFile(name, project, missingClassName)) return true;

            int start = 0;
            int count = bootClassPathArray.Length - 1;
            while (count > 0)
            {
                string bootClassPath = string.Join(".", bootClassPathArray, start, count) + ".jar";
                if (project.LocationsOfDependencies.FirstOrDefault(x => File.Exists(Path.Combine(x, bootClassPath))) !=
                    null)
                {
                    return project.AddDependency(name, bootClassPath);
                }
                start++;
                count--;
            }
            return false;
        }

        //Error while disassembling method Lcom/android/settings/wifi/AccessPointListDialog;->updateWpsEvent(I)V. Continuing.
        //org.jf.dexlib.Code.Analysis.ValidationException: class Lcom/sec/android/touchwiz/widget/TwProgressDialog; cannot be resolved.
        //    at org.jf.dexlib.Code.Analysis.ClassPath$UnresolvedClassDef.unresolvedValidationException(ClassPath.java:535)
        //    at org.jf.dexlib.Code.Analysis.ClassPath$UnresolvedClassDef.getClassDepth(ClassPath.java:543)
        //    at org.jf.dexlib.Code.Analysis.ClassPath.getCommonSuperclass(ClassPath.java:384)
        //    at org.jf.dexlib.Code.Analysis.RegisterType.merge(RegisterType.java:275)
        //    at org.jf.dexlib.Code.Analysis.AnalyzedInstruction.mergeRegister(AnalyzedInstruction.java:185)
        //    at org.jf.dexlib.Code.Analysis.MethodAnalyzer.propagateRegisterToSuccessors(MethodAnalyzer.java:444)
        //    at org.jf.dexlib.Code.Analysis.MethodAnalyzer.setPostRegisterTypeAndPropagateChanges(MethodAnalyzer.java:424)
        //    at org.jf.dexlib.Code.Analysis.MethodAnalyzer.setDestinationRegisterTypeAndPropagateChanges(MethodAnalyzer.java:396)
        //    at org.jf.dexlib.Code.Analysis.MethodAnalyzer.analyzeIgetWideObject(MethodAnalyzer.java:2601)
        //    at org.jf.dexlib.Code.Analysis.MethodAnalyzer.analyzeInstruction(MethodAnalyzer.java:776)
        //    at org.jf.dexlib.Code.Analysis.MethodAnalyzer.analyze(MethodAnalyzer.java:208)
        //    at org.jf.baksmali.Adaptors.MethodDefinition.addAnalyzedInstructionMethodItems(MethodDefinition.java:353)
        //    at org.jf.baksmali.Adaptors.MethodDefinition.getMethodItems(MethodDefinition.java:290)
        //    at org.jf.baksmali.Adaptors.MethodDefinition.writeTo(MethodDefinition.java:130)
        //    at org.jf.baksmali.Adaptors.ClassDefinition.writeMethods(ClassDefinition.java:322)
        //    at org.jf.baksmali.Adaptors.ClassDefinition.writeDirectMethods(ClassDefinition.java:291)
        //    at org.jf.baksmali.Adaptors.ClassDefinition.writeTo(ClassDefinition.java:135)
        //    at org.jf.baksmali.baksmali.disassembleDexFile(baksmali.java:191)
        //    at org.jf.baksmali.main.main(main.java:278)
        //opcode: iget-object
        //CodeAddress: 45
        //Method: Lcom/android/settings/wifi/AccessPointListDialog;->updateWpsEvent(I)V

        //Error occured while loading boot class path files. Aborting.
        //org.jf.dexlib.Code.Analysis.ClassPath$ClassNotFoundException: Could not find superclass Lcom/sec/android/touchwiz/widget/TwTabActivity;
        //    at org.jf.dexlib.Code.Analysis.ClassPath$ClassDef.loadSuperclass(ClassPath.java:784)
        //    at org.jf.dexlib.Code.Analysis.ClassPath$ClassDef.<init>(ClassPath.java:668)
        //    at org.jf.dexlib.Code.Analysis.ClassPath.loadClassDef(ClassPath.java:280)
        //    at org.jf.dexlib.Code.Analysis.ClassPath.initClassPath(ClassPath.java:163)
        //    at org.jf.dexlib.Code.Analysis.ClassPath.InitializeClassPathFromOdex(ClassPath.java:110)
        //    at org.jf.baksmali.baksmali.disassembleDexFile(baksmali.java:98)
        //    at org.jf.baksmali.main.main(main.java:278)
        //Error while loading class Lcom/sec/android/app/clockpackage/ClockPackage; from file D:\CrcStudio\I9000\Gingerbread\I9000XWJVB\XWJVB_XEEJV3\system\app\ClockPackage.odex
        //Error while loading ClassPath class Lcom/sec/android/app/clockpackage/ClockPackage;
        private string[] GetBootClassPath(string baksmaliOutput)
        {
            int start = -1;
            if (baksmaliOutput.StartsWith("Error occured while loading boot class path files. Aborting.",
                                          StringComparison.OrdinalIgnoreCase))
            {
                string indexText = "Could not find superclass L";
                start = baksmaliOutput.IndexOf(indexText, StringComparison.OrdinalIgnoreCase);
                if (start == -1)
                {
                    indexText = "Could not find interface L";
                    start = baksmaliOutput.IndexOf(indexText, StringComparison.OrdinalIgnoreCase);
                }
                if (start == -1) return new string[0];
                start += indexText.Length;
            }
            else if (baksmaliOutput.StartsWith("Error while disassembling method L", StringComparison.OrdinalIgnoreCase))
            {
                string indexText = ".ValidationException: class L";
                start = baksmaliOutput.IndexOf(indexText, StringComparison.OrdinalIgnoreCase);
                if (start == -1) return new string[0];
                start += indexText.Length;
            }
            if (start == -1) return new string[0];
            int len = baksmaliOutput.IndexOf(';', start);
            if (len == -1) return new string[0];
            len -= start;
            return baksmaliOutput.Substring(start, len).Split('/');
        }

        private bool FindBootClassPathFile(string name, CrcsProject project, string missingClassName)
        {
            foreach (string location in project.LocationsOfDependencies)
            {
                string[] files = Directory.GetFiles(location, missingClassName + ".smali", SearchOption.AllDirectories);
                if (files.Length > 0)
                {
                    string folder = files[0].Substring(location.Length).TrimStart(Path.DirectorySeparatorChar).Substring(7).TrimStart(Path.DirectorySeparatorChar);
                    folder = folder.Split(Path.DirectorySeparatorChar)[0];
                    return project.AddDependency(name, folder);
                }
            }
            return false;
        }

        protected string GetLocationOfDependencies(CrcsProject project)
        {
            if (!_pathToDependencies.ContainsKey(project))
            {
                string pathToDependencies = "";
                foreach (string path in project.LocationsOfDependencies)
                {
                    pathToDependencies += " -d \"" + path + "\"";
                }
                _pathToDependencies.Add(project, pathToDependencies);
            }
            return _pathToDependencies[project];
        }

        protected void Recompile(string compositFilePath, string inputFolder)
        {
            var ep = new ExecuteProgram();

            var arguments = new StringBuilder();
            arguments.Append("-jar");
            arguments.Append(" ").Append(_smaliFile);
            arguments.Append(" \"").Append(inputFolder).Append("\"");
            string classesDexFile = Path.Combine(FolderUtility.CreateTempFolder(), "classes.dex");
            arguments.Append(" -o \"").Append(classesDexFile).Append("\"");

            if (ep.Execute(_javaFile, arguments.ToString(), Path.GetDirectoryName(classesDexFile)) != 0)
            {
                throw ep.CreateException(Path.GetFileName(_smaliFile));
            }

            // add to program file apk/jar...
            using (var zf = new AndroidArchive(compositFilePath))
            {
                zf.Add(classesDexFile, Path.GetFileName(classesDexFile));
            }
        }
    }
}