using UnityEngine;
using System.Collections;

public class HideShowGameObject : MonoBehaviour {

	public GameObject target;
	public float delay;

	// Use this for initialization
	void Start () {
	
	}
	
	private void Hide(){
		if(target!=null){
			target.SetActive(false);
		}else{
			Debug.LogError("[HideShowGameObject] target is null");
		}
	}

	private void Show(){
		if(target!=null){
			target.SetActive(true);
		}else{
			Debug.LogError("[HideShowGameObject] target is null");
		}
	}

	private IEnumerator StartHideAndShow(){
		Debug.Log("HideAndShow");
		Hide();
		yield return new WaitForSeconds(delay);
		Show();
	}

	public void HideAndShow(){
		StartCoroutine(StartHideAndShow());
	}
}
