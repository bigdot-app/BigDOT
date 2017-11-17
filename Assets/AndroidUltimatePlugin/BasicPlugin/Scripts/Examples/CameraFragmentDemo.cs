using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraFragmentDemo : MonoBehaviour {

	public Text cameraStatusText;
	private CameraFragmentPlugin cameraFragmentPlugin;


	private void Awake(){
		cameraFragmentPlugin = CameraFragmentPlugin.GetInstance();
		cameraFragmentPlugin.SetDebug(1);
		cameraFragmentPlugin.Init();
	}

	// Use this for initialization
	void Start () {
	
	}

	public void OpenCamera(){
		cameraFragmentPlugin.OpenCamera();
		UpdateCameraStatus("opening...");
	}

	private void UpdateCameraStatus(string val){
		if(cameraStatusText!=null){
			cameraStatusText.text = string.Format("Status: {0}", val);
		}
	}
}
