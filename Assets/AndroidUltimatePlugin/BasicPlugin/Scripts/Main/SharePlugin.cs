using UnityEngine;
using System.Collections;
using System;

public class SharePlugin : MonoBehaviour {
	
	private static SharePlugin instance;
	private static GameObject container;
	private static AUPHolder aupHolder;
	private const string TAG="[SharePlugin]: ";
	
	#if UNITY_ANDROID
	private static AndroidJavaObject jo;
	#endif	
	
	public bool isDebug =true;
	
	public static SharePlugin GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name="SharePlugin";
			instance = container.AddComponent( typeof(SharePlugin) ) as SharePlugin;
			DontDestroyOnLoad(instance.gameObject);
			aupHolder = AUPHolder.GetInstance();
			instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
		}
		
		return instance;
	}
	
	private void Awake(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.shareintent.ShareIntentPlugin");
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

	//share intent
	/// <summary>
	/// Shares the image.
	/// </summary>
	/// <param name="subject">Subject.</param>
	/// <param name="subjectContent">Subject content.</param>
	/// <param name="imagepath">Imagepath.</param>
	public void ShareImage(string subject,string subjectContent, string imagepath){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("shareImage",subject,subjectContent,imagepath);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	/// <summary>
	/// Shares the URL.
	/// </summary>
	/// <param name="subject">Subject.</param>
	/// <param name="subjectContent">Subject content.</param>
	/// <param name="url">URL.</param>
	public void ShareUrl(string subject,string subjectContent, string url){		
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("shareUrl",subject,subjectContent,url);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
}