using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Logging;
using RobloxScriptExecutor.UI.ViewModels;

namespace RobloxScriptExecutor.UI.Controls
{
    public partial class LuauEditor : UserControl
    {
        public LuauEditor()
        {
            InitializeComponent();
            LoadLuauSyntax();
        }

        private void LoadLuauSyntax()
        {
            try
            {
                // Load the custom Luau syntax definition from resources
                string xshdPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Luau.xshd");

                // In a real compiled app, this would be a ResourceStream
                // For this implementation, we load from the file system for transparency
                using (var reader = new StreamReader(xshdPath))
                {
                    var definition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                    Editor.SyntaxHighlighting = definition;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Syntax Highlighting Error: {ex.Message}");
                // Fallback to C# or none if Luau load fails
                Editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
            }
        }

        public string GetText() => Editor.Text;
        public void SetText(string text) => Editor.Text = text;
    }
}
