using UnityEngine;
using System.Collections;
using System;

public class CameraCallback :  AndroidJavaProxy {
	
	public Action <string>onCaptureImageComplete;
	public Action onCaptureImageCancel;
	public Action onCaptureImageFail;
	
	public CameraCallback() : base("com.gigadrillgames.androidplugin.camera.ICameraCallback") {}
	

	void CaptureImageComplete(String imagePath){
		onCaptureImageComplete(imagePath);
	}

	void CaptureImageCancel(){
		onCaptureImageCancel();
	}

	void CaptureImageFail(){
		onCaptureImageFail();
	}
}

