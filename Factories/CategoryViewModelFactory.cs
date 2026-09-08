using trackr.Models;
using trackr.Services;
using trackr.ViewModels;

namespace trackr.Factories
{
    public class CategoryViewModelFactory(IAccountDataService accountDataService) : ICategoryViewModelFactory
    {
        public async Task<CategoryViewModel> CreateCategoryAsync(
            Category category)
        {
            CategoryViewModel viewModel = new(category);

            IReadOnlyList<SubCategory> subCategories = await accountDataService.GetSubCategoriesForCategoryAsync(category.Id);
            IReadOnlyList<Transaction> transactions = await accountDataService.GetAllTransactionsAsync();

            foreach (SubCategory subCategory in subCategories)
            {
                // Calculate the total budget for the category by summing the monthly budgets of its subcategories
                viewModel.TotalBudget += subCategory.MonthlyBudget ?? 0;
                viewModel.RemainingBudget += subCategory.MonthlyBudget ?? 0;

                // Calculate the remaining budget for the category
                if (subCategory.MonthlyBudget.HasValue)
                {
                    viewModel.HasBudget = true;
                    viewModel.HasNoBudget = false;

                    viewModel.RemainingBudget -= transactions
                        .Where(t => t.SubCategoryId == subCategory.Id)
                        .Sum(t => t.Amount);
                }
            }

            return viewModel;
        }

        public async Task<SubCategoryViewModel> CreateSubCategoryAsync(
            SubCategory subCategory)
        {
            SubCategoryViewModel viewModel = new(subCategory)
            {
                Category = await CreateCategoryAsync(await accountDataService.GetCategoryAsync(subCategory.CategoryId))
            };

            return viewModel;
        }
    }
}