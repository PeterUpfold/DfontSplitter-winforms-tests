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
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DfontSplitterTests
{
    /// <summary>
    /// File conversion tests for DfontSplitter for Windows.
    /// </summary>
    public class ConversionTests
    {
        /// <summary>
        /// Allow console output using this object.
        /// </summary>
        private readonly ITestOutputHelper testOutputHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="testOutputHelper"></param>
        public ConversionTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestPTSansOTF()
        {
            TestContext context = Utils.SetUp();
            // this puts us in the temp folder for output files to be available with relative paths

            int fonduConvertResult = Utils.RunFonduOnRelativePath(Path.Combine("SampleFiles", "PTSans-Regular.otf.dfont"));

            // fondu should return 1 on success
            Assert.Equal(1, fonduConvertResult);

            // check expected file hash
            string ptSansExpected = "2101fe4abb8b97da7ec339afa93d4c3575d01a6498abc306e7349d811c128747";

            Assert.Equal(ptSansExpected, Utils.GetSHA256Hash("PTSans.otf"));

            // run FontForge normalisation
            int fontForgeResult = Utils.RunFontForgeOnRelativePath(context, "PTSans.otf", out string normalisedPath);

            Assert.Equal(0, fontForgeResult);

            string ptSansOutExpected = "a1037935b7f838b469fe2e02ec9a7088c09db20f3e1bfa632a24850b612955c1";
            Assert.Equal(ptSansOutExpected, Utils.GetSHA256Hash(normalisedPath));

            Utils.TearDown(context);
        }
    }
}
