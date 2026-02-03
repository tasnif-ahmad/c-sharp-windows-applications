using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DigitalWatchBot;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private DispatcherTimer timer;
    int seconds, minutes, hours;
    string meridiem;
    public MainWindow()
    {
        InitializeComponent();
        DateTime now = DateTime.Now;
        hours = Convert.ToInt32(DateTime.Now.ToString("hh"));
        minutes = now.Minute;
        seconds = now.Second;
        meridiem = now.ToString("tt").ToUpper();

        UpdateTimeDisplay();

        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += Timer_Tick;
        timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        seconds++;
        if (seconds > 59)
        {
            seconds = 0;
            minutes++;
            if (minutes > 59)
            {
                minutes = 0;
                hours++;
                if (hours > 12)
                {
                    hours = 1;
                }
                if (hours > 11)
                {
                    meridiem = (meridiem == "AM") ? "PM" : "AM";
                }
            }
        }

        UpdateTimeDisplay();
    }

    private void UpdateTimeDisplay()
    {
        Hour.Text = hours.ToString("D2");
        Minute.Text = minutes.ToString("D2");
        Second.Text = seconds.ToString("D2");
        Meridiem.Text = meridiem;
    }
}