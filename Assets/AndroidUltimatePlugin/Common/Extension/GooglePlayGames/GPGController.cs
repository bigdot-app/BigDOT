using UnityEngine;
using System.Collections;

//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
//using UnityEngine.SocialPlatforms;
//using GooglePlayGames.BasicApi.Multiplayer;
using System;

public class GPGController : MonoBehaviour {

	/*
	private static GPGController instance;
	private static GameObject container;
	private static AUPHolder aupHolder;

	private LeaderboardScoreData currentLeaderboardScoreData;
	private int currentLeaderboardScoreLen = 0;

	private Action <bool>SignInComplete;
	public event Action <bool>OnSignInComplete{
		add{SignInComplete+=value;}
		remove{SignInComplete-=value;}
	}

	private Action <bool>UnlockAchievementComplete;
	public event Action <bool>OnUnlockAchievementComplete{
		add{UnlockAchievementComplete+=value;}
		remove{UnlockAchievementComplete-=value;}
	}

	private Action <bool>IncrementAchievementComplete;
	public event Action <bool>OnIncrementAchievementComplete{
		add{IncrementAchievementComplete+=value;}
		remove{IncrementAchievementComplete-=value;}
	}

	private Action <bool>SubmitScoreComplete;
	public event Action <bool>OnSubmitScoreComplete{
		add{SubmitScoreComplete+=value;}
		remove{SubmitScoreComplete-=value;}
	}

	private Action <bool>LoadLeaderBoardScoreComplete;
	public event Action <bool>OnLoadLeaderBoardScoreComplete{
		add{LoadLeaderBoardScoreComplete+=value;}
		remove{LoadLeaderBoardScoreComplete-=value;}
	}

	private Action <bool>LoadNextLeaderBoardScoreComplete;
	public event Action <bool>OnLoadNextLeaderBoardScoreComplete{
		add{LoadNextLeaderBoardScoreComplete+=value;}
		remove{LoadNextLeaderBoardScoreComplete-=value;}
	}

	private Action <bool>LoadPrevLeaderBoardScoreComplete;
	public event Action <bool>OnLoadPrevLeaderBoardScoreComplete{
		add{LoadPrevLeaderBoardScoreComplete+=value;}
		remove{LoadPrevLeaderBoardScoreComplete-=value;}
	}

	private Action <CommonStatusCodes,GooglePlayGames.PlayGamesLocalUser.PlayerStats>LoadPlayerStatComplete;
	public event Action <CommonStatusCodes,GooglePlayGames.PlayGamesLocalUser.PlayerStats>OnLoadPlayerStatComplete{
		add{LoadPlayerStatComplete+=value;}
		remove{LoadPlayerStatComplete-=value;}
	}

	public static GPGController GetInstance(){
		if( instance == null ){
			container = new GameObject();
			container.name = "GPGController";
			instance = container.AddComponent(typeof(GPGController)) as GPGController;
			DontDestroyOnLoad(instance.gameObject);

			aupHolder = AUPHolder.GetInstance();
			instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
		}
		return instance;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	public void InitConfig(){
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			// enables saving game progress.
			.EnableSavedGames()
				// registers a callback to handle game invitations received while the game is not running.
				.WithInvitationDelegate(OnWithInvitationDelegate)
				// registers a callback for turn based match notifications received while the
				// game is not running.
				.WithMatchDelegate(OnWithMatchDelegate)
				.Build();
		
		PlayGamesPlatform.InitializeInstance(config);
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = true;
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate();
	}

	private void OnWithInvitationDelegate(Invitation invitation, bool shouldAutoAccept){
		Debug.Log("[GPGController]: OnWithInvitationDelegate invitation " + invitation + " shouldAutoAccept " + shouldAutoAccept);
	}

	private void OnWithMatchDelegate(TurnBasedMatch match, bool shouldAutoLaunch){
		Debug.Log("[GPGController]: OnWithMatchDelegate match " + match + " shouldAutoLaunch " + shouldAutoLaunch);
	}

	public void SignIn(){
		Social.localUser.Authenticate(OnSignIn);
	}

	public void SignOut(){
		PlayGamesPlatform.Instance.SignOut();
	}

	public void ShowAchievementUI(){
		Social.ShowAchievementsUI();
	}

	public void ShowLeaderboardUI(){
		Social.ShowLeaderboardUI();
	}

	public void UnlockAchievement(string achievementID){
		Social.ReportProgress(achievementID, 100.0f,OnUnlockAchievement);
	}

	public void IncrementAchievement(string achievementID, int incrementValue){
		PlayGamesPlatform.Instance.IncrementAchievement(achievementID, incrementValue,OnIncrementAchievement);
	}

	public void SubmitScore(string leaderboardID, int score){
		Social.ReportScore(score,leaderboardID,OnSubmitScore);
	}

	public void LoadLeaderBoardScore(
		string leaderboardID,LeaderboardStart start = LeaderboardStart.PlayerCentered
		,int rowCount = 100,LeaderboardCollection lbCollection = LeaderboardCollection.Public
		,LeaderboardTimeSpan lbTimeSpan = LeaderboardTimeSpan.AllTime
	){
		currentLeaderboardScoreLen = rowCount;

		PlayGamesPlatform.Instance.LoadScores(
			leaderboardID,
			start,
			rowCount,
			lbCollection,
			lbTimeSpan,OnLoadLeaderBoardScore);
	}

	public void LoadNextLeaderBoardScore(int rowCount){
		if(currentLeaderboardScoreData.Valid){
			if(currentLeaderboardScoreData.Scores.Length > currentLeaderboardScoreLen + rowCount){
				PlayGamesPlatform.Instance.LoadMoreScores(currentLeaderboardScoreData.NextPageToken, rowCount,OnNextLeaderBoardScore);
			}
		}
	}

	public void LoadPrevLeaderBoardScore(int rowCount){
		if(currentLeaderboardScoreData.Valid){
			if(currentLeaderboardScoreData.Scores.Length > currentLeaderboardScoreLen + rowCount){
				PlayGamesPlatform.Instance.LoadMoreScores(currentLeaderboardScoreData.PrevPageToken, rowCount,OnPrevLeaderBoardScore);
			}
		}
	}

	public void LoadPlayerStats(){
		((PlayGamesLocalUser)Social.localUser).GetStats(OnLoadPlayerStat);
	}

	private void OnUnlockAchievement(bool success){
		if(success){
			Debug.Log("[GPGController]: On UnlockAchievement successful!");
		}else{
			Debug.Log("[GPGController]: On UnlockAchievement failed!");
		}

		if(null!=UnlockAchievementComplete){
			UnlockAchievementComplete(success);
		}
	}

	private void OnIncrementAchievement(bool success){
		if(success){
			Debug.Log("[GPGController]: On IncrementAchievement successful!");
		}else{
			Debug.Log("[GPGController]: On IncrementAchievement failed!");
		}

		if(null!=IncrementAchievementComplete){
			IncrementAchievementComplete(success);
		}
	}

	private void OnSubmitScore(bool success){
		if(success){
			Debug.Log("[GPGController]: On SubmitScore successful!");
		}else{
			Debug.Log("[GPGController]: On SubmitScore failed!");
		}

		if(null!=SubmitScoreComplete){
			SubmitScoreComplete(success);
		}
	}

	private void OnLoadLeaderBoardScore(LeaderboardScoreData data){
		currentLeaderboardScoreData =  data;

		Debug.Log("[GPGController]: OnLoadLeaderBoardScore Leaderboard data valid: " + data.Valid);
		Debug.Log("[GPGController]: OnLoadLeaderBoardScore approx: " + data.ApproximateCount + " have " + data.Scores.Length);

		if(data.Valid){
			Debug.Log("[GPGController]: On LoadLeaderBoardScore successful!");
		}else{
			Debug.Log("[GPGController]: On LoadLeaderBoardScore failed!");
		}

		if(null!=LoadLeaderBoardScoreComplete){
			LoadLeaderBoardScoreComplete(data.Valid);
		}
	}

	private void OnNextLeaderBoardScore(LeaderboardScoreData data){
		currentLeaderboardScoreData = data;
		Debug.Log("[GPGController]: OnNextLeaderBoardScore Leaderboard data valid: " + data.Valid);
		Debug.Log("[GPGController]: OnNextLeaderBoardScore approx: " + data.ApproximateCount + " have " + data.Scores.Length);

		if(null!=LoadNextLeaderBoardScoreComplete){
			LoadNextLeaderBoardScoreComplete(data.Valid);
		}
	}

	private void OnPrevLeaderBoardScore(LeaderboardScoreData data){
		currentLeaderboardScoreData = data;

		Debug.Log("[GPGController]: OnPrevLeaderBoardScore Leaderboard data valid: " + data.Valid);
		Debug.Log("[GPGController]: OnPrevLeaderBoardScore approx: " + data.ApproximateCount + " have " + data.Scores.Length);

		if(null!=LoadPrevLeaderBoardScoreComplete){
			LoadPrevLeaderBoardScoreComplete(data.Valid);
		}
	}

	private void OnLoadPlayerStat(CommonStatusCodes rc,GooglePlayGames.PlayGamesLocalUser.PlayerStats stats){
		if (rc <= 0){
			Debug.Log("[GPGController]:OnLoadPlayerStat successful ");
			Debug.Log("[GPGController]: OnLoadPlayerStat rc: " + rc + " stats " + stats);
			Debug.Log("It has been " + stats.DaysSinceLastPlayed + " days");
		}else{
			Debug.Log("[GPGController]: OnLoadPlayerStat failed ");
		}

		if(null!=LoadPlayerStatComplete){
			LoadPlayerStatComplete(rc,stats);
		}
	}

	private void OnSignIn(bool success){
		if(success){
			Debug.Log("[GPGController]: sign in successful!");
		}else{
			Debug.Log("[GPGController]: sign in failed!");
		}

		if(null!=SignInComplete){
			SignInComplete(success);
		}
	}
	*/
}
