using MauiTodo.Models;
using MauiTodo.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiTodo.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<TodoItem> Todos { get; set; } = new();

    public string NewTodoTitle { get; set; }  

    public DateTime NewTodoDue { get; set; } = DateTime.Now;

    public ICommand AddTodoCommand { get; set; }

    public ICommand CompleteTodoCommand { get; set; }

    public ICommand DeleteTodoCommand { get; set; }

    private readonly Database _database;

    public MainViewModel()
    {
        _database = new Database();

        AddTodoCommand = new Command(async () => await AddNewTodo());

        CompleteTodoCommand = new Command<TodoItem>(async (item) => await CompleteTodo(item));
        DeleteTodoCommand = new Command<TodoItem>(async (item) => await DeleteTodo(item));
    }

    public async Task Initialise()
    {
        var todos = await _database.GetTodos();

        foreach(var todo in todos)
        {
            Todos.Add(todo);
        }
    }

    public async Task AddNewTodo()
    {
        var todo = new TodoItem
        {
            Due = NewTodoDue,
            Title = NewTodoTitle
        };

        var inserted = await _database.AddTodo(todo);

        if (inserted != 0)
        {
            Todos.Add(todo);

            NewTodoTitle = String.Empty;
            NewTodoDue = DateTime.Now;

            RaisePropertyChanged(nameof(NewTodoDue), nameof(NewTodoTitle));
        }
    }

    public async Task CompleteTodo(TodoItem todoitem)
    {
        var completed = await _database.UpdateTodo(todoitem);

        OnPropertyChanged(nameof(Todos));
    }

    public async Task DeleteTodo(TodoItem todoitem)
    {
        var deleted = await _database.UpdateTodo(todoitem);

        Todos.Remove(todoitem);

        OnPropertyChanged(nameof(Todos));
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        var changed = PropertyChanged;
        if (changed == null)
            return;

        changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void RaisePropertyChanged(params string[] properties)
    {
        foreach (var propertyName in properties)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    #endregion
}
