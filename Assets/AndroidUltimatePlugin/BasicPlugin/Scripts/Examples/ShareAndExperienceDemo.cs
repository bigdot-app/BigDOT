using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ShareAndExperienceDemo : MonoBehaviour {	

	private bool isImmersive = false;
	private SharePlugin sharePlugin;
	private UtilsPlugin utilsPlugin;

	// Use this for initialization
	void Start (){
		utilsPlugin = UtilsPlugin.GetInstance();
		utilsPlugin.SetDebug(0);

		sharePlugin = SharePlugin.GetInstance();
		sharePlugin.SetDebug(0);
	}

	public void ImmersiveToggle(){
		if(!isImmersive){
			utilsPlugin.ImmersiveOn(500);
			isImmersive = true;
		}else{
			utilsPlugin.ImmersiveOff();
			isImmersive = false;
		}
	}

	public void ShareText(){		
		//share link
		sharePlugin.ShareUrl("my subject","my subject content","https://www.urltoshare.com");
	}

	public void ShareImage(){

		//if you want to save on Application.persistentDataPath, file on this path is remove when app is uninstal
		//string screenShotName = "AndroidUltimateScreenShot.png";
		//string path = Application.persistentDataPath + "/" + screenShotName;

		string screenShotName = "AUPScreenShot.jpg";
		string folderPath = utilsPlugin.CreateFolder("MyScreenShots",0);
		string path ="";

		if(!folderPath.Equals("",StringComparison.Ordinal)){
			path = folderPath + "/" + screenShotName;

			//note: we added new required variable to pass which is screenShotName to determined what image format to use
			//jpg or png, if format is not given set default to jpg format
			StartCoroutine(AUP.Utils.TakeScreenshot(path,screenShotName));
			sharePlugin.ShareImage("subject","subjectContent",path);
		}
	}
}