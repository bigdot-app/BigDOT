using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using AUP;

using ApiAiSDK;
using ApiAiSDK.Model;
using ApiAiSDK.Unity;

using UnityEngine.Networking;

using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class AppManagerScript : MonoBehaviour {
  private UtilsPlugin utilsPlugin;
  private SpeechPlugin speechPlugin;
  private TextToSpeechPlugin textToSpeechPlugin;
  private Dispatcher dispatcher;

  public RetrieveDataForBarGraph barGraphThing;
  public RetrieveDataForTimeGraph timeGraphThing;
  public GameObject barGraphObject, timeGraphObject, dataflowObject;

  public Image microphoneImage;
  public Image dotsImage;

  private ApiAiUnity apiaiClient;

  private string TAG = "[AppManagerScript]: ";

  void Start() {
    dispatcher = Dispatcher.GetInstance();

    utilsPlugin = UtilsPlugin.GetInstance();
    utilsPlugin.SetDebug(0);

    speechPlugin = SpeechPlugin.GetInstance();
    speechPlugin.SetDebug(0);
    speechPlugin.Init();

    textToSpeechPlugin = TextToSpeechPlugin.GetInstance();
    textToSpeechPlugin.SetDebug(0);
    textToSpeechPlugin.Initialize();

    AddSpeechPluginListener();
    AddTextToSpeechListener();

    apiaiClient = new ApiAiUnity();
    apiaiClient.Initialize(new AIConfiguration("a984bc306ae24d11b4105e251f892a21", SupportedLanguage.English));
  }

  void OnApplicationPause(bool val) {
    if (speechPlugin != null) {
      if (val) {
        RemoveSpeechPluginListener();
      } else {
        AddSpeechPluginListener();
      }
    }

    if (textToSpeechPlugin != null && textToSpeechPlugin.isInitialized()) {
      if (val) {
          textToSpeechPlugin.UnRegisterBroadcastEvent();
      } else {
          textToSpeechPlugin.RegisterBroadcastEvent();
      }
    }
  }

  void OnDestroy() {
    RemoveSpeechPluginListener();
    speechPlugin.StopListening();
    textToSpeechPlugin.ShutDownTextToSpeechService();
  }

  private void AddSpeechPluginListener() {
    if (speechPlugin != null) {
      speechPlugin.onReadyForSpeech += onReadyForSpeech;
      speechPlugin.onError += onError;
      speechPlugin.onResults += onResults;
    }
  }

  private void RemoveSpeechPluginListener() {
    if (speechPlugin != null) {
      speechPlugin.onReadyForSpeech -= onReadyForSpeech;
      speechPlugin.onError -= onError;
      speechPlugin.onResults -= onResults;
    }
  }

  private void AddTextToSpeechListener() {
    textToSpeechPlugin.OnInit += OnInit;
    textToSpeechPlugin.OnEndSpeech += OnEndOrErrorSpeech;
    textToSpeechPlugin.OnErrorSpeech += OnEndOrErrorSpeech;
  }

  private void RemoveTextToSpeechListener() {
    textToSpeechPlugin.OnInit -= OnInit;
    textToSpeechPlugin.OnEndSpeech -= OnEndOrErrorSpeech;
    textToSpeechPlugin.OnErrorSpeech -= OnEndOrErrorSpeech;
  }

  private void DelayUnMute() {
    if (utilsPlugin != null)
      utilsPlugin.UnMuteBeep();
  }

  private void CancelSpeech() {
    if (speechPlugin != null) {
      bool isSupported = speechPlugin.CheckSpeechRecognizerSupport();

      if (isSupported)
        speechPlugin.Cancel();
    }

    Debug.Log(TAG + " call CancelSpeech..  ");
  }

  private void StopListening() {
    if (speechPlugin != null)
      speechPlugin.StopListening();
    Debug.Log(TAG + " StopListening...  ");
  }

  private void StartListening() {
    Debug.Log(TAG + " StartListening...  ");
    bool isSupported = speechPlugin.CheckSpeechRecognizerSupport();

    if (isSupported) {
      utilsPlugin.UnMuteBeep();

      // enable offline
      // speechPlugin.EnableOffline(true);

      int numberOfResults = 3;
      speechPlugin.StartListening(numberOfResults);
    } else {
      utilsPlugin.ShowToastMessage(TAG + "Speech Recognizer not supported by this Android device ");
    }
  }

  private float waitingInterval = 2f;

  private void WaitingMode() {
    Debug.Log("Waiting...");
  }

  public void SpeakTTS(string whatToSay) {
    if (textToSpeechPlugin.isInitialized()) {
      utilsPlugin.UnMuteBeep();
      textToSpeechPlugin.SpeakOut(whatToSay, "null");
    }
  }

  public void SendTextToApiAi(string text) {
    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
      return true;
    };

    try {
      var response = apiaiClient.TextRequest(text);
      if (response != null) {
        ProcessOutput(response);
      } else {
        utilsPlugin.ShowToastMessage("Response is null");
      }
    } catch (Exception ex) {
      utilsPlugin.ShowAlertPopup("API.AI Error", "Error while processing API.AI response: " + ex);
    }
  }

  public void ProcessOutput(ApiAiSDK.Model.AIResponse res) {
    string action = res.Result.Action;
    string speech = res.Result.Fulfillment.Speech;

    if (!speech.Contains("?")) {
      switch (action) {
        case "show_dataflow":
          barGraphObject.SetActive(false);
          timeGraphObject.SetActive(false);
          dataflowObject.SetActive(true);
          break;
        case "show_bar_graph_realtime":
          barGraphThing.BarCommand = "realtime:" + res.Result.GetStringParameter("data-group");
          barGraphObject.SetActive(true);
          timeGraphObject.SetActive(false);
          dataflowObject.SetActive(false);
          break;
        case "show_bar_graph_analysis_results":
          barGraphThing.BarCommand = "analysis:null";
          barGraphObject.SetActive(true);
          timeGraphObject.SetActive(false);
          dataflowObject.SetActive(false);
          break;
        case "show_time_graph":
          string sensorType = res.Result.GetStringParameter("sensor-type");
          JObject durationObj = res.Result.GetJsonParameter("duration");
          int durationAmount = Int32.Parse(durationObj.SelectToken("amount"));
          string durationUnit = durationObj.SelectToken("unit");
          utilsPlugin.ShowToastMessage(action + " => " + sensorType + " => " + durationAmount + " " + durationUnit);
          utilsPlugin.ShowToastMessage(speech);

          timeGraphThing.SensorType = sensorType;
          switch (durationUnit) {
            case "h":
              speech += "ours";
              timeGraphThing.TimeFromNow = durationAmount + ":00:00";
              break;
            case "min":
              speech += "utes";
              timeGraphThing.TimeFromNow = "00:" + durationAmount + ":00";
              break;
            case "s":
              speech += "econds";
              timeGraphThing.TimeFromNow = "00:00:" + durationAmount;
              break;
          }

          timeGraphObject.SetActive(true);
          barGraphObject.SetActive(false);
          dataflowObject.SetActive(false);
          timeGraphThing.GetSensorData();
          break;
        case "hide_everything":
          barGraphObject.SetActive(false);
          timeGraphObject.SetActive(false);
          dataflowObject.SetActive(false);
          break;
        default:
          utilsPlugin.ShowToastMessage(action);
          break;
      }
    }

    SpeakTTS(speech);
  }

  //SpeechRecognizer Events
  private void onReadyForSpeech(string data) {
    dispatcher.InvokeAction(
      () => {
        if (speechPlugin != null)
          speechPlugin.EnableModal(false);

        microphoneImage.gameObject.SetActive(false);
        dotsImage.gameObject.SetActive(true);
      }
    );
  }

  private void onError(int data) {
    dispatcher.InvokeAction(
      () => {
        // unmute beep
        CancelInvoke("DelayUnMute");
        Invoke("DelayUnMute", 0.3f);

        SpeechRecognizerError error = (SpeechRecognizerError) data;

        utilsPlugin.ShowToastMessage("Error while Listening: " + error.ToString());
        microphoneImage.gameObject.SetActive(true);
        dotsImage.gameObject.SetActive(false);
      }
    );
  }

  private void onResults(string data) {
    dispatcher.InvokeAction(
      () => {
        // unmute beep
        CancelInvoke("DelayUnMute");
        Invoke("DelayUnMute", 0.3f);

        //sample showing the nearest result
        string text = data.Split(',').GetValue(0).ToString();
        SendTextToApiAi(text);

        microphoneImage.gameObject.SetActive(true);
        dotsImage.gameObject.SetActive(false);
      }
    );
  }
  //SpeechRecognizer Events

  //TextToSpeechPlugin Events
  private void OnInit(int status) {
    dispatcher.InvokeAction(
      () => {
        if (status == 1) {
          textToSpeechPlugin.SetLocale(SpeechLocale.US);
          textToSpeechPlugin.SetPitch(1f);
          textToSpeechPlugin.SetSpeechRate(1f);
        } else {
          utilsPlugin.ShowToastMessage("Error while initializing TTS!");
        }

        CancelInvoke("WaitingMode");
        Invoke("WaitingMode", waitingInterval);
      }
    );
  }

  private void OnEndOrErrorSpeech(string utteranceId) {
    dispatcher.InvokeAction(
      () => {
        CancelInvoke("WaitingMode");
        Invoke("WaitingMode", waitingInterval);
      }
    );
  }
  //TextToSpeechPlugin Events

  public void ToggleSpeechState() {
    if (microphoneImage.gameObject.activeInHierarchy) {
      // start listening to speech
      StartListening();
    } else {
      if (speechPlugin != null) speechPlugin.StopCancel();
      if (textToSpeechPlugin != null) textToSpeechPlugin.Stop();
    }
  }

  void Update() {
    if (apiaiClient != null) {
      apiaiClient.Update();
    }
  }
}