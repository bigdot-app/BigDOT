using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class BlueToothDemo : MonoBehaviour {
	private BlueToothPlugin blueToothPlugin;

	public Text blueToothMessageRecievedText;
	public Text blueToothStatusText;
	public Text blueEnableText;
	public InputField inputMessageText;
	public Button notifyButton;

	private string deviceMacAddress="";
	private string deviceName="";
	
	// Use this for initialization
	void Start (){		
		blueToothPlugin = BlueToothPlugin.GetInstance();
		blueToothPlugin.SetDebug(0);
		blueToothPlugin.Init();

		blueToothPlugin.SetConnectionCallbackListener(
			OnConnected
			,OnConnecting
			,OnNotConnected
			,OnConnectionFailed
			,OnConnectionLost
			,OnConnectToDevice
			);

		blueToothPlugin.SetDataCallbackListener(OnRecievedMessage,OnSentMessage,OnLogMessage);

		bool isEnable = blueToothPlugin.CheckBlueTooth();
		Debug.Log("[BlueToothDemo] isBlueToothEnable: " + isEnable);
		
		if(!isEnable){
			blueToothPlugin.EnsureDiscoverable();
		}else{
			blueToothPlugin.InitServer();
		}
	}

	public void EnsureDiscoverable(){
		blueToothPlugin.EnsureDiscoverable();
	}

	public void CheckBlueTooth(){
		bool isEnable = blueToothPlugin.CheckBlueTooth();
		if(blueEnableText!=null){
			blueEnableText.text = string.Format("IsBlueToothEnable: {0}",isEnable);
		}

		if(isEnable){
			notifyButton.interactable = false;
		}else{
			notifyButton.interactable = true;
		}
	}

	public void InitializeServer(){
		//starts the server
		blueToothPlugin.InitServer();
	}

	public void NotifyUserToEnableBlueTooth(){
		blueToothPlugin.NotifyUserToEnableBlueTooth();
	}

	public void GetPairedDevices(){
		string pairedDevices = blueToothPlugin.GetPairedDevices();

		if(!pairedDevices.Equals("",StringComparison.Ordinal)){
			string[] pairedDeviceSet = pairedDevices.Split(',');
			int len = pairedDeviceSet.Length;
			
			if(len > 0){
				for(int index=0; index<len; index++ ){
					string device =  pairedDeviceSet.GetValue(index).ToString();
					string[] deviceInfo = device.Split('_');
					Debug.Log( "[BlueToothDemo]: Device Name: " + deviceInfo.GetValue(0) + " macAddress: " + deviceInfo.GetValue(1) );
				}
				
				//gets the 1st paired device
				string firstDevice =  pairedDeviceSet.GetValue(0).ToString();
				string[] firstDeviceInfo = firstDevice.Split('_');
				
				deviceName = firstDeviceInfo.GetValue(0).ToString();
				deviceMacAddress = firstDeviceInfo.GetValue(1).ToString();

				Debug.Log( "[BlueToothDemo]: First Paired Device Name: " + deviceName + " macAddress: " + deviceMacAddress);

				if(blueToothStatusText!=null){
					blueToothStatusText.text ="got macAddress ready to connect";
				}
			}
		}
	}

	public void Connect(){
		string macAddress = deviceMacAddress;
		bool isSecure = false;

		if(!macAddress.Equals("",StringComparison.Ordinal)){
			blueToothPlugin.Connect(macAddress,isSecure);
		}else{
			if(blueToothStatusText!=null){
				blueToothStatusText.text ="macAddress is empty";
			}
			Debug.Log("macAddress is empty can't connect ");
		}
	}

	public void DisConnect(){
		blueToothPlugin.DisConnect();
	}

	public void SendMessage(){
		string message= inputMessageText.text;

		if(inputMessageText!=null){
			message= inputMessageText.text;
		}else{
			message= "Test Message";
		}

		Debug.Log("[BlueToothDemo] SendMessage: " + message);
		blueToothPlugin.SendData(message);
	}

	public void StopServer(){
		blueToothPlugin.StopServer();
	}

	private void OnDestroy(){
		StopServer();
	}

	public void OnConnected(string data){
		UpdateBlueToothTextStatus(data.ToString());
	}
	
	public void OnConnecting(string data){
		UpdateBlueToothTextStatus(data.ToString());
	}
	
	public void OnNotConnected(string data){
		UpdateBlueToothTextStatus(data.ToString());
	}
	
	public void OnConnectionFailed(string data){
		UpdateBlueToothTextStatus(data.ToString());
	}
	
	public void OnConnectionLost(string data){
		UpdateBlueToothTextStatus(data.ToString());
	}

	public void OnLogMessage(string data){
		Debug.Log("[BlueToothEventListener] OnLogMessage " + data);
	}
	
	public void OnSentMessage(string data){
		Debug.Log("[BlueToothEventListener] OnSentMessage " + data);
	}
	
	public void OnRecievedMessage(string data){
		if(blueToothMessageRecievedText!=null){
			blueToothMessageRecievedText.text = string.Format("Recieved Message: {0}",data.ToString());
		}
	}
	
	public void OnConnectToDevice(string data){
		UpdateBlueToothTextStatus( "OnConnectToDevice device name: " +  data.ToString());
	}
	
	private void UpdateBlueToothTextStatus(string val){
		if(blueToothStatusText!=null){
			blueToothStatusText.text = string.Format("Status: {0}",val);
		}
	}
}
