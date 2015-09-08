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
    public class DirectorySyncTest
    {
        [SetUp]
        public void Setup()
        {
            _factory = new Mock<ISyncFactory>(MockBehavior.Strict);
            _fileSystem = new Mock<IFileSystem>(MockBehavior.Strict);
            _validator = new Mock<IDirectoryValidator>(MockBehavior.Strict);
            _synchronizer45 = new Mock<ISynchronizer>(MockBehavior.Strict);
            _synchronizerAll = new Mock<ISynchronizer>(MockBehavior.Strict);
            _validator = new Mock<IDirectoryValidator>(MockBehavior.Strict);
            _projectFactory = new Mock<IProjectFactory>(MockBehavior.Strict);
        }

        [TearDown]
        public void TearDown()
        {
            _factory.VerifyAll();
            _validator.VerifyAll();
            _fileSystem.VerifyAll();
            _validator.VerifyAll();
            _synchronizer45.VerifyAll();
            _synchronizerAll.VerifyAll();
            _projectFactory.VerifyAll();
        }

        private Mock<IFileSystem> _fileSystem;
        private Mock<IDirectoryValidator> _validator;
        private Mock<ISyncFactory> _factory;
        private Mock<ISynchronizer> _synchronizer45;
        private Mock<ISynchronizer> _synchronizerAll;
        private Mock<IProjectFactory> _projectFactory;

        [Test]
        public void SynchronizeTest()
        {
            var fileNet45 = "file1-net_4_5";
            var fileAll = "file2";
            string[] array = {fileNet45, fileAll};
            _fileSystem.Setup(x => x.GetFiles("path", "*.csproj", SearchOption.AllDirectories)).Returns(array);
            _synchronizer45.Setup(x => x.Synchronize());
            _synchronizerAll.Setup(x => x.Synchronize());
            _factory.Setup(x => x.CreateProjectSynchronizer(fileNet45)).Returns(_synchronizer45.Object);
            _factory.Setup(x => x.CreateProjectSynchronizer(fileAll)).Returns(_synchronizerAll.Object);
            _validator.Setup(x => x.IsDirectoryValid("file1-net_4_5")).Returns(true);
            _validator.Setup(x => x.IsDirectoryValid("file2")).Returns(true);
            _projectFactory.Setup(x => x.IsValidProject("file1-net_4_5")).Returns(true);
            _projectFactory.Setup(x => x.IsValidProject("file2")).Returns(true);
            var directorySync = new DirectorySync("path", _factory.Object, _fileSystem.Object, _validator.Object,
                _projectFactory.Object);
            directorySync.Synchronize();
        }
    }
}