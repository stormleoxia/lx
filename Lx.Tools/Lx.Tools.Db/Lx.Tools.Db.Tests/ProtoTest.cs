using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Lx.Tools.Db.Proto;
using NUnit.Framework;

namespace Lx.Tools.Db.Tests
{
    /// <summary>
    /// Not really Unit tests since it uses the file system
    /// However it presents ways to use LxDb
    /// </summary>
    [TestFixture]
    public class ProtoTest
    {
        private const string DatabaseFile = "test.db";

        [SetUp]
        public void Test()
        {
            if (File.Exists(DatabaseFile))
            {
                File.Delete(DatabaseFile);
            }
        }

        /// <summary>
        /// Save an item during a session will save it permanently.
        /// Note: Save is not like add, but more like add or update
        /// </summary>
        [Test]
        public void UsageSaveDbTest()
        {            
            LxDb db = new LxDb(DatabaseFile);
            using (var session = db.OpenSession())
            {
                session.Save("10", new MyContainer
                {
                    Name = "MyName",
                    Id = 15,
                    OtherIds = new List<int> {1, 2, 3}
                });
                var retrieved = session.Get<MyContainer>("10");
                CheckExists(retrieved);
            }
            db = new LxDb(DatabaseFile);
            using (var session = db.OpenSession())
            {
                var retrieved = session.Get<MyContainer>("10");
                CheckExists(retrieved);
            }
        }

        [Test]
        public void UsageRemoveTest()
        {
            LxDb db = new LxDb(DatabaseFile);
            using (var session = db.OpenSession())
            {
                session.Save("10", new MyContainer
                {
                    Name = "MyName",
                    Id = 15,
                    OtherIds = new List<int> {1, 2, 3}
                });
                var retrieved = session.Get<MyContainer>("10");
                CheckExists(retrieved);
                session.Remove("10");
                retrieved = session.Get<MyContainer>("10");
                Assert.IsNull(retrieved);
            }
        }

        [Test, ExpectedException(typeof (SerializationException))]
        public void UsageGetWithNotSameTypeTest()
        {
            LxDb db = new LxDb(DatabaseFile);
            using (var session = db.OpenSession())
            {
                session.Save("10", new MyContainer
                {
                    Name = "MyName",
                    Id = 15,
                    OtherIds = new List<int> {1, 2, 3}
                });
                // Exception.Message mentions the type that has been stored.
                session.Get<MyOtherContainer>("10");
            }
        }

        private static void CheckExists(MyContainer retrieved)
        {
            Assert.IsNotNull(retrieved);
            Assert.AreEqual("MyName", retrieved.Name);
            Assert.AreEqual(15, retrieved.Id);
            Assert.IsNotNull(retrieved.OtherIds);
            Assert.AreEqual(3, retrieved.OtherIds.Count);
        }

        /// <summary>
        /// Check Rollback removes any newly added items after last commit.
        /// </summary>
        [Test]
        public void UseRollbackTest()
        {
            LxDb db = new LxDb(DatabaseFile);
            using (var session = db.OpenSession())
            {
                var retrieved = session.Get<MyContainer>("10");
                Assert.IsNull(retrieved);
                session.Save("11", new MyContainer
                {
                    Name = "MyName",
                    Id = 15,
                    OtherIds = new List<int> {1, 2, 3}
                });
                session.Commit();
                session.Save("10", new MyContainer
                {
                    Name = "MyName",
                    Id = 15,
                    OtherIds = new List<int> {1, 2, 3}
                });
                session.Rollback();
                retrieved = session.Get<MyContainer>("10");
                Assert.IsNull(retrieved);
                retrieved = session.Get<MyContainer>("11");
                CheckExists(retrieved);
            }
        }

        /// <summary>
        /// Check Commit save permanently the data.
        /// </summary>
        [Test]
        public void UseCommitWithRemoveTest()
        {
            LxDb db = new LxDb(DatabaseFile);
            using (var session = db.OpenSession())
            {
                session.Save("10", new MyContainer
                {
                    Name = "MyName",
                    Id = 15,
                    OtherIds = new List<int> {1, 2, 3}
                });
                session.Commit();
                session.Remove("10");
                var retrieved = session.Get<MyContainer>("10");
                Assert.IsNull(retrieved);
            }
            using (var session = db.OpenSession())
            {
                var retrieved = session.Get<MyContainer>("10");
                CheckExists(retrieved);
            }
        }
    }

    public class MyOtherContainer
    {
    }

    public class MyContainer
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<int> OtherIds { get; set; }
    }
    

    
}
