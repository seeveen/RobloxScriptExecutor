using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RobloxScriptExecutor.Core.Services
{
    public class FileSystemService
    {
        public async Task<string> ReadFileAsync(string path)
        {
            return await File.ReadAllTextAsync(path);
        }

        public async Task WriteFileAsync(string path, string content)
        {
            await File.WriteAllTextAsync(path, content);
        }

        public IEnumerable<string> GetLuaFiles(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                return Enumerable.Empty<string>();

            return Directory.EnumerateFiles(folderPath, "*.*")
                .Where(file => file.EndsWith(".lua") || file.EndsWith(".luau"));
        }

        public void CreateNewFile(string folderPath, string fileName)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fullPath = Path.Combine(folderPath, fileName);
            File.WriteAllText(fullPath, "-- New Luau Script\n");
        }

        public void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
