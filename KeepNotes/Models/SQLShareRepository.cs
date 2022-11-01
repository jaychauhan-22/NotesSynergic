using CollegeApp.Modals;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeepNotes.Models
{
    public class SQLShareRepository : IShareRepository
    {
        private readonly AppDbContext context;
        public SQLShareRepository(AppDbContext context)
        {
            this.context = context;
        }

        public void Add(string share)
        {
            //share.NoteId = 1;
            //share.UserId = 1;
            ////share.ShareId = 1;
            //share.ToShareUserId = 3;
            //share.isWritable = true;

            //context.Share.FromSqlRaw()
            //context.Share.Add(share);
            //int isWritable = 0;
            //if (share.isWritable)
            //{
            //    isWritable = 1;
            //}
            context.Database.ExecuteSqlRaw(share);
            //Share newShare = null;
            //context.Share.FromSqlRaw(share).AsEnumerable();

            //context.SaveChanges();
            //return newShare;
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
