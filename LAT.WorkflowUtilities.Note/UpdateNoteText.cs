using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace LAT.WorkflowUtilities.Note
{
    public sealed class UpdateNoteText : WorkFlowActivityBase
    {
        public UpdateNoteText() : base(typeof(UpdateNoteText)) { }

        [RequiredArgument]
        [Input("Note To Update")]
        [ReferenceTarget("annotation")]
        public InArgument<EntityReference> NoteToUpdate { get; set; }

        [RequiredArgument]
        [Input("New Text")]
        public InArgument<string> NewText { get; set; }

        [Output("Updated Text")]
        public OutArgument<string> UpdatedText { get; set; }

        protected override void ExecuteCrmWorkFlowActivity(CodeActivityContext context, LocalWorkflowContext localContext)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (localContext == null)
                throw new ArgumentNullException(nameof(localContext));

            EntityReference noteToUpdate = NoteToUpdate.Get(context);
            if (noteToUpdate == null)
                throw new ArgumentNullException("Note cannot be null");

            string newText = NewText.Get(context);

            Entity note = new Entity("annotation")
            {
                Id = noteToUpdate.Id,
                ["notetext"] = newText
            };
            localContext.OrganizationService.Update(note);

            UpdatedText.Set(context, newText);
        }
    }
}