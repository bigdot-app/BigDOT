using UnityEngine;
using System.Collections;
using System;

public class BluetoothConnectionCallback :  AndroidJavaProxy {
	
	public Action <string>onConnected;
	public Action <string>onConnecting;
	public Action <string>onNotConnected;
	public Action <string>onConnectionFailed;
	public Action <string>onConnectionLost;

	public Action <string>onConnectToDevice;
	
	public BluetoothConnectionCallback() : base("com.gigadrillgames.androidplugin.bluetooth.IConnectionCallback") {}	


	void Connected(String val){
		onConnected(val);
	} 

	void Connecting(String val){
		onConnecting(val);
	}

	void NotConnected(String val){
		onNotConnected(val);
	}

	void ConnectionFailed(String val){
		onConnectionFailed(val);
	}

	void ConnectionLost(String val){
		onConnectionLost(val);
	}

	void ConnectToDevice(String val){
		onConnectToDevice(val);
	}
}

