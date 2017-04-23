﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct GameTime {
	public int day;
	public int hours;
	public int minutes;
}

public class TimeManager : MonoBehaviour {

	public static System.Action<GameTime> OnTicTriggered;
	public GameTime m_currentTime;
	public float realTime = 2.0f;
	private float currentRealTime; 
	public int gameTimeJump = 10;
	bool timePlay = true;
	public Text clockText;

	// Use this for initialization
	void Start () {
		m_currentTime.day = 0;
		m_currentTime.hours = 18;
		m_currentTime.minutes = 0;
		currentRealTime = realTime;
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
						timePlay = false;
						//TODO GONEXTDAY
					}
					OnTicTriggered (m_currentTime);
				}
			}
		}
		if (clockText) {
			clockText.text = m_currentTime.hours.ToString () + ':' + m_currentTime.minutes.ToString ();
			if (m_currentTime.minutes == 0) {
				clockText.text += '0';
			}
		}
	}
}
