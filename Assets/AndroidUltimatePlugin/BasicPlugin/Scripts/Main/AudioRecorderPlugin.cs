using UnityEngine;
using System.Collections;
using System;
using AUP;

public class AudioRecorderPlugin : MonoBehaviour {
	
	private static AudioRecorderPlugin instance;
	private static GameObject container;
	private const string TAG="[AudioRecorderPlugin]: ";
	private static AUPHolder aupHolder;
	
	#if UNITY_ANDROID
	private static AndroidJavaObject jo;
	#endif	
	
	public bool isDebug =true;
	
	public static AudioRecorderPlugin GetInstance(){
		if(instance==null){
			aupHolder = AUPHolder.GetInstance();
			container = new GameObject();
			container.name="AudioRecorderPlugin";
			instance = container.AddComponent( typeof(AudioRecorderPlugin) ) as AudioRecorderPlugin;
			DontDestroyOnLoad(instance.gameObject);
			instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
		}
		
		return instance;
	}
	
	private void Awake(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.audiorecorder.AudioRecorderPlugin");
		}
		#endif
	}
	
	/// <summary>
	/// Sets the debug.
	/// 0 - false, 1 - true
	/// </summary>
	/// <param name="debug">Debug.</param>
	public void SetDebug(int debug){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("SetDebug",debug);
			AUP.Utils.Message(TAG,"SetDebug");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	/// <summary>
	/// Sets the volume of audio recorder
	/// </summary>
	/// <param name="val">Value.</param>
	public void SetVolume(float val){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("setVolume",val);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	/// <summary>
	/// Initialize the audio record.
	/// </summary>
	/// <param name="outputPath">Output path.</param>
	/// <param name="filename">Filename.</param>
	/// <param name="usePersistentPath">If set to <c>true</c> use persistent path.</param>
	public void InitAudioRecorder(String outputPath,String filename,bool usePersistentPath){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("initAudioRecorder",outputPath,filename,usePersistentPath);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	/// <summary>
	/// Starts the audio record.
	/// </summary>
	public void StartAudioRecord(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("startAudioRecord");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	/// <summary>
	/// Loads the recorded audio.
	/// </summary>
	public void LoadRecordedAudio(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("loadRecordedAudio");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	
	/// <summary>
	/// Stops the audio record.
	/// </summary>
	public void StopAudioRecord(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("stopAudioRecord");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	/// <summary>
	/// Plays recorded audio.
	/// , 1f pitch normal voice
	/// , 0.5f pitch mimic ogre voice
	/// , 1.5f to 2f pitch mimic chipmunk voice
	/// </summary>
	/// <param name="pitch">Pitch.</param>
	public void PlayAudioRecord(float pitch){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("playAudioRecord",pitch);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	
	/// <summary>
	/// Stops the audio record play.
	/// </summary>
	public void StopAudioRecordPlay(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("stopAudioRecordPlay");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	/// <summary>
	/// Pauses the audio record play.
	/// </summary>
	public void PauseAudioRecordPlay(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("pauseAudioRecordPlay");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	/// <summary>
	/// Releases the audio service - always call this when you are done using audio recorder failed to do this result on 
	/// other application or your application unable to access audio or mic service
	/// when this happened you need to restart you phone to used audio service again
	/// </summary>
	public void ReleaseAudio(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("releaseAudio");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
}