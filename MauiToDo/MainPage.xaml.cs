using MauiTodo.Data;
using MauiTodo.Models;
using System.Collections.ObjectModel;
using MauiToDo;

namespace MauiTodo;

public partial class MainPage : ContentPage
{
    public ObservableCollection<TodoItem> Todos { get; set; } = new();

    readonly Database _database;

    public MainPage()
    {
        InitializeComponent();
        _database = new Database();

        // You represent lists of complex models onscreen using a CollectionView. A CollectionView has a property called ItemsSource that can be bound to a List of items using data binding. 
        // Sets the ItemSource of the ToDoCollection CollectionView to the Observable Collection
        // This can also be achieved in the XAML, preffered when using MVVM to maintain clean seperation of concerns
        //TodosCollection.ItemsSource = Todos;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
 
        var todos = await _database.GetTodos();
 
        foreach (var todo in todos)
        {
            Todos.Add(todo);
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var todo = new TodoItem
        {
            Due = DueDatepicker.Date,
            Title = TodoTitleEntry.Text
        };

        var inserted = await _database.AddTodo(todo);

        if (inserted != 0)
        {
            Todos.Add(todo);

            TodoTitleEntry.Text = String.Empty;
            DueDatepicker.Date = DateTime.Now;
        }
    }
    
    private async void SwipeItem_Invoked(object sender, EventArgs e)
    {
        var item = sender as SwipeItem;
 
        await App.Current.MainPage.
            DisplayAlert(item.Text, $"You invoked the {item.Text} action.", "OK");
    }
}