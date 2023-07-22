using System.Speech.Recognition;
using System.Windows;
using Speech2Text;

namespace Speech2Text
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SpeechRecognitionEngine speechRecognitionEngine;
        private SpeechToTextConverter speechConverter;
        private bool isListening = false;

        public MainWindow()
        {
            InitializeComponent();
            speechConverter = new SpeechToTextConverter();
            speechConverter.TextRecognized += SpeechConverter_TextRecognized;
            ClearButton.Click += ClearButton_Click;
        }
        private void SpeechConverter_TextRecognized(object sender, string recognizedText)
        {
            Dispatcher.Invoke(() =>
            {
                textBox.Text += recognizedText + " ";
                // Assuming you have a method that converts the recognizedText to the desired format.
                string convertedText = ConvertRecognizedText(recognizedText);
                convertedTextBox.Text = convertedText;
            });
        }
        private string ConvertRecognizedText(string recognizedText)
        {
            // Your logic to convert the recognized text to the desired format goes here.
            // For now, let's simply return the recognized text as it is.
            return recognizedText;
        }
        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            if (!speechConverter.IsListening)
            {
                // Start recognition
                speechConverter.StartRecognition();
                StartStopButton.Content = "Stop Recognition";
            }
            else
            {
                // Stop recognition
                speechConverter.StopRecognition();
                StartStopButton.Content = "Start Recognition";
            }
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            textBox.Clear();
            convertedTextBox.Clear();
        }
    }
}
