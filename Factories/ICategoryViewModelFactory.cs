using trackr.Models;
using trackr.ViewModels;

namespace trackr.Factories
{
    public interface ICategoryViewModelFactory
    {
        Task<CategoryViewModel> CreateCategoryAsync(Category category);
        Task<SubCategoryViewModel> CreateSubCategoryAsync(SubCategory subCategory);
    }
}