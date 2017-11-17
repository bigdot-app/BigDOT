using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TimeDemo : MonoBehaviour {

	public Text timeText;
	public Text packageText;
	public Text versionText;

	private TimePlugin timePlugin;
	private UtilsPlugin utilsPlugin;
	
	// Use this for initialization
	void Start (){
		timePlugin = TimePlugin.GetInstance();
		timePlugin.SetDebug(0);

		utilsPlugin = UtilsPlugin.GetInstance();
		utilsPlugin.SetDebug(0);
	}
	
	public void GetTime(){
		timeText.text =  String.Format("Time: {0}",timePlugin.GetTime());
	}

	public void GetPackage(){
		string appPackage = utilsPlugin.GetPackageId();
		packageText.text =  String.Format("Package: {0}",appPackage);
		Debug.Log(" packageId " + appPackage);
	}

	public void GetVersion(){
		string androidOSVersion = utilsPlugin.GetAndroidVersion();
		versionText.text =  String.Format("Version: {0}",androidOSVersion);
		Debug.Log(" androidOSVersion " + androidOSVersion);
	}
}
