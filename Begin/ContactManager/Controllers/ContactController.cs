using ContactManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ContactManager.Controllers
{
    public class ContactController : ApiController
    {
        private static readonly IContactRepository _contacts = new ContactRepository();

        public IQueryable<Contact> Get()
        {
            return _contacts.GetAllContacts().AsQueryable();
        }

        ///GET - api/contact/{id}
        public Contact Get(string id)
        {
            Contact contact = _contacts.GetContact(id);
            if (contact == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return contact;
        }

        public Contact Post(Contact person)
        {
            Contact contact = _contacts.AddContact(person);
            return contact;
        }

        public void Put(string id, Contact value)
        {
            if (!_contacts.UpdateContact(id, value))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public void Delete(string id)
        {
            if (!_contacts.RemoveContact(id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}
