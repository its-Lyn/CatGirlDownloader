using Avalonia.Controls;
using CatGirlDownloader.ViewModels;

namespace CatGirlDownloader.Views;

public partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext!;
    public static MainWindow Instance = null!;
    
    public MainWindow()
    {
        Instance = this;
        
        InitializeComponent();
        Loaded += Load;
    }

    private async void Load(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        => await ViewModel.GetNewKitty();
}