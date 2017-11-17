using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class AndroidAdvanceInfoDemo : MonoBehaviour {

	public Text androidIdText;
	public Text secureAndroidIdText;
	public Text telephonyDeviceIdText;
	public Text telephonySimSerialNumberText;
	public Text advertisingIdText;
	public Text uniqueIdText;
	public Text simStatusText;

	private DeviceInfoPlugin deviceInfoPlugin;

	// Use this for initialization
	void Start () {
		deviceInfoPlugin = DeviceInfoPlugin.GetInstance();
		deviceInfoPlugin.SetDebug(0);
		deviceInfoPlugin.Init();
	}
	
	public void GetAndroidId(){
		// we pass 1 to get md5 value this values usually use for admob test id
		string androidId = deviceInfoPlugin.GetAndroidId(1);
		if(androidIdText!=null){
			androidIdText.text =  String.Format("Android ID: {0}",androidId);
			Debug.Log(" Android ID: " + androidId);
		}
	}

	public void GetSecureAndroidId(){
		string secureAndroidId = deviceInfoPlugin.GetSecureAndroidId();
		if(secureAndroidIdText!=null){
			secureAndroidIdText.text =  String.Format("Secure Android ID: {0}",secureAndroidId);
			Debug.Log("Secure Android Id: " + secureAndroidId);
		}
	}

	public void GetTelephonyDeviceId(){
		if(deviceInfoPlugin.CheckSim()){
			string telephonyDeviceId = deviceInfoPlugin.GetTelephonyDeviceId();
			if(telephonyDeviceIdText!=null){
				telephonyDeviceIdText.text =  String.Format("telephony Device ID: {0}",telephonyDeviceId);
				Debug.Log(" telephony Device ID: " + telephonyDeviceId);
			}
		}else{
			telephonyDeviceIdText.text =  String.Format("telephony Device ID: {0}","no sim");
			Debug.Log("no sim");
		}
	}

	public void GetTelephonySimSerialNumber(){
		if(deviceInfoPlugin.CheckSim()){
			string telephonySimSerialNumber = deviceInfoPlugin.GetTelephonySimSerialNumber();
			if(telephonySimSerialNumberText!=null){
				telephonySimSerialNumberText.text =  String.Format("Telephony SimSerial Number: {0}",telephonySimSerialNumber);
				Debug.Log("Telephony SimSerial Number: " + telephonySimSerialNumber);
			}
		}else{
			telephonySimSerialNumberText.text =  String.Format("Telephony SimSerial Number: {0}","no sim");
			Debug.Log("no sim");
		}
	}

	public void GetAdvertisingId(){
		// pass the method to call when sucessfull or failed
		deviceInfoPlugin.GetAdvertisingId(onGetAdvertisingIdComplete,onGetAdvertisingIdFail);
	}

	public void GenerateUniqueId(){
		if(deviceInfoPlugin.CheckSim()){
			string uniqueId = deviceInfoPlugin.GenerateUniqueId();
			if(uniqueIdText!=null){
				uniqueIdText.text =  String.Format("Unique ID: {0}",uniqueId);
				Debug.Log("Unique ID: " + uniqueId);
			}
		}else{
			uniqueIdText.text =  String.Format("Unique ID: {0}","no sim");
			Debug.Log("no sim");
		}
	}

	public void CheckSim(){
		bool hasSim = deviceInfoPlugin.CheckSim();
		if(simStatusText!=null){
			simStatusText.text =  String.Format("Has Sim: {0}",hasSim);
			Debug.Log("Has Sim: " + hasSim);
		}
	}

	private void onGetAdvertisingIdComplete(string advertisingId){
		if(advertisingIdText!=null){
			advertisingIdText.text =  String.Format("Advertising ID: {0}",advertisingId);
			Debug.Log("onGetAdvertisingIdComplete Advertising ID: " + advertisingId);
		}
	}

	private void onGetAdvertisingIdFail(string errorMessage){
		Debug.Log("onGetAdvertisingIdFail errorMessage: " + errorMessage);
	}
}