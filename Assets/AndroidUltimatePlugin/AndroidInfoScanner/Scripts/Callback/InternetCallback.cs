using UnityEngine;
using System.Collections;
using System;

public class InternetCallback :  AndroidJavaProxy {
	
	public Action OnWifiConnect;
	public Action OnWifiDisconnect;
	public Action <int,int>OnWifiSignalStrengthChange;
	
	public InternetCallback() : base("com.gigadrillgames.androidplugin.internetchecker.IInternetCallback") {}

	void onWifiConnect(){
		OnWifiConnect();
	}

	void onWifiDisconnect(){
		OnWifiDisconnect();
	}

	void onWifiSignalStrengthChange(int signalStrength, int signalDifference){
		OnWifiSignalStrengthChange(signalStrength,signalDifference);
	}
}