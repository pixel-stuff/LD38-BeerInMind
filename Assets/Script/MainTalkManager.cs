using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainTalkManager : MonoBehaviour {

	public GameObject m_bulle;
	public Image m_customer;
	public Text m_text;
	public Text m_name;
	public GameObject m_back;
	public bool m_isActivate = false;

	public string m_textToDisplay = "I'm a baby girl in a baby world";
	public float m_animationDisplayLetterEvery = 0.07f;

	public static MainTalkManager m_instance;
	// Use this for initialization
	void Awake () {
		if(m_instance == null){
			m_instance = this;
			RestartInit ();
		}else{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != m_instance)
				Destroy(this.gameObject);
		}
	}
	
	public void RestartInit () {
		this.StopAllCoroutines ();
		TimeManager.timePlay = true;
		m_bulle.SetActive (false);
		m_back.SetActive (false);
		m_customer.color = new Color (0.0f, 0.0f, 0.0f, 0.0f);
		m_customer.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0.0f,0.0f);
		StartCoroutine (CoolDownControl ());
	}


	public void StartDisplayAnimation(string txt, Sprite sprite, string caracName){
		m_customer.sprite = sprite;
		m_isActivate = true;
		if (txt == null) {
			m_textToDisplay = "Le texte recu est null jeremy :(";
		} else {
			m_textToDisplay = txt;
		}
		m_name.text = caracName;
		RestartInit ();
		this.GetComponent<Animation> ().Play ();
		TimeManager.timePlay = false;
		if(BarmanManager.m_instance != null)
			BarmanManager.m_instance.Dismiss ();
		StartCoroutine (DisplayAnimationCorout());
	}

	//Do not call form an other class
	public IEnumerator DisplayAnimationCorout(){
		yield return new WaitForSeconds (this.GetComponent<Animation> ().clip.length);
		int characDisplay = 0;
		//Debug.Log ("m_textToDisplay :" + m_textToDisplay);
		int characTarget = m_textToDisplay.Length;
		m_text.text = "";
		do{
			m_text.text += m_textToDisplay[characDisplay];
			characDisplay++;
			yield return new WaitForSeconds(m_animationDisplayLetterEvery);
		}while(characDisplay < characTarget);
	}

	public IEnumerator CoolDownControl(){
		yield return new WaitForSeconds (0.5f);
		m_isActivate = false;
	}
}
