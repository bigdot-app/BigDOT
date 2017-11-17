using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
//using GooglePlayGames.BasicApi;

public class GooglePlayGamesControllerDemo : MonoBehaviour {

	private GPGController gpgController;
	public Text statusText;

	// Use this for initialization
	void Start () {
		/*gpgController = GPGController.GetInstance();

		gpgController.InitConfig();

		gpgController.OnSignInComplete+=OnSignInComplete;

		gpgController.OnSubmitScoreComplete+=OnSubmitScoreComplete;
		gpgController.OnLoadLeaderBoardScoreComplete+=OnLoadLeaderBoardScoreComplete;
		gpgController.OnLoadNextLeaderBoardScoreComplete+=OnLoadNextLeaderBoardScoreComplete;
		gpgController.OnLoadPrevLeaderBoardScoreComplete+=OnLoadPrevLeaderBoardScoreComplete;

		gpgController.OnUnlockAchievementComplete+=OnUnlockAchievementComplete;
		gpgController.OnIncrementAchievementComplete+=OnIncrementAchievementComplete;

		gpgController.OnLoadPlayerStatComplete+=OnLoadPlayerStatComplete;
		*/
	}

	private void OnDestroy(){

		/*
		gpgController.OnSignInComplete-=OnSignInComplete;

		gpgController.OnSubmitScoreComplete-=OnSubmitScoreComplete;
		gpgController.OnLoadLeaderBoardScoreComplete-=OnLoadLeaderBoardScoreComplete;
		gpgController.OnLoadNextLeaderBoardScoreComplete-=OnLoadNextLeaderBoardScoreComplete;
		gpgController.OnLoadPrevLeaderBoardScoreComplete-=OnLoadPrevLeaderBoardScoreComplete;

		gpgController.OnUnlockAchievementComplete-=OnUnlockAchievementComplete;
		gpgController.OnIncrementAchievementComplete-=OnIncrementAchievementComplete;

		gpgController.OnLoadPlayerStatComplete-=OnLoadPlayerStatComplete;
		*/
	}
	
	public void SignIn(){
		//gpgController.SignIn();
	}

	public void SignOut(){
		//gpgController.SignOut();
		//UpdateStatusText("SignOut Successfull");
	}

	public void ShowAchievement(){
		//gpgController.ShowAchievementUI();
	}

	public void UnlockAchievement(){
		//gpgController.UnlockAchievement(GPGConstants.achievement_greenhorn);
	}

	public void IncrementAchievement(){
		//int incrementValue = 1;
		//gpgController.IncrementAchievement(GPGConstants.achievement_quick_flex,incrementValue);
	}

	public void ShowLeaderboard(){
		//gpgController.ShowLeaderboardUI();
	}

	public void SubmitScore(){
		//int score = 5;
		//gpgController.SubmitScore(GPGConstants.leaderboard_hoppy_island_top_score,score);
	}

	public void LoadLeaderboardScore(){
		//gpgController.LoadLeaderBoardScore(GPGConstants.leaderboard_hoppy_island_top_score);
	}

	public void LoadNextLeaderboardScore(){
		//gpgController.LoadNextLeaderBoardScore(10);
	}

	public void LoadPrevLeaderboardScore(){
		//gpgController.LoadPrevLeaderBoardScore(10);
	}

	public void LoadPlayerStats(){
		//gpgController.LoadPlayerStats();
	}

	private void UpdateStatusText(string val){
		/*if(statusText!=null){
			statusText.text = String.Format("Status: {0}",val);
		}*/
	}

	private void OnSignInComplete(bool status){
		/*if(status){
			UpdateStatusText("SignIn successfull");	
		}else{
			UpdateStatusText("SignIn Failed");
		}*/
	}

	private void OnSubmitScoreComplete(bool status){
		/*if(status){
			UpdateStatusText("SubmitScoreComplete successfull");	
		}else{
			UpdateStatusText("SubmitScoreComplete Failed");
		}*/
	}

	private void OnLoadLeaderBoardScoreComplete(bool status){
		/*if(status){
			UpdateStatusText("LoadLeaderboardScore successfull");	
		}else{
			UpdateStatusText("LoadLeaderboardScore Failed");
		}*/
	}

	private void OnLoadNextLeaderBoardScoreComplete(bool status){
		/*if(status){
			UpdateStatusText("LoadNextLeaderboardScore successfull");	
		}else{
			UpdateStatusText("LoadNextLeaderboardScore Failed");
		}*/
	}

	private void OnLoadPrevLeaderBoardScoreComplete(bool status){
		/*if(status){
			UpdateStatusText("LoadPrevLeaderBoardScoreComplete successfull");	
		}else{
			UpdateStatusText("LoadPrevLeaderBoardScoreComplete Failed");
		}*/
	}

	private void OnUnlockAchievementComplete(bool status){
		/*if(status){
			UpdateStatusText("UnlockAchievementComplete successfull");	
		}else{
			UpdateStatusText("UnlockAchievementComplete Failed");
		}*/
	}

	private void OnIncrementAchievementComplete(bool status){
		/*if(status){
			UpdateStatusText("IncrementAchievementComplete successfull");
		}else{
			UpdateStatusText("IncrementAchievementComplete Failed");
		}*/
	}

	/*private void OnLoadPlayerStatComplete(CommonStatusCodes rc,GooglePlayGames.PlayGamesLocalUser.PlayerStats stats){
		if (rc <= 0){
			UpdateStatusText("LoadPlayerStat successful");
			Debug.Log("[GooglePlayGamesControllerDemo]: OnLoadPlayerStat rc: " + rc + " stats " + stats);
			Debug.Log("[GooglePlayGamesControllerDemo] It has been " + stats.DaysSinceLastPlayed + " days");
		}else{
			UpdateStatusText("LoadPlayerStat failed");
		}
	}*/
}
