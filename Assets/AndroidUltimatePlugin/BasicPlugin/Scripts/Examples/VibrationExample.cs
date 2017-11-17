using UnityEngine;
using System.Collections;

public class VibrationExample : MonoBehaviour {

	private VibratorPlugin vibratorPlugin;
	private bool isVibrate = false;

	// Use this for initialization
	void Start (){		
		vibratorPlugin = VibratorPlugin.GetInstance();
		vibratorPlugin.SetDebug(0);
		vibratorPlugin.Init();
	}

	private void OnDisable(){
		StopVibrate();
	}
	
	//not just vibration this is special vibration because you can create vibration pattern
	public void PatternVibrate(){
		StopVibrate();

		if(!isVibrate){
			long[] pattern = {0,500,500,600,400,200,400,200};
			vibratorPlugin.Vibrate(pattern);
			
			//or if you just want a normal vibration use this
			//androidUltimatePluginController.Vibrate(500);
			
			isVibrate = true;
		}
	}

	//not just vibration this is special vibration because you can create vibration pattern
	public void NormalVibrate(){
		StopVibrate();

		if(!isVibrate){
			vibratorPlugin.Vibrate(500);			
			isVibrate = true;
		}
	}

	public void StopVibrate(){
		if(isVibrate){
			vibratorPlugin.StopVibrate();
			isVibrate = false;
		}
	}		

	private void OnApplicationQuit(){
		StopVibrate();
	}
}
