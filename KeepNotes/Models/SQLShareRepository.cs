using CollegeApp.Modals;
using System.Collections.Generic;
using System.Linq;

namespace KeepNotes.Models
{
    public class SQLShareRepository : IShareRepository
    {
        private readonly AppDbContext context;
        public SQLShareRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Share Add(Share share)
        {
            context.Share.Add(share);
            context.SaveChanges();
            return share;
        }

        public void Delete(int nid,int uid)
        {
            Share Share = context.Share.Where(s => s.UserId == uid && s.NoteId == nid).FirstOrDefault();
            if (Share != null)
            {
                context.Share.Remove(Share);
                context.SaveChanges();
            }
        }

        public IEnumerable<Share> GetAllShares(int noteId,int uid)
        {
            return context.Share.Where(s => s.UserId == uid && s.NoteId == noteId).ToList();
        }
    }
}
