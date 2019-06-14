using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace LAT.WorkflowUtilities.Note
{
    public sealed class UpdateNoteTitle : WorkFlowActivityBase
    {
        public UpdateNoteTitle() : base(typeof(UpdateNoteTitle)) { }

        [RequiredArgument]
        [Input("Note To Update")]
        [ReferenceTarget("annotation")]
        public InArgument<EntityReference> NoteToUpdate { get; set; }

        [RequiredArgument]
        [Input("New Title")]
        public InArgument<string> NewTitle { get; set; }

        [Output("Updated Title")]
        public OutArgument<string> UpdatedTitle { get; set; }

        protected override void ExecuteCrmWorkFlowActivity(CodeActivityContext context, LocalWorkflowContext localContext)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (localContext == null)
                throw new ArgumentNullException(nameof(localContext));

            EntityReference noteToUpdate = NoteToUpdate.Get(context);
            if (noteToUpdate == null)
                throw new ArgumentNullException("Note cannot be null");

            string newTitle = NewTitle.Get(context);

            Entity note = new Entity("annotation")
            {
                Id = noteToUpdate.Id,
                ["subject"] = newTitle
            };
            localContext.OrganizationService.Update(note);

            UpdatedTitle.Set(context, newTitle);
        }
    }
}