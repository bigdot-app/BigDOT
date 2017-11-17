using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using AUP;

public class SharedPrefDemo : MonoBehaviour {

	private const string TAG = "[SharedPrefDemo]: ";

	public InputField inputStringField;
	public InputField inputIntField;

	public Text loadStringText;
	public Text loadIntText;

	private SharedPrefPlugin sharedPrefPlugin;
	private string sharedPrefNameKey;
	private string sampleStringKey;
	private string sampleIntKey;

	private void Awake(){
		sharedPrefPlugin = SharedPrefPlugin.GetInstance();
		sharedPrefPlugin.SetDebug(0);
		sharedPrefPlugin.Init();

		sharedPrefNameKey = "sampleSharedPrefName";
		sampleStringKey = "someString";
		sampleIntKey = "someInt";
	}

	// Use this for initialization
	void Start () {
	
	}
	
	public void Save(){
		string stringToSave = inputStringField.text;
		sharedPrefPlugin.SaveString( sharedPrefNameKey, sampleStringKey,stringToSave);

		int intToSave =0;
		bool res = int.TryParse(inputIntField.text, out intToSave);
		if(res){
			sharedPrefPlugin.SaveInt( sharedPrefNameKey, sampleIntKey,intToSave);	
		}else{
			Debug.Log( "failed saving int on sharedPref" );
		}
	}

	public void Load(){
		loadStringText.text = "Load String: " + sharedPrefPlugin.LoadString(sharedPrefNameKey,sampleStringKey);
		loadIntText.text =  "Load Int: " +sharedPrefPlugin.LoadInt(sharedPrefNameKey,sampleIntKey);
	}
}
