using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IronCurtainManager : MonoBehaviour {

	public static IronCurtainManager m_instance;
	public Text m_textExplicatif;
	public Text m_buttonText;
	public bool m_isActivate = false;
	public AudioClip m_background;

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
		m_isActivate = true;
		GameStateManager.setGameState (GameState.EndOfTheDay);
		TimeManager.timePlay = false;
		this.GetComponent<Animation> ().Play ("CurtainApparition");
	}

	public void StartNextDay(){
		m_buttonText.text = "Start next day ";
		GameStateManager.setGameState (GameState.Playing);
		TimeManager.timePlay = true;
		this.GetComponent<Animation> ().Play ("CurtainRemove");

		StartCoroutine (CoolDownControl ());
	}

	public void SetGameOver(string message){
		m_textExplicatif.text = message;
		m_isActivate = true;
		m_buttonText.text = "Restart From Monday ";
		GameStateManager.setGameState (GameState.GameOver);
		TimeManager.timePlay = false;
		this.GetComponent<Animation> ().Play ("CurtainApparition");
	}

	public void ButtonClick(){
		//Debug.Log ("BUTTON CLICK");
		 if (GameStateManager.getGameState () == GameState.GameOver) {
			GameStateManager.setGameState (GameState.Playing);
			SceneManager.LoadScene ("levelScene");
		} else {
			StartNextDay ();
		}
	}

	public IEnumerator CoolDownControl(){
		yield return new WaitForSeconds (0.5f);
		m_isActivate = false;
	}

	public void OnDestroy(){
		TimeManager.m_DayEnding -= EndTheDay;
	}

	public void PlayMusic(){
		if (this.GetComponent<AudioSource> ().isPlaying) {
			this.GetComponent<AudioSource> ().UnPause ();
		}else{
			this.GetComponent<AudioSource> ().Play();
		}
	}

	public void StopMusic(){
		this.GetComponent<AudioSource> ().Stop ();
	}
}
