using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using trackr.Factories;
using trackr.Messages;
using trackr.Models;
using trackr.Services;

namespace trackr.ViewModels
{
    public partial class BudgetPageViewModel : ObservableObject
    {
        private readonly IAccountDataService accountDataService;
        private readonly ICategoryViewModelFactory categoryViewModelFactory;
        public ObservableCollection<CategoryViewModel> Categories { get; set; } = [];

        [ObservableProperty]
        private decimal totalAmountSpent;

        [ObservableProperty]
        private decimal monthlyBudget;

        [ObservableProperty]
        private decimal remainingBudget;

        [ObservableProperty]
        private double budgetPercentageUsed;

        // Load categories from the database and populate the Categories list
        public async Task LoadCategoriesAsync()
        {
            try
            {
                // Clear the existing Categories list to avoid duplicates when reloading
                Categories.Clear();

                IReadOnlyList<Category> categories = await accountDataService.GetAllCategoriesAsync();

                // Create CategoryViewModel instances for each category and add them to the Categories list
                foreach (Category category in categories.OrderBy(c => c.Name))
                {
                    CategoryViewModel categoryViewModel = await categoryViewModelFactory.CreateCategoryAsync(category);

                    Categories.Add(categoryViewModel);
                }

                // Add a default "Uncategorized" category to the list of categories
                Categories.Add(await categoryViewModelFactory.CreateCategoryAsync(new Category
                {
                    Name = "Uncategorized"
                }));

                await UpdateBudgetSummaryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading categories: {ex.Message}\n");
            }
        }

        // Update the net worth, assets, and liabilities totals based on the current accounts
        private async Task UpdateBudgetSummaryAsync()
        {
            TotalAmountSpent = Categories.Sum(c => c.TotalBudget - c.RemainingBudget);
            MonthlyBudget = Categories.Sum(c => c.TotalBudget);
            RemainingBudget = MonthlyBudget - TotalAmountSpent;
            BudgetPercentageUsed = MonthlyBudget == 0 ? 0 : (double)(TotalAmountSpent / MonthlyBudget);

            await Task.CompletedTask;
        }

        public async Task OnTransactionUpdatedAsync(int transactionId)
        {
            Console.WriteLine(
            $"Budget Page updating transaction category for {transactionId}.");

            Transaction? transaction = await accountDataService.GetTransactionAsync(transactionId);

            if (transaction is null)
                return;

            if (transaction.SubCategoryId is null)
                return;

            SubCategory subCategory = await accountDataService.GetSubCategoryAsync(transaction.SubCategoryId.Value);

            CategoryViewModel existingCategory = Categories.First(c => c.Model.Id == subCategory.CategoryId);

            CategoryViewModel newCategoryViewModel = await categoryViewModelFactory.CreateCategoryAsync(existingCategory.Model);

            int index =
                Categories.IndexOf(
                    existingCategory);

            Categories[index] =
                newCategoryViewModel;

            await UpdateBudgetSummaryAsync();

            Console.WriteLine(
                $"Budget Page updated transaction category for {transaction.Id} and recalculated the budget.");
        }

        // Constructor for BudgetPageViewModel
        public BudgetPageViewModel(IAccountDataService accountDataService, ICategoryViewModelFactory categoryViewModelFactory)
        {
            this.accountDataService = accountDataService;
            this.categoryViewModelFactory = categoryViewModelFactory;

            WeakReferenceMessenger.Default.Register<
                BudgetPageViewModel,
                TransactionUpdatedMessage>(
                this,
                static (recipient, message) =>
                {
                    _ = recipient.OnTransactionUpdatedAsync(
                        message.Value);
                });
        }
    }
}
