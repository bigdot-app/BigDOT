using UnityEngine;
using System.Collections;
using System;

public class BatteryCallback :  AndroidJavaProxy {
	
	public Action <float>onBatteryLifeChange;
	
	public BatteryCallback() : base("com.gigadrillgames.androidplugin.battery.IBattery") {}
	
	void BatteryLifeChange(float val){
		onBatteryLifeChange(val);
	}
}