using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace InternetConnectionBot;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private DispatcherTimer timer;
    public MainWindow()
    {
        InitializeComponent();

        double screenWidth = SystemParameters.PrimaryScreenWidth;

        this.Left = screenWidth - this.Width - 10;
        this.Top = 10;

        if (IsConnectionAvailable())
        {
            StatusImage.Source = new BitmapImage(
                new Uri("Assets/green-tick.png", UriKind.Relative));
        }
        else
        {
            StatusImage.Source = new BitmapImage(
                new Uri("Assets/red-cross.png", UriKind.Relative));
        }

        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += Timer_Tick;
        timer.Start();

    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (IsConnectionAvailable())
        {
            StatusImage.Source = new BitmapImage(
                new Uri("Assets/green-tick.png", UriKind.Relative));
        }
        else
        {
            StatusImage.Source = new BitmapImage(
                new Uri("Assets/red-cross.png", UriKind.Relative));
        }
    }

    private bool IsConnectionAvailable()
    {
        try
        {
            Ping myPing = new Ping();
            String host = "google.com";
            byte[] buffer = new byte[32];
            int timeout = 1000;
            PingOptions pingOptions = new PingOptions();
            PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
            return (reply.Status == IPStatus.Success);
        }
        catch (Exception)
        {
            return false;
        }

    }
}
