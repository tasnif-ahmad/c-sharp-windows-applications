using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using MiniFileExplorer.Models;
using MiniFileExplorer.ViewModel;

namespace MiniFileExplorer
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
        }

        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender is TreeViewItem item && item.DataContext is FolderItem folder)
            {
                _viewModel.LoadSubFolders(folder);
            }
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is FolderItem folder)
            {
                _viewModel.LoadFiles(folder.FullPath);
                txtSearch.Text = string.Empty; // reset search bar
            }
        }

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is DataGrid grid && grid.SelectedItem is FileItem file)
            {
                try
                {
                    Process.Start(new ProcessStartInfo(file.FullPath) { UseShellExecute = true });
                }
                catch
                {
                    MessageBox.Show("Unable to open file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.FilterFiles(txtSearch.Text);
        }
    }
}
