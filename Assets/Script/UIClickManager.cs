using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClickManager : MonoBehaviour {

	public GameObject m_UIDraugh;
	public GameObject m_UIRemote;
	public GameObject m_UIPhone;

	public static UIClickManager m_instance;
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
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartDraughApparition(){
		m_UIDraugh.GetComponent<Animation>().Play("DraughApparition");
	}

	public void StartPhoneApparition(){
		m_UIPhone.GetComponent<Animation>().Play("PhoneApparition");
	}

	public void StartRemoteApparition(){
		m_UIRemote.GetComponent<Animation>().Play("RemoteApparition");
	}

	public void StartDraughRemove(){
		m_UIDraugh.GetComponent<Animation>().Play("DraughRemove");
	}

	public void StartPhoneRemove(){
		m_UIPhone.GetComponent<Animation>().Play("PhoneRemove");
	}

	public void StartRemoteRemove(){
		m_UIRemote.GetComponent<Animation>().Play("RemoteRemove");
	}
}
