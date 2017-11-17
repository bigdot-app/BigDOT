using UnityEngine;
using System.Collections;

public class AUPHolder : MonoBehaviour {

	private static AUPHolder instance;
	private static GameObject container;
	
	public static AUPHolder GetInstance(){
		if(instance==null){
			container = new GameObject();
			container.name="AUPHolder";
			instance = container.AddComponent( typeof(AUPHolder) ) as AUPHolder;
			DontDestroyOnLoad(instance.gameObject);
		}
		
		return instance;
	}
}
