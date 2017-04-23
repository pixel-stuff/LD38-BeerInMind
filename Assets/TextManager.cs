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
			string str = parts[0];

			Node.eTextMiniType type = (Node.eTextMiniType)Enum.Parse(typeof(Node.eTextMiniType), str);
			txtStruc.m_textType = type;
			txtStruc.m_whisper = parts[1];
			txtStruc.m_mainTalk = parts[2];
			if (!m_dict.ContainsKey (type)) {
				m_dict.Add (type, new List<TextStruct> ());
			}

			m_dict [type].Add (txtStruc);
			//Debug.Log ("" + txtStruc.m_textType + " / " + txtStruc.m_whisper + " / " + txtStruc.m_mainTalk);
		}



		Debug.Log ("" + m_dict[Node.eTextMiniType.DRUNKGUY]);
	}

	public TextStruct GetTextStruc(Node.eTextMiniType typeText){
		TextStruct stru;
		List<TextStruct> list = m_dict [typeText];


		return list [0];
	}
}
