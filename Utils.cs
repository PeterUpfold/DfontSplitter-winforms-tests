/*
 DfontSplitter for Windows - GUI frontend to fondu, converts .dfont to .ttf
 Copyright (C) 2008-2020 Peter Upfold.

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
  
 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DfontSplitter;
using System.Security.Cryptography;

namespace DfontSplitterTests
{
    /// <summary>
    /// Utilities for use by many unit tests.
    /// </summary>
    static class Utils
    {

        /// <summary>
        /// Run Fondu to convert the font with the given path relating to the assembly code base path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>fondu return code -- 1 is success</returns>
        public static int RunFonduOnRelativePath(string path)
        {
            string dirName = GetProjectTestFilesDir();

            if (dirName == null)
            {
                throw new InvalidOperationException("Unable to get directory name of executing assembly.");
            }

            return FonduWrapper.fondu_simple_main(
                Path.Combine(dirName, path)
                );
        }

        /// <summary>
        /// Run FontForge normalisation process on TTFs with the given path relating to the assembly code base path.
        /// </summary>
        /// <param name="context">TestContext to get temporary directory from</param>
        /// <param name="filePath">File name only</param>
        /// <param name="normalisedTTFPath">The normalised TTF path on completion</param>
        /// <returns>process exit code -- 0 on success</returns>
        public static int RunFontForgeOnRelativePath(TestContext context, string filePath, out string normalisedTTFPath)
        {
            return FontForgeWrapper.NormaliseTTF(
                filePath,
                context.TempDirectory,
                out normalisedTTFPath
            );
        }

        /// <summary>
        /// Get the directory to the project test files
        /// </summary>
        /// <returns></returns>
        private static string GetProjectTestFilesDir()
        {
            // get codebase -- ref https://stackoverflow.com/questions/23515736/how-to-refer-to-test-files-from-xunit-tests-in-visual-studio

            Uri codeBase = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            return Path.GetDirectoryName(Uri.UnescapeDataString(codeBase.AbsolutePath));
        }

        /// <summary>
        ///  Set up for the conversion tests
        /// </summary>
        public static TestContext SetUp()
        {
            TestContext context = new TestContext {
                OriginalWorkingDirectory = Directory.GetCurrentDirectory(), /* I hope this does this assignment in this order! */
                TempDirectory = SetWorkingDirectoryToTemp()
            };

            return context;
        }

        /// <summary>
        /// Tidy up after a test.
        /// </summary>
        /// <param name="context"></param>
        public static void TearDown(TestContext context)
        {
            if (context != null)
            {
                if (string.IsNullOrWhiteSpace(context.OriginalWorkingDirectory))
                {
                    throw new InvalidOperationException("Unable to restore original working directory as it was null or blank.");
                }

                Directory.SetCurrentDirectory(context.OriginalWorkingDirectory);

                if (!string.IsNullOrWhiteSpace(context.TempDirectory))
                {
                    Directory.Delete(context.TempDirectory, true);
                }
            }
        }

        /// <summary>
        /// Set the current working directory to a temporary folder.
        /// </summary>
        public static string SetWorkingDirectoryToTemp()
        {
            string randomDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(randomDir);
            Directory.SetCurrentDirectory(randomDir);
            return randomDir;
        }

        /// <summary>
        /// Get the SHA256 hash for a file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetSHA256Hash(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new ArgumentException($"The filename provided '{fileName}' does not exist on disk.", nameof(fileName));
            }

            byte[] hash;

            using (FileStream stream = File.OpenRead(fileName))
            {
                SHA256Managed hasher = new SHA256Managed();
                hash = hasher.ComputeHash(stream);
            }

            if (hash.Length < 1)
            {
                throw new Exception("Hash did not return any data");
            }

            return BitConverter.ToString(hash).Replace("-","").ToLowerInvariant();
        }

    }
}
