using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ContactCallback :  AndroidJavaProxy{
	public Action <string,string>onGetContactComplete;
	public Action onGetContactFail;
	
	public ContactCallback() : base("com.gigadrillgames.androidplugin.contactinfo.IContactCallback") {}


	void GetContactComplete(String contactName, String contacPhoneNumber){
		onGetContactComplete(contactName,contacPhoneNumber);
	}

	void GetContactFail(){
		onGetContactFail();
	}
}
