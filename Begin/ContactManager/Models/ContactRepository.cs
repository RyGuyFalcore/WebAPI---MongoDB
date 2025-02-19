﻿using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactManager.Models
{
    public class ContactRepository : IContactRepository
    {
        MongoServer _server;
        MongoDatabase _database;
        MongoCollection<Contact> _contacts;

        public ContactRepository()
            : this("")
        {
        }

        public ContactRepository(string connection)
        {
            if (string.IsNullOrWhiteSpace(connection))
            {
                connection = "mongodb://localhost:27017";
            }
     
            _server = MongoServer.Create(connection);
            _database = _server.GetDatabase("Contacts", SafeMode.True);
            _contacts = _database.GetCollection<Contact>("contacts");
    
            // Reset database and add some default entries
            _contacts.RemoveAll();
            for (int index = 1; index < 5; index++)
            {
                Contact contact1 = new Contact
                {
                    Email = string.Format("test{0}@example.com", index),
                    Name = string.Format("test{0}", index),
                    Phone = string.Format("{0}{0}{0} {0}{0}{0} {0}{0}{0}{0}", index)
                };
                AddContact(contact1);
            }
        }
        public IEnumerable<Contact> GetAllContacts()
        {
            return _contacts.FindAll();
        }
     
        public Contact GetContact(string id)
        {
            IMongoQuery query = Query.EQ("_id", id);
            return _contacts.Find(query).FirstOrDefault();
        }
    
        public Contact AddContact(Contact item)
        {
            item.Id = ObjectId.GenerateNewId().ToString();
            item.LastModified = DateTime.UtcNow;
            _contacts.Insert(item);
            return item;
        }
    
        public bool RemoveContact(string id)
        {
            IMongoQuery query = Query.EQ("_id", id);
            SafeModeResult result = _contacts.Remove(query);
            return result.DocumentsAffected == 1;
        }
    
        public bool UpdateContact(string id, Contact item)
        {
            IMongoQuery query = Query.EQ("_id", id);
            item.LastModified = DateTime.UtcNow;
            IMongoUpdate update = Update
               .Set("Email", item.Email)
               .Set("LastModified", DateTime.UtcNow)
               .Set("Name", item.Name)
               .Set("Phone", item.Phone);
            SafeModeResult result = _contacts.Update(query, update);
            return result.UpdatedExisting;
        }
    }
}