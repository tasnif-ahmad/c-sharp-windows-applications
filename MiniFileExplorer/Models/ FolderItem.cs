using System.Collections.ObjectModel;

namespace MiniFileExplorer.Models
{
    public class FolderItem
    {
        public string Name { get; set; }
        public string FullPath { get; set; }

        public ObservableCollection<FolderItem> SubFolders { get; set; } = new();

        // Allow TreeView to show expand arrow
        public bool HasDummy => SubFolders.Count == 1 && SubFolders[0] == null;

        public void AddDummy()
        {
            SubFolders.Clear();
            SubFolders.Add(null);
        }
    }
}
