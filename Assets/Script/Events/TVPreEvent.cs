using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class TVPreEvent : MonoBehaviour {

	public static Action m_mainTrigger;
	public Sprite m_hover;
	public Sprite m_clic;

	public void OnMouseUp()
	{

		if (MainTalkManager.m_instance.m_isActivate || UIClickManager.m_instance.m_isActivate || IronCurtainManager.m_instance.m_isActivate || BarmanManager.m_instance.m_isActive)
			return;
		
		UIClickManager.m_instance.StartRemoteApparition ();
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
