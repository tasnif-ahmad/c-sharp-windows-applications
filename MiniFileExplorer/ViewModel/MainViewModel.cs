using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using MiniFileExplorer.Models;

namespace MiniFileExplorer.ViewModel
{
    public class MainViewModel
    {
        public ObservableCollection<FolderItem> RootFolders { get; set; } = new();
        public ObservableCollection<FileItem> Files { get; set; } = new();
        private List<FileItem> allFiles = new();

        public MainViewModel()
        {
            LoadDrives();
        }

        private void LoadDrives()
        {
            RootFolders.Clear();
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    var folder = new FolderItem
                    {
                        Name = drive.Name,
                        FullPath = drive.RootDirectory.FullName
                    };
                    folder.AddDummy();
                    RootFolders.Add(folder);
                }
            }
        }

        public void LoadFiles(string folderPath, bool recursive = false)
        {
            if (!Directory.Exists(folderPath))
                return;

            var files = new List<FileItem>();
            try
            {
                var options = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

                foreach (var filePath in Directory.GetFiles(folderPath, "*.*", options))
                {
                    var info = new FileInfo(filePath);
                    files.Add(new FileItem
                    {
                        Name = info.Name,
                        FullPath = info.FullName,
                        Type = info.Extension,
                        SizeText = (info.Length / 1024.0).ToString("N2") + " KB",
                        Modified = info.LastWriteTime.ToString()
                    });
                }
            }
            catch { /* skip inaccessible files */ }

            Files.Clear();
            foreach (var f in files)
                Files.Add(f);

            allFiles = files;
        }

        public void FilterFiles(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                Files.Clear();
                foreach (var f in allFiles)
                    Files.Add(f);
            }
            else
            {
                var filtered = allFiles
                    .Where(f => f.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                Files.Clear();
                foreach (var f in filtered)
                    Files.Add(f);
            }
        }

        public void LoadSubFolders(FolderItem folder)
        {
            if (!Directory.Exists(folder.FullPath))
                return;

            if (folder.HasDummy)
                folder.SubFolders.Clear();

            try
            {
                foreach (var dir in Directory.GetDirectories(folder.FullPath))
                {
                    var di = new DirectoryInfo(dir);
                    var subFolder = new FolderItem
                    {
                        Name = di.Name,
                        FullPath = di.FullName
                    };
                    subFolder.AddDummy();
                    folder.SubFolders.Add(subFolder);
                }
            }
            catch { /* skip inaccessible folders */ }
        }
    }
}
