using UnityEngine;
using System.Collections;
using System;

public class MediaScannerPlugin : MonoBehaviour {

	private static MediaScannerPlugin instance;
	private static GameObject container;
	private const string TAG="[CustomCameraPlugin]: ";
	private static AUPHolder aupHolder;

	#if UNITY_ANDROID
	private static AndroidJavaObject jo;
	#endif	

	public bool isDebug =true;
	private bool isInit = false;

	public static MediaScannerPlugin GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name="MediaScannerPlugin";
			instance = container.AddComponent( typeof(MediaScannerPlugin) ) as MediaScannerPlugin;
			DontDestroyOnLoad(instance.gameObject);
			aupHolder = AUPHolder.GetInstance();
			instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
		}

		return instance;
	}

	private void Awake(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.mediaScanner.MediaScannerPlugin");
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
	/// initialize the camera plugin
	/// </summary>
	public void Init(){
		if(isInit){
			return;
		}

		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("init");
			isInit = true;
			AUP.Utils.Message(TAG,"init");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}


	public void SetCallbackListener(Action onScanStarted,Action onScanComplete,Action onScanFail){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			MediaScannerCallback mediaScannerCallback = new MediaScannerCallback();
			mediaScannerCallback.onScanStarted = onScanStarted;
			mediaScannerCallback.onScanComplete = onScanComplete;
			mediaScannerCallback.onScanFail = onScanFail;

			jo.CallStatic("setCallbackListener",mediaScannerCallback);
			AUP.Utils.Message(TAG,"setCallbackListener");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Scan the specified absolutePath and mimeType.
	/// </summary>
	/// <param name="absolutePath">Absolute path.</param>
	/// <param name="mimeType">MIME type - sample "image/jpeg" </param> 
	public void Scan( string absolutePath, string mimeType ){		
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("scan",absolutePath,mimeType);
			AUP.Utils.Message(TAG,"Scan");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
}
