using UnityEngine;
using System.Collections;

public class CreditController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.C))
        {
            Application.LoadLevel("TitleScreen");
        }
    }
}

