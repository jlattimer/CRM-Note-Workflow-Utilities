using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace LAT.WorkflowUtilities.Note.Tests
{
    [TestClass]
    public class DeleteNoteTests
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
        public void DeleteNote_Delete()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note = new Entity("annotation")
            {
                Id = Guid.NewGuid()
            };

            var inputs = new Dictionary<string, object>
            {
                { "NoteToDelete", note.ToEntityReference() }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note });

            const bool expected = true;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<DeleteNote>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["WasNoteDeleted"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Note cannot be null")]
        public void DeleteNote_Null_Note()
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