using CommunityToolkit.Mvvm.ComponentModel;
using trackr.Models;

namespace trackr.ViewModels
{
    public partial class SubCategoryViewModel(SubCategory model) : ObservableObject
    {
        public SubCategory Model { get; } = model;

        public string Name
        {
            get => Model.Name;
            set => SetProperty(Model.Name, value, Model, (m, v) => m.Name = v);
        }

        public int? MonthlyBudget
        {
            get => Model.MonthlyBudget;
            set => SetProperty(Model.MonthlyBudget, value, Model, (m, v) => m.MonthlyBudget = v);
        }

        [ObservableProperty]
        private CategoryViewModel category = new(new Category());
    }
}