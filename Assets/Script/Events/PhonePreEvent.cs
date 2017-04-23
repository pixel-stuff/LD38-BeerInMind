using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class PhonePreEvent : MonoBehaviour {

	public static Action m_mainTrigger;

	public void OnMouseUp()
	{
		UIClickManager.m_instance.StartPhoneApparition ();
	}
}
