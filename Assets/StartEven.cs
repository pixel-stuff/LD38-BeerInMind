using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StartEven : MonoBehaviour {
	public static Action m_startTrigger;
	public Sprite m_hover;
	public Sprite m_clic;

	public void OnMouseUp()
	{
		//Debug.Log ("JEREMY");
		//Debug.Log (MainTalkManager.m_instance.m_isActivate + " / " + UIClickManager.m_instance.m_isActivate + " / " + IronCurtainManager.m_instance.m_isActivate);
		//if (MainTalkManager.m_instance.m_isActivate || UIClickManager.m_instance.m_isActivate || IronCurtainManager.m_instance.m_isActivate || BarmanManager.m_instance.m_isActive)
		//	return;

		if (m_startTrigger != null) {
			m_startTrigger ();
			this.transform.parent.position = new Vector3 (100f, 100f, 100f);
		}
		if (this.GetComponent<AudioSource> () != null) {
			this.GetComponent<AudioSource> ().Play ();
		}
	}

	public void OnMouseDown()
	{
		//Debug.Log ("JEREMY");


		Cursor.SetCursor (m_clic.texture, Vector2.zero, CursorMode.ForceSoftware);
	}


	void OnMouseEnter()
	{
		//Debug.Log ("JEREMY");

		Cursor.SetCursor (m_hover.texture, Vector2.zero, CursorMode.ForceSoftware);
	}

	void OnMouseExit()
	{
	//	Debug.Log ("JEREMY");
		Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
	}
}
