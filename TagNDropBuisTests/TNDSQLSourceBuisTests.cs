using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagNDropBuis;
using TagNDropLibrary;

namespace TagNDropBuis.Tests {
    [TestClass()]
    public class TNDSQLSourceBuisTests {
        //[TestMethod()]
        //public void InitTest() {

        //}
        public static string TestConnectionString = (new System.Data.SqlClient.SqlConnectionStringBuilder() {
            DataSource = ".",
            InitialCatalog = "TagNDropDB",
            IntegratedSecurity = true
        }).ConnectionString;

        [TestMethod()]
        public async Task SearchTermTest_Kunde() {
            var sut = new TNDSQLSourceBuis();
            ITNDApplicationBuis applicationBuis = A.Fake<ITNDApplicationBuis>();
            TNDConfiguration configuration = new TNDConfiguration();
            var metaSourceKunde = new TNDMetaSource() {
                MetaSourceName = "SourceKunde",
                Configuration = (new System.Data.SqlClient.SqlConnectionStringBuilder() {
                    DataSource = ".",
                    InitialCatalog = "TagNDropDB",
                    IntegratedSecurity = true
                }).ConnectionString,
                AssemblyQualifiedName = "",
                SourceBuis = sut
            };
            var metaStorageKunde = new TNDMetaStorage() {
                MetaStorageName = "",
                RootPath = "",
                AssemblyQualifiedName = "",
                StorageBuis = null
            };
            var metaEntityKunde = new TNDMetaEntity() {
                MetaEntityLevel = 0,
                MetaEntityParent = null,
                MetaEntityName = "Kunde",
                MetaEntityParentName = null,
                MetaEntityParentNamePropertyName = null,
                MetaSource = metaSourceKunde,
                MetaSourceName = "SourceKunde",
                MetaStorage = metaStorageKunde,
                MetaStorageName = "StorageKunde"
            };
            configuration.MetaEntities = new TNDMetaEntity[] { metaEntityKunde };
            configuration.MetaSources = new TNDMetaSource[] { metaSourceKunde };
            configuration.MetaStorages = new TNDMetaStorage[] { metaStorageKunde };
            sut.Init(applicationBuis, metaSourceKunde);
            var result = await sut.SearchTerm(metaEntityKunde, "k",null,null);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            result = await sut.SearchTerm(metaEntityKunde, "aa", null, null);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }
        [TestMethod()]
        public async Task SearchTermTest_Produkt() {
            var sut = new TNDSQLSourceBuis();
            ITNDApplicationBuis applicationBuis = A.Fake<ITNDApplicationBuis>();
            TNDConfiguration configuration = new TNDConfiguration();
            var metaSourceKunde = new TNDMetaSource() {
                MetaSourceName = "SourceKunde",
                Configuration = TestConnectionString,
                AssemblyQualifiedName = "",
                SourceBuis = sut
            };
            var metaSourceProdukt = new TNDMetaSource() {
                MetaSourceName = "SourceProdukt",
                Configuration = TestConnectionString,
                AssemblyQualifiedName = "",
                SourceBuis = sut
            };
            var metaStorageKunde = new TNDMetaStorage() {
                MetaStorageName = "Kunde",
                RootPath = "",
                AssemblyQualifiedName = "",
                StorageBuis = null
            };
            var metaStorageProdukt = new TNDMetaStorage() {
                MetaStorageName = "Produkt",
                RootPath = "",
                AssemblyQualifiedName = "",
                StorageBuis = null
            };
            var metaEntityKunde = new TNDMetaEntity() {
                MetaEntityLevel = 0,
                MetaEntityParent = null,
                MetaEntityName = "Kunde",
                MetaEntityParentName = null,
                MetaEntityParentNamePropertyName = null,
                MetaSource = metaSourceKunde,
                MetaSourceName = "SourceKunde",
                MetaStorage = metaStorageKunde,
                MetaStorageName = "StorageKunde"
            };
            var metaEntityProdukt = new TNDMetaEntity() {
                MetaEntityLevel = 0,
                MetaEntityParent = null,
                MetaEntityName = "Produkt",
                MetaEntityParentName = null,
                MetaEntityParentNamePropertyName = null,
                MetaSource = metaSourceProdukt,
                MetaSourceName = "SourceProdukt",
                MetaStorage = metaStorageProdukt,
                MetaStorageName = "StorageProdukt"
            };
            //
            configuration.MetaEntities = new TNDMetaEntity[] { metaEntityKunde, metaEntityProdukt };
            configuration.MetaSources = new TNDMetaSource[] { metaSourceKunde, metaSourceProdukt };
            configuration.MetaStorages = new TNDMetaStorage[] { metaStorageKunde, metaStorageProdukt };
            sut.Init(applicationBuis, metaSourceProdukt);
            var result = await sut.SearchTerm(metaEntityProdukt, "k%", null, null);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
            result = await sut.SearchTerm(metaEntityProdukt, "a", null, null);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 0);
        }
    }
}