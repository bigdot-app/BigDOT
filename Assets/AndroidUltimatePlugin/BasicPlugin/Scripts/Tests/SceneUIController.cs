using UnityEngine;
using System.Collections;

public class SceneUIController : MonoBehaviour {

	private SceneController sceneController;
	public string nextSceneToLoad;

	// Use this for initialization
	void Start () {
		sceneController = SceneController.GetInstance();
		sceneController.sceneNameToLoad = nextSceneToLoad;
	}
	
	public void loadNextScene(){
		sceneController.loadNextScene();
	}

	public void loadPrevScene(){
		sceneController.loadPrevScene();
	}
}
