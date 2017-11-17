using UnityEngine;
using System.Collections;
using System;

public class MediaScannerCallback :  AndroidJavaProxy {

	public Action onScanStarted;
	public Action onScanComplete;
	public Action onScanFail;

	public MediaScannerCallback() : base("com.gigadrillgames.androidplugin.mediaScanner.IMediaScannerCallback") {}


	void ScanStarted(){
		onScanStarted();
	}

	void ScanComplete(){
		onScanComplete();
	}

	void ScanFail(){
		onScanFail();
	}
}



