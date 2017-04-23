using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct TextStruct{
	public Node.eTextMiniType m_textType;
	public string m_whisper;
	public string m_mainTalk;
}

public class TextManager : MonoBehaviour {

	public TextAsset m_dialogues;
	private Dictionary<Node.eTextMiniType,List<TextStruct>> m_dict;

	public static TextManager m_instance;
	void Awake(){
		if(m_instance == null){
			m_instance = this;
			Init ();
		}else{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != m_instance)
				Destroy(this.gameObject);
		}
	}

	// Use this for initialization
	public void Init () {
		m_dict = new Dictionary<Node.eTextMiniType, List<TextStruct>> ();
		ParseCSV ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ParseCSV(){

		string[] lines = m_dialogues.text.Split('\n');

		//Debug.Log ("START READING FILE");
		foreach (string line in lines) {

			string[] parts = line.Split (';');

			TextStruct txtStruc = new TextStruct ();
			if (parts.Length > 0 && !parts[0].Equals("")) {
				txtStruc.m_textType = (Node.eTextMiniType)Enum.Parse (typeof(Node.eTextMiniType), parts [0]);
				txtStruc.m_whisper = parts [1];
				if (parts.Length > 2) {
					txtStruc.m_mainTalk = parts [2];
				} else {
					txtStruc.m_mainTalk = "";
				}
				if (!m_dict.ContainsKey (txtStruc.m_textType)) {
					m_dict.Add (txtStruc.m_textType, new List<TextStruct> ());
				}

				m_dict [txtStruc.m_textType].Add (txtStruc);
			}
			//Debug.Log ("" + txtStruc.m_textType + " / " + txtStruc.m_whisper + " / " + txtStruc.m_mainTalk);
		}
	}

	public TextStruct GetTextStruc(Node.eTextMiniType typeText){
		TextStruct stru;
		List<TextStruct> list = m_dict [typeText];

		int ran = UnityEngine.Random.Range (0, list.Count - 1);
		return list [ran];
	}
}
