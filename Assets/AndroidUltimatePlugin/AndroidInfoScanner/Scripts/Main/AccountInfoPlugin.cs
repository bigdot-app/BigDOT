using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AccountInfoPlugin : MonoBehaviour {
	
	private static AccountInfoPlugin instance;
	private static GameObject container;
	private const string TAG="[AccountInfoPlugin]: ";
	private static AUPHolder aupHolder;
	
	#if UNITY_ANDROID
	private static AndroidJavaObject jo;
	#endif	
	
	public bool isDebug =true;
	
	public static AccountInfoPlugin GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name="AccountInfoPlugin";
			instance = container.AddComponent( typeof(AccountInfoPlugin) ) as AccountInfoPlugin;
			DontDestroyOnLoad(instance.gameObject);
			aupHolder = AUPHolder.GetInstance();
			instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
		}
		
		return instance;
	}
	
	private void Awake(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.accountinfo.AccountInfoPlugin");
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
	
	public void SetAccountCallbackListener(Action <string,string> onGetAccountComplete,Action onGetAccountFail){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			AccountInfoCallback  accountCallback = new AccountInfoCallback();
			accountCallback.onGetAccountComplete = onGetAccountComplete;
			accountCallback.onGetAccountFail = onGetAccountFail;
			jo.CallStatic("setAccountCallbackListener",accountCallback);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Gets the account
	/// Note: there's a chance that Account info is empty or sometimes not complete
	/// sometimes email is duplicate
	/// </summary>
	public void GetAccount(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("getAccount");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
}