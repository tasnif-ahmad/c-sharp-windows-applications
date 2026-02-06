using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using ToDoListBot.Data;
using ToDoListBot.Models;

namespace ToDoListBot;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    #region Fields & Constructors
    ObservableCollection<TaskItem> Tasks = new();

    public MainWindow()
    {
        InitializeComponent();
        lstTasks.ItemsSource = Tasks;
        LoadTasks();
    }
    #endregion

    #region Event Handlers
    // Add Task Button Click
    private void BtnAddTask_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(txtTask.Text))
        {
            var task = new TaskItem
            {
                Title = txtTask.Text,
                IsCompleted = false
            };

            SaveTask(task);
            txtTask.Clear();
            LoadTasks();
        }
    }

    // Task Status Changed (Checkbox)
    private void TaskStatusChanged(object sender, RoutedEventArgs e)
    {
        if (sender is System.Windows.Controls.CheckBox cb &&
            cb.DataContext is TaskItem task)
        {
            UpdateTask(task);
        }
    }
    #endregion

    #region Helper Methods
    // Load tasks from the database
    private void LoadTasks()
    {
        Tasks.Clear();

        using var db = new AppDBContext();

        var tasks = db.Tasks.AsNoTracking().ToList();

        foreach (var task in tasks)
            Tasks.Add(task);
    }

    // Save a new task to the database
    private void SaveTask(TaskItem task)
    {
        using var db = new AppDBContext();

        db.Tasks.Add(task);
        db.SaveChanges();
    }

    // Update an existing task's completion status
    private void UpdateTask(TaskItem task)
    {
        using var db = new AppDBContext();

        var existingTask = db.Tasks.Find(task.Id);
        if (existingTask != null)
        {
            existingTask.IsCompleted = task.IsCompleted;
            db.SaveChanges();
        }
    }
    #endregion
}
