using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainTalkManager : MonoBehaviour {

	public Image m_bulleText;
	public Image m_customer;
	public Text m_text;

	public string m_textToDisplay = "I'm a baby girl in a baby world";
	public float m_animationDisplayLetterEvery = 0.07f;

	public static MainTalkManager m_instance;
	// Use this for initialization
	void Awake () {
		if(m_instance == null){
			m_instance = this;
			InitGameObject ();
		}else{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != m_instance)
				Destroy(this.gameObject);
		}
	}
	
	// Update is called once per frame
	public void InitGameObject () {
		m_bulleText.gameObject.SetActive (false);
		m_customer.color = new Color (0.0f, 0.0f, 0.0f, 0.0f);
		m_customer.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0.0f,0.0f);
	}

	public void StartDisplayAnimation(string txt){
		m_textToDisplay = txt;
		InitGameObject ();
		this.GetComponent<Animation> ().Play ();
		StartCoroutine (DisplayAnimationCorout());
	}

	//Do not call form an other class
	public IEnumerator DisplayAnimationCorout(){
		yield return new WaitForSeconds (this.GetComponent<Animation> ().clip.length);
		int characDisplay = 0;
		int characTarget = m_textToDisplay.Length;
		m_text.text = "";
		do{
			m_text.text += m_textToDisplay[characDisplay];
			characDisplay++;
			yield return new WaitForSeconds(m_animationDisplayLetterEvery);
		}while(characDisplay < characTarget);
	}
}
