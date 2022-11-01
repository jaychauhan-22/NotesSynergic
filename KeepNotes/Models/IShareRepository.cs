using System.Collections;
using System.Collections.Generic;

namespace KeepNotes.Models
{
    public interface IShareRepository
    {
        IEnumerable<Share> GetAllShares(int noteId,int uid);
        Share Add(Share share);
        void Delete(int nid,int uid);

    }
}
