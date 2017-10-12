using System.Collections; 
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PreBuild : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayerPrefs.DeleteAll();
        Directory.Delete(Application.dataPath + @"\Resources\Profile", true);
        Directory.Delete(Application.dataPath + @"\Resources\TempImages", true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
