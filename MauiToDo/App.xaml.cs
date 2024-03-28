using Microsoft.Maui.Controls;

namespace MauiToDo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
