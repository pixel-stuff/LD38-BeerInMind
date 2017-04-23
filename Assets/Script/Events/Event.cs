using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Event : MonoBehaviour {

	public static Action m_mainTrigger;

	public void OnMouseUp()
	{
		if (m_mainTrigger != null) {
			m_mainTrigger ();
		}
		if (this.GetComponent<AudioSource> () != null) {
			this.GetComponent<AudioSource> ().Play ();
		}
	}
}
