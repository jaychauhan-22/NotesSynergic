using CollegeApp.Modals;
using System.Collections.Generic;
using System.Linq;

namespace KeepNotes.Models
{
    public class SQLCategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext context;
        public SQLCategoryRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Category Add(Category category)
        {
            context.Category.Add(category);
            context.SaveChanges();
            return category;
        }

        public Category Delete(int Id)
        {
            Category Category = context.Category.Find(Id);
            if (Category != null)
            {
                context.Category.Remove(Category);
                context.SaveChanges();
            }
            return Category;
        }

        public IEnumerable<Category> GetAllCategories(int id)
        {
            return context.Category.Where(c => c.UserId == id).ToList();
        }

        public Category GetCategory(int id,int uid)
        {
            return context.Category.FirstOrDefault(m => m.CategoryId == id);
        }

        public Category Update(Category category)
        {
            var updatedCategory = context.Category.Attach(category);
            //user.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            updatedCategory.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return category;
        }
    }
}
