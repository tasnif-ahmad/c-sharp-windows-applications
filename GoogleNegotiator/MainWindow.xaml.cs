using System.Windows;
using GoogleNegotiator.Data;
using GoogleNegotiator.Models;
using Microsoft.Data.Sqlite;

namespace GoogleNegotiator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    public MainWindow()
    {
        InitializeComponent();
        btnAdd.IsEnabled = false;
    }

    // Search button
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
        btnAdd.IsEnabled = false;
        if (string.IsNullOrWhiteSpace(txtUser.Text))
        {
            MessageBox.Show("Invalid input.");
            return;
        }

        using (var context = new AppDBContext())
        {
            var result = context.Replies
                .Where(r => r.UserText == txtUser.Text)
                .Select(r => r.Response + " [" + r.ResponseDate.ToString("yyyy-MM-dd hh:mm tt") + "]")
                .ToList();

            txtGoogle.Text = "";

            if (result.Count > 0)
                txtGoogle.Text = string.Join("\n", result);
            else
                txtGoogle.Text = "No saved response found.";
        }
    }

    // Allow custom response
    private void btnMore_Click(object sender, RoutedEventArgs e)
    {
        btnAdd.IsEnabled = true;
        if (string.IsNullOrWhiteSpace(txtUser.Text))
        {
            MessageBox.Show("Enter something first.");
            return;
        }

        txtGoogle.IsReadOnly = false;
        txtGoogle.Text = "";
    }

    // Save to database
    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtUser.Text) ||
            string.IsNullOrWhiteSpace(txtGoogle.Text))
        {
            MessageBox.Show("Invalid entry.");
            return;
        }

        using (var context = new AppDBContext())
        {
            var reply = new Replies
            {
                UserText = txtUser.Text,
                Response = txtGoogle.Text,
                ResponseDate = DateTime.Now
            };

            context.Replies.Add(reply);
            context.SaveChanges();
        }

        MessageBox.Show("Reply saved to database!");
        txtGoogle.IsReadOnly = true;
    }
}
