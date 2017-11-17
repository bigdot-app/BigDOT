using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using AUP;

public class SpeechRecognizerDemo2 : MonoBehaviour
{
	
    private const string TAG = "[SpeechRecognizerDemo]: ";
	
    private SpeechPlugin speechPlugin;
    public Text resultText;
    public Text partialResultText;
    public Text statusText;
    public SpeechExtraLocale currentExtraLocale = SpeechExtraLocale.JP;

    public Text speechExtraLocaleText;
    public Slider speechExtaLocaleSlider;

    private Dispatcher dispatcher;
    private UtilsPlugin utilsPlugin;
	
    // Use this for initialization
    void Start()
    {		
        dispatcher = Dispatcher.GetInstance();
        // for accessing audio
        utilsPlugin = UtilsPlugin.GetInstance();
        utilsPlugin.SetDebug(0);

        speechPlugin = SpeechPlugin.GetInstance();
        speechPlugin.SetDebug(0);
        speechPlugin.Init();

        AddSpeechPluginListener();
    }

    private void OnEnable()
    {
        AddSpeechPluginListener();
    }

    private void OnDisable()
    {
        RemoveSpeechPluginListener();
    }

    private void AddSpeechPluginListener()
    {
        if (speechPlugin != null)
        {
            //add speech recognizer listener
            speechPlugin.onReadyForSpeech += onReadyForSpeech;
            speechPlugin.onBeginningOfSpeech += onBeginningOfSpeech;
            speechPlugin.onEndOfSpeech += onEndOfSpeech;
            speechPlugin.onError += onError;
            speechPlugin.onResults += onResults;
            speechPlugin.onPartialResults += onPartialResults;
        }
    }

    private void RemoveSpeechPluginListener()
    {
        if (speechPlugin != null)
        {
            //remove speech recognizer listener
            speechPlugin.onReadyForSpeech -= onReadyForSpeech;
            speechPlugin.onBeginningOfSpeech -= onBeginningOfSpeech;
            speechPlugin.onEndOfSpeech -= onEndOfSpeech;
            speechPlugin.onError -= onError;
            speechPlugin.onResults -= onResults;
            speechPlugin.onPartialResults -= onPartialResults;
        }
    }

    private void OnApplicationPause(bool val)
    {
        if (speechPlugin != null)
        {
            if (val)
            {
                RemoveSpeechPluginListener();
            }
            else
            {
                AddSpeechPluginListener();
            }
        }
    }

    //this is for debug or test purpose only to log available extra locale on adb using  "adb logcat -s Unity" comand on command prompt or terminal
    public void CheckSpeechRecognizerExtraLanguage()
    {
        string[] extraLanguageAvailable = speechPlugin.GetExtraLanguage();
        foreach (string extraLocale in extraLanguageAvailable)
        {
            Debug.Log(TAG + extraLocale);
        }
    }

    public void StartListeningWithExtraLanguage()
    {
        bool isSupported = speechPlugin.CheckSpeechRecognizerSupport();
		
        if (isSupported)
        {			

            // unmute beep
            utilsPlugin.UnMuteBeep();

            // enable offline
            //speechPlugin.EnableOffline(true);

            // enable partial Results
            speechPlugin.EnablePartialResult(true);
			
            int numberOfResults = 5;
            speechPlugin.StartListeningWithHackExtraLanguage(numberOfResults, currentExtraLocale.GetDescription());
        }
        else
        {
            Debug.Log("Speech Recognizer not supported by this Android device ");
        }
    }
	
    //cancel speech
    public void CancelSpeech()
    {
        if (speechPlugin != null)
        {
            bool isSupported = speechPlugin.CheckSpeechRecognizerSupport();
			
            if (isSupported)
            {			
                speechPlugin.Cancel();
            }
        }
		
        Debug.Log(TAG + " call CancelSpeech..  ");
    }

    public void StopListening()
    {
        if (speechPlugin != null)
        {
            speechPlugin.StopListening();
        }
        Debug.Log(TAG + " StopListening...  ");
    }

    public void StopCancel()
    {
        if (speechPlugin != null)
        {
            speechPlugin.StopCancel();
        }
        Debug.Log(TAG + " StopCancel...  ");
    }

    public void OnSpeechExtraLocaleSliderChange()
    {
        Debug.Log("[TextToSpeechDemo] OnExtraLocaleSliderChange");
        if (speechExtaLocaleSlider != null)
        {

            //update current extra locale here
            currentExtraLocale = (SpeechExtraLocale)speechExtaLocaleSlider.value;

            //update the status to notify user
            UpdateSpeechExtraLocale(currentExtraLocale);
        }
    }

    private void UpdateSpeechExtraLocale(SpeechExtraLocale ttsLocaleCountry)
    {
        if (speechExtraLocaleText != null)
        {
            speechExtraLocaleText.text = String.Format("Extra Locale: {0}", ttsLocaleCountry);
        }
    }

    private void UpdateStatus(string status)
    {
        if (statusText != null)
        {
            statusText.text = String.Format("Status: {0}", status);	
        }
    }

    public void NextExtraLocale()
    {
        if (speechExtaLocaleSlider != null)
        {
            if (speechExtaLocaleSlider.value < speechExtaLocaleSlider.maxValue)
            {
                speechExtaLocaleSlider.value++;
            }
        }
    }

    public void PrevExtraLocale()
    {
        if (speechExtaLocaleSlider != null)
        {
            if (speechExtaLocaleSlider.value > 1)
            {
                speechExtaLocaleSlider.value--;
            }
        }
    }

    private void OnDestroy()
    {
        RemoveSpeechPluginListener();
        speechPlugin.StopListening();
        speechPlugin.DestroySpeechController();
    }
	
    //SpeechRecognizer Events
    private void onReadyForSpeech(string data)
    {
        dispatcher.InvokeAction(
            () =>
            {
                if (speechPlugin != null)
                {
                    //Disables modal
                    speechPlugin.EnableModal(false);	
                }

                if (statusText != null)
                {
                    statusText.text = String.Format("Status: {0}", data.ToString()); 
                }
            }
        );
    }

    private void onBeginningOfSpeech(string data)
    {
        dispatcher.InvokeAction(
            () =>
            {
                if (statusText != null)
                {
                    statusText.text = String.Format("Status: {0}", data.ToString()); 
                }
            }
        );
    }

    private void onEndOfSpeech(string data)
    {
        dispatcher.InvokeAction(
            () =>
            {
                if (statusText != null)
                {
                    statusText.text = String.Format("Status: {0}", data.ToString()); 
                }
            }
        );
    }

    private void onError(int data)
    {
        dispatcher.InvokeAction(
            () =>
            {
                if (statusText != null)
                {
					
                    SpeechRecognizerError error = (SpeechRecognizerError)data;
                    statusText.text = String.Format("Status: {0}", error.ToString());
                }
				
                if (resultText != null)
                {
                    resultText.text = "Result: Waiting for result...";
                }
            }
        );
    }

    private void onResults(string data)
    {
        dispatcher.InvokeAction(
            () =>
            {
                if (resultText != null)
                {
                    string[] results = data.Split(',');
                    Debug.Log(TAG + " result length " + results.Length);

                    //when you set morethan 1 results index zero is always the closest to the words the you said
                    //but it's not always the case so if you are not happy with index zero result you can always 
                    //check the other index

                    //sample on checking other results
                    foreach (string possibleResults in results)
                    {
                        Debug.Log(TAG + " possibleResults " + possibleResults);
                    }

                    //sample showing the nearest result
                    string whatToSay = results.GetValue(0).ToString();
                    resultText.text = string.Format("Result: {0}", whatToSay);
                }		
            }		
        );

    }

    private void onPartialResults(string data)
    {
        dispatcher.InvokeAction(
            () =>
            {
                if (partialResultText != null)
                {
                    string[] results = data.Split(',');
                    Debug.Log(TAG + " partial result length " + results.Length);

                    //when you set morethan 1 results index zero is always the closest to the words the you said
                    //but it's not always the case so if you are not happy with index zero result you can always 
                    //check the other index

                    //sample on checking other results
                    foreach (string possibleResults in results)
                    {
                        Debug.Log(TAG + " partial possibleResults " + possibleResults);
                    }

                    //sample showing the nearest result
                    string whatToSay = results.GetValue(0).ToString();				
                    partialResultText.text = string.Format("Partial Result: {0}", whatToSay);				
                }
            }
        );
    }
    //SpeechRecognizer Events
}