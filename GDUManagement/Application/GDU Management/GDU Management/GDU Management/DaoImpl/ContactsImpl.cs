using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDU_Management.Model;
using GDU_Management.IDao;

namespace GDU_Management.DaoImpl
{
    class ContactsImpl:IDaoContacts
    {
        GDUDataConnectionsDataContext db = new GDUDataConnectionsDataContext();
        Contact listContact;

        public Contact GetContact()
        {
            db = new GDUDataConnectionsDataContext();
            Contact contacts = new Contact();
            contacts = db.Contacts.Single();
            return contacts;
        }

        public void InsertContacts(Contact contacts)
        {
            db = new GDUDataConnectionsDataContext();
            Contact cts = new Contact();
            cts = db.Contacts.Single(p => p.id == contacts.id);
            cts.Email = contacts.Email;
            cts.Pass = contacts.Pass;
            cts.Title = contacts.Title;
            cts.Message = contacts.Message;
            cts.Info = contacts.Info;
            db.SubmitChanges();
        }
    }
}
