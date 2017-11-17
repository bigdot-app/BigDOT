using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class DeviceInfoPlugin : MonoBehaviour {
	
	private static DeviceInfoPlugin instance;
	private static GameObject container;
	private const string TAG="[DeviceInfoPlugin]: ";
	private static AUPHolder aupHolder;
	
	#if UNITY_ANDROID
	private static AndroidJavaObject jo;
	#endif	
	
	public bool isDebug =true;
	
	public static DeviceInfoPlugin GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name="DeviceInfoPlugin";
			instance = container.AddComponent( typeof(DeviceInfoPlugin) ) as DeviceInfoPlugin;
			DontDestroyOnLoad(instance.gameObject);
			aupHolder = AUPHolder.GetInstance();
			instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
		}
		
		return instance;
	}
	
	private void Awake(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.deviceinfo.DeviceInfoPlugin");
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

	/*public void SetDeviceInfoCallbackListener(Action <String>onGetAdvertisingIdComplete,Action <String>onGetAdvertisingIdFail){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			DeviceInfoCallback deviceInfoCallback = new DeviceInfoCallback();
			deviceInfoCallback.onGetAdvertisingIdComplete = onGetAdvertisingIdComplete;
			deviceInfoCallback.onGetAdvertisingIdFail = onGetAdvertisingIdFail;
			jo.CallStatic("setDeviceInfoCallbackListener",deviceInfoCallback);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}*/


	/// <summary>
	/// Gets the android identifier.
	/// </summary>
	/// <returns>The android identifier.</returns>
	/// <param name="type">Type 0 = normal, 1 = md5 type 1, 2 = md5 type 2.</param>
	public String GetAndroidId(int type){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			return jo.CallStatic<String>("getAndroidId",type);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return "";
	}


	/// <summary>
	/// Gets the secure android identifier.
	/// </summary>
	/// <returns>The secure android identifier.</returns>
	public String GetSecureAndroidId(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			return jo.CallStatic<String>("getSecureAndroidId");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif

		return "";
	}



	/// <summary>
	/// Gets the phone number.
	/// Note: sometimes phone number is null or empty, because it is always depends on device settings
	/// </summary>
	/// <returns>The phone number.</returns>
	public String GetPhoneNumber(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			return jo.CallStatic<String>("getPhoneNumber");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return "";
	}

	public String GetTelephonyDeviceId(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			return jo.CallStatic<String>("getTelephonyDeviceId");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return "";
	}

	public String GetTelephonySimSerialNumber(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			return jo.CallStatic<String>("getTelephonySimSerialNumber");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return "";
	}

	/// <summary>
	/// Gets the advertising identifier. using unity3d methods reason
	/// google play change there api and hard to get it know from there
	/// advantage works on android,ios and windows
	/// </summary>
	/// <param name="onGetAdvertisingIdComplete">On get advertising identifier complete.</param>
	/// <param name="onGetAdvertisingIdFail">On get advertising identifier fail.</param>
	public void GetAdvertisingId(Action <String>onGetAdvertisingIdComplete,Action <String>onGetAdvertisingIdFail){		
		#if UNITY_ANDROID || UNITY_IOS
		Application.RequestAdvertisingIdentifierAsync (
			(string advertisingId, bool trackingEnabled, string error) =>
			{ 
				Debug.Log ("advertisingId " + advertisingId + " " + trackingEnabled + " " + error); 
				if(trackingEnabled){
					if(null!=onGetAdvertisingIdComplete){
						onGetAdvertisingIdComplete(advertisingId);
					}	
				}else{
					if(null!=onGetAdvertisingIdFail){
						onGetAdvertisingIdFail(error);
					}
				}
			}
		);
		#else
			AUP.Utils.Message(TAG,"warning: must run in actual mobile device");
		#endif
	}

	public String GenerateUniqueId(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			return jo.CallStatic<String>("generateUniqueId");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return "";
	}

	public bool CheckSim(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			return jo.CallStatic<bool>("checkSim");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return false;
	}
}