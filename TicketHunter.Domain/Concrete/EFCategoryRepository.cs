using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TicketHunter.Domain.Abstract;
using TicketHunter.Domain.Entities;

namespace TicketHunter.Domain.Concrete
{
    public class EFCategoryRepository : ICategoryRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Categories> Categories
        {
            get { return context.Categories; } 
        }

        public IEnumerable<SubCategories> SubCategories
        {
            get { return context.SubCategories; }
        }

        public IEnumerable<SelectListItem> CategoriesForDropList
        {
            get
            {
                IEnumerable<SelectListItem> selectListItems = new List<SelectListItem>();
                selectListItems = Categories.Select(x => new SelectListItem
                {
                    Value = x.EventCategoryID.ToString(),
                    Text = x.EventCategoryName,
                });
                return DefaultItem.Concat(selectListItems);
            }
        }

        public string GetCategory(int categoryId)
        {
            Categories foundCategory = context.Categories.FirstOrDefault(x => x.EventCategoryID == categoryId);
            return foundCategory.EventCategoryName;
        }

        public IEnumerable<SelectListItem> DefaultItem
        {
            get
            {
                return Enumerable.Repeat(new SelectListItem
                {
                    Value = "",
                    Text = "- Wybierz -"
                }, count: 1);
            }
        }
    }
}
