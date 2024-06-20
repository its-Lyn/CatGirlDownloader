using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using CatGirlDownloader.Models;
using CatGirlDownloader.Services.NekosBest;
using CatGirlDownloader.Views;
using ReactiveUI;

namespace CatGirlDownloader.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private readonly Nekos _neko = new Nekos();
    
    private Bitmap? _image;
    public Bitmap? Image
    {
        get => _image;
        set => this.RaiseAndSetIfChanged(ref _image, value);
    }
    
    public ICommand NewKittyCommand { get; }
    public ICommand SaveKittyCommand { get; }

    public MainWindowViewModel()
    {
        SaveKittyCommand = ReactiveCommand.Create(SaveKitty);
        NewKittyCommand = ReactiveCommand.Create(GetNewKitty);
    }

    private async Task SaveKitty()
    {
        var file = await MainWindow.Instance.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save CatGirl image.",
            SuggestedFileName = $"{Guid.NewGuid()}.png"
        });

        if (file is null)
            return;
        
        Image?.Save(file.Path.AbsolutePath);
    }

    public async Task GetNewKitty()
    {
        // Destroy the old image
        Image?.Dispose();

        KittyData data = await _neko.GetKittyStream();
        Image = new Bitmap(data.Stream);
        
        // Destroy the new stream data.
        await data.Stream.DisposeAsync();
    }
}