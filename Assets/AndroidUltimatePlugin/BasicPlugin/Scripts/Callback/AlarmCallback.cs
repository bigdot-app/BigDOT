using UnityEngine;
using System.Collections;
using System;

public class AlarmCallback :  AndroidJavaProxy {

	public AlarmCallback() : base("com.gigadrillgames.androidplugin.alarm.IAlarmCallback") {}

	public Action <string>onAlarmLoadComplete;
	public Action onAlarmLoadFail;

	void AlarmLoadComplete(string notifications){
		onAlarmLoadComplete(notifications);
	}
	
	void AlarmLoadFail(){
		onAlarmLoadFail();
	}
}
