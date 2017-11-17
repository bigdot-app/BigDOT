using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using AUP;

public class AccountInfoDemo : MonoBehaviour {
	
	public GameObject emailAccountText;
	public Text accountNameText;
	public Text phoneNumberText;
	public Button prevButton;
	public Button nextButton;
	
	public Transform contentPanel;
	private List<GameObject> emailAccountTextCollection = new List<GameObject>();
	private List<string> emailAccounts = new List<string>();
	private List<string> phoneNumerList = new List<string>();
	
	private int minIndex = 0;
	private int maxIndex = 0;
	private int currentIndex = 0;
	private int maxContacts = 10;

	private AccountInfoPlugin accountInfoPlugin;

	private Dispatcher dispatcher;
	
	// Use this for initialization
	void Start () {
		dispatcher = Dispatcher.GetInstance();

		accountInfoPlugin = AccountInfoPlugin.GetInstance();
		accountInfoPlugin.SetDebug(0);
		accountInfoPlugin.Init();
		accountInfoPlugin.SetAccountCallbackListener(onGetAccountComplete,onGetAccountFail);		
		CheckButton();
	}

	//for testing population of data
	private void TestPopulate(){
		string emailAccount= null;
		
		for(int index = 0; index < 33; index ++){
			if(emailAccount == null){
				emailAccount = "EmailAccount" + index;
			}else{
				emailAccount += "," + "EmailAccount" + index;
			}
		}
		
		onGetAccountComplete(emailAccount, "anyone dawdawdaw dadawdaw" + ",+123,+4165465451,+45893");
	}

	//getting email account and account name
	public void GetEmailAccount(){
		//TestPopulate();
		accountInfoPlugin.GetAccount();
	}

	//populate required text
	public void Populate(){
		if(emailAccountTextCollection.Count == 0 ){
			for(int index = 0; index < maxContacts; index++){
				GameObject textObject = Instantiate(emailAccountText) as GameObject;
				textObject.transform.SetParent (contentPanel,false);
				emailAccountTextCollection.Add(textObject);
			}
		}
	}

	//just reusing text to update info
	public void UpdateContact(){
		Debug.Log("[AccountInfoDemo] trying to update...");
		
		for(int index = 0; index < maxContacts; index++){
			int curr = currentIndex + index;
			Text emailAccountText = emailAccountTextCollection[index].GetComponent<Text>();
			if(emailAccountText!=null){
				if( curr < maxIndex){
					emailAccountText.gameObject.SetActive(true);
					emailAccountText.text = emailAccounts[curr];
					Debug.Log("[AccountInfoDemo] updating email ...");
				}else{
					Debug.Log("[AccountInfoDemo] hiding email ...");
					emailAccountText.gameObject.SetActive(false);
				}
			}
		}
	}

	//move to next set of email account
	public void NextEmail(){
		if((currentIndex + maxContacts ) < maxIndex){
			currentIndex += maxContacts;
		}
		
		CheckButton();
		UpdateContact();
		Debug.Log("[AccountInfoDemo] NextContact");
	}

	//move to previous set of email account
	public void PrevEmail(){
		if((currentIndex - maxContacts ) >=  minIndex){
			currentIndex -= maxContacts;
		}
		UpdateContact();
		CheckButton();
		Debug.Log("[AccountInfoDemo] PrevContact");
	}

	//check limit of buttons
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

	//updates account name
	private void UpdateAccountName(string accountName){
		if(accountNameText!=null){
			accountNameText.text = String.Format("Account Name: {0}",accountName);
		}
	}

	//updates account phone number
	private void UpdateAccountPhoneNumber(string phoneNumber){
		if(phoneNumberText!=null){
			phoneNumberText.text = String.Format("Phone Number: {0}",phoneNumber);
		}
	}

	//event recieved when sucessfully get email account and account name
	private void onGetAccountComplete(string emailSet,string accountInfo){
		dispatcher.InvokeAction(
			()=>{
				Debug.Log("[AccountInfoDemo]: onGetAccountComplete accountInfo: " + accountInfo);

				string[] accountInfoSet = accountInfo.Split(',');
				int phoneNumberCount = accountInfoSet.Length - 1;

				Debug.Log(" AccountInfoSet len " + accountInfoSet.Length);

				//update account name
				UpdateAccountName(accountInfoSet.GetValue(0).ToString());

				//clear all emails
				emailAccounts.Clear();

				//parse email accounts
				string [] emailAccount = emailSet.Split(',');		
				int count = emailAccount.Length;

				//store email accounts if there's any
				for(int index = 0; index < count; index++){
					Debug.Log(" email " + emailAccount.GetValue(index));
					emailAccounts.Add(String.Format("{0}",emailAccount.GetValue(index)));
				}

				//clear phone number lists
				phoneNumerList.Clear();


				//store phone numbers if there's any
				for(int index = 1; index <= phoneNumberCount; index++){
					Debug.Log(" Phone number " + accountInfoSet.GetValue(index));
					phoneNumerList.Add(String.Format("{0}",accountInfoSet.GetValue(index)));
				}

				if(phoneNumberCount>0){
					UpdateAccountPhoneNumber(phoneNumerList[0]);
				}

				//setting min and max for navigation
				minIndex = 0;
				maxIndex = emailAccounts.Count;

				//add the required text
				Populate();

				//update info on email list
				UpdateContact();

				//checks button limits
				CheckButton();
			}
		);
	}

	//event recieved when faild to get email account and account name
	private void onGetAccountFail(){
		dispatcher.InvokeAction(
			()=>{
				Debug.Log("[AccountInfoDemo]: onGetAccountFail ");
			}
		);
	}
}