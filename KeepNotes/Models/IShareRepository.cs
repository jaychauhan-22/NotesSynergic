using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeepNotes.Models
{
    public interface IShareRepository
    {
        IEnumerable<Share> GetAllShares(int noteId,int uid);
        IEnumerable<Share> CheckShareIdExists(int noteId, int uid,int suid);
        void Add(string share);
        void Delete(string query);


    }
}
