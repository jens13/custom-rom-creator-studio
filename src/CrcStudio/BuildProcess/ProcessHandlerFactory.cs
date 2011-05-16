//          Copyright Jens Granlund 2011.
//      Distributed under the New BSD License.
//     (See accompanying file notice.txt or at 
// http://www.opensource.org/licenses/bsd-license.php)
using System.Collections.Generic;
using CrcStudio.Project;

namespace CrcStudio.BuildProcess
{
    public class ProcessHandlerFactory
    {
        public static IEnumerable<IFileHandler> CreateFileHandlers(ProcessingOptions processingOptions,
                                                                   SolutionProperties properties)
        {
            bool optimizePng = (ProcessingOptions.OptimizePng & processingOptions) != 0;
            bool packageModified = false;

            var fileHandlers = new List<IFileHandler>();
            fileHandlers.Add(new BackupFilesHandler());

            if ((ProcessingOptions.DeOdex & processingOptions) != 0)
            {
                fileHandlers.Add(new DeOdexHandler(properties));
                packageModified = true;
                if (optimizePng)
                {
                    fileHandlers.Add(new UnPackHandler());
                    fileHandlers.Add(new OptiPngHandler(properties));
                    fileHandlers.Add(new RePackPngHandler());
                }
            }
            else
            {
                if ((ProcessingOptions.Decompile & processingOptions) != 0)
                {
                    fileHandlers.Add(new BaksmaliHandler(properties));
                }

                if ((ProcessingOptions.Decode & processingOptions) != 0)
                {
                    fileHandlers.Add(new DecodeHandler(properties));
                }
                else if (optimizePng)
                {
                    fileHandlers.Add(new UnPackHandler());
                }

                if ((ProcessingOptions.ProcessModifications & processingOptions) != 0)
                {
                    fileHandlers.Add(new ModPlugInHandler());
                }

                if (optimizePng)
                {
                    fileHandlers.Add(new OptiPngHandler(properties));
                }

                if ((ProcessingOptions.Encode & processingOptions) != 0)
                {
                    fileHandlers.Add(new EncodeHandler(properties));
                    packageModified = true;
                }
                else if (optimizePng)
                {
                    fileHandlers.Add(new RePackPngHandler());
                    packageModified = true;
                }

                if ((ProcessingOptions.Recompile & processingOptions) != 0)
                {
                    fileHandlers.Add(new SmaliHandler(properties));
                    packageModified = true;
                }

                if ((ProcessingOptions.ReSignApkFiles & processingOptions) != 0 || packageModified)
                {
                    fileHandlers.Add(new SignApkHandler(properties));
                }
            }
            if (packageModified)
            {
                fileHandlers.Add(new ZipAlignHandler(properties));
            }
            return fileHandlers;
        }
    }
}