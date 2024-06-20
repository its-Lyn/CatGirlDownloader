using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
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

    private bool _genericToggle;
    public bool GenericToggle
    {
        get => _genericToggle;
        set => this.RaiseAndSetIfChanged(ref _genericToggle, value);
    }

    private bool _previousEnabled;
    public bool PreviousEnabled
    {
        get => _previousEnabled;
        set => this.RaiseAndSetIfChanged(ref _previousEnabled, value);
    }
    
    private bool _loadingVisible;
    public bool LoadingVisible
    {
        get => _loadingVisible;
        set => this.RaiseAndSetIfChanged(ref _loadingVisible, value);
    }
    
    public ICommand NewKittyCommand { get; }
    public ICommand PreviousKittyCommand { get; }
    public ICommand SaveKittyCommand { get; }

    private int _activeUrl;
    private readonly ObservableCollection<string> _historyData = new ObservableCollection<string>();

    public MainWindowViewModel()
    {
        SaveKittyCommand = ReactiveCommand.Create(SaveKitty);
        NewKittyCommand = ReactiveCommand.Create(GetNewKitty);
        PreviousKittyCommand = ReactiveCommand.Create(GetPreviousKitty);
    }

    private void CheckActive()
    {
        PreviousEnabled = _activeUrl switch
        {
            > 0 => true,
            0 => false,
            _ => PreviousEnabled
        };
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
        LoadingVisible = true;
        GenericToggle = false;
        PreviousEnabled = false;
        
        _activeUrl--;
        await using MemoryStream kittyStream = await _neko.GetKittyStream(_historyData[_activeUrl]);
        Image = new Bitmap(kittyStream);

        CheckActive();
        
        GenericToggle = true;
        LoadingVisible = false;
    }

    public async Task GetNewKitty()
    {
        LoadingVisible = true;
        GenericToggle = false;
        PreviousEnabled = false;
        
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

        LoadingVisible = false;
        GenericToggle = true;
    }
}