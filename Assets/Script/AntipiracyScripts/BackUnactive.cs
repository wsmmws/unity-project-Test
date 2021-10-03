using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackUnactive : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick(){
		PlayerPrefs.SetInt ("ActiveState", 0);
		Debug.Log (PlayerPrefs.GetInt ("ActiveState") );
	}
}
