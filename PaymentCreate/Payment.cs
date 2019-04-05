using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Crm.Sdk.Messages;

namespace PaymentCreate
{
    public class Payment : CodeActivity
    {

        [Input("Terms")]
        public InArgument<string> Terms { get; set; }


        protected override void Execute(CodeActivityContext executionContext)
        {
            //Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            //Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            int terms = Convert.ToInt32(Terms.Get(executionContext));


           
            ExecuteMultipleRequest request = new ExecuteMultipleRequest();
          
            
            
            Entity payment;
            for (int i = 0; i < terms; i++)
            {

                payment = new Entity("biz_mortgagepayment");
                payment.Attributes.Add("biz_name", "Payment" + i);
                payment.Attributes.Add("biz_parentmortgage", new EntityReference("biz_mortgage", context.PrimaryEntityId));

                CreateRequest req = new CreateRequest();
                req.Target = payment;
                request.Requests.Add(req);

                //service.Create(payment);
            }

            service.Execute(request);


        }
    }
}
