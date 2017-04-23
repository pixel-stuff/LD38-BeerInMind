using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraughtEvent : Event {

	public Sprite m_animatedSprite;
	public Sprite m_idleSprite;

	public void DisplayAnimatedSprite(){
		this.GetComponent<Image> ().sprite = m_animatedSprite;
	}

	public void DisplayIdleSprite(){
		this.GetComponent<Image> ().sprite = m_idleSprite;
	}
}
