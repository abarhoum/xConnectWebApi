using Sitecore.XConnect;
using Sitecore.XConnect.Client.Configuration;
using Sitecore.XConnect.Collection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace xConnectWebApi.Controllers
{
    public class MyContactController : ApiController
    {
        public string Get()
        {
            return "Hello World";
        }
        [HttpGet, HttpPost]
        [Route("contactapi/add")]
        public async Task<IHttpActionResult> AddContact()
        {
            try
            {
                using (var client = SitecoreXConnectClientConfiguration.GetClient())
                {

                    var contact = new Contact(new ContactIdentifier("direct", "email", ContactIdentifierType.Known));
                    client.AddContact(contact);

                    // Add facets
                    var personalInfoFacet = new PersonalInformation
                    {
                        FirstName = "Ayman",
                        LastName = "Barhoum"
                    };
                    client.SetFacet(new FacetReference(contact, PersonalInformation.DefaultFacetKey), personalInfoFacet);

                    var emails = new EmailAddressList(new EmailAddress("ayman.barhoum@sitecore.com", true), "Home");
                    client.SetFacet(contact, emails);

                    var addresses = new AddressList(
                        new Address { AddressLine1 = "Juimerah", City = "Dubai", PostalCode = "123" }, "Home");
                    client.SetFacet(contact, AddressList.DefaultFacetKey, addresses);

                    // Must await for Id to be assigned
                    await client.SubmitAsync();

                    Guid? contactId = contact.Id;  // This is a nullable Guid

                    return Ok(new
                    {
                        status = "success",
                        contactId = contactId.HasValue ? contactId.Value.ToString() : null,
                        message = "Contact created successfully"
                    });
                }
            }
            catch (XdbExecutionException ex)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    status = "failed",
                    message = "xConnect execution error",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }



    }
}
