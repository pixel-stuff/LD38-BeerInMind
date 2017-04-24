using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class TVPreEvent : MonoBehaviour {

	public static Action m_mainTrigger;
	public Texture2D m_hover;

	public void OnMouseUp()
	{

		if (IronCurtainManager.m_instance.m_isActivate || UIClickManager.m_instance.m_isActivate || IronCurtainManager.m_instance.m_isActivate)
			return;
		
		UIClickManager.m_instance.StartRemoteApparition ();
	}

	void OnMouseEnter()
	{
		//Cursor.SetCursor (m_hover, Vector2.zero, CursorMode.Auto);
	}

	void OnMouseExit()
	{
		//Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}

}
