using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RobloxScriptExecutor.Core.Services;
using RobloxScriptExecutor.Integration;
using System.Windows;
using RobloxScriptExecutor.UI.Controls;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace RobloxScriptExecutor.UI.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly StudioDetectionService _studioDetectionService;
        private readonly BridgeServer _bridgeServer;
        private readonly HttpClient _httpClient;

        [ObservableProperty]
        private string _studioStatus = "Detecting Roblox Studio...";

        [ObservableProperty]
        private string _studioVersion = "Unknown";

        [ObservableProperty]
        private bool _isServerRunning;

        [ObservableProperty]
        private EditorViewModel _editor = new();

        [ObservableProperty]
        private FileExplorerViewModel _explorer = new();

        [ObservableProperty]
        private string _selectedFile = string.Empty;

        [ObservableProperty]
        private System.Collections.ObjectModel.ObservableCollection<LogEntryViewModel> _logs = new();

        public MainViewModel()
        {
            _studioDetectionService = new StudioDetectionService();
            _bridgeServer = new BridgeServer();
            _httpClient = new HttpClient();

            InitializeStudioDetection();
            StartBridgeServer();
            SetupLogListener();
        }

        private void SetupLogListener()
        {
            _bridgeServer.OnLogReceived += (log) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Logs.Add(new LogEntryViewModel(log.Level, log.Message));
                });
            };
        }

        private void InitializeStudioDetection()
        {
            var info = _studioDetectionService.DetectStudio();
            if (info != null)
            {
                StudioStatus = $"Roblox Studio Found: {info.Path}";
                StudioVersion = info.Version;
            }
            else
            {
                StudioStatus = "Roblox Studio NOT detected. Please ensure it's installed.";
            }
        }

        private async void StartBridgeServer()
        {
            try
            {
                _ = Task.Run(async () =>
                {
                    await _bridgeServer.StartAsync();
                    Application.Current.Dispatcher.Invoke(() => IsServerRunning = true);
                });
            }
            catch (Exception ex)
            {
                StudioStatus = $"Server Error: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task StopServer()
        {
            await _bridgeServer.StopAsync();
            IsServerRunning = false;
        }

        [RelayCommand]
        private async Task ExecuteScript()
        {
            string code = Editor.Code;
            string fileName = Editor.CurrentFileName;

            if (string.IsNullOrEmpty(code))
            {
                return;
            }

            try
            {
                var request = new ExecutionRequest(code, fileName, DateTime.UtcNow);
                string json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"http://localhost:{_bridgeServer.Port}/execute", content);
                if (response.IsSuccessStatusCode)
                {
                    Logs.Add(new LogEntryViewModel("Info", $"Script '{fileName}' queued for execution."));
                }
            }
            catch (Exception ex)
            {
                Logs.Add(new LogEntryViewModel("Error", $"Execution Error: {ex.Message}"));
            }
        }

        [RelayCommand]
        private void ClearLogs()
        {
            Logs.Clear();
        }

        // Handle file selection from explorer
        partial void OnSelectedFileChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Editor.OpenFileCommand.Execute(value);
            }
        }
    }
}
