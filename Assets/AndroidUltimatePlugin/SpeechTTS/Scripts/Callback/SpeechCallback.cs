using UnityEngine;
using System.Collections;
using System;

public class SpeechCallback :  AndroidJavaProxy {	

	public Action <string>onReadyForSpeech;
	public Action <string>onBeginningOfSpeech;
	public Action <string>onEndOfSpeech;

	//public Action <string>onError;
	public Action <int>onError;

	public Action <string>onResults;
	public Action <string>onPartialResults;
	public Action <string>onShowExtraSupportedLanguage;

	public Action <string>onSpeechSearchComplete;
	public Action <string>onSpeechSearchFail;


	
	public SpeechCallback() : base("com.gigadrillgames.androidplugin.speech.ISpeech") {}

	void ReadyForSpeech(String val){
		onReadyForSpeech(val);
	}

	void BeginningOfSpeech(String val){
		onBeginningOfSpeech(val);
	}


	void EndOfSpeech(String val){
		onEndOfSpeech(val);
	}

	/*void Error(String val){
		onError(val);
	}*/

	void Error(int val){
		onError(val);
	}

	void Results(String val){
		onResults(val);
	}

	void PartialResults(String val){
		onPartialResults(val);
	}

	void ShowExtraSupportedLanguage(String val){
		onShowExtraSupportedLanguage(val);
	}

	void SpeechSearchComplete(String val){
		onSpeechSearchComplete(val);
	}

	void SpeechSearchFail(String val){
		onSpeechSearchFail(val);
	}
}
