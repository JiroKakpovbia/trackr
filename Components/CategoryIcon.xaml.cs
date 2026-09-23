namespace trackr.Components;

public partial class CategoryIcon : ContentView
{
    public static readonly BindableProperty ColourProperty = BindableProperty.Create(
        nameof(Colour),
        typeof(Color),
        typeof(CategoryIcon),
        Colors.Transparent);

    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon),
        typeof(string),
        typeof(CategoryIcon),
        string.Empty);

    public static readonly BindableProperty SizeProperty = BindableProperty.Create(
        nameof(Size),
        typeof(double),
        typeof(CategoryIcon),
        46d);

    public Color Colour
    {
        get => (Color)GetValue(ColourProperty);
        set => SetValue(ColourProperty, value);
    }

    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public double Size
    {
        get => (double)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public CategoryIcon()
    {
        InitializeComponent();
    }
}