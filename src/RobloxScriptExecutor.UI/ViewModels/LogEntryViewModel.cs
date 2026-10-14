using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Windows.Media;

namespace RobloxScriptExecutor.UI.ViewModels
{
    public partial class LogEntryViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _timestamp;

        [ObservableProperty]
        private string _level;

        [ObservableProperty]
        private string _message;

        [ObservableProperty]
        private Brush _foreground;

        public LogEntryViewModel(string level, string message)
        {
            Timestamp = DateTime.Now.ToString("HH:mm:ss");
            Level = level.ToUpper();
            Message = message;
            Foreground = GetColorForLevel(level);
        }

        private Brush GetColorForLevel(string level)
        {
            return level.ToLower() switch
            {
                "error" => Brushes.Crimson,
                "warning" or "warn" => Brushes.Gold,
                "info" => Brushes.LightCyan,
                _ => Brushes.White
            };
        }
    }
}
