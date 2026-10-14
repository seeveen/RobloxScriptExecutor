using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RobloxScriptExecutor.Core.Services;

namespace RobloxScriptExecutor.UI.ViewModels
{
    public partial class EditorViewModel : ObservableObject
    {
        private readonly FileSystemService _fileSystemService;
        private string _currentFilePath = string.Empty;

        [ObservableProperty]
        private string _code = "";

        [ObservableProperty]
        private string _currentFileName = "Untitled.luau";

        public EditorViewModel()
        {
            _fileSystemService = new FileSystemService();
        }

        [RelayCommand]
        public async Task OpenFile(string path)
        {
            _currentFilePath = path;
            CurrentFileName = Path.GetFileName(path);
            Code = await _fileSystemService.ReadFileAsync(path);
        }

        [RelayCommand]
        public async Task SaveFile()
        {
            if (string.IsNullOrEmpty(_currentFilePath))
            {
                // Simple implementation: default save path if none specified
                _currentFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts", CurrentFileName);
                Directory.CreateDirectory(Path.GetDirectoryName(_currentFilePath)!);
            }

            await _fileSystemService.WriteFileAsync(_currentFilePath, Code);
        }

        [RelayCommand]
        public async Task CreateNewFile()
        {
            Code = "";
            _currentFilePath = string.Empty;
            CurrentFileName = "NewScript.luau";
            // In a real app, we'd open a "Save As" dialog here
        }
    }
}
