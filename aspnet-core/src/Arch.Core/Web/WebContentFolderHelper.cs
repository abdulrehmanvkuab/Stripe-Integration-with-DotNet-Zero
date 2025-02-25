﻿using System;
using System.IO;
using System.Linq;
using Abp.Reflection.Extensions;

namespace Arch.Web
{
    /// <summary>
    /// This class is used to find root path of the web project in;
    /// unit tests (to find views) and entity framework core command line commands (to find conn string).
    /// </summary>
    public static class WebContentDirectoryFinder
    {
        public static string CalculateContentRootFolder()
        {
            var coreAssemblyDirectoryPath = Path.GetDirectoryName(typeof(ArchCoreModule).GetAssembly().Location);

            try
            {
                if (coreAssemblyDirectoryPath == null)
                {
                    if (AppContext.BaseDirectory == null)
                    {
                        throw new Exception("Could not find location of Arch.Core assembly!");

                    }

                    return AppContext.BaseDirectory;
                    //throw new Exception("Could not find location of Arch.Core assembly!");
                }

                var directoryInfo = new DirectoryInfo(coreAssemblyDirectoryPath);
                while (!DirectoryContains(directoryInfo.FullName, "Arch.Web.sln"))
                {
                    if (directoryInfo.Parent == null)
                    {
                        //      throw new Exception("Could not find content root folder!");
                    }

                    directoryInfo = directoryInfo.Parent;
                }

                var webMvcFolder = Path.Combine(directoryInfo.FullName, $"src{Path.DirectorySeparatorChar}Arch.Web.Mvc");
                if (Directory.Exists(webMvcFolder))
                {
                    return webMvcFolder;
                }

                var webHostFolder = Path.Combine(directoryInfo.FullName, $"src{Path.DirectorySeparatorChar}Arch.Web.Host");
                if (Directory.Exists(webHostFolder))
                {
                    return webHostFolder;
                }
            }
            catch (Exception Ex)
            {
                try
                {
                    return coreAssemblyDirectoryPath;
                }
                catch (Exception Ex2)
                {
                    return AppContext.BaseDirectory;
                }

            }
            return AppContext.BaseDirectory;
            //  throw new Exception("Could not find root folder of the web project!");
        }

        private static bool DirectoryContains(string directory, string fileName)
        {
            return Directory.GetFiles(directory).Any(filePath => string.Equals(Path.GetFileName(filePath), fileName));
        }
    }
}
