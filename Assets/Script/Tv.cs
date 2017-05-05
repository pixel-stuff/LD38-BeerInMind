using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tv : MonoBehaviour {

	public GameObject m_on;
	public GameObject m_off;
	public bool state;
	public bool stateAtTheStartOfTheDay;

	// Use this for initialization
	void Start () {
		TVEvent.m_mainTrigger += SwitchTV;
		TimeManager.m_DayEnding += OnDayChanged;
		IronCurtainManager.OnDayRestart += OnDayRestart;
		state = false;
		m_on.SetActive (false);
	}
	
	// Update is called once per frame
	public void SwitchTV (bool isOn) {
		if (isOn) {
			m_on.SetActive (true);
			m_off.SetActive (false);
			state = true;
		} else {
			m_on.SetActive (false);
			m_off.SetActive (true);
			state = false;
		}
	}

	void OnDestroy(){
		TVEvent.m_mainTrigger -= SwitchTV;
		TimeManager.m_DayEnding -= OnDayChanged;
		IronCurtainManager.OnDayRestart -= OnDayRestart;
	}

	void OnDayChanged(){
		stateAtTheStartOfTheDay = state;
	}

	void OnDayRestart(){
		SwitchTV (stateAtTheStartOfTheDay);
	}
}
