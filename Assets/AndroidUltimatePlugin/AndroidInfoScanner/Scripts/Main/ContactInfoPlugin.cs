using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ContactInfoPlugin : MonoBehaviour {
	
	private static ContactInfoPlugin instance;
	private static GameObject container;
	private const string TAG="[ContactInfoPlugin]: ";
	private static AUPHolder aupHolder;
	
	#if UNITY_ANDROID
	private static AndroidJavaObject jo;
	#endif	
	
	public bool isDebug =true;
	
	public static ContactInfoPlugin GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name="ContactInfoPlugin";
			instance = container.AddComponent( typeof(ContactInfoPlugin) ) as ContactInfoPlugin;
			DontDestroyOnLoad(instance.gameObject);
			aupHolder = AUPHolder.GetInstance();
			instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
		}
		
		return instance;
	}
	
	private void Awake(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.contactinfo.ContactControllerPlugin");
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
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}	
	
	
	public void Init(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("init");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}	
	
	public void SetContactCallbackListener(Action <string,string>onGetContactComplete,Action onGetContactFail){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			ContactCallback contactCallback = new ContactCallback();
			contactCallback.onGetContactComplete = onGetContactComplete;
			contactCallback.onGetContactFail = onGetContactFail;
			jo.CallStatic("setContactCallbackListener",contactCallback);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	
	public void GetContact(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("getContact");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	public void DestroyLoader(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("destroyLoader");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
}