using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class CameraDemo : MonoBehaviour {

	private SharePlugin sharePlugin;

	public Text statusText;
	public RawImage rawImage;

	private CameraPlugin cameraPlugin;
	private string folderName="AndroidUltimatePluginImages";
	private string imagePath="";

	public Button shareButton;

	// Use this for initialization
	void Start (){
		sharePlugin = SharePlugin.GetInstance();
		sharePlugin.SetDebug(0);

		cameraPlugin = CameraPlugin.GetInstance();
		cameraPlugin.SetDebug(0);
		cameraPlugin.Init(folderName);

		cameraPlugin.SetCameraCallbackListener(onCaptureImageComplete,onCaptureImageCancel,onCaptureImageFail);

		EnableDisableShareButton(false);
	}
	
	public void OpenCamera(){
		cameraPlugin.OpenCamera();
		EnableDisableShareButton(false);
		UpdateStatus("Opening Camera");
	}

	public void SharePicture(){
		if(!imagePath.Equals("",StringComparison.Ordinal)){
			sharePlugin.ShareImage("MyPictureSubject","MyPictureSubjectContent",imagePath);
			UpdateStatus("Sharing Picture");
		}else{
			Debug.Log("[CameraDemo] imagepath is empty");
			UpdateStatus("can't image path is empty");
		}
	}

	private void UpdateStatus(string status){
		if(statusText!=null){
			statusText.text = String.Format("Status: {0}",status);
		}
	}

	private void DelayLoadImage(){
		//loads texture
		rawImage.texture = AUP.Utils.LoadTexture(imagePath);

		UpdateStatus("load image complete");
		EnableDisableShareButton(true);
	}

	private void LoadImageMessage(){
		UpdateStatus("Loading Image...");
	}

	private void EnableDisableShareButton(bool val){
		shareButton.interactable = val;
	}

	public void onCaptureImageComplete(string imagePath){
		this.imagePath = imagePath;
		UpdateStatus("CaptureImageComplete");

		Invoke("LoadImageMessage",0.3f);
		Invoke("DelayLoadImage",0.5f);
		Debug.Log("[CameraDemo] onCaptureImageComplete imagePath " + imagePath);
	}

	public void onCaptureImageCancel(){
		UpdateStatus("CaptureImageCancel");
		Debug.Log("[CameraDemo] onCaptureImageCancel");
	}

	public void onCaptureImageFail(){
		UpdateStatus("CaptureImageFail");
		Debug.Log("[CameraDemo] onCaptureImageFail");
	}

}
