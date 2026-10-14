using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RobloxScriptExecutor.Core.Services;

namespace RobloxScriptExecutor.UI.ViewModels
{
    public partial class FileExplorerViewModel : ObservableObject
    {
        private readonly FileSystemService _fileSystemService;
        private string _scriptsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts");

        [ObservableProperty]
        private System.Collections.ObjectModel.ObservableCollection<string> _files = new();

        public FileExplorerViewModel()
        {
            _fileSystemService = new FileSystemService();
            RefreshFiles();
        }

        [RelayCommand]
        public void RefreshFiles()
        {
            Files.Clear();
            var luaFiles = _fileSystemService.GetLuaFiles(_scriptsFolder);
            foreach (var file in luaFiles)
            {
                Files.Add(file);
            }
        }

        [RelayCommand]
        public void CreateFile(string fileName)
        {
            _fileSystemService.CreateNewFile(_scriptsFolder, fileName);
            RefreshFiles();
        }
    }
}
