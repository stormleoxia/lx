#region Copyright (c) 2015 Leoxia Ltd

// #region Copyright (c) 2015 Leoxia Ltd
// 
// // Copyright © 2015 Leoxia Ltd.
// // 
// // This file is part of Lx.
// //
// // Lx is released under GNU General Public License unless stated otherwise.
// // You may not use this file except in compliance with the License.
// // You can redistribute it and/or modify it under the terms of the GNU General Public License 
// // as published by the Free Software Foundation, either version 3 of the License, 
// // or any later version.
// // 
// // In case GNU General Public License is not applicable for your use of Lx, 
// // you can subscribe to commercial license on 
// // http://www.leoxia.com 
// // by contacting us through the form page or send us a mail
// // mailto:contact@leoxia.com
// //  
// // Unless required by applicable law or agreed to in writing, 
// // Lx is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES 
// // OR CONDITIONS OF ANY KIND, either express or implied. 
// // See the GNU General Public License for more details.
// //
// // You should have received a copy of the GNU General Public License along with Lx.
// // It is present in the Lx root folder SolutionItems/GPL.txt
// // If not, see http://www.gnu.org/licenses/.
// //
// 
// #endregion

#endregion

using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.SourceDump;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.SourceDump
{
    [TestFixture]
    public class SourceDumperOptionsTest
    {
        [SetUp]
        public void Setup()
        {
            _environment = new Mock<IEnvironment>();
            _console = new Mock<IConsole>();
            _options = new SourceDumperOptions(_environment.Object, _console.Object);
        }

        private SourceDumperOptions _options;
        private Mock<IEnvironment> _environment;
        private Mock<IConsole> _console;

        [Test]
        public void GetOptionsShouldDetectAbsolutePath()
        {
            var arguments = new[] {"-unix-paths", "--absolute-paths"};
            var options = _options.ParseOptions(arguments);
            Assert.IsNotNull(options);
            Assert.AreEqual(1, options.Count);
            Assert.IsTrue(options.Contains(SourceDumperOptions.AbsolutePaths));
            Assert.IsNotNull(arguments[0]);
            Assert.IsNull(arguments[1]);
        }

        [Test]
        public void GetOptionsShouldDetectOutputFile()
        {
            var arguments = new[] {"myfiles", "--output-file"};
            var options = _options.ParseOptions(arguments);
            Assert.IsNotNull(options);
            Assert.AreEqual(1, options.Count);
            Assert.IsTrue(options.Contains(SourceDumperOptions.OutputFile));
            Assert.IsNotNull(arguments[0]);
            Assert.IsNull(arguments[1]);
        }

        [Test]
        public void GetOptionsShouldDetectRelativePath()
        {
            var arguments = new[] {"-unix-paths", "--relative-paths"};
            var options = _options.ParseOptions(arguments);
            Assert.IsNotNull(options);
            Assert.AreEqual(1, options.Count);
            Assert.IsTrue(options.Contains(SourceDumperOptions.RelativePaths));
            Assert.IsNotNull(arguments[0]);
            Assert.IsNull(arguments[1]);
        }

        [Test]
        public void GetOptionsShouldDetectUnixPath()
        {
            var arguments = new[] {"--unix-paths", "--myfile"};
            var options = _options.ParseOptions(arguments);
            Assert.IsNotNull(options);
            Assert.AreEqual(1, options.Count);
            Assert.IsTrue(options.Contains(SourceDumperOptions.UnixPaths));
            Assert.IsNull(arguments[0]);
            Assert.IsNotNull(arguments[1]);
        }

        [Test]
        public void GetOptionsShouldDetectWinPath()
        {
            var arguments = new[] {"-unix-paths", "--windows-paths"};
            var options = _options.ParseOptions(arguments);
            Assert.IsNotNull(options);
            Assert.AreEqual(1, options.Count);
            Assert.IsTrue(options.Contains(SourceDumperOptions.WindowsPaths));
            Assert.IsNotNull(arguments[0]);
            Assert.IsNull(arguments[1]);
        }
    }
}