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
	public GameObject m_middleBubble;
	public GameObject m_leftBubble;
	public GameObject m_rightBubble;
	public float m_factorByChar = 600f/35f;
	public int m_minWidth = 200;
	public int m_maxWidth = 1500;

	public string m_textToDisplay = "I'm a baby girl in a baby world";
	public float m_animationDisplayLetterEvery = 0.015f;

	public static MainTalkManager m_instance;
	// Use this for initialization
	void Awake () {
		if(m_instance == null){
			m_instance = this;
			this.StopAllCoroutines ();
			m_bulle.SetActive (false);
			m_back.SetActive (false);
			m_customer.color = new Color (0.0f, 0.0f, 0.0f, 0.0f);
			m_customer.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0.0f,0.0f);
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
		int nb_char = txt.Length/2 +1;
		int addHeight = 0;
		int totalLength = (int)(m_factorByChar * nb_char);
		totalLength = (totalLength < m_minWidth) ? m_minWidth : totalLength;
		totalLength = (totalLength > m_maxWidth) ? m_maxWidth : totalLength;
		if (totalLength == m_maxWidth) {
			addHeight = 96 / 2;
		}
		m_middleBubble.GetComponent<RectTransform> ().sizeDelta = new Vector2 (totalLength +50 ,m_middleBubble.GetComponent<RectTransform> ().rect.height + addHeight);
		m_text.GetComponent<RectTransform> ().sizeDelta = new Vector2 (totalLength ,m_text.GetComponent<RectTransform> ().rect.height + addHeight);
		m_leftBubble.GetComponent<RectTransform> ().sizeDelta = new Vector2 (m_leftBubble.GetComponent<RectTransform> ().rect.width ,m_leftBubble.GetComponent<RectTransform> ().rect.height + addHeight);
		m_rightBubble.GetComponent<RectTransform> ().sizeDelta = new Vector2 (m_rightBubble.GetComponent<RectTransform> ().rect.width ,m_rightBubble.GetComponent<RectTransform> ().rect.height + addHeight);
		m_bulle.SetActive (false);
		m_back.SetActive (false);
		m_customer.color = new Color (0.0f, 0.0f, 0.0f, 0.0f);
		m_customer.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0.0f,0.0f);

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
