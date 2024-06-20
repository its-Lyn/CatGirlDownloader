using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

    private bool _previousEnabled;
    public bool PreviousEnabled
    {
        get => _previousEnabled;
        set => this.RaiseAndSetIfChanged(ref _previousEnabled, value);
    }

    public ICommand NewKittyCommand { get; }
    public ICommand PreviousKittyCommand { get; }
    public ICommand SaveKittyCommand { get; }

    private int _activeUrl;
    private readonly ObservableCollection<string> _historyData = new ObservableCollection<string>();

    public MainWindowViewModel()
    {
        _previousEnabled = false;
        
        SaveKittyCommand = ReactiveCommand.Create(SaveKitty);
        NewKittyCommand = ReactiveCommand.Create(GetNewKitty);
        PreviousKittyCommand = ReactiveCommand.Create(GetPreviousKitty);
    }

    private void CheckActive()
    {
        if (_activeUrl > 0)
            PreviousEnabled = true;
        else if (_activeUrl == 0)
            PreviousEnabled = false;
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

    private async Task GetPreviousKitty()
    {
        _activeUrl--;
        CheckActive();
        
        await using MemoryStream kittyStream = await _neko.GetKittyStream(_historyData[_activeUrl]);
        Image = new Bitmap(kittyStream);
    }

    public async Task GetNewKitty()
    {
        // Destroy the old image
        Image?.Dispose();
        
        if (_historyData.Count == 0 || _activeUrl == _historyData.Count - 1)
        {
            await using KittyData data = await _neko.GetKittyStream();
            Image = new Bitmap(data.Stream);
            _historyData.Add(data.Url);
            _activeUrl = _historyData.Count == 0 ? 0 : _historyData.Count - 1;
        }
        else
        {
            _activeUrl++;
            await using MemoryStream kittyStream = await _neko.GetKittyStream(_historyData[_activeUrl]); 
            Image = new Bitmap(kittyStream);
        }

        CheckActive();
    }
}