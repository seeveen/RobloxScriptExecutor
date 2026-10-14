using System;
using System.Collections.Generic;

namespace RobloxScriptExecutor.Core.Models
{
    public class AppConfig
    {
        public string StudioPath { get; set; } = string.Empty;
        public string LastOpenedFolder { get; set; } = string.Empty;
        public List<string> RecentFiles { get; set; } = new List<string>();
        public EditorSettings Editor { get; set; } = new EditorSettings();
    }

    public class EditorSettings
    {
        public int FontSize { get; set; } = 12;
        public string Theme { get; set; } = "Dark";
        public bool AutoIndent { get; set; } = true;
    }
}
