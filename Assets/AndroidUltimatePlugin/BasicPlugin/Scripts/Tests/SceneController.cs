using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class SceneController : MonoBehaviour {

	public string sceneNameToLoad;
	private static SceneController instance;
	private static GameObject container;

	private string prevScene;
	private string currentScene;

	public static SceneController GetInstance(){
		if(instance == null){
			container = new GameObject();
			container.name = "SceneController";
			instance = container.AddComponent( typeof(SceneController)  ) as SceneController;
			DontDestroyOnLoad(instance.gameObject);
		}

		return instance;
	}

	// Use this for initialization
	void Start () {
		Scene scene = SceneManager.GetActiveScene();
		currentScene = scene.name;
	}
	
	public void loadNextScene(){
		prevScene = currentScene;
		SceneManager.LoadScene(sceneNameToLoad);
	}

	public void loadPrevScene(){
		if(!prevScene.Equals("",StringComparison.Ordinal)){
			string sceneToLoad = prevScene;
			prevScene = currentScene;
			SceneManager.LoadScene(sceneToLoad);
		}else{
			Debug.Log("prev scene is empty!");
		}
	}
}
