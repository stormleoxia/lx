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

using System.IO;
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.Sync;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class SyncFactoryTest
    {
        [SetUp]
        public void Setup()
        {
            _projectFactory = new Mock<IProjectFactory>(MockBehavior.Strict);
            _console = new Mock<IConsole>(MockBehavior.Strict);
            _fileSystem = new Mock<IFileSystem>(MockBehavior.Strict);
            _validator = new Mock<IDirectoryValidator>(MockBehavior.Strict);
        }

        [TearDown]
        public void TearDown()
        {
            _validator.VerifyAll();
            _projectFactory.VerifyAll();
            _console.VerifyAll();
            _fileSystem.VerifyAll();
        }

        private Mock<IProjectFactory> _projectFactory;
        private Mock<IFileSystem> _fileSystem;
        private Mock<IConsole> _console;
        private Mock<IDirectoryValidator> _validator;

        [Test]
        public void CreateDirectorySyncTest()
        {
            _fileSystem.Setup(x => x.GetFiles("directoryPath", "*.csproj", SearchOption.AllDirectories))
                .Returns(new[] {"file1.csproj", "file2.csproj"});
            _validator.Setup(x => x.IsDirectoryValid("file1.csproj")).Returns(true);
            _validator.Setup(x => x.IsDirectoryValid("file2.csproj")).Returns(true);
            _projectFactory.Setup(x => x.IsValidProject("file1.csproj")).Returns(true);
            _projectFactory.Setup(x => x.IsValidProject("file2.csproj")).Returns(true);
            var factory = new SyncFactory(_projectFactory.Object, _console.Object, _fileSystem.Object, _validator.Object);
            var sync = factory.CreateDirectorySynchronizer("directoryPath");
            Assert.IsNotNull(sync);
        }

        [Test]
        public void CreateProjecSyncTest()
        {
            var factory = new SyncFactory(_projectFactory.Object, _console.Object, _fileSystem.Object, _validator.Object);
            var sync = factory.CreateProjectSynchronizer("filePath");
            Assert.IsNotNull(sync);
        }
    }
}