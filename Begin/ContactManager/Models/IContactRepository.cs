using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Models
{
    public interface IContactRepository
    {
        IEnumerable<Contact> GetAllContacts();

        Contact GetContact(string id);

        Contact AddContact(Contact item);

        bool RemoveContact(string id);

        bool UpdateContact(string id, Contact item);
    }
}
