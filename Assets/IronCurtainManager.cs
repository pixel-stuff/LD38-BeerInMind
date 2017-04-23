using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IronCurtainManager : MonoBehaviour {

	public static IronCurtainManager m_instance;
	public Text m_textExplicatif;
	public Text m_buttonText;

	void Awake(){
		if(m_instance == null){
			m_instance = this;
			Init ();
		}else{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != m_instance)
				Destroy(this.gameObject);
		}
	}
	
	// Update is called once per frame
	public void Init () {
		TimeManager.m_DayEnding += EndTheDay;
	}

	public void EndTheDay(){
		GameStateManager.setGameState (GameState.EndOfTheDay);
		TimeManager.timePlay = false;
		this.GetComponent<Animation> ().Play ("CurtainApparition");
	}

	public void StartNextDay(){
		GameStateManager.setGameState (GameState.Playing);
		TimeManager.timePlay = true;
		this.GetComponent<Animation> ().Play ("CurtainRemove");
	}

	public void SetGameOver(string message){
		GameStateManager.setGameState (GameState.GameOver);
		TimeManager.timePlay = false;
		this.GetComponent<Animation> ().Play ("CurtainApparition");
	}

	public void ButtonClick(){
		if (GameStateManager.getGameState () == GameState.EndOfTheDay) {
			StartNextDay ();
		} else if (GameStateManager.getGameState () == GameState.EndOfTheDay) {
			GameStateManager.setGameState (GameState.Playing);
			SceneManager.LoadScene ("levelScene");
		}
	}

	public void OnDestroy(){
		TimeManager.m_DayEnding -= EndTheDay;
	}


}
