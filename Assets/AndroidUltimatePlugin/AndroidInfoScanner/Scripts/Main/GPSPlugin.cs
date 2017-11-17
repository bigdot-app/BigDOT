using UnityEngine;
using System.Collections;
using System;

public class GPSPlugin : MonoBehaviour {
	
	private static GPSPlugin instance;
	private static GameObject container;
	private const string TAG="[GPSPlugin]: ";
	private static AUPHolder aupHolder;

	private Action <double,double>LocationChange;
	public event Action <double,double>onLocationChange{
		add{ LocationChange+=value;}
		remove{ LocationChange-=value;}
	}

	private Action <string>EnableGPS;
	public event Action <string>onEnableGPS{
		add{ EnableGPS+=value;}
		remove{ EnableGPS-=value;}
	}

	private Action <double,double>GetLocationComplete;
	public event Action <double,double>onGetLocationComplete{
		add{ GetLocationComplete+=value;}
		remove{ GetLocationComplete-=value;}
	}

	private Action GetLocationFail;
	public event Action onGetLocationFail{
		add{ GetLocationFail+=value;}
		remove{ GetLocationFail-=value;}
	}

	private Action <string>LocationChangeInformation;
	public event Action <string>onLocationChangeInformation{
		add{ LocationChangeInformation+=value;}
		remove{ LocationChangeInformation-=value;}
	}

	private Action <string>GetLocationCompleteInformation;
	public event Action <string>onGetLocationCompleteInformation{
		add{ GetLocationCompleteInformation+=value;}
		remove{ GetLocationCompleteInformation-=value;}
	}

	private Action <long,string>NmeaReceived;
	public event Action <long,string>onNmeaReceived{
		add{ NmeaReceived+=value;}
		remove{ NmeaReceived-=value;}
	}

	/*
	/*  Action<double,double> onLocationChange
			,Action<string>onEnableGPS
			,Action<double,double>onGetLocationComplete
			,Action onGetLocationFail
			,Action<string> onLocationChangeInformation
			,Action<string>onGetLocationCompleteInformation
			,Action <long,string>onNmeaReceived 
	  
	 * /
	*/
	
	#if UNITY_ANDROID
	private static AndroidJavaObject jo;
	#endif	
	
	public bool isDebug =true;
	private bool isInit = false;

	public static GPSPlugin GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name="GPSPlugin";
			instance = container.AddComponent( typeof(GPSPlugin) ) as GPSPlugin;
			DontDestroyOnLoad(instance.gameObject);
			aupHolder = AUPHolder.GetInstance();
			instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
		}
		
		return instance;
	}
	
	private void Awake(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.gps.GPSPlugin");
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
	/// Initialize the GPS.
	/// </summary>
	/// <param name="updateInterval">Update interval.</param>
	/// <param name="minimumMeterChangeForUpdate">Minimum meter change for update.</param>
	public void Init(long updateInterval,long minimumMeterChangeForUpdate){
		if(isInit){
			return;
		}

		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			object[] array = new object[2];
			array[0] = updateInterval;
			array[1] = minimumMeterChangeForUpdate;
			
			//jo.CallStatic("initGPS",new object[] { updateInterval, minimumMeterChangeForUpdate });
			isInit = true;
			jo.CallStatic("initGPS",array);

			SetLocationChangeListener(
					onGPSLocationChange,
					onGPSEnableGPS,
					onGPSGetLocationComplete,
					onGPSGetLocationFail,
					onGPSLocationChangeInformation,
					onGPSGetLocationCompleteInformation,
					onGPSNmeaReceived
				);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	public void StartGPS(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			isInit = true;
			jo.CallStatic("startGPS");			
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	private void SetLocationChangeListener(
			Action<double,double> onLocationChange
			,Action<string>onEnableGPS
			,Action<double,double>onGetLocationComplete
			,Action onGetLocationFail
			,Action<string> onLocationChangeInformation
			,Action<string>onGetLocationCompleteInformation
			,Action <long,string>onNmeaReceived 
	){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			GPSCallback gpsCallback = new GPSCallback();
			gpsCallback.onEnableGPS = onEnableGPS;
			gpsCallback.onLocationChange = onLocationChange;
			gpsCallback.onGetLocationComplete = onGetLocationComplete;
			gpsCallback.onGetLocationFail = onGetLocationFail;

			gpsCallback.onLocationChangeInformation = onLocationChangeInformation;
			gpsCallback.onGetLocationCompleteInformation = onGetLocationCompleteInformation;

			gpsCallback.onNmeaReceived = onNmeaReceived;

			jo.CallStatic("setLocationChangeListener",gpsCallback);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}

	public bool hasInit(){
		return isInit;
	}
	
	/// <summary>
	/// Checks the GPS if enable
	/// </summary>
	/// <returns><c>true</c>, if GPS was Enabled, <c>false</c> otherwise.</returns>
	public bool CheckGPS(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			return jo.CallStatic<bool>("checkGPS");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return false;
	}
	
	/// <summary>
	/// Shows the GPS alert to ask user to enable GPS
	/// </summary>
	public void ShowGPSAlert(string title, string message, string buttonLabelYes, string buttonLabelNo){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("showGPSAlert",title,message,buttonLabelYes,buttonLabelNo);
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	/// <summary>
	/// Stops the GPS Events	 
	/// </summary>
	public void StopGPS(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			jo.CallStatic("stopGPS");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
	}
	
	/// <summary>
	/// Gets both Latitude and longitude.
	/// </summary>
	/// <returns>The location.</returns>
	public String GetLocation(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			return jo.CallStatic<String>("getLocation");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif

		return "";
	}
	
	/// <summary>
	/// Gets the latitude.
	/// </summary>
	/// <returns>The latitude.</returns>
	public double GetLatitude(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			return jo.CallStatic<double>("getLatitude");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif

		return 0.0;
	}
	
	/// <summary>
	/// Gets the longitude.
	/// </summary>
	/// <returns>The longitude.</returns>
	public double GetLongitude(){
		#if UNITY_ANDROID
		if(Application.platform == RuntimePlatform.Android){
			return jo.CallStatic<double>("getLongitude");
		}else{
			AUP.Utils.Message(TAG,"warning: must run in actual android device");
		}
		#endif
		
		return 0.0;
	}

	private void onGPSLocationChange(double latitude, double longitude){		
		Debug.Log("[GPSDemo] OnLocationChange latitude: " + latitude + " longitude: " + longitude);
		if(null!=LocationChange){
			LocationChange(latitude,longitude);
		}
	}

	private void onGPSLocationChangeInformation(string information){
		Debug.Log("[GPSDemo] onLocationChangeInformation " + information);
		if(null!=LocationChangeInformation){
			LocationChangeInformation(information);
		}

		//string[] informations = information.Split(',');

		//note: here's the orders of information
		//latidude,longitude,speed,altitude,bearing
	}

	private void onGPSEnableGPS(string status){
		if(null!=EnableGPS){
			EnableGPS(status);
		}
	}

	private void onGPSGetLocationComplete(double latitude, double longitude){
		Debug.Log("[GPSDemo] OnGetLocationComplete latitude: " + latitude + " longitude: " + longitude);
		if(null!=GetLocationComplete){
			GetLocationComplete(latitude,longitude);
		}
	}

	private void onGPSGetLocationCompleteInformation(string information){
		Debug.Log("[GPSDemo] OnGetLocationCompleteInformation " + information);
		if(null!=GetLocationCompleteInformation){
			GetLocationCompleteInformation(information);
		}


		//string[] informations = information.Split(',');
		
		//note: here's the orders of information
		//latidude,longitude,speed,altitude,bearing		
	}

	private void onGPSGetLocationFail(){
		Debug.Log("[GPSDemo] OnGetLocationFail");
		if(null!=GetLocationFail){
			GetLocationFail();
		}
	}

	public void onGPSNmeaReceived(long timeStamp,string nmea){
		if(null!=NmeaReceived){
			NmeaReceived(timeStamp,nmea);
		}
	}
}
