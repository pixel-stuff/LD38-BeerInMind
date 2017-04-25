using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class KeysEvent : MonoBehaviour {
	public static Action m_mainTrigger;
	public Sprite m_hover;
	public Sprite m_clic;

	public void OnMouseUp()
	{
		//Debug.Log (MainTalkManager.m_instance.m_isActivate + " / " + UIClickManager.m_instance.m_isActivate + " / " + IronCurtainManager.m_instance.m_isActivate);
		if (MainTalkManager.m_instance.m_isActivate || UIClickManager.m_instance.m_isActivate || IronCurtainManager.m_instance.m_isActivate || BarmanManager.m_instance.m_isActive)
			return;

		if (m_mainTrigger != null) {
			m_mainTrigger ();
		}
		if (this.GetComponent<AudioSource> () != null) {
			this.GetComponent<AudioSource> ().Play ();
		}
		Cursor.SetCursor (m_hover.texture, Vector2.zero, CursorMode.ForceSoftware);
	}

	public void OnMouseDown()
	{

		if (MainTalkManager.m_instance.m_isActivate || UIClickManager.m_instance.m_isActivate || IronCurtainManager.m_instance.m_isActivate || BarmanManager.m_instance.m_isActive)
			return;

		Cursor.SetCursor (m_clic.texture, Vector2.zero, CursorMode.ForceSoftware);
	}


	void OnMouseEnter()
	{
		if (MainTalkManager.m_instance.m_isActivate || UIClickManager.m_instance.m_isActivate || IronCurtainManager.m_instance.m_isActivate || BarmanManager.m_instance.m_isActive)
			return;

		Cursor.SetCursor (m_hover.texture, Vector2.zero, CursorMode.ForceSoftware);
	}

	void OnMouseExit()
	{
		Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
	}
}
