using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace LAT.WorkflowUtilities.Note.Tests
{
    [TestClass]
    public class DeleteAttachmentTests
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
        public void DeleteAttachment_Delete_From_0_To_10000_No_Extensions()
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
                { "DeleteSizeMax", 10000},
                { "DeleteSizeMin", 0 },
                { "Extensions" , null },
                { "AppendNotice", false }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note1 });

            const int expected = 0;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<DeleteAttachment>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["NumberOfAttachmentsDeleted"]);
        }

        [TestMethod]
        public void DeleteAttachment_Delete_From_0_To_100000_No_Extensions()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note1 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["filesize"] = 100000,
                ["filename"] = "text.docx",
                ["isdocument"] = true
            };

            var inputs = new Dictionary<string, object>
            {
                { "NoteWithAttachment", note1.ToEntityReference() },
                { "DeleteSizeMax", 10000},
                { "DeleteSizeMin", 0 },
                { "Extensions" , null },
                { "AppendNotice", false }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note1 });

            const int expected = 1;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<DeleteAttachment>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["NumberOfAttachmentsDeleted"]);
        }

        [TestMethod]
        public void DeleteAttachment_Delete_0_From_10000_To_Max_No_Extensions()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note1 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["filesize"] = 50000,
                ["filename"] = "text.docx",
                ["isdocument"] = true
            };

            var inputs = new Dictionary<string, object>
            {
                { "NoteWithAttachment", note1.ToEntityReference() },
                { "DeleteSizeMax", 0},
                { "DeleteSizeMin", 10000 },
                { "Extensions" , null },
                { "AppendNotice", false }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note1 });

            const int expected = 0;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<DeleteAttachment>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["NumberOfAttachmentsDeleted"]);
        }

        [TestMethod]
        public void DeleteAttachment_Delete_1_From_10000_To_Max_No_Extensions()
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
                { "DeleteSizeMax", 0},
                { "DeleteSizeMin", 10000 },
                { "Extensions" , null },
                { "AppendNotice", false }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note1 });

            const int expected = 1;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<DeleteAttachment>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["NumberOfAttachmentsDeleted"]);
        }

        [TestMethod]
        public void DeleteAttachment_Delete_0_From_3000_To_75000_No_Extensions()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note1 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["filesize"] = 5000,
                ["filename"] = "text1.docx",
                ["isdocument"] = true
            };

            var inputs = new Dictionary<string, object>
            {
                { "NoteWithAttachment", note1.ToEntityReference() },
                { "DeleteSizeMax", 75000},
                { "DeleteSizeMin", 3000 },
                { "Extensions" , null },
                { "AppendNotice", false }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note1 });

            const int expected = 0;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<DeleteAttachment>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["NumberOfAttachmentsDeleted"]);
        }

        [TestMethod]
        public void DeleteAttachment_Delete_1_From_3000_To_75000_No_Extensions()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note1 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["filesize"] = 500000,
                ["filename"] = "text1.docx",
                ["isdocument"] = true
            };

            var inputs = new Dictionary<string, object>
            {
                { "NoteWithAttachment", note1.ToEntityReference() },
                { "DeleteSizeMax", 75000},
                { "DeleteSizeMin", 3000 },
                { "Extensions" , null },
                { "AppendNotice", false }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note1 });

            const int expected = 1;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<DeleteAttachment>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["NumberOfAttachmentsDeleted"]);
        }

        [TestMethod]
        public void DeleteAttachment_Delete_0_From_0_To_1_Pdf_Extension_100000()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note1 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["filesize"] = 100000,
                ["filename"] = "text.docx",
                ["isdocument"] = true
            };

            var inputs = new Dictionary<string, object>
            {
                { "NoteWithAttachment", note1.ToEntityReference() },
                { "DeleteSizeMax", 1},
                { "DeleteSizeMin", 0 },
                { "Extensions" , "pdf" },
                { "AppendNotice", false }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note1 });

            const int expected = 0;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<DeleteAttachment>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["NumberOfAttachmentsDeleted"]);
        }

        [TestMethod]
        public void DeleteAttachment_Delete_1_From_0_To_1_Pdf_Extension()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note1 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["filesize"] = 100000,
                ["filename"] = "text.pdf",
                ["isdocument"] = true
            };

            var inputs = new Dictionary<string, object>
            {
                { "NoteWithAttachment", note1.ToEntityReference() },
                { "DeleteSizeMax", 1},
                { "DeleteSizeMin", 0 },
                { "Extensions" , "pdf" },
                { "AppendNotice", false }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note1 });

            const int expected = 1;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<DeleteAttachment>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["NumberOfAttachmentsDeleted"]);
        }

        [TestMethod]
        public void DeleteAttachment_Delete_0_From_0_To_1_Pdf_Extension_5000()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note1 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["filesize"] = 5000,
                ["filename"] = "text1.pdf",
                ["isdocument"] = true
            };

            var inputs = new Dictionary<string, object>
            {
                { "NoteWithAttachment", note1.ToEntityReference() },
                { "DeleteSizeMax", 1},
                { "DeleteSizeMin", 0 },
                { "Extensions" , "pdf" },
                { "AppendNotice", false }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note1 });

            const int expected = 1;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<DeleteAttachment>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["NumberOfAttachmentsDeleted"]);
        }

        [TestMethod]
        public void DeleteAttachment_Delete_From_3000_To_75000_Pdf_Extension()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note1 = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["filesize"] = 5000,
                ["filename"] = "text1.pdf",
                ["isdocument"] = true
            };

            var inputs = new Dictionary<string, object>
            {
                { "NoteWithAttachment", note1.ToEntityReference()  },
                { "DeleteSizeMax", 75000},
                { "DeleteSizeMin", 3000 },
                { "Extensions" , "pdf" },
                { "AppendNotice", false }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note1 });

            const int expected = 0;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<DeleteAttachment>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["NumberOfAttachmentsDeleted"]);
        }

        [TestMethod]
        public void DeleteAttachment_Not_Document()
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
                { "NoteWithAttachment", note1.ToEntityReference()  },
                { "DeleteSizeMax", 75000},
                { "DeleteSizeMin", 3000 },
                { "Extensions" , "pdf" },
                { "AppendNotice", false }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note1 });

            const int expected = 0;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<DeleteAttachment>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["NumberOfAttachmentsDeleted"]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Note cannot be null")]
        public void DeleteAttachment_Null_Note()
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
