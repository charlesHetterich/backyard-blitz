using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("loadGame");
	}
	
	// Update is called once per frame
	void Update () {
        StartCoroutine("loadGame");
    }
    IEnumerator loadGame()
    {
        yield return new WaitForSeconds(2.0f);
        Debug.Log("hey");
        Application.LoadLevel("TitleScreen");
    }
}
