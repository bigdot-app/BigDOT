using UnityEngine;
using System.Collections;
using System;
using AUP;

public class SpeechPlugin : MonoBehaviour
{

    private static SpeechPlugin instance;
    private static GameObject container;
    private const string TAG = "[SpeechPlugin]: ";
    private static AUPHolder aupHolder;

    #if UNITY_ANDROID
    private static AndroidJavaObject jo;
    #endif
	
    public bool isDebug = true;
    private string[] extraLanguage;

    private Action <string> ReadyForSpeech;

    public event Action <string>onReadyForSpeech
    {
        add{ ReadyForSpeech += value;}
        remove{ ReadyForSpeech -= value;}
    }

    private Action <string> BeginningOfSpeech;

    public event Action <string>onBeginningOfSpeech
    {
        add{ BeginningOfSpeech += value;}
        remove{ BeginningOfSpeech -= value;}
    }

    private Action <string> EndOfSpeech;

    public event Action <string>onEndOfSpeech
    {
        add{ EndOfSpeech += value;}
        remove{ EndOfSpeech -= value;}
    }

    private Action <int> Error;

    public event Action <int>onError
    {
        add{ Error += value;}
        remove{ Error -= value;}
    }

    private Action <string> Results;

    public event Action <string>onResults
    {
        add{ Results += value;}
        remove{ Results -= value;}
    }

    private Action <string> PartialResults;

    public event Action <string>onPartialResults
    {
        add{ PartialResults += value;}
        remove{ PartialResults -= value;}
    }

    private Action <string> ShowExtraSupportedLanguage;

    public event Action <string>onShowExtraSupportedLanguage
    {
        add{ ShowExtraSupportedLanguage += value;}
        remove{ ShowExtraSupportedLanguage -= value;}
    }

    public static event Action <string> OnSpeechSearchComplete;
    public static event Action <string> OnSpeechSearchFail;

    //new
    private bool hasInit = false;

	
    public static SpeechPlugin GetInstance()
    {
        if (instance == null)
        {
            container = new GameObject();
            container.name = "SpeechPlugin";
            instance = container.AddComponent(typeof(SpeechPlugin)) as SpeechPlugin;
            DontDestroyOnLoad(instance.gameObject);
            aupHolder = AUPHolder.GetInstance();
            instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
        }
		
        return instance;
    }

    private void Awake()
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.speech.SpeechPlugin");
        }
        #endif
    }

    /// <summary>
    /// Sets the debug.
    /// 0 - false, 1 - true
    /// </summary>
    /// <param name="debug">Debug.</param>
    public void SetDebug(int debug)
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("SetDebug", debug);
            AUP.Utils.Message(TAG, "SetDebug");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    /// <summary>
    /// Initialize SpeechPlugin.
    /// Note: must be called 1st before calling other methods
    /// </summary>
    public void Init()
    {
        if (!hasInit)
        {
            hasInit = true;
            #if UNITY_ANDROID
            if (Application.platform == RuntimePlatform.Android)
            {
                jo.CallStatic("init");
                setSpeechEventListener(onSpeechRecognizerReadyForSpeech
										, onSpeechRecognizerBeginningOfSpeech
				                       	, onSpeechRecognizerEndOfSpeech
										, onSpeechRecognizerError
										, onSpeechRecognizerResults
										, onSpeechRecognizerPartialResults
										, onSpeechRecognizerShowExtraSupportedLanguage
				                       	, onSpeechSearchComplete
										, onSpeechSearchFail
                );
                Invoke("CheckLanguageDetails", 0.2f);
                //CheckLanguageDetails();
            }
            else
            {
                AUP.Utils.Message(TAG, "warning: must run in actual android device");
            }
            #endif
        }
        else
        {
            AUP.Utils.Message(TAG, "Speech Recognizer is already initialized calling this method is useless");
        }
    }


    /// <summary>
    /// Sets the calling package.
    /// this method is optional you just need to set this if your app
    /// is for kids or children
    ///you can put your app package here ex. com.mycoolcompany.mygreatgame
    /// if you don't set package here it will auto 
    /// default on the calling activity which is activity of
    /// Unity3d
    /// </summary>
    /// <param name="val">Value.</param>
    public void SetCallingPackage(string val)
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("setCallingPackage", val);
            AUP.Utils.Message(TAG, "setCallingPackage");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    /// <summary>
    /// Enables or Disables modal.
    /// Note: must be call after sucessful init
    /// </summary>
    /// <param name="val">If set to <c>true</c> value.</param>
    public void EnableModal(bool val)
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("enableModal", val);
            AUP.Utils.Message(TAG, "enableModal");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    /// <summary>
    /// Enables the offline. not if this always works but try it
    /// </summary>
    /// <param name="val">If set to <c>true</c> value.</param>
    public void EnableOffline(bool val)
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("enableOffline", val);
            AUP.Utils.Message(TAG, "enableOffline");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    public void EnablePartialResult(bool val)
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("enablePartialResult", val);
            AUP.Utils.Message(TAG, "enablePartialResult");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    /// <summary>
    /// Checks the speech recognizer support.
    /// </summary>
    /// <returns><c>true</c>, if speech recognizer support was checked, <c>false</c> otherwise.</returns>
    public bool CheckSpeechRecognizerSupport()
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            return jo.CallStatic<bool>("checkSpeechRecognizer");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
		
        return false;
    }

    /// <summary>
    /// Checks what language available on device
    /// </summary>
    private void CheckLanguageDetails()
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("checkLanguageDetails");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    /// <summary>
    /// Sets the speech event listener.
    /// </summary>
    /// <param name="onReadyForSpeech">On ready for speech.</param>
    /// <param name="onBeginningOfSpeech">On beginning of speech.</param>
    /// <param name="onEndOfSpeech">On end of speech.</param>
    /// <param name="onError">On error.</param>
    /// <param name="onResults">On results.</param>
    /// <param name="onShowExtraSupportedLanguage">On show extra supported language.</param>
    /// <param name="onSpeechSearchComplete">On speech search complete.</param>
    /// <param name="onSpeechSearchFail">On speech search fail.</param>
    private void setSpeechEventListener(
        Action <string>onReadyForSpeech
		, Action <string>onBeginningOfSpeech
		, Action <string>onEndOfSpeech
		, Action <int>onError
		, Action <string>onResults
		, Action <string>onPartialResults
		, Action <string>onShowExtraSupportedLanguage
		, Action <string>onSpeechSearchComplete
		, Action <string>onSpeechSearchFail
    )
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            SpeechCallback speechCallback = new SpeechCallback();
            speechCallback.onReadyForSpeech = onReadyForSpeech;
            speechCallback.onBeginningOfSpeech = onBeginningOfSpeech;
            speechCallback.onEndOfSpeech = onEndOfSpeech;
            speechCallback.onError = onError;
            speechCallback.onResults = onResults;
            speechCallback.onPartialResults = onPartialResults;
            speechCallback.onShowExtraSupportedLanguage = onShowExtraSupportedLanguage;
            speechCallback.onSpeechSearchComplete = onSpeechSearchComplete;
            speechCallback.onSpeechSearchFail = onSpeechSearchFail;
			
			
            jo.CallStatic("setSpeechEventListener", speechCallback);
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    /// <summary>
    /// Activate the listener for your speech or voice
    /// it will now detect what words you said
    /// although it's not always correct but it is alway nearest
    /// to the words that you said
    /// </summary>
    /// <param name="numberOfResults">Number of results.</param>
    public void StartListening(int numberOfResults)
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("startListening", numberOfResults);
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    /// <summary>
    /// Starts the listening with extra setting. you can use this if you want to control 
    /// the speech length and waiting time to consider the speech is complete
    /// Note: you only need to try this if you really don't like the default one
    /// usually this is not the best choice because you need to tune it carefully
    /// setting this values to different values especially un correct one might result to different scenarios
    /// maybe Good or worst your call
    /// </summary>
    /// <param name="numberOfResults">Number of results.</param>
    /// <param name="speechMinimumLengthInMillis">Speech minimum length in millis.</param>
    /// <param name="speechWaitingTimeInMillis">Speech waiting time in millis.</param>
    public void StartListeningWithExtraSetting(
        int numberOfResults
		, int speechMinimumLengthInMillis
		, int speechWaitingTimeInMillis
    )
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("startListeningWithExtraSetting", numberOfResults, speechMinimumLengthInMillis, speechWaitingTimeInMillis);
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    /// <summary>
    /// Starts the listening with extra language.
    /// Note: not almost all the time any language is available and working, it may work or not work
    /// </summary>
    /// <param name="numberOfResults">Number of results.</param>
    /// <param name="extraLanguage">Extra language.</param>
    public void StartListeningWithExtraLanguage(int numberOfResults, string extraLanguage)
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("startListeningWithExtraLanguage", numberOfResults, extraLanguage);
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    /// <summary>
    /// Starts the listening with hack extra language.
    /// </summary>
    /// <param name="numberOfResults">Number of results.</param>
    /// <param name="extraLanguage">Extra language.</param>
    public void StartListeningWithHackExtraLanguage(int numberOfResults, string extraLanguage)
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("startListeningWithHackExtraLanguage", numberOfResults, extraLanguage);
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    /// <summary>
    /// Cancels the speech recognition
    /// </summary>
    public void Cancel()
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("cancel");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    /// <summary>
    /// Removes Speech Recognizer listener.
    /// </summary>
    public void StopListening()
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("stopListening");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    /// <summary>
    /// if cancel and stop listener don't work use this
    /// Stops the cancel Speech Recognizer
    /// </summary>
    public void StopCancel()
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("stopCancel");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    /// <summary>
    /// Destroys the speech Recognizer controller
    /// Note: Call this when you are done using it.
    /// </summary>
    public void DestroySpeechController()
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("destroySpeechController");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    public string[] GetExtraLanguage()
    {
        return Utils.DeepCopy<string[]>(extraLanguage);
    }

    public int GetExtraLanguageLength()
    {
        return extraLanguage.Length;
    }

    public string GetExtraLanguageByIndex(int index)
    {
        return (string)extraLanguage.GetValue(index);
    }

    private void onSpeechRecognizerReadyForSpeech(string val)
    {
        if (null != ReadyForSpeech)
        {
            ReadyForSpeech(val);
        }
    }

    private void onSpeechRecognizerBeginningOfSpeech(string val)
    {
        if (null != BeginningOfSpeech)
        {
            BeginningOfSpeech(val);
        }
    }

    private void onSpeechRecognizerEndOfSpeech(string val)
    {
        if (null != EndOfSpeech)
        {
            EndOfSpeech(val);
        }
    }

    private void onSpeechRecognizerError(int val)
    {
        if (null != Error)
        {
            Error(val);
        }
    }

    private void onSpeechRecognizerResults(string val)
    {
        if (null != Results)
        {
            Results(val);
        }
    }

    private void onSpeechRecognizerPartialResults(string val)
    {
        if (null != PartialResults)
        {
            PartialResults(val);
        }
    }

    private void onSpeechRecognizerShowExtraSupportedLanguage(string extraSupportedLanguage)
    {
        Debug.Log(TAG + "OnShowExtraSupportedLanguage extraSupportedLanguage: " + extraSupportedLanguage);
        extraLanguage = extraSupportedLanguage.Split(',');
        Debug.Log(TAG + "first element extraSupportedLanguage: " + extraLanguage.GetValue(0));

        if (null != ShowExtraSupportedLanguage)
        {
            ShowExtraSupportedLanguage(extraSupportedLanguage);
        }
    }

    private void onSpeechSearchComplete(string val)
    {
        Debug.Log(TAG + "onSpeechSearchComplete val: " + val);
        SpeechSearchComplete(val);
    }

    private void onSpeechSearchFail(string val)
    {
        Debug.Log(TAG + "onSpeechSearchFail val: " + val);
        SpeechSearchFail(val);
    }

    internal void SpeechSearchComplete(string val)
    {
        if (OnSpeechSearchComplete != null)
        {
            OnSpeechSearchComplete(val);
        }
    }

    internal void SpeechSearchFail(string val)
    {
        if (OnSpeechSearchFail != null)
        {
            OnSpeechSearchFail(val);
        }
    }
}