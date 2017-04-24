using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClickManager : MonoBehaviour {

	public GameObject m_UIDraugh;
	public GameObject m_UIRemote;
	public GameObject m_UIPhone;
	public GameObject m_back;
	public bool m_isActivate = false;

	public static UIClickManager m_instance;
	// Use this for initialization
	void Awake () {
		if(m_instance == null){
			m_instance = this;
			m_back.SetActive (false);
		}else{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != m_instance)
				Destroy(this.gameObject);
		}
	}

	public void StartDraughApparition(){
		m_back.SetActive (true);
		m_isActivate = true;
		m_UIDraugh.GetComponent<Animation>().Play("DraughApparition");
	}

	public void StartPhoneApparition(){
		m_back.SetActive (true);
		m_isActivate = true;
		m_UIPhone.GetComponent<Animation>().Play("PhoneApparition");
	}

	public void StartRemoteApparition(){
		m_back.SetActive (true);
		m_isActivate = true;
		m_UIRemote.GetComponent<Animation>().Play("RemoteApparition");
	}

	public void StartDraughRemove(){
		m_back.SetActive (false);
		m_isActivate = false;
		m_UIDraugh.GetComponent<Animation>().Play("DraughRemove");
	}

	public void StartPhoneRemove(){
		m_back.SetActive (false);
		m_isActivate = false;
		m_UIPhone.GetComponent<Animation>().Play("PhoneRemove");
	}

	public void StartRemoteRemove(){
		m_back.SetActive (false);
		m_isActivate = false;
		m_UIRemote.GetComponent<Animation>().Play("RemoteRemove");
	}

	public void StopAnimations(){
		m_back.SetActive (false);
		m_UIDraugh.GetComponent<Animation> ().Stop ();
		m_UIPhone.GetComponent<Animation> ().Stop ();
		m_UIRemote.GetComponent<Animation> ().Stop ();
		m_UIDraugh.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
		m_UIPhone.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
		m_UIRemote.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
		StartCoroutine (CoolDownControl ());
	}

	public IEnumerator CoolDownControl(){
		yield return new WaitForSeconds (0.5f);
		m_isActivate = false;
	}
}
