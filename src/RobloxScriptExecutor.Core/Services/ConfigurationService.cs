using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using RobloxScriptExecutor.Core.Models;

namespace RobloxScriptExecutor.Core.Services
{
    public class ConfigurationService
    {
        private readonly string _configPath;

        public ConfigurationService()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _configPath = Path.Combine(appData, "RobloxScriptExecutor", "settings.json");
        }

        public async Task SaveConfigAsync(AppConfig config)
        {
            string directory = Path.GetDirectoryName(_configPath)!;
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_configPath, json);
        }

        public async Task<AppConfig> LoadConfigAsync()
        {
            if (!File.Exists(_configPath))
                return new AppConfig();

            try
            {
                string json = await File.ReadAllTextAsync(_configPath);
                return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
            }
            catch
            {
                return new AppConfig();
            }
        }
    }
}
