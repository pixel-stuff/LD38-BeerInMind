using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public struct EditorEdge {
	public Test[] condition;
	public EditorNode targetNode;

}

public enum Test{
	lifeEnd,
	toto,
	titi
}