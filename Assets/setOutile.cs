using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setOutile : MonoBehaviour {

	// Use this for initialization
	void Start () {
			MaterialPropertyBlock mpb = new MaterialPropertyBlock();
			this.GetComponent<SpriteRenderer>().GetPropertyBlock(mpb);
			mpb.SetFloat("_Outline", true ? 1f : 0);
			mpb.SetColor("_OutlineColor", Color.white);
			this.GetComponent<SpriteRenderer>().SetPropertyBlock(mpb);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
