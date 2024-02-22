using MauiTodo.Data;
using MauiTodo.Models;
using System.Collections.ObjectModel;

namespace MauiTodo;

public partial class MainPage : ContentPage
{
    public ObservableCollection<TodoItem> Todos { get; set; } = new();

    readonly Database _database;

    public MainPage()
    {
        InitializeComponent();
        _database = new Database();

        _ = Initialize();

        // Sets the ItemSource of the ToDoCollection CollectionView to the Observable Collection
        // This can also be achieved in the XAML, preffered when using MVVM to maintain clean seperation of concerns
        //TodosCollection.ItemsSource = Todos;
    }

    private async Task Initialize()
    {
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
}