using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Globalization;

public class AudioRecorderExample : MonoBehaviour {

	private AudioRecorderPlugin audioRecorderPlugin;
	private float wait = 1f;

	public Text statusText;
	private float pitch = 1f;
	private float volume =1f;

	public Text pitchText;
	public Slider pitchSlider;

	// Use this for initialization
	void Start (){
		audioRecorderPlugin = AudioRecorderPlugin.GetInstance();
		audioRecorderPlugin.SetDebug(0);

		string filename = "testAudioRecord";
		string outputPath = Application.persistentDataPath;
		audioRecorderPlugin.InitAudioRecorder(outputPath,filename,true);

		SetVolume();
		UpdatePitch();
	}

	void OnApplicationPause(bool pauseStatus) {
		audioRecorderPlugin.ReleaseAudio();
	}

	private void OnDestroy(){
		audioRecorderPlugin.ReleaseAudio();
	}

	public void SetVolume(){		
		audioRecorderPlugin.SetVolume(volume);
	}

	public void StartAudioRecord(){
		//androidUltimatePluginController.StopAudioRecord();

		CancelInvoke("DelayIdle");
		SetStatus("recording");
		audioRecorderPlugin.StartAudioRecord();
	}

	public void StopAudioRecord(){
		SetStatus("stop record");
		Invoke("DelayIdle",wait);

		audioRecorderPlugin.StopAudioRecord();
		audioRecorderPlugin.LoadRecordedAudio();
	}

	public void PlayAudioRecord(){
		audioRecorderPlugin.StopAudioRecordPlay();
		audioRecorderPlugin.PlayAudioRecord(pitch);
		CancelInvoke("DelayIdle");
		//SetStatus("play record" + pitch );
		SetStatus("play record");
	}

	public void StopAudioRecordPlay(){
		SetStatus("stop play");
		Invoke("DelayIdle",wait);

		audioRecorderPlugin.StopAudioRecordPlay();
	}

	public void PauseAudioRecordPlay(){
		SetStatus("Pause play");
		Invoke("DelayIdle",wait);
		
		audioRecorderPlugin.PauseAudioRecordPlay();
	}

	private void DelayIdle(){
		SetStatus("Idle...");
	}

	public void OnSliderValueChange(){
		pitch =  pitchSlider.value;
		UpdatePitch();

		Debug.Log("OnSliderValueChange pitch " + pitch);
	}

	private void UpdatePitch(){
		pitchText.text = "Pitch: " + pitch;
	}

	private void SetStatus(string val){
		statusText.text = val;
	}
}