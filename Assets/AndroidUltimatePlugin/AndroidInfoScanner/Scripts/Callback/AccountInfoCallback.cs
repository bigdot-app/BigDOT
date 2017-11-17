using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AccountInfoCallback :  AndroidJavaProxy {
	public Action <string,string>onGetAccountComplete;
	public Action onGetAccountFail;
	
	public AccountInfoCallback() : base("com.gigadrillgames.androidplugin.accountinfo.IAccountCallback") {}
	
	void GetAccountComplete(String emailSet, String accountInfo){
		onGetAccountComplete(emailSet,accountInfo);
	}
	
	void GetAccountFail(){
		onGetAccountFail();
	}
}
