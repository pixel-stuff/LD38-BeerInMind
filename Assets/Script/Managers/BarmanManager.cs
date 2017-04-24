using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BarmanManager : MonoBehaviour {

	#region Singleton
	public static BarmanManager m_instance;
	void Awake(){
		if(m_instance == null){
			//If I am the first instance, make me the Singleton
			m_instance = this;
			Dismiss ();
		}else{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != m_instance)
				Destroy(this.gameObject);
		}
	}
	#endregion Singleton

	public GameObject m_firstBulle;
	public GameObject m_answerClickable;

	public TextMesh m_answer1;
	public TextMesh m_answer2;

	public Action<string> Answer; //renvoyer le text cliquer

	public void Says(string answer1, string answer2){
		m_answer1.text = answer1;
		m_answer2.text = answer2;
		m_firstBulle.SetActive (true);
	}

	public void FirstBulleClick(){
		TimeManager.timePlay = false;
		m_firstBulle.SetActive (false);
		m_answerClickable.SetActive (true);
	}

	public void StartAnswer1Click(){
		if (Answer != null) {
			Answer (m_answer1.text);
		}
		Dismiss ();
		TimeManager.timePlay = true;
	}

	public void StartAnswer2Click(){
		if (Answer != null) {
			Answer (m_answer2.text);
		}
		Dismiss ();
		TimeManager.timePlay = true;
	}

	public void Dismiss(){
		m_firstBulle.SetActive (false);
		m_answerClickable.SetActive (false);
		m_firstBulle.SetActive (false);
	}
}
