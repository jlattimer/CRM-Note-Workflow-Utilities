﻿using FakeXrmEasy;
using FakeXrmEasy.FakeMessageExecutors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;

namespace LAT.WorkflowUtilities.Note.Tests
{
    [TestClass]
    public class CopyNoteTests
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
        public void CopyNote_Copy_To_New_Record()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["objectid"] = new EntityReference("contact", Guid.NewGuid())
            };

            var inputs = new Dictionary<string, object>
            {
                { "NoteToCopy", note.ToEntityReference() },
                { "RecordUrl", "https://test.crm.dynamics.com:443/main.aspx?etc=1&id=ba166c72-5f7b-e611-80db-fc15b4282d80&histKey=694632068&newWindow=true&pagetype=entityrecord" },
                { "CopyAttachment", true }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note });
            var fakeRetrieveMetadataChangesRequest = new FakeRetrieveMetadataChangesRequestExecutor();
            xrmFakedContext.AddFakeMessageExecutor<RetrieveMetadataChangesRequest>(fakeRetrieveMetadataChangesRequest);

            const bool expected = true;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<CopyNote>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["WasNoteCopied"]);
        }

        [TestMethod]
        public void CopyNote_Copy_To_Same_Record()
        {
            //Arrange
            XrmFakedWorkflowContext workflowContext = new XrmFakedWorkflowContext();

            Entity note = new Entity("annotation")
            {
                Id = Guid.NewGuid(),
                ["objectid"] = new EntityReference("account", new Guid("ba166c72-5f7b-e611-80db-fc15b4282d80"))
            };

            var inputs = new Dictionary<string, object>
            {
                { "NoteToCopy", note.ToEntityReference() },
                { "RecordUrl", "https://test.crm.dynamics.com:443/main.aspx?etc=1&id=ba166c72-5f7b-e611-80db-fc15b4282d80&histKey=694632068&newWindow=true&pagetype=entityrecord" },
                { "CopyAttachment", true }
            };

            XrmFakedContext xrmFakedContext = new XrmFakedContext();
            xrmFakedContext.Initialize(new List<Entity> { note });
            var fakeRetrieveMetadataChangesRequest = new FakeRetrieveMetadataChangesRequestExecutor();
            xrmFakedContext.AddFakeMessageExecutor<RetrieveMetadataChangesRequest>(fakeRetrieveMetadataChangesRequest);

            const bool expected = false;

            //Act
            var result = xrmFakedContext.ExecuteCodeActivity<CopyNote>(workflowContext, inputs);

            //Assert
            Assert.AreEqual(expected, result["WasNoteCopied"]);
        }

        private class FakeRetrieveMetadataChangesRequestExecutor : IFakeMessageExecutor
        {
            public bool CanExecute(OrganizationRequest request)
            {
                return request is RetrieveMetadataChangesRequest;
            }

            public Type GetResponsibleRequestType()
            {
                return typeof(RetrieveMetadataChangesRequest);
            }

            public OrganizationResponse Execute(OrganizationRequest request, XrmFakedContext ctx)
            {
                OrganizationResponse response = new OrganizationResponse();
                EntityMetadataCollection metadataCollection = new EntityMetadataCollection();
                EntityMetadata entityMetadata = new EntityMetadata { LogicalName = "account" };
                metadataCollection.Add(entityMetadata);
                ParameterCollection results = new ParameterCollection { { "EntityMetadata", metadataCollection } };
                response.Results = results;

                return response;
            }
        }
    }
}