using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tv : MonoBehaviour {

	public GameObject m_on;
	public GameObject m_off;

	// Use this for initialization
	void Start () {
		TVEvent.m_mainTrigger += SwitchTV;
		m_on.SetActive (false);
	}
	
	// Update is called once per frame
	public void SwitchTV (bool isOn) {
		if (isOn) {
			m_on.SetActive (true);
			m_off.SetActive (false);
		} else {
			m_on.SetActive (false);
			m_off.SetActive (true);
		}
	}

	void OnDestroy(){
		TVEvent.m_mainTrigger -= SwitchTV;
	}
}
