using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WhisperTalkManager : MonoBehaviour {

	public TextMesh m_text;
	public GameObject m_container;
	public int m_tickAlive = 3;
	private int m_tickBeforeErase = 0;

	public Action m_tickDisplayOver;
	// Use this for initialization
	void Awake () {
		StopDisplayWhisper ();
		StartDisplayWhisper ("bfsbrv",true);
		TimeManager.OnTicTriggered += TickHappen;
	}

	public void StartDisplayWhisper(string txt, bool displayOnRight = true){
		Debug.Log ("");
		m_text.text = txt;
		m_tickBeforeErase = m_tickAlive;
		m_container.SetActive (true);
		if (!displayOnRight) {
			m_container.transform.localRotation = Quaternion.identity;
			m_text.transform.localRotation = Quaternion.identity;
			m_container.transform.localPosition = new Vector3 (-4.18f,0.39f,0.0f);
		} else {
			m_container.transform.localPosition = new Vector3 (-0.42f,0.39f,0.0f);
			Vector3 rot = m_container.transform.rotation.eulerAngles;
			rot = new Vector3(rot.x,-180f,rot.z);
			m_container.transform.rotation = Quaternion.Euler(rot);
			rot = new Vector3(rot.x,180f,rot.z);
			m_text.transform.rotation = Quaternion.Euler(rot);
		}
	}

	public void StopDisplayWhisper(){
		m_text.text = "";
		m_container.SetActive (false);
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
