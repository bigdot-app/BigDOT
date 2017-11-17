using UnityEngine;
using System.Collections;
using System;

public class TTSCallback :  AndroidJavaProxy{
	
	public Action <int>OnInit;
	public Action <string>OnGetLocaleCountry;
	public Action <int>OnSetLocale;
	public Action <string>OnStartSpeech;
	public Action <string>OnDoneSpeech;
	public Action <string>OnErrorSpeech;

	public TTSCallback() : base("com.gigadrillgames.androidplugin.tts.TTSCallback") {}

	void Init(int status){
		OnInit(status);
	}

	void onGetLocaleCountry(String localeCountry){
		OnGetLocaleCountry(localeCountry);
	}

	void onSetLocale(int status){
		OnSetLocale(status);
	}

	void onStartSpeech(String utteranceId){
		OnStartSpeech(utteranceId);
	}

	void onDoneSpeech(String utteranceId){
		OnDoneSpeech(utteranceId);
	}

	void onErrorSpeech(String utteranceId){
		OnErrorSpeech(utteranceId);
	}
}