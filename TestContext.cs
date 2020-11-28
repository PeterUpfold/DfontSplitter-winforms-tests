using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DfontSplitterTests
{
    /// <summary>
    /// Hold information about a test set up environment so we can tear it down later.
    /// </summary>
    internal class TestContext
    {
        /// <summary>
        /// The temporary directory used for files.
        /// </summary>
        public string TempDirectory;

        /// <summary>
        /// For restoring the original working directory before tear down.
        /// </summary>
        public string OriginalWorkingDirectory;
    }
}
