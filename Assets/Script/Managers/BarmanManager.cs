using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BarmanManager : MonoBehaviour {

	#region Singleton
	public static BarmanManager m_instance;
	void Awake(){
		if(m_instance == null){
			//If I am the first instance, make me the Singleton
			m_instance = this;
			DontDestroyOnLoad(this.gameObject);
		}else{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != m_instance)
				Destroy(this.gameObject);
		}
	}
	#endregion Singleton


	public Action<string> Answer; //renvoyer le text cliquer

	public void Says(string answer1, string answer2){
		
	}

	public void Dismiss(){
	
	}
}
