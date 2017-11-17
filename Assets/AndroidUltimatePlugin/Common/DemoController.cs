using UnityEngine;
using System.Collections;

public class DemoController : MonoBehaviour {

	private int currentPage = 0;
	private int maxPage;

	public GameObject[] demoSet;

	private void Awake(){
		maxPage = demoSet.Length;
	}

	// Use this for initialization
	void Start (){
		ShowDemo(currentPage);
	}
	
	public void nextPage(){
		currentPage++;
		if(currentPage >= maxPage){
			currentPage =maxPage;
		}
		ShowDemo(currentPage);
	}
	
	public void prevPage(){
		currentPage--;
		if(currentPage <= 0){
			currentPage = 0;
		}
		ShowDemo(currentPage);
	}

	private void ShowDemo(int demo){
		int setSize = demoSet.Length;
		for(int index=0;index<setSize;index++){
			GameObject obj = demoSet.GetValue(index) as GameObject;
			if(demo == index){
				obj.SetActive(true);
			}else{
				obj.SetActive(false);
			}
		}
	}

	public void Quit(){
		Debug.Log("Quit");
		Application.Quit();
	}
}
