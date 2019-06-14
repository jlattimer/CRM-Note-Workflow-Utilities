using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace LAT.WorkflowUtilities.Note.Tests
{
    [TestClass]
    public class UpdateNoteTitleTests
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
        public void UpdateNoteTitle_Set_Title()
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
                { "NewTitle", "test" }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note });

            const string expected = "test";

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<UpdateNoteTitle>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["UpdatedTitle"]);
        }

        [TestMethod]
        public void UpdateNoteTitle_Set_Null_Title()
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
                { "NewTitle", null }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note });

            const string expected = null;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<UpdateNoteTitle>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["UpdatedTitle"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Note cannot be null")]
        public void UpdateNoteTitle_Null_Note()
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
