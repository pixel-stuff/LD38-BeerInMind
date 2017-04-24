using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WhisperTalkManager : MonoBehaviour {

	public TextMesh m_text;
	public GameObject m_container;
	public GameObject m_arrow;
	public int m_tickAlive = 3;
	private int m_tickBeforeErase = 0;
	private Quaternion m_startContainerRotation;
	private Quaternion m_startTextRotation;

	public Action m_tickDisplayOver;
	// Use this for initialization
	void Awake () {
		m_startContainerRotation = m_container.transform.localRotation;
		m_startTextRotation = m_text.transform.localRotation;
		StopDisplayWhisper ();
		StartDisplayWhisper ("coucou",true);
		TimeManager.OnTicTriggered += TickHappen;
	}

	public void StartDisplayWhisper(string txt, bool displayOnRight = true){
		m_text.text = txt;
		m_tickBeforeErase = m_tickAlive;
		m_container.SetActive (true);
		if (!displayOnRight) {
			m_container.transform.localRotation = Quaternion.identity;
			m_text.transform.localRotation = Quaternion.identity;
			m_container.transform.localPosition = new Vector3 (-4.18f,0.39f,0.0f);
			m_arrow.transform.localPosition = new Vector3(1.06f,-0.09f,0.0f);
		} else {
			m_container.transform.localPosition = new Vector3 (-1.31f,0.39f,0.0f);
			m_container.transform.localRotation = Quaternion.identity;
			m_text.transform.localRotation = Quaternion.identity;
			m_arrow.transform.localPosition = new Vector3(-1.06f,-0.09f,0.0f);

			//m_container.transform.rotation = m_startContainerRotation;
			//m_text.transform.rotation = Quaternion.Euler(0,180,0);
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
