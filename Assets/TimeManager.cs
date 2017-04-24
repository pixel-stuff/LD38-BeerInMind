using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[System.Serializable]
public struct GameTime {
	public int day;
	public int hours;
	public int minutes;
}

public class TimeManager : MonoBehaviour {

	public static Action<GameTime> OnTicTriggered;
	public static Action m_DayEnding;
	public GameTime m_currentTime;
	public float realTime = 2.0f;
	private float currentRealTime; 
	public int gameTimeJump = 10;
	public static bool timePlay = false;
	public Text clockText;

	// Use this for initialization
	void Start () {
		m_currentTime.day = 0;
		m_currentTime.hours = 18;
		m_currentTime.minutes = 0;
		currentRealTime = realTime;
		//StartDay (); //-> Make the call from somewhere else

	}

	public void StartDay(){
		timePlay = true;

	}
	
	// Update is called once per frame
	void Update () {
		if (timePlay) {
			currentRealTime -= Time.deltaTime;
			if (currentRealTime < 0) {
				currentRealTime = realTime;
				m_currentTime.minutes += gameTimeJump;
				if (m_currentTime.minutes >= 60) {
					m_currentTime.hours++;
					m_currentTime.minutes -= 60;
					if (m_currentTime.hours > 24) {
						m_currentTime.day++;
						m_currentTime.hours = 18;
						m_currentTime.minutes = 0;
						EndOfday ();
					}
				}
				OnTicTriggered (m_currentTime);
			}
		}
		if (clockText) {
			clockText.text = m_currentTime.hours.ToString () + ':' + m_currentTime.minutes.ToString ();
			if (m_currentTime.minutes == 0) {
				clockText.text += '0';
			}
		}
	}


	public void EndOfday(){
		timePlay = false;
		if (m_DayEnding != null) {
			m_DayEnding ();
		}
		if (BarClosingEvent.m_mainTrigger != null) {
			BarClosingEvent.m_mainTrigger ();
		}
	}
}
