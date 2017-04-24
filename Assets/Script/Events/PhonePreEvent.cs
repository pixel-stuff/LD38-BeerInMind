using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class PhonePreEvent : MonoBehaviour {

	public static Action m_mainTrigger;
	public Sprite m_hover;
	public Sprite m_clic;

	public void OnMouseUp()
	{

		if (IronCurtainManager.m_instance.m_isActivate || UIClickManager.m_instance.m_isActivate || IronCurtainManager.m_instance.m_isActivate)
			return;
		
		UIClickManager.m_instance.StartPhoneApparition ();
		Cursor.SetCursor (m_hover.texture, Vector2.zero, CursorMode.ForceSoftware);
	}

	public void OnMouseDown()
	{

		if (IronCurtainManager.m_instance.m_isActivate || UIClickManager.m_instance.m_isActivate || IronCurtainManager.m_instance.m_isActivate)
			return;

		Cursor.SetCursor (m_clic.texture, Vector2.zero, CursorMode.ForceSoftware);
	}


	void OnMouseEnter()
	{
		if (IronCurtainManager.m_instance.m_isActivate || UIClickManager.m_instance.m_isActivate || IronCurtainManager.m_instance.m_isActivate)
			return;

		Cursor.SetCursor (m_hover.texture, Vector2.zero, CursorMode.ForceSoftware);
	}

	void OnMouseExit()
	{
		Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
	}
}
