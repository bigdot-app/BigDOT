using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public class LocalNotificationDemo : MonoBehaviour{

	private const string TAG = "[LocalNotificationDemo]";

	public Text scheduleText;
	public Text statusText;
	private LocalNotificationPlugin localNotificationPlugin;
	private TimePlugin timePlugin;

	//store request code of all local notification
	//tip save this on playerpref so that you can still access it when your player quit and the open your application
	private List<int> notificationRequestCodeCollection = new List<int>();

	private int demoMinute1 = 1;
	private int demoMinute2 = 2;

	//VALUE OF one minute in milli seconds
	private const int ONE_MINUTE = 60000;

	//this must be unique for every alarm that you will create, 
	//that's why we added request code to numbers of request code you have 
	//created
	//you will need this to remove or cancel them
	private int REQUEST_CODE = 601;

	private void Awake(){
		UpdateStatus("Status: Not Ready");

		//listen for event
		LocalNotificationPlugin.OnLocalNotificationLoadComplete+=OnLocalNotificationLoadComplete;
		LocalNotificationPlugin.OnLocalNotificationLoadFail+=OnLocalNotificationLoadFail;


		localNotificationPlugin = LocalNotificationPlugin.GetInstance();
		localNotificationPlugin.SetDebug(0);
		localNotificationPlugin.Init();
		
		timePlugin = TimePlugin.GetInstance();
		timePlugin.SetDebug(0);
		
		int isOpenUsingNotification = localNotificationPlugin.IsOpenUsingNotification();
		Debug.Log("[ShareAndExperienceDemo]: isOpenUsingNotification " + isOpenUsingNotification);
		
		if(isOpenUsingNotification == 1){
			//do something here
		}else{
			//do something here
		}
	}
	
	// Use this for initialization
	void Start (){
		localNotificationPlugin.LoadNotification();
	}

	private void OnDestroy(){
		//remove listener
		LocalNotificationPlugin.OnLocalNotificationLoadComplete-=OnLocalNotificationLoadComplete;
		LocalNotificationPlugin.OnLocalNotificationLoadFail-=OnLocalNotificationLoadFail;
	}

	private void UpdateStatus(string val){
		if( statusText!=null){
			statusText.text = val;
		}
	}
	
	public void ScheduleLocalNotification(){
		Debug.Log("ScheduleLocalNotification");
		//schedule notification
		//2 minutes
		int delay = 2 * ONE_MINUTE;
		//request code is the unique id of local notification
		int requestCode =REQUEST_CODE +  notificationRequestCodeCollection.Count;

		//i'm planning to remove this call in the future because this is useless anymore because you can do Schedule a specific notificataion now
		localNotificationPlugin.ScheduleNotification("my notification title","my notification message","my notification ticker message",delay,true,true,requestCode);
		notificationRequestCodeCollection.Add(requestCode);
		
		Debug.Log("added scheduled notification with requestCode " + requestCode );
	}

	//new 1.5.9
	public void ScheduleSpecificLocalNotification(){
		Debug.Log("ScheduleLocalNotification");
		//schedule Specific notification

		//request code is the unique id of local notification
		int requestCode = REQUEST_CODE +  notificationRequestCodeCollection.Count;

		int hour = timePlugin.GetIntHour();
		int minute = timePlugin.GetIntMinute() + demoMinute1;


		int sec = timePlugin.GetIntSec();

		//1 = pm && 0 = am
		int amOrPM = timePlugin.GetIntAmOrPm();
		string stateOfDay;

		if(amOrPM == 1){
			stateOfDay = "PM";
		}else{
			stateOfDay = "AM";
		}

		if(scheduleText!=null){
			scheduleText.text = String.Format("H:{0} M:{1} S:{2} - {3}",hour,minute,sec,stateOfDay);
		}

		localNotificationPlugin.ScheduleSpecificNotification(requestCode + "ntifcation title",requestCode,hour,minute,sec,amOrPM, "my notification message","my notification ticker message",true,true);

		//save request code for future usage ex. canceling notification or removing it
		notificationRequestCodeCollection.Add(requestCode);
		
		Debug.Log("added scheduled Specific notification with requestCode " + requestCode );
	}

	public void ScheduleMultipleSpecificLocalNotification(){
		Debug.Log("ScheduleLocalNotification");
		//schedule Specific notification
		int hour = timePlugin.GetIntHour();
		int minute = timePlugin.GetIntMinute() + demoMinute2;

		int sec = timePlugin.GetIntSec();
		
		//1 = pm && 0 = am
		int amOrPM = timePlugin.GetIntAmOrPm();
		string stateOfDay;
		
		if(amOrPM == 1){
			stateOfDay = "PM";
		}else{
			stateOfDay = "AM";
		}

		int len = 5;
		StringBuilder sb = new StringBuilder();

		for(int i =0; i < len; i++ ){
			//request code is the unique id of local notification
			int requestCode = REQUEST_CODE + notificationRequestCodeCollection.Count;

			//add 5 minutes between notifications
			minute += demoMinute1;

			//if minute reach 60 reset it back to 5 and then increment hour
			if(minute >= 60){
				minute = 5;
				hour++;
			}

			Debug.Log( "hour " + hour + " minute " + minute );
			localNotificationPlugin.ScheduleSpecificNotification(requestCode + "mltiple notifcation title",requestCode,hour,minute,sec,amOrPM, "my notification message_" + i,"my notification ticker message_" + i,true,true);
			
			//save request code for future usage ex. canceling notification or removing it
			notificationRequestCodeCollection.Add(requestCode);
			Debug.Log("added scheduled Specific notification with requestCode " + requestCode );

			sb.Append(String.Format("H:{0} M:{1} S:{2} - {3} \n",hour,minute,sec,stateOfDay));
		}

		if(scheduleText!=null){
			scheduleText.text = sb.ToString();
		}
	}

	public void ScheduleMultipleNotification(){
		Debug.Log("ScheduleMultipleNotification");

		int notificationCount = 3;

		for(int index=0;index<notificationCount;index++){
			//schedule notification
			//2 + index = minutes delay
			int delay = ( 2 + index) * ONE_MINUTE ;

			//request code is the unique id of local notification
			int requestCode = REQUEST_CODE + notificationRequestCodeCollection.Count;
			localNotificationPlugin.ScheduleNotification(requestCode + "notification title","my notification message","notification ticker message" + index,delay,true,true,requestCode);
			notificationRequestCodeCollection.Add(requestCode);
			
			Debug.Log("added scheduled notification with requestCode " + requestCode );
		}

	}

	public void ScheduleNotificationAfterDay(){
		Debug.Log("ScheduleNotificationAfterDay");

		//request code is the unique id of local notification
		int requestCode = REQUEST_CODE +  notificationRequestCodeCollection.Count;

		// will notify after 1 day
		int day = 1;

		int hour = timePlugin.GetIntHour();
		int minute = timePlugin.GetIntMinute() + demoMinute1;


		int sec = timePlugin.GetIntSec();

		//1 = pm && 0 = am
		int amOrPM = timePlugin.GetIntAmOrPm();
		string stateOfDay;

		if(amOrPM == 1){
			stateOfDay = "PM";
		}else{
			stateOfDay = "AM";
		}

		if(scheduleText!=null){
			scheduleText.text = String.Format("H:{0} M:{1} S:{2} - {3}",hour,minute,sec,stateOfDay);
		}

		localNotificationPlugin.ScheduleAfterDay(requestCode + "notifcation title",requestCode,day,hour,minute,sec,
			amOrPM, "my notification message","my notification ticker message",true,true);

		//save request code for future usage ex. canceling notification or removing it
		notificationRequestCodeCollection.Add(requestCode);

		Debug.Log("added scheduled after day notification with requestCode " + requestCode );


	}

	public void ScheduleShortTime(){
		Debug.Log("ScheduleShortTime");

		//request code is the unique id of local notification
		int requestCode = REQUEST_CODE +  notificationRequestCodeCollection.Count;


		// will notify after day value if not zero
		int day = 0;
		// will notify after 3 hours
		int hour = 3;
		// will notify after minute value if not zero
		int minute = 0;
		// will notify after sec value if not zero
		int sec = 0;

		if(scheduleText!=null){
			scheduleText.text = String.Format("H:{0} M:{1} S:{2}",hour,minute,sec);
		}

		localNotificationPlugin.ScheduleShortTime(requestCode + "notifcation title",requestCode,day,hour,minute,sec,
			"my notification message","my notification ticker message",true,true);

		//save request code for future usage ex. canceling notification or removing it
		notificationRequestCodeCollection.Add(requestCode);

		Debug.Log("added scheduled short time notification with requestCode " + requestCode );
	}

	public void ScheduleLongTime(){
		Debug.Log("ScheduleLongTime");

		//request code is the unique id of local notification
		int requestCode = REQUEST_CODE +  notificationRequestCodeCollection.Count;

		// will notify after year value if not zero
		int year = 0;
		// will notify after month value if not zero
		int month = 2;
		// will notify after week value if not zero
		int week = 0;


		// will notify after day value if not zero
		int day = 0;

		// will notify after 3 hours
		int hour = 3;
		// will notify after minute value if not zero
		int minute = 0;
		// will notify after sec value if not zero
		int sec = 0;

		if(scheduleText!=null){
			scheduleText.text = String.Format("H:{0} M:{1} S:{2}",hour,minute,sec);
		}

		localNotificationPlugin.ScheduleLongTime(requestCode + "notifcation title",requestCode,year,month,week,day,hour,minute,sec,
			"my notification message","my notification ticker message",true,true);

		//save request code for future usage ex. canceling notification or removing it
		notificationRequestCodeCollection.Add(requestCode);

		Debug.Log("added scheduled long time notification with requestCode " + requestCode );
	}
	
	public void CancelScheduledNotification(){
		//assuming that in this scenario we want to cancel the previous local notifcation that we scheduled 
		//before it get fired,please try to avoid canceling or removing an already fired local notification because its useless
		
		if(notificationRequestCodeCollection.Count > 0){
			int prevRequestCode = notificationRequestCodeCollection[notificationRequestCodeCollection.Count - 1];
			CancelSpecificNotification(prevRequestCode);
		}
	}
	

	//this is how we cancel or remove specific local notification 
	//request code is the unique id of local notification use this to cancel or remove specific scheduled pending notification 
	private void CancelSpecificNotification(int requestCode){
		int len = notificationRequestCodeCollection.Count;
		Debug.Log("trying to cancel scheduled notification requestCode " + requestCode );
		
		for(int index=0;index<len;index++){
			int currentRequestCode = notificationRequestCodeCollection[index];
			if(currentRequestCode == requestCode){
				Debug.Log("found Scheduled notification with requestCode " + requestCode + " cancelling... ");
				localNotificationPlugin.CancelScheduledNotification(requestCode);
				notificationRequestCodeCollection.RemoveAt(index);
				break;
			}
		}
	}
	
	public void CancelAllNotification(){
		localNotificationPlugin.ClearAllScheduledNotification();
		notificationRequestCodeCollection.Clear();
	}

	private void OnLocalNotificationLoadComplete(string notifications){
		if(!notifications.Equals("",StringComparison.Ordinal)){
			//remove brackets
			notifications =  notifications.Replace( "[","" ).Replace("]","");
			Debug.Log(TAG + "remove bracket OnLocalNotificationLoadComplete notifcations: " +  notifications);
			
			//get split the request codes
			string[] loadedRequestCode = notifications.Split(',');
			
			//convert them to int and place them on notification request collections
			foreach(string reqCode in loadedRequestCode){
				notificationRequestCodeCollection.Add(  int.Parse(reqCode));
			}
		}else{
			Debug.Log(TAG + "empty no save request code...");
		}
	}

	private void OnLocalNotificationLoadFail(){
		Debug.Log(TAG + " OnLocalNotificationLoadFail");
	}
}