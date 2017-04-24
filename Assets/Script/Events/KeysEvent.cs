using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class KeysEvent : MonoBehaviour {
	public static Action m_mainTrigger;

	public void OnMouseUp()
	{
		//Debug.Log (MainTalkManager.m_instance.m_isActivate + " / " + UIClickManager.m_instance.m_isActivate + " / " + IronCurtainManager.m_instance.m_isActivate);
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
