using System.Collections.Generic;
using System.Web.Mvc;
using TicketHunter.Domain.Entities;

namespace TicketHunter.Domain.Abstract
{
    public interface ICategoryRepository
    {
        IEnumerable<Categories> Categories { get; }
        IEnumerable<SubCategories> SubCategories { get; }
        IEnumerable<SelectListItem> CategoriesForDropList { get; }
        string GetCategory(int categoryId);

    }
}
