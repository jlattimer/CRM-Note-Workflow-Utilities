using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace LAT.WorkflowUtilities.Note.Tests
{
    [TestClass]
    public class DeleteAttachmentByNameTests
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

        [TestMethod]
        public void DeleteAttachmentByName_Found_No_Match()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note1 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["filesize"] = 5000,
                ["filename"] = "text.docx",
                ["isdocument"] = true
            };

            var inputs = new Dictionary<string, object>
            {
                { "NoteWithAttachment", note1.ToEntityReference() },
                { "FileName", "test.txt"},
                { "AppendNotice", false }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note1 });

            const int expected = 0;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<DeleteAttachmentByName>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["NumberOfAttachmentsDeleted"]);
        }

        [TestMethod]
        public void DeleteAttachmentByName_Found_One_Match()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note1 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["filesize"] = 5000,
                ["filename"] = "test.txt",
                ["isdocument"] = true
            };

            var inputs = new Dictionary<string, object>
            {
                { "NoteWithAttachment", note1.ToEntityReference() },
                { "FileName", "test.txt"},
                { "AppendNotice", false }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note1 });

            const int expected = 1;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<DeleteAttachmentByName>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["NumberOfAttachmentsDeleted"]);
        }

        [TestMethod]
        public void DeleteAttachmentByName_Not_A_Document()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note1 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["isdocument"] = false
            };

            var inputs = new Dictionary<string, object>
            {
                { "NoteWithAttachment", note1.ToEntityReference() },
                { "FileName", "test.txt"},
                { "AppendNotice", false }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note1 });

            const int expected = 0;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<DeleteAttachmentByName>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["NumberOfAttachmentsDeleted"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Note cannot be null")]
        public void DeleteAttachmentByName_Null_Note()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            var inputs = new Dictionary<string, object>
            {
                { "NoteToCheck", null }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();

            //Act
            xrmFakedContext.ExecuteCodeActivity<CheckAttachment>(workflowContext, inputs);
        }
    }
}