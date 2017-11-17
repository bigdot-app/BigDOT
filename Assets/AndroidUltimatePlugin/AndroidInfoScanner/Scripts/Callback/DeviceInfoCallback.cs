using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DeviceInfoCallback :  AndroidJavaProxy{
	
	public Action <String>onGetAdvertisingIdComplete;
	public Action <String>onGetAdvertisingIdFail;
	
	public DeviceInfoCallback() : base("com.gigadrillgames.androidplugin.deviceinfo.IDeviceInfoCallback") {}
	
	void GetAdvertisingIdComplete(String advertisingId){
		onGetAdvertisingIdComplete(advertisingId);
	}

	void GetAdvertisingIdFail(String errorMessage){
		onGetAdvertisingIdFail(errorMessage);
	}
}