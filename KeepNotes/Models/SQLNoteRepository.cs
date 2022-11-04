using CollegeApp.Modals;
using System.Collections.Generic;
using System.Linq;

namespace KeepNotes.Models
{
    public class SQLNoteRepository : INoteRepository
    {
        private readonly AppDbContext context;
        public SQLNoteRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Note Add(Note note)
        {
            context.Note.Add(note);
            context.SaveChanges();
            return note;
        }

        public Note Delete(int Id)
        {
            Note Note = context.Note.Find(Id);
            if (Note != null)
            {
                context.Note.Remove(Note);
                context.SaveChanges();
            }
            return Note;
        }

        public IEnumerable<Note> GetAllNotes(int uid)
        {
            return context.Note.Where(c => c.UserId == uid).ToList();
        }

        public Note GetNote(int id)
        {
            return context.Note.FirstOrDefault(m => m.NoteId == id);
        }

        public Note Update(Note note)
        {
            var updatedNote = context.Note.Attach(note);
            //user.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            updatedNote.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return note;
        }
    }
}
