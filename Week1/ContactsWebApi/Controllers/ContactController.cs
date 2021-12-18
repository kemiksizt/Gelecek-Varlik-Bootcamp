using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;


namespace ContactsWebApi.Controllers
{
    [ApiController]
    [Route("[controller]s")]

    public class ContactController : ControllerBase
    {

        private static List<Contact> ContactList = new List<Contact>()
        {
            new Contact {
                id = 1,
                name = "Lebron",
                surname = "James",
                age = 37,
                phone = 12345678,
                email = "lebronjames@gmail.com",
                city = "Akron"       
            },

            new Contact {
                id = 2,
                name = "Kobe",
                surname = "Bryant",
                age = 42,
                phone = 87654321,
                email = "kobebryant@gmail.com",
                city = "Pensilvanya"
            },

            new Contact {
                id = 3,
                name = "Michael",
                surname = "Jordan",
                age = 58,
                phone = 12332112,
                email = "michaeljordan@gmail.com",
                city = "Brooklyn"
            },

            new Contact {
                id = 4,
                name = "Blake",
                surname = "Griffin",
                age = 32,
                phone = 33445566,
                email = "blakegriffin@gmail.com",
                city = "Oklahoma City"
            }
        };
        
        //GET Requests
        [HttpGet]
        public List<Contact> GetContacts()
        {
            var contactList = ContactList.OrderBy(x => x.id).ToList<Contact>();
            return contactList;
        }


        [HttpGet("{id}")]

        public Contact GetById(int id)
        {
            var contact = ContactList.Where(contact => contact.id == id).SingleOrDefault();
            return contact;
        }

 
        //POST Request

        [HttpPost]

        public IActionResult AddContact([FromBody] Contact newContact)
        {
            var contact = ContactList.SingleOrDefault(x => x.name == newContact.name);

            if(contact != null)
                return BadRequest();


            ContactList.Add(newContact);
            return Ok();
        }

        //PUT Request
        [HttpPut("{id}")]
        public IActionResult UpdateContact(int id,[FromBody] Contact updatedContact)
        {
            var contact = ContactList.SingleOrDefault(x => x.id == id);

            if(contact == null)
                return BadRequest();

            contact.name = updatedContact.name != default ? updatedContact.name : contact.name;
            contact.surname = updatedContact.surname != default ? updatedContact.surname : contact.surname;
            contact.age = updatedContact.age != default ? updatedContact.age : contact.age;
            contact.phone = updatedContact.phone != default ? updatedContact.phone : contact.phone;
            contact.email = updatedContact.email != default ? updatedContact.email : contact.email;
            contact.city = updatedContact.city != default ? updatedContact.city : contact.city;

            return Ok();    
        }

        //DELETE Request
        [HttpDelete("{id}")]

        public IActionResult DeleteContact(int id)
        {
            var contact = ContactList.SingleOrDefault(x => x.id == id);

            if(contact == null)
                return BadRequest();


            ContactList.Remove(contact);
            return Ok();
        }


    }
}
