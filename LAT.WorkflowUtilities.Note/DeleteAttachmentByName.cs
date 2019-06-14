using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Text;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace LAT.WorkflowUtilities.Note
{
    public sealed class DeleteAttachmentByName : WorkFlowActivityBase
    {
        public DeleteAttachmentByName() : base(typeof(DeleteAttachmentByName)) { }

        [RequiredArgument]
        [Input("Note With Attachment To Remove")]
        [ReferenceTarget("annotation")]
        public InArgument<EntityReference> NoteWithAttachment { get; set; }

        [RequiredArgument]
        [Input("File Name With Extension")]
        public InArgument<string> FileName { get; set; }

        [RequiredArgument]
        [Input("Add Delete Notice As Text?")]
        [Default("false")]
        public InArgument<bool> AppendNotice { get; set; }

        [Output("Number Of Attachments Deleted")]
        public OutArgument<int> NumberOfAttachmentsDeleted { get; set; }

        protected override void ExecuteCrmWorkFlowActivity(CodeActivityContext context, LocalWorkflowContext localContext)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (localContext == null)
                throw new ArgumentNullException(nameof(localContext));

            EntityReference noteWithAttachment = NoteWithAttachment.Get(context);
            if (noteWithAttachment == null)
                throw new ArgumentNullException("Note cannot be null");

            string fileName = FileName.Get(context);
            bool appendNotice = AppendNotice.Get(context);

            Entity note = GetNote(localContext.OrganizationService, noteWithAttachment.Id);
            if (!CheckForAttachment(note))
                return;

            StringBuilder notice = new StringBuilder();
            int numberOfAttachmentsDeleted = 0;

            if (String.Equals(note.GetAttributeValue<string>("filename"), fileName, StringComparison.CurrentCultureIgnoreCase))
            {
                numberOfAttachmentsDeleted++;

                if (appendNotice)
                    notice.AppendLine("Deleted Attachment: " + note.GetAttributeValue<string>("filename") + " " +
                                      DateTime.Now.ToShortDateString());

                UpdateNote(localContext.OrganizationService, note, notice.ToString());
            }

            NumberOfAttachmentsDeleted.Set(context, numberOfAttachmentsDeleted);
        }

        private static bool CheckForAttachment(Entity note)
        {
            bool hasValue = note.Attributes.TryGetValue("isdocument", out var oIsAttachment);
            if (!hasValue)
                return false;

            return (bool)oIsAttachment;
        }

        private static Entity GetNote(IOrganizationService service, Guid noteId)
        {
            return service.Retrieve("annotation", noteId, new ColumnSet("filename", "isdocument", "notetext"));
        }

        private static void UpdateNote(IOrganizationService service, Entity note, string notice)
        {
            Entity updateNote = new Entity("annotation") { Id = note.Id };
            if (!string.IsNullOrEmpty(notice))
            {
                string newText = note.GetAttributeValue<string>("notetext");
                if (!string.IsNullOrEmpty(newText))
                    newText += "\r\n";

                updateNote["notetext"] = newText + notice;
            }
            updateNote["isdocument"] = false;
            updateNote["filename"] = null;
            updateNote["documentbody"] = null;
            updateNote["filesize"] = null;

            service.Update(updateNote);
        }
    }
}