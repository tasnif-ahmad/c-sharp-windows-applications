using System.IO;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Threading;

namespace SofiyaBotApplication
{
    public partial class MainWindow : Window
    {
        string baseDebugDirectory = Environment.CurrentDirectory;
        string defaultCommandsPath;

        DispatcherTimer tmrSpeaking;

        SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
        SpeechRecognitionEngine startListening = new SpeechRecognitionEngine();
        SpeechSynthesizer sophia = new SpeechSynthesizer();

        Random random = new Random();
        int recognizeTimeOut = 0;
        DateTime timeNow = DateTime.Now;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            tmrSpeaking = new DispatcherTimer();
            tmrSpeaking.Interval = TimeSpan.FromSeconds(1);
            tmrSpeaking.Tick += TmrSpeaking_Tick;
            tmrSpeaking.Start();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
#if DEBUG
            defaultCommandsPath = Path.GetFullPath(
                Path.Combine(baseDebugDirectory, @"..\..\..\DefaultCommands.txt"));
#else
            defaultCommandsPath = Path.Combine(baseDebugDirectory, @"\DefaultCommands.txt");
#endif
            Logger.Info($"Base Debug Directory: {baseDebugDirectory}");
            Logger.Info($"Default Commands Path: {defaultCommandsPath}");
            // MAIN RECOGNIZER
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.LoadGrammarAsync(
                new Grammar(new GrammarBuilder(
                    new Choices(File.ReadAllLines(defaultCommandsPath)))));

            recognizer.SpeechRecognized += Default_SpeechRecognized;
            recognizer.SpeechDetected += Recognizer_SpeechDetected;
            recognizer.RecognizeAsync(RecognizeMode.Multiple);

            // WAKE WORD LISTENER
            startListening.SetInputToDefaultAudioDevice();
            startListening.LoadGrammarAsync(
                new Grammar(new GrammarBuilder(
                    new Choices(File.ReadAllLines(defaultCommandsPath)))));

            startListening.SpeechRecognized += StartListening_SpeechRecognized;
        }

        private void Default_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;

            Dispatcher.Invoke(() =>
            {
                if (speech == "Hello")
                    sophia.SpeakAsync("Hi, I am here");

                else if (speech == "How are you")
                    sophia.SpeakAsync("I am working normally");

                else if (speech == "What time is it")
                    sophia.SpeakAsync(DateTime.Now.ToString("h mm tt"));

                else if (speech == "Stop talking")
                {
                    sophia.SpeakAsyncCancelAll();
                    sophia.SpeakAsync(
                        random.Next(1, 3) == 1 ? "Yes sir" : "I am sorry, I will be quiet");
                }

                else if (speech == "Stop listening")
                {
                    sophia.SpeakAsync("If you need me just ask");
                    recognizer.RecognizeAsyncCancel();
                    startListening.RecognizeAsync(RecognizeMode.Multiple);
                }

                else if (speech == "Show commands")
                {
                    lstbCommands.Items.Clear();
                    lstbCommands.Visibility = Visibility.Visible;
                    lstbCommands.IsEnabled = false;

                    foreach (var cmd in File.ReadAllLines(defaultCommandsPath))
                        lstbCommands.Items.Add(cmd);
                }

                else if (speech == "Hide commands")
                {
                    lstbCommands.Visibility = Visibility.Collapsed;
                }

                else if (speech == "Exit")
                    Application.Current.Shutdown();
            });
        }

        private void Recognizer_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            recognizeTimeOut = 0;
        }

        private void StartListening_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "Wake up")
            {
                startListening.RecognizeAsyncCancel();
                sophia.SpeakAsync("Yes, I am here");
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        private void TmrSpeaking_Tick(object sender, EventArgs e)
        {
            recognizeTimeOut++;

            if (recognizeTimeOut == 10)
            {
                recognizer.RecognizeAsyncCancel();
            }
            else if (recognizeTimeOut == 11)
            {
                tmrSpeaking.Stop();
                startListening.RecognizeAsync(RecognizeMode.Multiple);
            }
        }
    }
}
