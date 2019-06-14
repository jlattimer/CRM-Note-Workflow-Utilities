using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace LAT.WorkflowUtilities.Note.Tests
{
    [TestClass]
    public class UpdateNoteTextTests
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
        public void UpdateNoteText_Set_Valid_Text()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note = new Entity("annotation")
            {
                Id = Guid.NewGuid()
            };

            var inputs = new Dictionary<string, object>
            {
                { "NoteToUpdate", note.ToEntityReference() },
                { "NewText", "test" }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note });

            const string expected = "test";

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<UpdateNoteText>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["UpdatedText"]);
        }

        [TestMethod]
        public void UpdateNoteText_Set_Null_Text()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note = new Entity("annotation")
            {
                Id = Guid.NewGuid()
            };

            var inputs = new Dictionary<string, object>
            {
                { "NoteToUpdate", note.ToEntityReference() },
                { "NewText", null }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note });

            const string expected = null;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<UpdateNoteText>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["UpdatedText"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Note cannot be null")]
        public void UpdateNoteText_Null_Note()
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