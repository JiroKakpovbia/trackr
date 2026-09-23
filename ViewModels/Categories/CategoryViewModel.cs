using CommunityToolkit.Mvvm.ComponentModel;
using trackr;
using trackr.Models;

namespace trackr.ViewModels
{
    public partial class CategoryViewModel(Category model) : ObservableObject
    {
        // Default colour resource for each category
        private string DefaultColourResource => Name switch
        {
            "Income" => "CategoryGreenSurface",
            "Savings" => "CategoryTealSurface",
            "Housing" => "CategoryOrangeSurface",
            "Communications" => "CategoryBlueSurface",
            "Food" => "CategoryYellowSurface",
            "Insurance" => "CategoryPurpleSurface",
            "Transportation" => "CategorySkySurface",
            "Education" => "CategoryIndigoSurface",
            "Recreation" => "CategoryPinkSurface",
            "Personal Care" => "CategoryRoseSurface",
            "Fees" => "CategoryRedSurface",
            "Transfers" => "CategoryMintSurface",
            _ => "CategoryGreySurface"
        };

        private Color DefaultColour
        {
            get
            {
                bool isDarkMode = Application.Current?.RequestedTheme == AppTheme.Dark;
                string resourceKey = $"{DefaultColourResource}{(isDarkMode ? "Dark" : "Light")}";

                if (Application.Current?.Resources.TryGetValue(resourceKey, out object? resource) == true && resource is Color colour)
                    return colour;
                return isDarkMode ? Color.FromArgb("#2C2C2C") : Color.FromArgb("#EFEFEF"); // Fallback if the resource cannot be found
            }
        }

        public Category Model { get; } = model;

        public string Name
        {
            get => Model.Name;
            set
            {
                if (SetProperty(Model.Name, value, Model, (m, v) => m.Name = v))
                    OnPropertyChanged(nameof(Icon));
            }
        }

        public Color Colour
        {
            get => !string.IsNullOrWhiteSpace(Model.Colour)
                ? Color.FromArgb(Model.Colour)
                : DefaultColour;

            set
            {
                if (SetProperty(
                    Model.Colour,
                    value?.ToArgbHex(),
                    Model,
                    (m, v) => m.Colour = v))
                {
                    OnPropertyChanged();
                }
            }
        }

        public string Icon => Name switch
        {
            "Income" => Glyphs.Income,
            "Savings" => Glyphs.Savings,
            "Housing" => Glyphs.Housing,
            "Communications" => Glyphs.Communications,
            "Food" => Glyphs.Food,
            "Insurance" => Glyphs.Insurance,
            "Transportation" => Glyphs.Transportation,
            "Education" => Glyphs.Education,
            "Recreation" => Glyphs.Recreation,
            "Personal Care" => Glyphs.PersonalCare,
            "Fees" => Glyphs.Fees,
            "Transfers" => Glyphs.Transfers,
            _ => Glyphs.Unknown
        };

        [ObservableProperty]
        private int totalBudget;

        [ObservableProperty]
        private decimal remainingBudget;

        [ObservableProperty]
        private bool hasBudget = false;

        [ObservableProperty]
        private bool hasNoBudget = true;

        public double BudgetProgress
        {
            get
            {
                if (TotalBudget <= 0)
                    return 0;

                return Math.Clamp(
                    (double)(TotalBudget - RemainingBudget) / TotalBudget,
                    0,
                    1);
            }
        }
    }
}