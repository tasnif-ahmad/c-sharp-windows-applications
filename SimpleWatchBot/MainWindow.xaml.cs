using System.Windows;
using System.Windows.Threading;

namespace SimpleWatchBot;

public partial class MainWindow : Window
{
    private DispatcherTimer timer;
    public MainWindow()
    {
        InitializeComponent();
        TimeText.Text =  DateTime.Now.ToString("hh:mm:ss tt");
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += Timer_Tick;
        timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        TimeText.Text = DateTime.Now.ToString("hh:mm:ss tt");
    }
}