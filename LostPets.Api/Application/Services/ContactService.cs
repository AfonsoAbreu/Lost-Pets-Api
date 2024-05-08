using Application.Exceptions;
using Application.Services.Base;
using Application.Services.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Application.Services
{
    public class ContactService : BaseService, IContactService
    {
        protected readonly IContactRepository _contactRepository;

        public ContactService(ApplicationDbContext applicationDbContext, IContactRepository contactRepository) : base(applicationDbContext)
        {
            _contactRepository = contactRepository;
        }

        public Contact AddOrUpdate(Contact contact, bool withSaveChanges = true)
        {
            try
            {
                return Update(contact, withSaveChanges);
            }
            catch (ResourceNotFoundDomainException)
            {
                return Add(contact, withSaveChanges);
            }
        }

        public Contact Add(Contact contact, bool withSaveChanges = true)
        {
            _contactRepository.Add(contact);

            if (withSaveChanges)
            {
                SaveChanges();
            }

            return contact;
        }

        public Contact Update(Contact contact, bool withSaveChanges = true)
        {
            Contact? existingContact = _contactRepository.GetById(contact.Id);

            if (existingContact == null)
            {
                throw new ResourceNotFoundDomainException(ResourceNotFoundDomainException.DefaultMessage("Contact"));
            }
            else if (existingContact.UserId != contact.UserId)
            {
                throw new MismatchedUserDomainException(MismatchedUserDomainException.DefaultMessage("Contact"));
            }

            existingContact.Content = contact.Content;
            existingContact.Type = contact.Type;

            if (withSaveChanges)
            {
                SaveChanges();
            }

            return existingContact;
        }

        public IEnumerable<Contact> AddOrUpdate(IEnumerable<Contact> contacts, bool withSaveChanges = true)
        {
            return contacts.Select(contact => AddOrUpdate(contact, withSaveChanges)).ToList();
        }

        public IEnumerable<Contact> Add(IEnumerable<Contact> contacts, bool withSaveChanges = true)
        {
            return contacts.Select(contact => Add(contact, withSaveChanges)).ToList();
        }

        public IEnumerable<Contact> Update(IEnumerable<Contact> contacts, bool withSaveChanges = true)
        {
            return contacts.Select(contact => Update(contact, withSaveChanges)).ToList();
        }

        public Contact? GetById(Guid id)
        {
            Contact? contact = _contactRepository.GetById(id);

            return contact;
        }

        public void Remove(Contact contact)
        {
            _contactRepository.Remove(contact);
            SaveChanges();
        }
    }
}
