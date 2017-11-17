using UnityEngine;
using System.Collections;

public class AndroidNativeUIDemo : MonoBehaviour {

	private UtilsPlugin utilsPlugin;
	private DemoController demoController;
	private bool isLoading = false;
	
	// Use this for initialization
	void Start () {
		demoController = GameObject.FindObjectOfType<DemoController>();		

		utilsPlugin = UtilsPlugin.GetInstance();
		utilsPlugin.SetDebug(0);
	}
	
	public void ShowRateUsPopup(){
		utilsPlugin.ShowRatePopup("your rate us title","your rate us message","http://www.google.com");
	}
	
	public void ShowAlertPopup(){
		utilsPlugin.ShowAlertPopup("your native popup title","your native popup message");
	}
	
	public void ShowNativeLoading(){
		if(!isLoading){
			isLoading = true;

			utilsPlugin.ShowNativeLoading("loading please wait...",false);
			Invoke("HideNativeLoading",1f);
		}
	}
	
	public void HideNativeLoading(){
		utilsPlugin.HideNativeLoading();
		isLoading = false;
	}

	public void ShowToastMessage(){
		utilsPlugin.ShowToastMessage("insert your message here");
	}
	
	public void NextDemo(){
		demoController.nextPage();
	}
	
	public void PrevDemo(){
		demoController.prevPage();
	}

	public void Quit(){
		Debug.Log("Quit");
		Application.Quit();
	}
}


