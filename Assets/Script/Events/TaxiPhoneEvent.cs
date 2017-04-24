using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiPhoneEvent : Event {
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
