using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {
            Application.LoadLevel("scene");
        }
        else if (Input.GetKey(KeyCode.C))
        {
            Application.LoadLevel("Credits");
        }
    }
}
