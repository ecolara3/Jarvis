using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Media.Capture;
using Windows.Media.SpeechRecognition;
using rlara.prototypes.jarvis.Helpers;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace rlara.prototypes.jarvis
{
    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;

        private SpeechRecognizer _speechRecognizer;
        private static bool _stayAlive = true;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            Debug.WriteLine(await MicTest());

            _speechRecognizer = new SpeechRecognizer();

            string[] responses = { "jarvis main light", "jarvis night light" };


            var listConstraint = new Windows.Media.SpeechRecognition.SpeechRecognitionListConstraint(responses, "yesOrNo");

            _speechRecognizer.Constraints.Add(listConstraint);

            await _speechRecognizer.CompileConstraintsAsync();

            _speechRecognizer.ContinuousRecognitionSession.ResultGenerated += ContinuousRecognitionSession_ResultGenerated;
            await _speechRecognizer.ContinuousRecognitionSession.StartAsync();



            while (_stayAlive)
            {

            }
            _deferral.Complete();


        }

        private async Task<bool> MicTest()
        {
            try
            {
                MediaCaptureInitializationSettings init = new MediaCaptureInitializationSettings();
                init.StreamingCaptureMode = StreamingCaptureMode.Audio;
                init.MediaCategory = MediaCategory.Speech;
                MediaCapture capture = new MediaCapture();

                await capture.InitializeAsync(init);

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        private static void ContinuousRecognitionSession_ResultGenerated(
            SpeechContinuousRecognitionSession sender,
            SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {

            Debug.WriteLine(args.Result.Text);

            string mainlight = "jarvis main light";
            string nightlight = "jarvis night light";

            if (args.Result.Text.Contains(mainlight))
            {

                Wemo.Toggle(57);

            }
            else if (args.Result.Text.Contains(nightlight))
            {

                Wemo.Toggle(47);

            }

        }

    }
}
