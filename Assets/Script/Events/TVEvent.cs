using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TVEvent : MonoBehaviour {
	public static Action<bool> m_mainTrigger;
	private bool m_tvIsOn = false;

	public AudioClip m_on;
	public AudioClip m_off;

	public void OnMouseUp()
	{
		m_tvIsOn = !m_tvIsOn;
		if (m_mainTrigger != null) {
			m_mainTrigger (m_tvIsOn);
		}
		if (m_tvIsOn) {
			this.GetComponent<AudioSource> ().clip = m_on;
		} else {
			this.GetComponent<AudioSource> ().clip = m_off;
		}
		this.GetComponent<AudioSource> ().Play();
	}
}
