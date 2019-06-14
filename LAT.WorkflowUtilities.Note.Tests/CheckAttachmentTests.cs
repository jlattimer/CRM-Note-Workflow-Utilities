using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace LAT.WorkflowUtilities.Note.Tests
{
    [TestClass]
    public class CheckAttachmentTests
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
        public void CheckAttachment_Has_Attachment()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["documentbody"] = "412321",
                ["filesize"] = 1,
                ["filename"] = "test.txt",
                ["isdocument"] = true
            };

            var inputs = new Dictionary<string, object>
            {
                { "NoteToCheck", note.ToEntityReference() }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note });

            const bool expected = true;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<CheckAttachment>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["HasAttachment"]);
        }

        [TestMethod]
        public void CheckAttachment_No_Attachment()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["isdocument"] = false,
            };

            var inputs = new Dictionary<string, object>
            {
                { "NoteToCheck", note.ToEntityReference() }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(note);

            const bool expected = false;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<CheckAttachment>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["HasAttachment"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Note cannot be null")]
        public void CheckAttachment_Null_Note()
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