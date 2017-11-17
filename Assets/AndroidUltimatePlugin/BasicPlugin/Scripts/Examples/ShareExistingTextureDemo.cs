using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ShareExistingTextureDemo : MonoBehaviour {	

	private SharePlugin sharePlugin;
	private Texture2D existingTexture;
	private string texturePath="";
	public RawImage rawImage;
	
	// Use this for initialization
	void Start (){
		sharePlugin = SharePlugin.GetInstance();
		sharePlugin.SetDebug(0);
	}

	public void ShareExistingTexture(){
		SaveExistingTextureOnDevice();
		ShareImage();
	}
	
	private void SaveExistingTextureOnDevice(){
		string textureName = "sampleTexture.png";
		texturePath = Application.persistentDataPath + "/" + textureName;

		existingTexture = rawImage.texture as Texture2D;
		StartCoroutine(AUP.Utils.SaveTexureOnDevice(texturePath,existingTexture));
	}
	
	private void ShareImage(){
		if(!texturePath.Equals("",StringComparison.Ordinal)){
			sharePlugin.ShareImage("ExistingTexture","ExistingTextureContent",texturePath);
		}else{
			Debug.Log("[CameraDemo] texturePath is empty");
		}
	}
}
