using System;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace RobloxScriptExecutor.Core.Services
{
    public class StudioDetectionService
    {
        private const string RobloxVersionsFolder = @"Roblox\Versions";
        private const string StudioExecutable = "RobloxStudioBeta.exe";

        public record StudioInfo(string Path, string Version);

        /// <summary>
        /// Detects the most recent installation of Roblox Studio on the system.
        /// </summary>
        /// <returns>A StudioInfo object if found, otherwise null.</returns>
        public StudioInfo? DetectStudio()
        {
            try
            {
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string versionsPath = Path.Combine(localAppData, RobloxVersionsFolder);

                if (!Directory.Exists(versionsPath))
                    return null;

                // Look for folders that start with "version-" and contain the executable
                var versionFolders = Directory.GetDirectories(versionsPath)
                    .Where(dir => Path.GetFileName(dir).StartsWith("version-"))
                    .OrderByDescending(dir => new DirectoryInfo(dir).CreationTime)
                    .ToList();

                foreach (var folder in versionFolders)
                {
                    string exePath = Path.Combine(folder, StudioExecutable);
                    if (File.Exists(exePath))
                    {
                        string version = ExtractVersion(folder);
                        return new StudioInfo(exePath, version);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error detecting Roblox Studio: {ex.Message}");
            }

            return null;
        }

        private string ExtractVersion(string folderPath)
        {
            // Usually the folder name is "version-xxxxxxxx"
            string folderName = Path.GetFileName(folderPath);
            if (folderName.StartsWith("version-"))
            {
                return folderName.Replace("version-", "");
            }
            return "Unknown";
        }
    }
}
