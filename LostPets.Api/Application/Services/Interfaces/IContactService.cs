using Infrastructure.Data.Entities;

namespace Application.Services.Interfaces
{
    public interface IContactService
    {
        Contact AddOrUpdate(Contact contact, bool withSaveChanges = true);
        IEnumerable<Contact> AddOrUpdate(IEnumerable<Contact> contacts, bool withSaveChanges = true);
        Contact Add(Contact contact, bool withSaveChanges = true);
        IEnumerable<Contact> Add(IEnumerable<Contact> contacts, bool withSaveChanges = true);
        Contact Update(Contact contact, bool withSaveChanges = true);
        IEnumerable<Contact> Update(IEnumerable<Contact> contacts, bool withSaveChanges = true);
        void Remove(Contact contact);
        Contact? GetById(Guid id);
    }
}
