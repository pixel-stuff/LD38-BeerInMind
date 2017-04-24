using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FreeBeerEvent : MonoBehaviour {
	public static Action m_mainTrigger;

	public void OnMouseUp()
	{
		if (MainTalkManager.m_instance.m_isActivate || UIClickManager.m_instance.m_isActivate || IronCurtainManager.m_instance.m_isActivate)
			return;
		
		if (m_mainTrigger != null) {
			m_mainTrigger ();
		}
		if (this.GetComponent<AudioSource> () != null) {
			this.GetComponent<AudioSource> ().Play ();
		}
	}
}
