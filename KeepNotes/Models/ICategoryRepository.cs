using System.Collections;
using System.Collections.Generic;

namespace KeepNotes.Models
{
    public interface ICategoryRepository
    {
        Category GetCategory(int id,int uid);
        IEnumerable<Category> GetAllCategories(int id);
        Category Add(Category category);
        Category Update(Category category);
        Category Delete(int Id);

    }
}
