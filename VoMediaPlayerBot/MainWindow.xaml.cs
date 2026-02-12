using Microsoft.Win32;
using System;
using System.Windows;
using System.Speech.Recognition;
using System.Speech.Synthesis;


namespace VoMediaPlayerBot
{
    public partial class MainWindow : Window
    {
       
        SpeechRecognitionEngine recognizer;
        SpeechSynthesizer speaker;


        public MainWindow()
        {
            InitializeComponent(); 

            recognizer = new SpeechRecognitionEngine();
            speaker = new SpeechSynthesizer();

            recognizer.SetInputToDefaultAudioDevice();

            Choices commands = new Choices();
            commands.Add(new string[]
{
               "Open",
               "Play",
               "Pause",
               "Volume up",
               "Volume down",
               "Mute",
               "Max volume",
               "Exit"
});


            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(commands);

            Grammar grammar = new Grammar(gb);
            recognizer.LoadGrammar(grammar);

            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;

            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence < 0.7)
                return;

            string command = e.Result.Text;

            Dispatcher.Invoke(() =>
            {
                if (command == "Open")
                    Open_Click(null, null);

                else if (command == "Play")
                    Player.Play();

                else if (command == "Pause")
                    Player.Pause();
                    

                else if (command == "Volume up")
                {
                    if (Player.Volume < 1.0)
                        Player.Volume += 0.1;
                }

                else if (command == "Volume down")
                {
                    if (Player.Volume > 0.0)
                        Player.Volume -= 0.1;
                }

                else if (command == "Mute")
                {
                    Player.Volume = 0;
                }

                else if (command == "Max volume")
                {
                    Player.Volume = 1;
                }
                else if (command == "Exit")
                {
                    recognizer?.RecognizeAsyncCancel();
                    recognizer?.Dispose();
                    Application.Current.Shutdown();
                }
            });
        }


        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
               
               Filter = "Media Files|*.mp3;*.wav;*.mp4;*.wmv"

            };

            if (ofd.ShowDialog() == true)
            {
               
                Player.Source = new Uri(ofd.FileName);
                Player.Play();
            }            
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            Player.Play();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            Player.Pause();
        }

        private void VolumeUp_Click(object sender, RoutedEventArgs e)
        {
            Player.Volume = Math.Min(1.0, Player.Volume + 0.1);
        }

        private void VolumeDown_Click(object sender, RoutedEventArgs e)
        {
            Player.Volume = Math.Max(0.0, Player.Volume - 0.1);
        }

        private void Mute_Click(object sender, RoutedEventArgs e)
        {
            Player.Volume = 0;
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
