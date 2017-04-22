using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhisperTalkManager : MonoBehaviour {

	public TextMesh m_text;

	// Use this for initialization
	void Awake () {
		this.GetComponent<SpriteRenderer> ().enabled = false;
		m_text.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void StartDisplayWhisper(string txt, bool displayOnRight = true){
		m_text.gameObject.SetActive(true);
		this.GetComponent<SpriteRenderer> ().enabled = true;
		m_text.text = txt;
		if (!displayOnRight) {
			this.transform.localRotation = Quaternion.identity;
			m_text.transform.localRotation = Quaternion.identity;
			Vector3 pos = this.transform.localPosition;
			pos.x = -pos.x;
			this.transform.localPosition = pos;
			pos = m_text.transform.localPosition;
			pos.x = -pos.x;
			m_text.transform.localPosition = pos;
		}
	}
}
