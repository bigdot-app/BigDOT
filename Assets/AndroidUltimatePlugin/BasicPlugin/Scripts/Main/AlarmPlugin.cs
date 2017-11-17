using UnityEngine;
using System.Collections;
using System;
using AUP;

public class AlarmPlugin : MonoBehaviour {

	//events 
	public static event Action <string>OnAlarmLoadComplete;
	public static event Action OnAlarmLoadFail;

	private static AlarmPlugin instance;
	private static GameObject container;
	private const string TAG="[AlarmPlugin]: ";

	private static AUPHolder aupHolder;
	
	#if UNITY_ANDROID
	private static AndroidJavaObject jo;
	#endif	
	
	public bool isDebug =true;
	
	public static AlarmPlugin GetInstance(){
		if(instance==null){
			aupHolder = AUPHolder.GetInstance();

			container = new GameObject();
			container.name="AlarmPlugin";
			instance = container.AddComponent( typeof(AlarmPlugin) ) as AlarmPlugin;
			DontDestroyOnLoad(instance.gameObject);
			instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
		}
		
		return instance;
	}
	
	private void Awake(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.alarm.AlarmPlugin");
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
	/// Enables or Disables the alarm sound.
	/// </summary>
	/// <param name="val">If set to <c>true</c> value.</param>
	public void EnableSound(bool val){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("enableSound",val);
			AUP.Utils.Message(TAG,"EnableSound");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Enables or Disables the alarm vibration.
	/// </summary>
	/// <param name="val">If set to <c>true</c> value.</param>
	public void EnableVibrate(bool val){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("enableVibrate",val);
			AUP.Utils.Message(TAG,"EnableVibrate");
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

		SetAlarmCallbackListener(onAlarmLoadComplete,onAlarmLoadFail);
		#endif
	}

	private void SetAlarmCallbackListener(Action <string> onAlarmLoadComplete,Action onAlarmLoadFail){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			AlarmCallback  alarmCallback = new AlarmCallback();
			alarmCallback.onAlarmLoadComplete = onAlarmLoadComplete;
			alarmCallback.onAlarmLoadFail = onAlarmLoadFail;
			jo.CallStatic("setAlarmCallbackListener",alarmCallback);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	public void LoadAlarms(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("loadAlarms");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Sets the alarm.
	/// </summary>
	/// <param name="requestCode">Request code.</param>
	/// <param name="hour">Hour. note: 1 to 12 just indicate the amOrPm</param>
	/// <param name="minute">Minute.</param>
	/// <param name="sec">Sec.</param>
	/// <param name="amOrPm">Am or pm. 0 for AM and 1 for PM </param>
	/// <param name="alarmTile">Alarm tile.</param>
	/// <param name="alarmMessage">Alarm message.</param>
	/// <param name="alarmTickerMessage">Alarm ticker message.</param>

	public void SetAlarm(int requestCode,int hour, int minute,int sec, int amOrPm,string alarmTile,string alarmMessage,string alarmTickerMessage){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("setAlarm",requestCode,hour,minute,sec,amOrPm,alarmTile,alarmMessage,alarmTickerMessage);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}


	/// <summary>
	/// Sets the Exact alarm.
	/// </summary>
	/// <param name="requestCode">Request code.</param>
	/// <param name="hour">Hour. note: 1 to 12 just indicate the amOrPm</param>
	/// <param name="minute">Minute.</param>
	/// <param name="sec">Sec.</param>
	/// <param name="amOrPm">Am or pm. 0 for AM and 1 for PM </param>
	/// <param name="alarmTile">Alarm tile.</param>
	/// <param name="alarmMessage">Alarm message.</param>
	/// <param name="alarmTickerMessage">Alarm ticker message.</param>
	public void SetExactAlarm(int requestCode,int hour, int minute,int sec, int amOrPm,string alarmTile,string alarmMessage,string alarmTickerMessage){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("setExactAlarm",requestCode,hour,minute,sec,amOrPm,alarmTile,alarmMessage,alarmTickerMessage);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Sets the alarm after day.
	/// </summary>
	/// <param name="requestCode">Request code.</param>
	/// <param name="day">Day. ex. 1 plus 1 day, 2 plus 2 days, 3 plus 3 days on the current calendar on your device</param>
	/// <param name="hour">Hour range(0 to 23).</param>
	/// <param name="minute">Minute range(0 to 59).</param>
	/// <param name="sec">Sec range(0 to 59).</param>
	/// <param name="amOrPm">Am or pm range(0 to 1).</param>
	/// <param name="alarmTile">Alarm tile.</param>
	/// <param name="alarmMessage">Alarm message.</param>
	/// <param name="alarmTickerMessage">Alarm ticker message.</param>
	public void SetAlarmAfterDay(int requestCode,int day,int hour, int minute,int sec, int amOrPm,string alarmTile,string alarmMessage,string alarmTickerMessage){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("setAlarmAfterDay",requestCode,day,hour,minute,sec,amOrPm,alarmTile,alarmMessage,alarmTickerMessage);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}


	/// <summary>
	/// Sets the Repeating alarm.
	/// </summary>
	/// <param name="requestCode">Request code.</param>
	/// <param name="hour">Hour. note: 1 to 12 just indicate the amOrPm</param>
	/// <param name="minute">Minute.</param>
	/// <param name="sec">Sec.</param>
	/// <param name="amOrPm">Am or pm. 0 for AM and 1 for PM </param>
	/// <param name="delay">Delay.</param>
	/// <param name="alarmTile">Alarm tile.</param>
	/// <param name="alarmMessage">Alarm message.</param>
	/// <param name="alarmTickerMessage">Alarm ticker message.</param>
	public void SetRepeatingAlarm(int requestCode,int hour, int minute,int sec,int amOrPm,long delay,string alarmTile,string alarmMessage,string alarmTickerMessage){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("setRepeatingAlarm",requestCode,hour,minute,sec,amOrPm,delay,alarmTile,alarmMessage,alarmTickerMessage);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}


	/// <summary>
	/// Sets the Exact Repeating alarm.
	/// </summary>
	/// <param name="requestCode">Request code.</param>
	/// <param name="hour">Hour. note: 1 to 12 just indicate the amOrPm</param>
	/// <param name="minute">Minute.</param>
	/// <param name="sec">Sec.</param>
	/// <param name="amOrPm">Am or pm. 0 for AM and 1 for PM </param>
	/// <param name="delay">Delay.</param>
	/// <param name="alarmTile">Alarm tile.</param>
	/// <param name="alarmMessage">Alarm message.</param>
	/// <param name="alarmTickerMessage">Alarm ticker message.</param>
	public void SetInExactRepeatingAlarm(int requestCode,int hour, int minute,int sec,int amOrPm,long delay,string alarmTile,string alarmMessage,string alarmTickerMessage){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("setInExactRepeatingAlarm",requestCode,hour,minute,sec,amOrPm,delay,alarmTile,alarmMessage,alarmTickerMessage);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}


	/// <summary>
	/// Sets the Exact Elapse Real Time Wake up Repeating alarm.
	/// </summary>
	/// <param name="requestCode">Request code.</param>
	/// <param name="hour">Hour. note: 1 to 12 just indicate the amOrPm</param>
	/// <param name="minute">Minute.</param>
	/// <param name="sec">Sec.</param>
	/// <param name="amOrPm">Am or pm. 0 for AM and 1 for PM </param>
	/// <param name="delay">Delay.</param>
	/// <param name="alarmTile">Alarm tile.</param>
	/// <param name="alarmMessage">Alarm message.</param>
	/// <param name="alarmTickerMessage">Alarm ticker message.</param>
	public void SetInExactRepeatingAlarmElapseRealTimeWakeUp(int requestCode,int hour, int minute,int sec,int amOrPm,long delay,string alarmTile,string alarmMessage,string alarmTickerMessage){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("setInExactRepeatingAlarmElapseRealTimeWakeUp",requestCode,hour,minute,sec,amOrPm,delay,alarmTile,alarmMessage,alarmTickerMessage);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}


	/// <summary>
	/// Sets the Repeating alarm delay is Per Day Auto. best if you want to set alarm every day on the exact time that
	/// you set
	/// </summary>
	/// <param name="requestCode">Request code.</param>
	/// <param name="hour">Hour. note: 1 to 12 just indicate the amOrPm</param>
	/// <param name="minute">Minute.</param>
	/// <param name="sec">Sec.</param>
	/// <param name="amOrPm">Am or pm. 0 for AM and 1 for PM </param>
	/// <param name="alarmTile">Alarm tile.</param>
	/// <param name="alarmMessage">Alarm message.</param>
	/// <param name="alarmTickerMessage">Alarm ticker message.</param>
	public void SetInExactPerDayRepeatingAlarm(int requestCode,int hour, int minute,int sec,int amOrPm,string alarmTile,string alarmMessage,string alarmTickerMessage){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("setInExactPerDayRepeatingAlarm",requestCode,hour,minute,sec,amOrPm,alarmTile,alarmMessage,alarmTickerMessage);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Cancels the alarm, remove alarm that is just created on this session
	/// </summary>
	/// <returns><c>true</c> if this instance cancel alarm; otherwise, <c>false</c>.</returns>
	public void CancelAlarm(int requestCode){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("cancelAlarm",requestCode);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Removes all save alarm. use this if you want to remove all saves alarms
	/// this will always works if this does not work there's a bug on Android Side 
	/// or you really don't have save alarms :)
	/// </summary>
	public void RemoveAllSaveAlarm(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("removeAllSaveAlarm");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	/// <summary>
	/// Stops current active alarm
	/// if alarm is repeating this will just silent the alarm
	/// but if the alarm reach interval alarm will be active again
	/// if you don't want this to happen call stop alarm and then call Cancel alarm pass the
	/// request code that you use to create this active alarm
	/// Note: always save your request code on player pref or other means 
	/// without this request code you can't cancel any alarm that you created
	/// and this will create a horror experience on user
	/// </summary>
	public void StopAlarm(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("stopAlarm");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	private void onAlarmLoadComplete(string alarms){
		AlarmLoadComplete(alarms);
	}

	private void onAlarmLoadFail(){
		AlarmLoadFail();
	}

	internal void AlarmLoadComplete(string alarms){
		if(null!=OnAlarmLoadComplete){
			OnAlarmLoadComplete(alarms);
		}
	}

	internal void AlarmLoadFail(){
		if(null!=OnAlarmLoadFail){
			OnAlarmLoadFail();
		}
	}
}