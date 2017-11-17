using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class InternetSpeedChecker : MonoBehaviour {

	public Text infoStatusText;
	public Text actionText;

	public Text mobileConnectionText;
	public Text mobileConnectionSpeedText;

	public Text wifiConnectionText;
	public Text wifiSignalStrengthText;

	private InternetPlugin internetPlugin;
	private bool isChecking = false;

	private bool isMobileConnected = false;
	private bool isMobileFast = false;

	private bool isWifiConnected = false;
	private bool isWifiFast = false;


	// Use this for initialization
	void Start () {
		internetPlugin = InternetPlugin.GetInstance();
		internetPlugin.SetDebug(0);
		internetPlugin.Init();
		internetPlugin.setInternetCallbackListener(OnWifiConnect,OnWifiDisconnect,OnWifiSignalStrengthChange);
		UpdateStatus("Waiting...");
	}
	
	public void checkInternetSpeed(){
		if(!isChecking){
			isChecking = true;
			UpdateStatus("Checking...");

			if(internetPlugin.IsMobileConnected()){
				// check mobile speed here	
				if(mobileConnectionText!=null){
					mobileConnectionText.text = "mobile is connected";
					isMobileConnected = true;
				}

				if(internetPlugin.IsMobileConnectionFast()){
					isMobileFast = true;
					UpdateMobileSpeed("mobile is fast");
					// do something here
				}else{			
					isMobileFast = false;
					UpdateMobileSpeed("mobile is slow");
					// do something here
				}
			}else{
				isMobileConnected = false;
				if(mobileConnectionText!=null){
					mobileConnectionText.text = "mobile is not connected";
				}
			}

			if(internetPlugin.IsWifiConnected()){
				isWifiConnected = true;
				if(wifiConnectionText!=null){
					wifiConnectionText.text = "wifi is connected";
				}
				internetPlugin.ScanWifi();
			}else{
				isWifiConnected = false;
				if(wifiConnectionText!=null){
					wifiConnectionText.text = "wifi is not connected";
				}

				isChecking = false;
				UpdateStatus("Done Checking.");
				FinalCheck();
			}
		}
	}

	private void FinalCheck(){
		if(isWifiConnected && isMobileConnected){
			if(isWifiFast && isMobileFast){
				// do something here
				UpdateAction("Load Scene!");
			}else{
				UpdateAction("don't Load Scene!");
			}
		}else if(isWifiConnected && !isMobileConnected){
			if(isWifiFast){
				UpdateAction("Load Scene!");
			}else{
				UpdateAction("don't Load Scene!");
			}
		}else if(!isWifiConnected && isMobileConnected){
			if(isMobileFast){
				UpdateAction("Load Scene!");
			}else{
				UpdateAction("don't Load Scene!");
			}
		}else{
			UpdateAction("don't Load Scene!");
		}
	}

	private void UpdateMobileSpeed(string val){
		if(mobileConnectionSpeedText!=null){
			mobileConnectionSpeedText.text = val;
		}
	}

	private void UpdateWifiSpeed(string val){
		if(wifiSignalStrengthText!=null){
			wifiSignalStrengthText.text = val;
		}
	}

	private void UpdateStatus(string val){
		if(infoStatusText!=null){			
			infoStatusText.text = val;
		}
	}

	private void UpdateAction(string val){
		if(actionText!=null){
			actionText.text = val;
		}
	}

	void OnWifiConnect(){
		Debug.Log("[InternetInfoDemo] OnWifiConnect");
	}

	void OnWifiDisconnect(){
		Debug.Log("[InternetInfoDemo] OnWifiDisconnect");
	}

	void OnWifiSignalStrengthChange(int signalStrength, int signalDifference){
		Debug.Log("[InternetInfoDemo] OnWifiSignalStrengthChange signalStrength " + signalStrength + " signalDifference " + signalDifference);

		if(wifiSignalStrengthText!=null){
			wifiSignalStrengthText.text = String.Format("wifi Signal Strength: {0}", signalStrength);
		}

		// this is a good signal
		if(signalStrength > 2 ){
			// do something here
			UpdateWifiSpeed("wifi signal is fast!");
		}else{
			UpdateWifiSpeed("wifi signal is slow!");
			// do something here
		}

		isChecking = false;
		UpdateStatus("Done Checking.");
		FinalCheck();
	}
}
