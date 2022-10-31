using System.Collections;
using System.Collections.Generic;

namespace KeepNotes.Models
{
    public interface IUserRepository
    {
        Users GetUserOnlyEmail(string Email);
        Users GetUserFromId(int Id);
        Users GetUserEmailPassword(string Email,string password);
        IEnumerable<Users> GetAllUsers();
        Users Add(Users user);
        Users Update(Users user);
        Users Delete(int Id);

    }
}
