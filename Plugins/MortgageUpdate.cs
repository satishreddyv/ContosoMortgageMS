using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace MyPlugins
{
    public class MortgageUpdate : IPlugin
    {



        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService =
               (ITracingService)serviceProvider.GetService(typeof(ITracingService));


            tracingService.Trace("Plugin just started....");
            IPluginExecutionContext context = (IPluginExecutionContext)
               serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the organization service reference which you will need for  
            // web service calls.  
            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
        

            if (context.InputParameters.Contains("Target") &&
                  context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity mortgage = (Entity)context.InputParameters["Target"];

                Entity mortgageImage = (Entity)context.PreEntityImages["PreImage"];

                try
                {


                     // Plug-in business logic goes here.  

                    decimal amountNew = ((Money)mortgage.Attributes["biz_amount"]).Value;

                    decimal amountOld = ((Money)mortgageImage.Attributes["biz_amount"]).Value;

                    if (mortgageImage.Attributes.Contains("biz_number"))
                    {
                        string number = mortgageImage.Attributes["biz_number"].ToString();

                        EntityReference contactEntityReference = (EntityReference)mortgageImage.Attributes["biz_contactid"];

                        Entity newTask = new Entity("task");

                        newTask.Attributes.Add("subject", "Follow up from Plugin");

                        newTask.Attributes.Add("description", "Amount is changed from"+ amountOld + " to " + amountNew);

                        newTask.Attributes.Add("scheduledend", DateTime.Now.AddDays(3));



                        newTask.Attributes.Add("regardingobjectid", contactEntityReference);

                        service.Create(newTask);

                       



                    }




                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in MyPlug-in.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("MyPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}