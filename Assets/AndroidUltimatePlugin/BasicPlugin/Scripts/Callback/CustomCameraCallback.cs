using UnityEngine;
using System.Collections;
using System;

public class CustomCameraCallback :  AndroidJavaProxy {
	
	public Action <string>onCaptureImageComplete;
	public Action onCaptureImageCancel;
	public Action onCaptureImageFail;
	
	public CustomCameraCallback() : base("com.gigadrillgames.androidplugin.camera.ICustomCameraCallback") {}
	
	
	void CaptureImageComplete(String imagePaths){
		onCaptureImageComplete(imagePaths);
	}
	
	void CaptureImageCancel(){
		onCaptureImageCancel();
	}
	
	void CaptureImageFail(){
		onCaptureImageFail();
	}
}


