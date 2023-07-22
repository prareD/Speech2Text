using System;
using System.Diagnostics;
using System.Speech.Recognition;

namespace Speech2Text
{
    public class SpeechToTextConverter
    {
        private SpeechRecognitionEngine speechRecognitionEngine;
        private bool isInitialized = false;
        private bool isRecognizing = false;

        public event EventHandler<string> TextRecognized;

        public bool IsListening { get; private set; }

        public SpeechToTextConverter()
        {
            InitializeSpeechRecognitionEngine();
        }

        private void InitializeSpeechRecognitionEngine()
        {
            speechRecognitionEngine = new SpeechRecognitionEngine();
            speechRecognitionEngine.SetInputToDefaultAudioDevice();

            // Load a grammar (we're using DictationGrammar for this example)
            var dictationGrammar = new DictationGrammar();
            speechRecognitionEngine.LoadGrammar(dictationGrammar);

            speechRecognitionEngine.SpeechRecognized += SpeechRecognitionEngine_SpeechRecognized;
            if (!IsListening)
            {
                speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
                IsListening = true;
            }
            IsListening = false;
        }


        private void SpeechRecognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result != null)
            {
                Debug.WriteLine("Recognized: " + e.Result.Text + ", Confidence: " + e.Result.Confidence);
                if (e.Result.Confidence > 0)
                {
                    string recognizedText = e.Result.Text;
                    OnTextRecognized(recognizedText);
                }
            }
            isRecognizing = false;
        }


        protected virtual void OnTextRecognized(string recognizedText)
        {
            TextRecognized?.Invoke(this, recognizedText);
        }

        public void StartRecognition()
        {
            if (!isInitialized)
            {
                speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
                isInitialized = true;
            }

            if (!IsListening && !isRecognizing)
            {
                // Set the flag to indicate that recognition is in progress
                isRecognizing = true;
                speechRecognitionEngine.RecognizeAsync();
                IsListening = true;
            }
        }

        public void StopRecognition()
        {
            if (IsListening)
            {
                speechRecognitionEngine.RecognizeAsyncCancel();
                IsListening = false;
            }
        }
    }
}
