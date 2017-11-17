using UnityEngine;
using System.Collections;
using System;

public class BatteryPlugin : MonoBehaviour {
	
	private static BatteryPlugin instance;
	private static GameObject container;
	private static AUPHolder aupHolder;
	private const string TAG="[BatteryPlugin]: ";
	
	#if UNITY_ANDROID
	private static AndroidJavaObject jo;
	#endif	
	
	public bool isDebug =true;
	
	public static BatteryPlugin GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name="BatteryPlugin";
			instance = container.AddComponent( typeof(BatteryPlugin) ) as BatteryPlugin;
			DontDestroyOnLoad(instance.gameObject);
			aupHolder = AUPHolder.GetInstance();
			instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
		}
		
		return instance;
	}
	
	private void Awake(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.battery.BatteryPlugin");
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

	public void setBatteryLifeChangeListener(Action<float> onBatteryChange){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			BatteryCallback batteryCallback = new BatteryCallback();
			batteryCallback.onBatteryLifeChange = onBatteryChange;
			jo.CallStatic("setBatteryLifeChangeListener",batteryCallback);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	/// <summary>
	/// Gets the battery life.
	/// </summary>
	/// <returns>The battery life.</returns>
	public float GetBatteryLife(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			return jo.CallStatic<float>("getBatteryLife");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return 0.0f;
	}
}