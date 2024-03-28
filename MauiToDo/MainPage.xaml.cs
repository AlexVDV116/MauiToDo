using MauiTodo.ViewModels;

namespace MauiTodo;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage()
    {
        InitializeComponent();
        
        // Set the binding context to the view model
        // Anywhere we declare a binding to a source property in the XAML, it will be a property on the view model
        _viewModel = new MainViewModel();
        BindingContext = _viewModel;
    }

    // Call the Initialise method on the view model when the page appears to hook back into page lifecycle methods
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.Initialise();
    }
}