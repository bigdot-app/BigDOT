using UnityEngine;
using System.Collections;
using System;

public class BluetoothDataCallback :  AndroidJavaProxy {
	
	public Action <string>onDataSent;
	public Action <string>onRecieved;
	public Action <string>onLogMessage;
	
	public BluetoothDataCallback() : base("com.gigadrillgames.androidplugin.bluetooth.IDataCallback") {}	
	
	
	void RecievedData(String val){
		onRecieved(val);
	} 
	
	void SentData(String val){
		onDataSent(val);
	}
	
	void LogMessage(String val){
		onLogMessage(val);
	}
}

