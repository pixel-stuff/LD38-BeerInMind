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

	public static TimeManager m_instance;
	// Use this for initialization
	void Awake () {
		if(m_instance == null){
			m_instance = this;
		}else{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != m_instance)
				Destroy(this.gameObject);
		}
	}


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
		m_currentTime.hours = 17;
		m_currentTime.minutes = 50;
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
						m_currentTime.hours = 17;
						m_currentTime.minutes = 50;
						EndOfday ();
					}
				}
				OnTicTriggered (m_currentTime);
			}
		}
		if (clockText) {
			if (m_currentTime.hours >= 18) {
				string hoursString = m_currentTime.hours.ToString ();
				if (m_currentTime.hours == 24) {
					hoursString = "00";
				}
				clockText.text = hoursString + ':' + m_currentTime.minutes.ToString ();
				if (m_currentTime.minutes == 0) {
					clockText.text += '0';
				}
			} else {
				clockText.text = ""; 
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


	public bool skipClicked = false;
	private float previousRealTime = 0.0f;
	public void SkipMonday(){
		skipClicked = true;
		previousRealTime = realTime;
		realTime = 0.2f;
	}

	public void ResetRealTimeToNormal(){
		skipClicked = false;
		realTime = previousRealTime;
	}
}
