using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GunTextController : MonoBehaviour {

    public float riseSpeed;
    public float disapearSpeed;
    float alpha;

    public string gunText;

	// Use this for initialization
	void Start() {
	}
	
	// Update is called once per frame
	void Update () {

        GetComponent<Text>().text = gunText;

        transform.position = new Vector3(transform.position.x, transform.position.y + riseSpeed, transform.position.z);

        alpha -= disapearSpeed;
        if (alpha < 0)
            alpha = 0;

        GetComponent<Text>().color = new Color(GetComponent<Text>().color.r, GetComponent<Text>().color.g, GetComponent<Text>().color.b, alpha);
	}

    public void setText(float x, float y, string newText)
    {
        gunText = newText;
        transform.position = new Vector3(x, y, transform.position.z);
        alpha = 2;
    }
}