using System.Collections;
using System.Collections.Generic;

namespace KeepNotes.Models
{
    public interface INoteRepository
    {
        Note GetNote(int id);
        IEnumerable<Note> GetAllNotes(int uid);
        IEnumerable<Note> GetPublicNotes(int uid);
        Note Add(Note note);
        Note Update(Note note);
        Note Delete(int Id);
    }
}
