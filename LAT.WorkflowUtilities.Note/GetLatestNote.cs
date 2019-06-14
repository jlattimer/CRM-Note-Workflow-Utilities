using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace LAT.WorkflowUtilities.Note
{
    public class GetLatestNote : WorkFlowActivityBase
    {
        public GetLatestNote() : base(typeof(GetLatestNote)) { }

        [Input("Record Dynamic Url")]
        [RequiredArgument]
        public InArgument<string> RecordUrl { get; set; }

        [Output("Found Note")]
        [ReferenceTarget("annotation")]
        public OutArgument<EntityReference> FoundNote { get; set; }

        protected override void ExecuteCrmWorkFlowActivity(CodeActivityContext context, LocalWorkflowContext localContext)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (localContext == null)
                throw new ArgumentNullException(nameof(localContext));

            var recordUrl = RecordUrl.Get<string>(context);

            var dup = new DynamicUrlParser(recordUrl);

            var foundNote = GetNote(localContext.OrganizationService, dup.Id);

            FoundNote.Set(context, foundNote);
        }

        private static EntityReference GetNote(IOrganizationService service, Guid objectId)
        {
            var query = new FetchExpression($@"<fetch top='1' >
                                                      <entity name='annotation' >
                                                        <attribute name='annotationid' />
                                                        <filter type='and' >
                                                          <condition attribute='objectid' operator='eq' value='{objectId}' />
                                                        </filter>
                                                        <order attribute='createdon' descending='true' />
                                                      </entity>
                                                    </fetch>");

            var results = service.RetrieveMultiple(query);

            return results.Entities.Count == 1
                ? results.Entities[0].ToEntityReference()
                : null;
        }
    }
}
