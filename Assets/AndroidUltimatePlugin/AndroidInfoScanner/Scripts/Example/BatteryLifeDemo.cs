using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using AUP;

public class BatteryLifeDemo : MonoBehaviour {
	
	public Text batterLifeText;
	private BatteryPlugin batteryPlugin;
	private Dispatcher dispatcher;

	private void Awake(){
		dispatcher = Dispatcher.GetInstance();

		batteryPlugin = BatteryPlugin.GetInstance();
		batteryPlugin.SetDebug(0);
		batteryPlugin.setBatteryLifeChangeListener(OnBatteryLifeChange);
	}

	void Start (){
	}

	private void OnBatteryLifeChange(float percent){
		dispatcher.InvokeAction(
			()=>{
				if(percent==1){
					percent = 100;
				}
				batterLifeText.text = String.Format("BatteryLife: {0}%",percent);		
			}
		);
	}

	//for getting battery life
	public void GetBatteryLife(){
		float batteryLife = batteryPlugin.GetBatteryLife();

		if(batteryLife==1){
			batteryLife = 100;
		}

		if(batterLifeText!=null){
			batterLifeText.text = String.Format("BatteryLife: {0}%",batteryLife);
		}
	}
}

