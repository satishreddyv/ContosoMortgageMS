using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace MyPlugins
{
    public class MortgagePreCreate : IPlugin
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


                try
                {
                    // Plug-in business logic goes here.  

                    string number = mortgage.Attributes["biz_number"].ToString();

                    mortgage.Attributes["biz_number"] = "M" + number;


                    //decimal amount = ((Money)mortgage.Attributes["biz_amount"]).Value;

                    //int terms = Convert.ToInt32( mortgage.Attributes["biz_terms"]);


                    mortgage.Attributes.Add("biz_monthlypayment", new Money(Convert.ToDecimal(new Random().Next(100))));




                    //(amount * (5 / interestRate) ) / (1 - Math.Pow(Convert.ToDouble( (1 + (5 / interestRate)), -terms))


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