using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace LAT.WorkflowUtilities.Note
{
    public sealed class MoveNote : WorkFlowActivityBase
    {
        public MoveNote() : base(typeof(MoveNote)) { }

        [RequiredArgument]
        [Input("Note To Move")]
        [ReferenceTarget("annotation")]
        public InArgument<EntityReference> NoteToMove { get; set; }

        [Input("Record Dynamic Url")]
        [RequiredArgument]
        public InArgument<string> RecordUrl { get; set; }

        [Output("Was Note Moved")]
        public OutArgument<bool> WasNoteMoved { get; set; }

        protected override void ExecuteCrmWorkFlowActivity(CodeActivityContext context, LocalWorkflowContext localContext)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (localContext == null)
                throw new ArgumentNullException(nameof(localContext));

            EntityReference noteToMove = NoteToMove.Get(context);
            if (noteToMove == null)
                throw new ArgumentNullException("Note cannot be null");

            string recordUrl = RecordUrl.Get<string>(context);

            var dup = new DynamicUrlParser(recordUrl);

            string newEntityLogical = dup.GetEntityLogicalName(localContext.OrganizationService);

            Entity note = GetNote(localContext.OrganizationService, noteToMove.Id);
            if (note.GetAttributeValue<EntityReference>("objectid").Id == dup.Id
                && note.GetAttributeValue<EntityReference>("objectid").LogicalName == newEntityLogical)
            {
                WasNoteMoved.Set(context, false);
                return;
            }

            Entity updateNote = new Entity("annotation")
            {
                Id = noteToMove.Id,
                ["objectid"] = new EntityReference(newEntityLogical, dup.Id)
            };

            localContext.OrganizationService.Update(updateNote);

            WasNoteMoved.Set(context, true);
        }

        private static Entity GetNote(IOrganizationService service, Guid noteId)
        {
            return service.Retrieve("annotation", noteId, new ColumnSet("objectid", "objecttypecode"));
        }
    }
}