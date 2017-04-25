using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarmanBubble : MonoBehaviour {

	public Sprite m_hover;
	public Sprite m_clic;

	public void OnClick()
	{

		if (MainTalkManager.m_instance.m_isActivate || UIClickManager.m_instance.m_isActivate || IronCurtainManager.m_instance.m_isActivate)
			return;

		Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
	}


	public void OnOver()
	{
		if (MainTalkManager.m_instance.m_isActivate || UIClickManager.m_instance.m_isActivate || IronCurtainManager.m_instance.m_isActivate)
			return;

		Cursor.SetCursor (m_hover.texture, Vector2.zero, CursorMode.ForceSoftware);
	}

	public void OnNoOver()
	{
		Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
	}
}
