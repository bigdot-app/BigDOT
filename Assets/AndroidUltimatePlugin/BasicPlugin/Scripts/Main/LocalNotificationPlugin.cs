using UnityEngine;
using System.Collections;
using System;

public class LocalNotificationPlugin : MonoBehaviour{

	public static event Action <string>OnLocalNotificationLoadComplete;
	public static event Action OnLocalNotificationLoadFail;

	private static LocalNotificationPlugin instance;
	private static GameObject container;
	private static AUPHolder aupHolder;
	private const string TAG="[LocalNotificationPlugin]: ";
	
	#if UNITY_ANDROID
	private static AndroidJavaObject jo;
	#endif	
	
	public bool isDebug =true;
	
	public static LocalNotificationPlugin GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name="LocalNotificationPlugin";
			instance = container.AddComponent( typeof(LocalNotificationPlugin) ) as LocalNotificationPlugin;
			DontDestroyOnLoad(instance.gameObject);
			aupHolder = AUPHolder.GetInstance();
			instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
		}
		
		return instance;
	}
	
	private void Awake(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.notification.NotificationPlugin");
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

		//add listener
		SetNotificationCallbackListener(onNotificationLoadComplete,onNotificationLoadFail);

		#endif
	}

	public void LoadNotification(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("loadNotification");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}		
		#endif
	}

	private void OnDestroy(){
		SetNotificationCallbackListener(null,null);
	}

	private void SetNotificationCallbackListener(Action <string> onNotificationLoadComplete,Action onNotificationLoadFail){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			NotificationCallback  notificationCallback = new NotificationCallback();
			notificationCallback.onNotificationLoadComplete = onNotificationLoadComplete;
			notificationCallback.onNotificationLoadFail = onNotificationLoadFail;
			jo.CallStatic("setNotificationCallbackListener",notificationCallback);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	//note: local notification will not show if the application is currently running
	
	/// <summary>
	/// Schedules the notification.
	/// </summary>
	/// <param name="notificationTitle">Notification title.</param>
	/// <param name="notificationMessage">Notification message.</param>
	/// <param name="notificationTickerMessage">Notification ticker message.</param>
	/// <param name="delay">Delay.</param>
	/// <param name="enableVibrate">If set to <c>true</c> enable vibrate.</param>
	/// <param name="enableSound">If set to <c>true</c> enable sound.</param>
	public void ScheduleNotification(string notificationTitle,string notificationMessage,string notificationTickerMessage, int delay,bool enableVibrate,bool enableSound, int requestCode ){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("scheduleNotification",notificationTitle,notificationMessage,notificationTickerMessage,delay,enableVibrate,enableSound,requestCode);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Schedules the specific notification.
	/// </summary>
	/// <param name="notificationTitle">Notification title.</param>
	/// <param name="requestCode">Request code.</param>
	/// <param name="hour">Hour.</param>
	/// <param name="minute">Minute.</param>
	/// <param name="sec">Sec.</param>
	/// <param name="amOrPm">Am or pm.</param>
	/// <param name="notificationMessage">Notification message.</param>
	/// <param name="notificationTickerMessage">Notification ticker message.</param>
	/// <param name="enableVibrate">If set to <c>true</c> enable vibrate.</param>
	/// <param name="enableSound">If set to <c>true</c> enable sound.</param>
	public void ScheduleSpecificNotification(string notificationTitle,int requestCode,int hour, int minute, int sec,int amOrPm, string notificationMessage,string notificationTickerMessage,bool enableVibrate,bool enableSound ){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("scheduleSpecificNotification",notificationTitle,requestCode,hour,minute,sec,amOrPm,notificationMessage,notificationTickerMessage,enableVibrate,enableSound);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Schedules LocalNotification the every day in exact time.
	/// </summary>
	/// <param name="notificationTitle">Notification title.</param>
	/// <param name="requestCode">Request code.</param>
	/// <param name="hour">Hour.</param>
	/// <param name="minute">Minute.</param>
	/// <param name="sec">Sec.</param>
	/// <param name="amOrPm">Am or pm.</param>
	/// <param name="notificationMessage">Notification message.</param>
	/// <param name="notificationTickerMessage">Notification ticker message.</param>
	/// <param name="enableVibrate">If set to <c>true</c> enable vibrate.</param>
	/// <param name="enableSound">If set to <c>true</c> enable sound.</param>
	public void ScheduleEveryDay(string notificationTitle,int requestCode,
		int hour, int minute, int sec,int amOrPm, string notificationMessage,
		string notificationTickerMessage,bool enableVibrate,bool enableSound 
	){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("scheduleEveryDay",notificationTitle,requestCode,hour,minute,sec,amOrPm,notificationMessage,notificationTickerMessage,enableVibrate,enableSound);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Schedules Local Notification after the x days and time that you supply
	/// </summary>
	/// <param name="notificationTitle">Notification title.</param>
	/// <param name="requestCode">Request code.</param>
	/// <param name="day">Day.</param>
	/// <param name="hour">Hour.</param>
	/// <param name="minute">Minute.</param>
	/// <param name="sec">Sec.</param>
	/// <param name="amOrPm">Am or pm.</param>
	/// <param name="notificationMessage">Notification message.</param>
	/// <param name="notificationTickerMessage">Notification ticker message.</param>
	/// <param name="enableVibrate">If set to <c>true</c> enable vibrate.</param>
	/// <param name="enableSound">If set to <c>true</c> enable sound.</param>
	public void ScheduleAfterDay(string notificationTitle,int requestCode,
		int day,int hour, int minute, int sec,int amOrPm, string notificationMessage,
		string notificationTickerMessage,bool enableVibrate,bool enableSound 
	){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("scheduleAfterDay",notificationTitle,requestCode,day,hour,minute,sec,amOrPm,notificationMessage,notificationTickerMessage,enableVibrate,enableSound);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Schedules the short time this one is unique image you will add days or hours or minutes then it will triggered
	/// ex. you to triggered this after 3 hours just gives 3 hour then your good goor for life in game or energy
	/// </summary>
	/// <param name="notificationTitle">Notification title.</param>
	/// <param name="requestCode">Request code.</param>
	/// <param name="day">Day. any value 1 equals 1 day</param>
	/// <param name="hour">Hour. any value 1 equal 1 hour</param>
	/// <param name="minute">Minute. any value 1 equal 1 minute</param>
	/// <param name="sec">Sec. any value 1 equals 1 sec</param>
	/// <param name="notificationMessage">Notification message.</param>
	/// <param name="notificationTickerMessage">Notification ticker message.</param>
	/// <param name="enableVibrate">If set to <c>true</c> enable vibrate.</param>
	/// <param name="enableSound">If set to <c>true</c> enable sound.</param>
	public void ScheduleShortTime(string notificationTitle,int requestCode,
		int day,int hour, int minute, int sec, string notificationMessage,
		string notificationTickerMessage,bool enableVibrate,bool enableSound 
	){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("scheduleShortTime",notificationTitle,requestCode,day,hour,minute,sec,notificationMessage,notificationTickerMessage,enableVibrate,enableSound);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Schedules the long time just like short time but this one you can schedule for years or months or weeks
	/// </summary>
	/// <param name="notificationTitle">Notification title.</param>
	/// <param name="requestCode">Request code.</param>
	/// <param name="year">Year.</param>
	/// <param name="month">Month.</param>
	/// <param name="week">Week.</param>
	/// <param name="day">Day.</param>
	/// <param name="hour">Hour.</param>
	/// <param name="minute">Minute.</param>
	/// <param name="sec">Sec.</param>
	/// <param name="notificationMessage">Notification message.</param>
	/// <param name="notificationTickerMessage">Notification ticker message.</param>
	/// <param name="enableVibrate">If set to <c>true</c> enable vibrate.</param>
	/// <param name="enableSound">If set to <c>true</c> enable sound.</param>
	public void ScheduleLongTime(string notificationTitle,int requestCode,
		int year, int month, int week,
		int day,int hour, int minute, int sec, string notificationMessage,
		string notificationTickerMessage,bool enableVibrate,bool enableSound 
	){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("scheduleLongTime",notificationTitle,requestCode,year,month,week,day,hour,minute,sec,notificationMessage,notificationTickerMessage,enableVibrate,enableSound);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Cancel or remove specific local notification, using request code, request code is unique id of every local notification
	/// assigning same request code to different local notification will result on a single or one notification only, so please avoid
	/// that scenario
	/// tip: save all your request code in an array or list and then save it on playerpref in order to remove or cancel them anytime
	/// you want
	/// </summary>
	/// <param name="requestCode">Request code.</param>
	public void CancelScheduledNotification(int requestCode){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("cancelScheduledNotification",requestCode);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}


	/// <summary>
	/// Clears all scheduled notification before it fires
	/// </summary>
	public void ClearAllScheduledNotification(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("clearAllScheduledNotification");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	/// <summary>
	/// Determines whether this instance is open using notification.
	/// </summary>
	/// <returns><c>true</c> if this instance is open using notification; otherwise, <c>false</c>.</returns>
	public int IsOpenUsingNotification(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			return jo.CallStatic<int>("isOpenUsingNotification");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return 0;
	}

	private void onNotificationLoadComplete(string notifications){
		LocalNotificationLoadComplete(notifications);
	}

	private void onNotificationLoadFail(){
		LocalNotificationLoadFail();
	}

	internal void LocalNotificationLoadComplete(string notifications){
		if(OnLocalNotificationLoadComplete!=null){
			OnLocalNotificationLoadComplete(notifications);
		}
	}

	internal void LocalNotificationLoadFail(){
		if(OnLocalNotificationLoadFail!=null){
			OnLocalNotificationLoadFail();
		}
	}
}