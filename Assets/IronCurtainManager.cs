using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronCurtainManager : MonoBehaviour {

	public static IronCurtainManager m_instance;
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
		TimeManager.m_DayEnding += EndTheDay ();
	}

	public void EndTheDay(){
		GameStateManager.setGameState (GameState.EndOfTheDay);
	}

	public void SetGameOver(string message){
	}

	public void OnDestroy(){
		TimeManager.m_DayEnding -= EndTheDay ();
	}

}
