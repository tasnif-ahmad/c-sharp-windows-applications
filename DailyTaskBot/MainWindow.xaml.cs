using System;
using System.Windows;
using DailyTaskBot.Models;
using DailyTaskBot.Data;

namespace DailyTaskBot
{
    public partial class MainWindow : Window
    {
        #region Fields and Constructor
        // Fields to track the current step and store the report data
        private int step = 1;
        private EmployeeDailyReport currentReport = new EmployeeDailyReport();

        // Constructor
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers
        // Event handler for the Next button click
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            string answer = AnswerBox.Text.Trim();
            if (string.IsNullOrEmpty(answer))
            {
                MessageBox.Show("Please enter a response!");
                return;
            }

            switch (step)
            {
                case 1:
                    currentReport.EmployeeName = Environment.UserName;
                    currentReport.YesterdaysTask = answer;
                    QuestionText.Text = "What are you doing today?";
                    AnswerBox.Clear();
                    step++;
                    break;

                case 2:
                    currentReport.TodaysTask = answer;
                    QuestionText.Text = "Any obstacles?";
                    AnswerBox.Clear();
                    NextButton.Content = "Submit";
                    step++;
                    break;

                case 3:
                    currentReport.Obstacle = answer;
                    SaveReport(currentReport);
                    MessageBox.Show("Report saved successfully!");

                    this.Close();
                    break;
            }
        }
        #endregion

        #region Helper Methods
        // Method to save the report to the database
        private void SaveReport(EmployeeDailyReport report)
        {
            using (var context = new DailyTaskBotContext())
            {
                report.CreatedDate = DateTime.Now;
                context.Reports.Add(report);
                context.SaveChanges();
            }
        }
        #endregion
    }
}
