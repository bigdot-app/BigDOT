using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class AlarmDemo : MonoBehaviour {

	private const string TAG = "[AlarmDemo]";

	private AlarmPlugin alarmPlugin;

	public InputField hourInput;
	public InputField minuteInput;
	public InputField delayIntervalInput;

	private TimePlugin timePlugin;
	private int hour = 0;
	private int minute = 0;
	private int sec;

	//1 = pm && 0 = am
	private int amOrPM;

	//VALUE OF one minute in milli seconds
	private const int ONE_MINUTE = 60000;

	//this must be unique for every alarm that you will create, 
	//that's why we added request code to numbers of request code you have 
	//created
	//you will need this to remove or cancel them
	private int REQUEST_CODE = 301;

	private int delayInterval = 0;

	//private string stateOfDay;

	//store request code of all Pending Alarm Notifications
	//tip save this on playerpref so that you can still access it when your player quit and the open your application
	private List<int> alarmRequestCodeCollection = new List<int>();

	private int demoMinuteAlarm = 2;
	private int demoSecAlarm = 0;

	private void Awake(){
		AlarmPlugin.OnAlarmLoadComplete+=OnAlarmLoadComplete;
		AlarmPlugin.OnAlarmLoadFail+=OnAlarmLoadFail;

		timePlugin = TimePlugin.GetInstance();
		timePlugin.SetDebug(0);
		
		alarmPlugin = AlarmPlugin.GetInstance();
		alarmPlugin.SetDebug(0);
		alarmPlugin.EnableSound(true);
		alarmPlugin.EnableVibrate(true);
		alarmPlugin.Init();
	}

	// Use this for initialization
	void Start (){
		alarmPlugin.LoadAlarms();
	}


	private void OnDestroy(){
		AlarmPlugin.OnAlarmLoadComplete-=OnAlarmLoadComplete;
		AlarmPlugin.OnAlarmLoadFail-=OnAlarmLoadFail;
	}

	//get the time you input on text field if not available default to current time plus 5 minutes
	private void getAlarmTime(){
		if(hourInput!=null){
			if(hourInput.text.Equals("",StringComparison.Ordinal)){
				//default get current hour
				hour = timePlugin.GetIntHour();
				if(hour == 0){
					hour = 12;
				}
				hourInput.text =hour.ToString();
			}else{
				//hour =  int.Parse(hourInput.text);
				if(!int.TryParse(hourInput.text,out hour)){
					//default get current hour
					hour = timePlugin.GetIntHour();

					if(hour == 0){
						hour = 12;
					}

					hourInput.text =hour.ToString();
				}else{

					if(hour >= 12){
						hour = 12;
					}

					hourInput.text =hour.ToString();

					//revert back to valid value
					//because 0 is 12
					if(hour >= 12){
						hour = 0;
					}
				}
			}
		}
		
		if(minuteInput!=null){
			if(minuteInput.text.Equals("",StringComparison.Ordinal)){
				minute = timePlugin.GetIntMinute() + demoMinuteAlarm;
				if(minute > 60){
					int minCorrection = (demoMinuteAlarm - (minute - 60));
					minute = (minute -  60) + minCorrection;
				}
				minuteInput.text =minute.ToString();
			}else{
				//minute =  int.Parse(minuteInput.text);
				if(!int.TryParse(minuteInput.text,out minute)){
					//default 5 minutes
					minute = timePlugin.GetIntMinute() + demoMinuteAlarm;

					if(minute > 60){
						int minCorrection = (demoMinuteAlarm - (minute - 60));
						minute = (minute -  60) + minCorrection;
					}

					minuteInput.text =minute.ToString();
				}else{
					minuteInput.text =minute.ToString();
				}
			}
		}

		//sec = timePlugin.GetIntSec();
		sec = demoSecAlarm;
		
		//1 = pm && 0 = am
		amOrPM = timePlugin.GetIntAmOrPm();
		
		/*if(amOrPM == 1){
			stateOfDay = "PM";
		}else{
			stateOfDay = "AM";
		}*/

		//for sure
		//revert back to valid value
		//because 0 is 12
		if(hour >= 12){
			hour = 0;
		}
	}


	//gets the default interval
	private void getDelayInterval(){
		delayInterval = 0;

		if(delayIntervalInput!=null){
			if(delayIntervalInput.text.Equals("",StringComparison.Ordinal)){
				//default 1 minute interval between alarms when using interval
				delayInterval = 1 * ONE_MINUTE;
				delayIntervalInput.text = delayInterval.ToString();
			}else{
				//delayInterval =  int.Parse(delayIntervalInput.text);
				if(!int.TryParse(delayIntervalInput.text,out delayInterval)){
					//default 1 minute interval between alarms when using interval
					delayInterval = 1 * ONE_MINUTE;
					delayIntervalInput.text = delayInterval.ToString();
				}else{
					//delayInterval =delayInterval * ONE_MINUTE;
					delayIntervalInput.text = delayInterval.ToString();
				}
			}
		}
	}

	public void SetAlarm(){
		Debug.Log("ScheduleLocalNotification");
		//schedule Specific notification
		
		//request code is the unique id of local notification
		int requestCode = REQUEST_CODE +  alarmRequestCodeCollection.Count;

		//get the time input on text if blank get time on device add a 5 minutes before alarm activated
		getAlarmTime();

		alarmPlugin.SetAlarm(requestCode,hour,minute,sec,amOrPM,"alarm notification title","my alarm notification message","my alarm notification ticker message");
		
		//save request code for future usage ex. canceling notification or removing it
		alarmRequestCodeCollection.Add(requestCode);
		
		Debug.Log("added scheduled Specific notification with requestCode " + requestCode );
	}

	public void SetExactAlarm(){
		Debug.Log("ScheduleLocalNotification");
		//schedule Specific notification
		
		//request code is the unique id of local notification
		int requestCode = REQUEST_CODE + alarmRequestCodeCollection.Count;
		
		//get the time input on text if blank get time on device add a 5 minutes before alarm activated
		getAlarmTime();
		
		alarmPlugin.SetExactAlarm(requestCode,hour,minute,sec,amOrPM,"alarm notification title" + requestCode,"my alarm notification message","my alarm notification ticker message");
		
		//save request code for future usage ex. canceling notification or removing it
		alarmRequestCodeCollection.Add(requestCode);
		
		Debug.Log("added scheduled Specific notification with requestCode " + requestCode );
	}

	public void SetRepeatingAlarm(){
		Debug.Log("ScheduleLocalNotification");
		//schedule Specific notification
		
		//request code is the unique id of local notification
		int requestCode =REQUEST_CODE +  alarmRequestCodeCollection.Count;

		//check delay text input if blank set to default delay interval
		getDelayInterval();
		
		//get the time input on text if blank get time on device add a 5 minutes before alarm activated
		getAlarmTime();
		
		alarmPlugin.SetRepeatingAlarm(requestCode,hour,minute,sec,amOrPM,delayInterval,"alarm notification title" + requestCode,"my alarm notification message","my alarm notification ticker message");
		
		//save request code for future usage ex. canceling notification or removing it
		alarmRequestCodeCollection.Add(requestCode);
		
		Debug.Log("added scheduled Specific notification with requestCode " + requestCode );
	}

	public void SetInExactRepeatingAlarm(){
		Debug.Log("ScheduleLocalNotification");
		//schedule Specific notification
		
		//request code is the unique id of local notification
		int requestCode =REQUEST_CODE +  alarmRequestCodeCollection.Count;

		//check delay text input if blank set to default delay interval
		getDelayInterval();
		
		//get the time input on text if blank get time on device add a 5 minutes before alarm activated
		getAlarmTime();
		
		alarmPlugin.SetInExactRepeatingAlarm(requestCode,hour,minute,sec,amOrPM,delayInterval,"alarm notification title" + requestCode,"my alarm notification message","my alarm notification ticker message");
		
		//save request code for future usage ex. canceling notification or removing it
		alarmRequestCodeCollection.Add(requestCode);
		
		Debug.Log("added scheduled Specific notification with requestCode " + requestCode );
	}

	public void SetInExactPerDayRepeatingAlarm(){
		Debug.Log("ScheduleLocalNotification");
		//schedule Specific notification
		
		//request code is the unique id of local notification
		int requestCode =REQUEST_CODE + alarmRequestCodeCollection.Count;
		
		//get the time input on text if blank get time on device add a 5 minutes before alarm activated
		getAlarmTime();
		
		alarmPlugin.SetInExactPerDayRepeatingAlarm(requestCode,hour,minute,sec,amOrPM,"alarm notification title" + requestCode,"my alarm notification message","my alarm notification ticker message");
		
		//save request code for future usage ex. canceling notification or removing it
		alarmRequestCodeCollection.Add(requestCode);
		
		Debug.Log("added scheduled Specific notification with requestCode " + requestCode );
	}


	public void CancelAllScheduledAlarm(){
		//this only works when you just place alarm and you are still not closing the app
		//to make it work anytime you need to save the request code on player pref or
		//any tool that can save data and pass it on Cancel Alarm

		int len = alarmRequestCodeCollection.Count;
		
		for(int index=0;index<len;index++){
			int currentRequestCode = alarmRequestCodeCollection[index];
			alarmPlugin.CancelAlarm(currentRequestCode);
		}

		alarmRequestCodeCollection.Clear();
	}

	public void CancelPrevScheduledAlarm(){
		//this only works when you just place alarm and you are still not closing the app
		//to make it work anytime you need to save the request code on player pref or
		//any tool that can save data and pass it on Cancel Alarm

		if(alarmRequestCodeCollection.Count > 0){
			int prevRequestCode = alarmRequestCodeCollection[alarmRequestCodeCollection.Count - 1];
			//alarmPlugin.CancelAlarm(prevRequestCode);
			CancelAlarm(prevRequestCode);
		}
	}

	//this is how we cancel or remove specific scheduled alarm
	//request code is the unique id of alarm use this to cancel or remove specific scheduled alarm
	private void CancelAlarm(int requestCode){
		int len = alarmRequestCodeCollection.Count;
		Debug.Log("trying to cancel scheduled alarm requestCode " + requestCode );
		
		for(int index=0;index<len;index++){
			int currentRequestCode = alarmRequestCodeCollection[index];
			if(currentRequestCode == requestCode){
				Debug.Log("found Scheduled alarm with requestCode " + requestCode + " cancelling... ");
				alarmPlugin.CancelAlarm(requestCode);
				alarmRequestCodeCollection.RemoveAt(index);
				break;
			}
		}
	}

	//this is to stop the alarm when its activated
	public void StopAlarm(){
		alarmPlugin.StopAlarm();
	}

	/// <summary>
	/// Removes all save alarm. alarm request code is save now so you can also remove alarms now
	/// this work always compare to remove all alarm which remove all alarm that is just created on this session
	/// </summary>
	public void RemoveAllSaveAlarm(){
		alarmPlugin.RemoveAllSaveAlarm();
		alarmRequestCodeCollection.Clear();
	}

	private void OnAlarmLoadComplete(string alarms){
		if(!alarms.Equals("",StringComparison.Ordinal)){
			//remove brackets
			alarms =  alarms.Replace( "[","" ).Replace("]","");
			Debug.Log(TAG + "remove bracket OnLocalNotificationLoadComplete notifcations: " +  alarms);
			
			//get split the request codes
			string[] loadedRequestCode = alarms.Split(',');
			
			//convert them to int and place them on notification request collections
			foreach(string reqCode in loadedRequestCode){
				alarmRequestCodeCollection.Add(  int.Parse(reqCode));
			}
		}else{
			Debug.Log(TAG + "empty no save request code...");
		}
	}

	private void OnAlarmLoadFail(){
		Debug.Log(TAG + " OnAlarmLoadFail");
	}
}
