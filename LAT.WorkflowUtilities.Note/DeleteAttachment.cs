using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Text;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace LAT.WorkflowUtilities.Note
{
    public sealed class DeleteAttachment : WorkFlowActivityBase
    {
        public DeleteAttachment() : base(typeof(DeleteAttachment)) { }

        [RequiredArgument]
        [Input("Note With Attachments To Remove")]
        [ReferenceTarget("annotation")]
        public InArgument<EntityReference> NoteWithAttachment { get; set; }

        [Input("Delete >= Than X Bytes (Empty = 2,147,483,647)")]
        public InArgument<int> DeleteSizeMax { get; set; }

        [Input("Delete <= Than X Bytes (Empty = 0)")]
        public InArgument<int> DeleteSizeMin { get; set; }

        [Input("Limit To Extensions (Comma Delimited, Empty = Ignore)")]
        public InArgument<string> Extensions { get; set; }

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

            int deleteSizeMax = DeleteSizeMax.Get(context);
            int deleteSizeMin = DeleteSizeMin.Get(context);
            string extensions = Extensions.Get(context);
            bool appendNotice = AppendNotice.Get(context);

            if (deleteSizeMax == 0) deleteSizeMax = int.MaxValue;
            if (deleteSizeMin > deleteSizeMax)
            {
                localContext.TracingService.Trace("Exception: {0}", "Min:" + deleteSizeMin + " Max:" + deleteSizeMax);
                throw new InvalidPluginExecutionException("Minimum Size Cannot Be Greater Than Maximum Size");
            }

            Entity note = GetNote(localContext.OrganizationService, noteWithAttachment.Id);
            if (!CheckForAttachment(note))
                return;

            string[] filetypes = new string[0];
            if (!string.IsNullOrEmpty(extensions))
                filetypes = extensions.Replace(".", string.Empty).Split(',');

            StringBuilder notice = new StringBuilder();
            int numberOfAttachmentsDeleted = 0;

            bool delete = note.GetAttributeValue<int>("filesize") >= deleteSizeMax;

            if (note.GetAttributeValue<int>("filesize") <= deleteSizeMin)
                delete = true;

            if (filetypes.Length > 0 && delete)
                delete = ExtensionMatch(filetypes, note.GetAttributeValue<string>("filename"));

            if (delete)
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
            return service.Retrieve("annotation", noteId, new ColumnSet("filename", "filesize", "isdocument", "notetext"));
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

        private static bool ExtensionMatch(IEnumerable<string> extenstons, string filename)
        {
            foreach (string ex in extenstons)
            {
                if (filename.EndsWith("." + ex))
                    return true;
            }

            return false;
        }
    }
}