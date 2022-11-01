using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeepNotes.Models
{
    public interface IShareRepository
    {
        IEnumerable<Share> GetAllShares(int noteId,int uid);
        void Add(string share);
        void Delete(int nid,int uid);

    }
}
