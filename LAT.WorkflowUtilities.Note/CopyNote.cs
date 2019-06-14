using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace LAT.WorkflowUtilities.Note
{
    public sealed class CopyNote : WorkFlowActivityBase
    {
        public CopyNote() : base(typeof(CopyNote)) { }

        [RequiredArgument]
        [Input("Note To Copy")]
        [ReferenceTarget("annotation")]
        public InArgument<EntityReference> NoteToCopy { get; set; }

        [Input("Record Dynamic Url")]
        [RequiredArgument]
        public InArgument<string> RecordUrl { get; set; }

        [RequiredArgument]
        [Default("True")]
        [Input("Copy Attachment?")]
        public InArgument<bool> CopyAttachment { get; set; }

        [Output("Was Note Copied")]
        public OutArgument<bool> WasNoteCopied { get; set; }

        protected override void ExecuteCrmWorkFlowActivity(CodeActivityContext context, LocalWorkflowContext localContext)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (localContext == null)
                throw new ArgumentNullException(nameof(localContext));

            EntityReference noteToCopy = NoteToCopy.Get(context);
            if (noteToCopy == null)
                throw new ArgumentNullException("Note cannot be null");

            string recordUrl = RecordUrl.Get<string>(context);
            bool copyAttachment = CopyAttachment.Get(context);

            var dup = new DynamicUrlParser(recordUrl);

            string newEntityLogical = dup.GetEntityLogicalName(localContext.OrganizationService);

            Entity note = GetNote(localContext.OrganizationService, noteToCopy.Id);
            if (note.GetAttributeValue<EntityReference>("objectid").Id == dup.Id && note.GetAttributeValue<EntityReference>("objectid").LogicalName == newEntityLogical)
            {
                WasNoteCopied.Set(context, false);
                return;
            }

            Entity newNote = new Entity("annotation")
            {
                ["objectid"] = new EntityReference(newEntityLogical, dup.Id),
                ["notetext"] = note.GetAttributeValue<string>("notetext"),
                ["subject"] = note.GetAttributeValue<string>("subject")
            };
            if (copyAttachment)
            {
                newNote["isdocument"] = note.GetAttributeValue<bool>("isdocument");
                newNote["filename"] = note.GetAttributeValue<string>("filename");
                newNote["filesize"] = note.GetAttributeValue<int>("filesize");
                newNote["documentbody"] = note.GetAttributeValue<string>("documentbody");
            }
            else
                newNote["isdocument"] = false;

            localContext.OrganizationService.Create(newNote);

            WasNoteCopied.Set(context, true);
        }

        private static Entity GetNote(IOrganizationService service, Guid noteId)
        {
            return service.Retrieve("annotation", noteId, new ColumnSet("objectid", "documentbody", "filename", "filesize", "isdocument", "notetext", "subject"));
        }
    }
}