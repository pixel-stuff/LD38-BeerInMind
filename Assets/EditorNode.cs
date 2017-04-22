using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct EditorNode {
	public string name;
	public string text;
	public string completedText;
	public int lifetime;
	public EditorEdge[] edges;
}


