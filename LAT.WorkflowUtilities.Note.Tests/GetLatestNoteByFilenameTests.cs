using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace LAT.WorkflowUtilities.Note.Tests
{
    [TestClass]
    public class GetLatestNoteByFilenameTests
    {
        #region Test Initialization and Cleanup
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) { }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup]
        public static void ClassCleanup() { }

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void TestMethodInitialize() { }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void TestMethodCleanup() { }
        #endregion

        private const string Filename = "Account Summary.docx";
        private static readonly Guid SourceRecordId = new Guid("3439550f-0e71-e911-a819-000d3a3b59f9");
        private readonly string _sourceRecordDynamicUrl =
            $"https://test.crm.dynamics.com:443/main.aspx?etc=1&id={SourceRecordId.ToString()}&histKey=912214339&newWindow=true&pagetype=entityrecord";

        [TestMethod]
        public void GetLatestNoteByFilename_Multiple_Same_Regarding_Same_Filenames()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note1 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["createdon"] = new DateTime(2019, 6, 13, 20, 24, 1),
                ["objectid"] = SourceRecordId,
                ["filename"] = Filename
            };

            Entity note2 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["createdon"] = new DateTime(2019, 6, 13, 20, 24, 5),
                ["objectid"] = SourceRecordId,
                ["filename"] = Filename
            };

            Entity note3 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["createdon"] = new DateTime(2019, 6, 12, 20, 24, 5),
                ["objectid"] = SourceRecordId,
                ["filename"] = Filename
            };

            var inputs = new Dictionary<string, object>
            {
                { "RecordUrl", _sourceRecordDynamicUrl },
                { "Filename", Filename }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note1, note2, note3 });

            EntityReference expected = note2.ToEntityReference();

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<GetLatestNoteByFilename>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected.Id, ((EntityReference)result["FoundNote"]).Id);
        }

        [TestMethod]
        public void GetLatestNoteByFilename_Multiple_Same_Regarding_Different_Filenames()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note1 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["createdon"] = new DateTime(2019, 6, 13, 20, 24, 1),
                ["objectid"] = SourceRecordId,
                ["filename"] = Filename
            };

            Entity note2 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["createdon"] = new DateTime(2019, 6, 13, 20, 24, 5),
                ["objectid"] = SourceRecordId,
                ["filename"] = Filename
            };

            Entity note3 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["createdon"] = new DateTime(2019, 6, 13, 20, 24, 5),
                ["objectid"] = SourceRecordId,
                ["filename"] = "Some Other File.pdf"
            };

            var inputs = new Dictionary<string, object>
            {
                { "RecordUrl", _sourceRecordDynamicUrl },
                { "Filename", Filename }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note1, note2, note3 });

            EntityReference expected = note2.ToEntityReference();

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<GetLatestNoteByFilename>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected.Id, ((EntityReference)result["FoundNote"]).Id);
        }

        [TestMethod]
        public void GetLatestNoteByFilename_Multiple_Different_Regarding_Same_Filename()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note1 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["createdon"] = new DateTime(2019, 6, 13, 20, 24, 1),
                ["objectid"] = SourceRecordId,
                ["filename"] = Filename
            };

            Entity note2 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["createdon"] = new DateTime(2019, 6, 13, 20, 24, 1),
                ["objectid"] = Guid.NewGuid(),
                ["filename"] = Filename
            };

            Entity note3 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["createdon"] = new DateTime(2019, 6, 12, 20, 24, 5),
                ["objectid"] = SourceRecordId,
                ["filename"] = Filename
            };

            var inputs = new Dictionary<string, object>
            {
                { "RecordUrl", _sourceRecordDynamicUrl },
                { "Filename", Filename }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note1, note2, note3 });

            EntityReference expected = note1.ToEntityReference();

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<GetLatestNoteByFilename>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected.Id, ((EntityReference)result["FoundNote"]).Id);
        }

        [TestMethod]
        public void GetLatestNoteByFilename_Multiple_Different_Regarding_Different_Filename()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note1 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["createdon"] = new DateTime(2019, 6, 13, 20, 24, 1),
                ["objectid"] = SourceRecordId,
                ["filename"] = Filename
            };

            Entity note2 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["createdon"] = new DateTime(2019, 6, 13, 20, 24, 1),
                ["objectid"] = Guid.NewGuid(),
                ["filename"] = "Some Other File.pdf"
            };

            Entity note3 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["createdon"] = new DateTime(2019, 6, 12, 20, 24, 5),
                ["objectid"] = SourceRecordId,
                ["filename"] = Filename
            };

            var inputs = new Dictionary<string, object>
            {
                { "RecordUrl", _sourceRecordDynamicUrl },
                { "Filename", Filename }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note1, note2, note3 });

            EntityReference expected = note1.ToEntityReference();

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<GetLatestNoteByFilename>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected.Id, ((EntityReference)result["FoundNote"]).Id);
        }
    }
}