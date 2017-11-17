using UnityEngine;
using System.Collections;
using System;

public class InternetPlugin : MonoBehaviour {
	
	private static InternetPlugin instance;
	private static GameObject container;
	private const string TAG="[InternetPlugin]: ";
	private static AUPHolder aupHolder;
	
	#if UNITY_ANDROID
	private static AndroidJavaObject jo;
	#endif	
	
	public bool isDebug =true;
	
	public static InternetPlugin GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name="InternetPlugin";
			instance = container.AddComponent( typeof(InternetPlugin) ) as InternetPlugin;
			DontDestroyOnLoad(instance.gameObject);
			aupHolder = AUPHolder.GetInstance();
			instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
		}
		
		return instance;
	}
	
	private void Awake(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.internetchecker.InternetPlugin");
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

	public void Init(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("init");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	public void setInternetCallbackListener(Action OnWifiConnect,Action OnWifiDisconnect,Action <int,int>OnWifiSignalStrengthChange){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			InternetCallback internetCallback = new InternetCallback();
			internetCallback.OnWifiConnect = OnWifiConnect;
			internetCallback.OnWifiDisconnect = OnWifiDisconnect;
			internetCallback.OnWifiSignalStrengthChange = OnWifiSignalStrengthChange;
			jo.CallStatic("setInternetCallbackListener",internetCallback);
			AUP.Utils.Message(TAG,"setInternetCallbackListener");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	public void RegisterEvent(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			AUP.Utils.Message(TAG,"registerEvent");
			jo.CallStatic("registerEvent");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	public void UnRegisterEvent(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			AUP.Utils.Message(TAG,"unRegisterEvent");
			jo.CallStatic("unRegisterEvent");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	public void ScanWifi(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			AUP.Utils.Message(TAG,"ScanWifi");
			jo.CallStatic("scanWifi");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	public bool CheckInternet(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			AUP.Utils.Message(TAG,"CheckInternet");
			return jo.CallStatic<bool>("checkInternet");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif

		return false;
	}


	public bool IsWifiConnected(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			AUP.Utils.Message(TAG,"IsWifiConnected");
			return jo.CallStatic<bool>("isWifiConnected");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return false;
	}

	public bool IsMobileConnected(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			AUP.Utils.Message(TAG,"IsMobileConnected");
			return jo.CallStatic<bool>("isMobileConnected");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return false;
	}

	public bool IsMobileConnectionFast(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			AUP.Utils.Message(TAG,"IsMobileConnectionFast");
			return jo.CallStatic<bool>("isMobileConnectionFast");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return false;
	}

	public string GetWifiIP(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			AUP.Utils.Message(TAG,"getWifiIP");
			return jo.CallStatic<string>("getWifiIP");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return "";
	}

	public string GetWifiSSID(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			AUP.Utils.Message(TAG,"getWifiSSID");
			return jo.CallStatic<string>("getWifiSSID");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return "";
	}

	public string GetWifiBSSID(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			AUP.Utils.Message(TAG,"getWifiBSSID");
			return jo.CallStatic<string>("getWifiBSSID");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return "";
	}

	public string GetWifiSpeed(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			AUP.Utils.Message(TAG,"getWifiSpeed");
			return jo.CallStatic<string>("getWifiSpeed");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return "";
	}

	public string GetWifiRssi(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			AUP.Utils.Message(TAG,"getWifiRssi");
			return jo.CallStatic<string>("getWifiRssi");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return "";
	}

}
