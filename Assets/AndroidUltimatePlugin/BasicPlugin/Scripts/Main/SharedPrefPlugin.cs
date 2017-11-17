using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SharedPrefPlugin : MonoBehaviour {

	private static SharedPrefPlugin instance;
	private static GameObject container;
	private const string TAG="[CustomCameraPlugin]: ";
	private static AUPHolder aupHolder;

	#if UNITY_ANDROID
	private static AndroidJavaObject jo;
	#endif	

	public bool isDebug =true;
	private bool isInit = false;

	public static SharedPrefPlugin GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name="SharedPrefPlugin";
			instance = container.AddComponent( typeof(SharedPrefPlugin) ) as SharedPrefPlugin;
			DontDestroyOnLoad(instance.gameObject);
			aupHolder = AUPHolder.GetInstance();
			instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
		}

		return instance;
	}

	private void Awake(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.sharedpref.SharedPrefPlugin");
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


	public void SaveString( string sharedPrefname, string dataKey, string value ){		
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("saveString",sharedPrefname,dataKey,value);
			AUP.Utils.Message(TAG,"SaveString");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	public void SaveInt( string sharedPrefname, string dataKey, int value ){		
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("saveInt",sharedPrefname,dataKey,value);
			AUP.Utils.Message(TAG,"SaveInt");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	public void SaveArrayString( string sharedPrefname, string dataKey, List<string> value ){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("saveArrayString",sharedPrefname,dataKey,value);
			AUP.Utils.Message(TAG,"SaveArrayString");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	public String LoadString(string sharedPrefname, string dataKey){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			return jo.CallStatic<String>("loadString",sharedPrefname,dataKey);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif

		return "";
	}

	public int LoadInt(string sharedPrefname, string dataKey){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			return jo.CallStatic<int>("loadInt",sharedPrefname,dataKey);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif

		return 0;
	}

	public List<string> loadArrayString(string sharedPrefname, string dataKey){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			return jo.CallStatic<List<string>>("loadArrayString",sharedPrefname,dataKey);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif

		return null;
	}
}
