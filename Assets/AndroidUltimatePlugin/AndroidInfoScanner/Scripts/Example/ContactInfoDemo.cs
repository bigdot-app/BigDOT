using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using AUP;

public class ContactInfoDemo : MonoBehaviour {

	public GameObject contactText;
	public Button prevButton;
	public Button nextButton;

	public Transform contentPanel;
	private List<GameObject> contactTextCollection = new List<GameObject>();
	private List<string> contacts = new List<string>();

	private int minIndex = 0;
	private int maxIndex = 0;
	private int currentIndex = 0;
	private int maxContacts = 15;


	private ContactInfoPlugin contactInfoPlugin;

	private Dispatcher dispatcher;

	// Use this for initialization
	void Start () {
		dispatcher = Dispatcher.GetInstance();

		contactInfoPlugin = ContactInfoPlugin.GetInstance();
		contactInfoPlugin.SetDebug(0);
		contactInfoPlugin.Init();
		contactInfoPlugin.SetContactCallbackListener(OnGetContactComplete,OnGetContactFail);

		CheckButton();
	}

	private void OnApplicationPause(bool val){
		if(contactInfoPlugin!=null){
			if(val){
				contactInfoPlugin.DestroyLoader();
			}
		}
	}

	private void TestPopulate(){
		string conName= null;
		string pName= null;

		for(int index = 0; index < 33; index ++){
			if(conName == null){
				conName = "contactName" + index;
			}else{
				conName += "," + "contactName" + index;
			}

			if(pName == null){
				pName = "contactPhone" + index;
			}else{
				pName += "," + "contactPhone" + index;
			}
		}

		OnGetContactComplete(conName,pName);
	}
	
	public void GetContact(){
		//TestPopulate();
		contactInfoPlugin.GetContact();
	}

	public void Populate(){
		if(contactTextCollection.Count == 0 ){
			for(int index = 0; index < maxContacts; index++){
				GameObject textObject = Instantiate(contactText) as GameObject;
				textObject.transform.SetParent (contentPanel,false);
				contactTextCollection.Add(textObject);
			}
		}
	}

	public void UpdateContact(){
		Debug.Log("[ContactInfoDemo] trying to update...");

		for(int index = 0; index < maxContacts; index++){
			int curr = currentIndex + index;
			Text contactText = contactTextCollection[index].GetComponent<Text>();
			if(contactText!=null){
				if( curr < maxIndex){
					contactText.gameObject.SetActive(true);
					contactText.text = contacts[curr];
					Debug.Log("[ContactInfoDemo] updating contact ...");
				}else{
					Debug.Log("[ContactInfoDemo] hiding contact ...");
					contactText.gameObject.SetActive(false);
				}
			}
		}
	}

	public void NextContact(){
		if((currentIndex + maxContacts ) < maxIndex){
			currentIndex += maxContacts;
		}

		CheckButton();
		UpdateContact();
		Debug.Log("[ContactInfoDemo] NextContact");
	}

	public void PrevContact(){
		if((currentIndex - maxContacts ) >=  minIndex){
			currentIndex -= maxContacts;
		}
		UpdateContact();
		CheckButton();
		Debug.Log("[ContactInfoDemo] PrevContact");
	}

	private void CheckButton(){
		if(currentIndex < maxIndex && maxIndex > maxContacts && (currentIndex + maxContacts) < maxIndex ){
			nextButton.interactable = true;
		}else{
			nextButton.interactable = false;
		}

		if(currentIndex > minIndex){
			prevButton.interactable = true;
		}else{
			prevButton.interactable = false;
		}
	}

	private void OnGetContactComplete(string contactNameSet, string contactPhoneNumberSet){
		dispatcher.InvokeAction(
			()=>{
				Debug.Log("[ContactInfoDemo]: OnGetContactComplete");
				contacts.Clear();

				string [] contactNames = contactNameSet.Split(',');
				string [] contactPhoneNumbers = contactPhoneNumberSet.Split(',');
				
				int count = contactNames.Length;
				
				for(int index = 0; index < count; index++){
					Debug.Log(" name " + contactNames.GetValue(index) + " phone " + contactPhoneNumbers.GetValue(index));
					contacts.Add(String.Format("{0} - #: {1}",contactNames.GetValue(index),contactPhoneNumbers.GetValue(index)));
				}

				minIndex = 0;
				maxIndex = contacts.Count;
				Populate();
				UpdateContact();
				CheckButton();
			}
		);
	}

	private void OnGetContactFail(){
		dispatcher.InvokeAction(
			()=>{
				Debug.Log("[ContactInfoDemo]: OnGetContactFail");
			}
		);
	}
}
