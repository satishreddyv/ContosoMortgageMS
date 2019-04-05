using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            string connectionString = "Url=https://dlabscrm.crm8.dynamics.com; Username=satish@dlabscrm.onmicrosoft.com; Password=Pearl@123; authtype=Office365";

            CrmServiceClient service = new CrmServiceClient(connectionString);


            Entity contact = new Entity("contact");
            contact.Attributes.Add("lastname", "Console");

            Guid guid = service.Create(contact);


            QueryByAttribute query = new QueryByAttribute("account");
            query.ColumnSet.AllColumns = true;
            query.AddAttributeValue("name", "Adventure Works");





            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
  <entity name='account'>
    <attribute name='name' />
    <attribute name='address1_city' />
    <attribute name='primarycontactid' />
    <attribute name='telephone1' />
    <attribute name='accountid' />
    <order attribute='name' descending='false' />
    <filter type='and'>
      <condition attribute='statecode' operator='eq' value='0' />
    </filter>
    <link-entity name='contact' from='contactid' to='primarycontactid' visible='false' link-type='outer' alias='accountprimarycontactidcontactcontactid'>
      <attribute name='emailaddress1' />
    </link-entity>
  </entity>
</fetch>";

            EntityCollection collection = service.RetrieveMultiple(new FetchExpression(fetch));




            QueryExpression query = new QueryExpression("account");
            query.ColumnSet.AllColumns = true;

            //query.Criteria.AddCondition()
            EntityCollection collection = service.RetrieveMultiple(query);
           

            foreach(Entity item in collection.Entities)
            {
                Console.Write(item.Attributes["name"].ToString());
            }


       
            Console.Read();


        }
    }
}
