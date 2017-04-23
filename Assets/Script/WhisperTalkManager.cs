using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WhisperTalkManager : MonoBehaviour {

	public TextMesh m_text;
	public int m_tickAlive = 3;
	private int m_tickBeforeErase = 0;

	public Action m_tickDisplayOver;
	// Use this for initialization
	void Awake () {
		StopDisplayWhisper ();
		TimeManager.OnTicTriggered += TickHappen;
	}

	public void StartDisplayWhisper(string txt, bool displayOnRight = true){
		m_text.gameObject.SetActive(true);
		this.GetComponent<SpriteRenderer> ().enabled = true;
		m_text.text = txt;
		m_tickBeforeErase = m_tickAlive;
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

	public void StopDisplayWhisper(){
		m_text.gameObject.SetActive(false);
		this.GetComponent<SpriteRenderer> ().enabled = false;
		m_text.text = "";
	}

	public void TickHappen(GameTime gt){
		m_tickBeforeErase--;
		if (m_tickBeforeErase <= 0) {
			StopDisplayWhisper ();
			if (m_tickDisplayOver != null) {
				m_tickDisplayOver();
			}
		}
	}

	public void OnDestroy(){
		TimeManager.OnTicTriggered -= TickHappen;
	}
}
