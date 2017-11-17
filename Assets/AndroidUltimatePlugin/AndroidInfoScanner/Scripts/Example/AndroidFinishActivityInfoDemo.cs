using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class AndroidFinishActivityInfoDemo : MonoBehaviour {

	private UtilsPlugin utilsPlugin;
	private int currentStatus;
	public Text activityFinishStatusText;


	// Use this for initialization
	void Start () {
		utilsPlugin = UtilsPlugin.GetInstance();
		utilsPlugin.SetDebug(0);
	}

	public void GetFinishActivityStatus(){
		if(utilsPlugin!=null){
			currentStatus = utilsPlugin.GetAlwaysFinishActivity();
			UpdateFinishActivityStatusText(currentStatus);
		}
	}
	
	public void ToggleSetFinishActivity(){
		if(utilsPlugin!=null){
			currentStatus = utilsPlugin.GetAlwaysFinishActivity();
			UpdateFinishActivityStatusText(currentStatus);
		}

		if(currentStatus == 0){
			SetFinishActivity(1);
		}else{
			SetFinishActivity(0);
		}
	}

	private void SetFinishActivity(int val){
		if(utilsPlugin!=null){
			utilsPlugin.SetAlwaysFinishActivity(val);
			currentStatus = utilsPlugin.GetAlwaysFinishActivity();
			UpdateFinishActivityStatusText(currentStatus);
		}
	}

	private void UpdateFinishActivityStatusText(int val){
		if(activityFinishStatusText!=null){
			activityFinishStatusText.text = String.Format("Status: {0}",val);
		}
	}
}
