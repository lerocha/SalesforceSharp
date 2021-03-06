using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Sharpforce.Test
{
    [TestFixture]
    public class ReadmeTest : AbstractSalesforceClientTest
    {
        [Test]
        public void Readme()
        {
            // Instantiate the client using a RefreshToken
            var service = new SalesforceClient(ConsumerKey, ConsumerSecret, RefreshToken);

            //-----------------------------------------------------------------------------
            // Queries
            //-----------------------------------------------------------------------------

            // Execute a SOQL query
            IList<Contact> contacts = service.Query<Contact>("SELECT id, name from Contact");

            // Iterate through the records returned.
            foreach (Contact account in contacts)
            {
                Console.WriteLine(account.Name);
            }

            //-----------------------------------------------------------------------------
            // CRUD Operations
            //-----------------------------------------------------------------------------

            // Add a new record using anonymous object
            var id = service.Add<Contact>(new { FirstName = "John", LastName = "Smith" });

            // Add a new record using POCO object
            id = service.Add<Contact>(new Contact { FirstName = "John", LastName = "Smith" });

            // Read a record
            Contact contact = service.Get<Contact>(id);

            // Update a record using POCO object (null values are not serialized)
            contact = new Contact {Id = id, Email = "jsmith@gmail.com"};
            service.Update<Contact>(contact);

            // Update a record using anonymous object
            service.Update<Contact>(new { Email = "jsmith@yahoo.com" }, id);

            // Delete a record
            service.Delete<Contact>(id);

            //-----------------------------------------------------------------------------
            // Error Handling
            //-----------------------------------------------------------------------------

            try
            {
                service.Add<Contact>(new { Name = "Read-only property" });
            }
            catch (SalesforceException e)
            {
                Console.WriteLine("ErrorCode={0}; StatusCode={1}; Message={2}", 
                    e.ErrorCode, e.StatusCode, e.Message);
                // Output:
                // ErrorCode=INVALID_FIELD_FOR_INSERT_UPDATE; 
                // StatusCode=BadRequest; 
                // Message=Unable to create/update fields: Name. Please check the security settings of this field
                //         and verify that it is read/write for your profile or permission set.
	            
                // TODO: handle the exception
            }
        }
    }
}