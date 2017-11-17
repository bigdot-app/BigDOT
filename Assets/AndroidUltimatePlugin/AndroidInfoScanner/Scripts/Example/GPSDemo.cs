using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using AUP;

public class GPSDemo : MonoBehaviour {

	private GPSPlugin gpsPlugin;

	public Text gpsStatusText;
	public Text latitudeText;
	public Text longitudeText;
	public Text speedText;
	public Text altitudeText;
	public Text bearingText;

	// new 
	public Text accuracyText;
	public Text distanceInMetersText;

	//nmea
	public Text timeStampText;
	public Text nmeaText;

	private Dispatcher dispatcher;


	// Use this for initialization
	void Start (){		
		dispatcher = Dispatcher.GetInstance();

		gpsPlugin = GPSPlugin.GetInstance();
		gpsPlugin.SetDebug(0);

		//long updateInterval = 1000 * 60 * 1; // 1 minute
		//long updateInterval = 1000 * 5; // 5 seconds
		long updateInterval = 200; // 200 millsecs 

		//long minimumMeterChangeForUpdate = 10 //10 meters;
		long minimumMeterChangeForUpdate = 0; //0 meters reason it's a hack because LocationManager in androind is buggy and unreliable


		gpsPlugin.Init(updateInterval,minimumMeterChangeForUpdate);
		AddGPSEventListeners();

		// start the gps
		gpsPlugin.StartGPS();
	}

	private void OnEnable(){
		AddGPSEventListeners();
	}

	private void OnDisable(){
		RemoveGPSEventListeners();
	}

	private void AddGPSEventListeners(){
		if(gpsPlugin!=null){
			gpsPlugin.onLocationChange += OnLocationChange;
			gpsPlugin.onEnableGPS += OnEnableGPS;
			gpsPlugin.onGetLocationComplete += OnGetLocationComplete;
			gpsPlugin.onGetLocationFail += OnGetLocationFail;
			gpsPlugin.onLocationChangeInformation += onLocationChangeInformation;
			gpsPlugin.onGetLocationCompleteInformation += OnGetLocationCompleteInformation;
			gpsPlugin.onNmeaReceived += OnNmeaReceived;	
		}
	}

	private void RemoveGPSEventListeners(){
		if(gpsPlugin!=null){
			gpsPlugin.onLocationChange -= OnLocationChange;
			gpsPlugin.onEnableGPS -= OnEnableGPS;
			gpsPlugin.onGetLocationComplete -= OnGetLocationComplete;
			gpsPlugin.onGetLocationFail -= OnGetLocationFail;
			gpsPlugin.onLocationChangeInformation -= onLocationChangeInformation;
			gpsPlugin.onGetLocationCompleteInformation -= OnGetLocationCompleteInformation;
			gpsPlugin.onNmeaReceived -= OnNmeaReceived;
		}
	}

	private void OnApplicationPause(bool val){
		if(gpsPlugin!=null){
			if(gpsPlugin.hasInit()){
				if(val){
					RemoveGPSEventListeners();
				}else{
					AddGPSEventListeners();
				}
			}
		}
	}

	private void OnDestroy(){
		gpsPlugin.StopGPS();
		RemoveGPSEventListeners();
	}

	//note: make sure before calling this that GPS is Enable
	//use this if you only want to get latitude
	public void GetLatitude(){
		double latitude = gpsPlugin.GetLatitude();
		UpdateLatitudeText(latitude.ToString());
		Debug.Log( "Latitude  " +  latitude);
	}


	//note: make sure before calling this that GPS is Enable
	//use this if you only want to get longitude
	public void GetLongitude(){
		double longitude = gpsPlugin.GetLongitude();
		UpdateLongitudeText(longitude.ToString());
		Debug.Log( "Longitude  " +  longitude);
	}

	//check if GPS is enable or disable
	public void CheckGPS(){
		Debug.Log( "[GPSDemo] CheckGPS");

		bool isEnable = gpsPlugin.CheckGPS();
		Debug.Log("[GPSDemo]GPS enable: " + isEnable);

		if(gpsStatusText!=null){
			if(isEnable){
				gpsStatusText.text = "Status: Enable";
			}else{
				gpsStatusText.text = "Status: Disable";
			}
		}

		if(!isEnable){
			//GPS is disable
			//show alert dialog to ask player or user to Enable GPS

			string title = "Alert: GPS Settings is disabled";
			string message = "Your GPS seems to be disabled, do you want to enable it?";
			string buttonLabelYes = "YES";
			string buttonLabelNo = "NO";

			gpsPlugin.ShowGPSAlert(title,message,buttonLabelYes,buttonLabelNo);
		}
	}

	//note: make sure before calling this that GPS is Enable
	public void GetLocation(){
		string locationSet = gpsPlugin.GetLocation();
		string[] locations = locationSet.Split(',');

		string latitude =locations.GetValue(0).ToString();
		string longitude =locations.GetValue(1).ToString();
		string speed =locations.GetValue(2).ToString();
		string altitude =locations.GetValue(3).ToString();
		string bearing =locations.GetValue(4).ToString();

		UpdateLatitudeText(latitude);
		UpdateLongitudeText(longitude);
		UpdateSpeed(speed);
		UpdateAltitude(altitude);
		UpdateBearing(bearing);


		Debug.Log( "latitude: " + latitude);
		Debug.Log( "longitude: " + longitude);
		Debug.Log( "speed: " + speed);
		Debug.Log( "altitude: " + altitude);
		Debug.Log( "bearing: " + bearing);
	}

	public void StartGPS(){
		// start the gps
		gpsPlugin.StartGPS();
	}

	public void StopGPS(){
		gpsPlugin.StopGPS();
	}

	private void UpdateLatitudeText(string latitude){
		if(latitudeText!=null){
			latitudeText.text = String.Format("Latitude:{0}",latitude);
		}
	}

	private void UpdateLongitudeText(string longitude){
		if(longitudeText!=null){
			longitudeText.text = String.Format("Longitude:{0}",longitude);
		}
	}

	private void UpdateSpeed(string val){
		if(speedText!=null){
			speedText.text = String.Format("Speed:{0}",val);
		}
	}

	private void UpdateAltitude(string val){
		if(altitudeText!=null){
			altitudeText.text = String.Format("Altitude:{0}",val);
		}
	}

	private void UpdateBearing(string val){
		if(bearingText!=null){
			bearingText.text = String.Format("Bearing:{0}",val);
		}
	}

	private void UpdateAccuracy(string val){
		if(accuracyText!=null){
			accuracyText.text = String.Format("Accuracy:{0}",val);
		}
	}

	private void UpdateDistanceInMeters(string val){
		if(distanceInMetersText!=null){
			distanceInMetersText.text = String.Format("DistanceInMeters:{0}",val);
		}
	}

	private void UpdateTimeStamp(string val){
		if(timeStampText!=null){
			timeStampText.text = String.Format("TimeStamp:{0}",val);
		}
	}

	private void UpdateNmea(string val){
		if(nmeaText!=null){
			nmeaText.text = String.Format("Nmea:{0}",val);
		}
	}



	private void OnLocationChange(double latitude, double longitude){
		dispatcher.InvokeAction(
			()=>{				
			UpdateLatitudeText(latitude.ToString());
			UpdateLongitudeText(longitude.ToString());
			Debug.Log("[GPSDemo] OnLocationChange latitude: " + latitude + " longitude: " + longitude);
			}
		);
	}

	private void onLocationChangeInformation(string information){
		dispatcher.InvokeAction(
			()=>{
				Debug.Log("[GPSDemo] onLocationChangeInformation " + information);

				string[] informations = information.Split(',');

				//note: here's the orders of information
				//latidude,longitude,speed,altitude,bearing

				UpdateSpeed(informations.GetValue(2).ToString());
				UpdateAltitude(informations.GetValue(3).ToString());
				UpdateBearing(informations.GetValue(4).ToString());

				foreach( string info in informations  ){
					Debug.Log( "info " + info );
				}
			}
		);
	}

	private void OnEnableGPS(string status){
		dispatcher.InvokeAction(
			()=>{
				if(gpsStatusText!=null){
					gpsStatusText.text = String.Format("Status: {0}",status);
				}
			}
		);
	}

	private void OnGetLocationComplete(double latitude, double longitude){
		dispatcher.InvokeAction(
			()=>{
				Debug.Log("[GPSDemo] OnGetLocationComplete latitude: " + latitude + " longitude: " + longitude);
				UpdateLatitudeText(latitude.ToString());
				UpdateLongitudeText(longitude.ToString());
			}
		);
	}

	private void OnGetLocationCompleteInformation(string information){
		dispatcher.InvokeAction(
			()=>{
				Debug.Log("[GPSDemo] OnGetLocationCompleteInformation " + information);
				string[] informations = information.Split(',');
				
				//note: here's the orders of information
				//latidude,longitude,speed,altitude,bearing

				UpdateSpeed(informations.GetValue(2).ToString());
				UpdateAltitude(informations.GetValue(3).ToString());
				UpdateBearing(informations.GetValue(4).ToString());
				UpdateAccuracy(informations.GetValue(5).ToString());
				UpdateDistanceInMeters(informations.GetValue(6).ToString());
				
				foreach( string info in informations  ){
					Debug.Log( "info " + info );
				}
			}
		);
	}

	private void OnGetLocationFail(){
		dispatcher.InvokeAction(
			()=>{
				Debug.Log("[GPSDemo] OnGetLocationFail");
			}
		);
	}

	public void OnNmeaReceived(long timeStamp,string nmea){
		dispatcher.InvokeAction(
			()=>{
				UpdateTimeStamp(timeStamp.ToString());
				UpdateNmea(nmea.ToString());
			}
		);
	}
}
