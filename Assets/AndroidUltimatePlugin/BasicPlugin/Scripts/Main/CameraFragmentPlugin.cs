using UnityEngine;
using System.Collections;
using System;

public class CameraFragmentPlugin : MonoBehaviour {
	
	private static CameraFragmentPlugin instance;
	private static GameObject container;
	private const string TAG="[CameraFragmentPlugin]: ";
	private static AUPHolder aupHolder;
	
	#if UNITY_ANDROID
	private static AndroidJavaObject jo;
	#endif	
	
	public bool isDebug =true;
	private bool isInit = false;
	
	public static CameraFragmentPlugin GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name="CameraFragmentPlugin";
			instance = container.AddComponent( typeof(CameraFragmentPlugin) ) as CameraFragmentPlugin;
			DontDestroyOnLoad(instance.gameObject);
			aupHolder = AUPHolder.GetInstance();
			instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
		}
		
		return instance;
	}
	
	private void Awake(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.camera.CameraFragmentPlugin");
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
	
	public void OpenCamera(){		
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("openCamera");
			AUP.Utils.Message(TAG,"openCamera");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
}
