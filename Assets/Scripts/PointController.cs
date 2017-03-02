using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointController : MonoBehaviour {

    public int numPoints;
    bool fading = false;

	// Use this for initialization
	void Start () {
        Invoke("selfDestruct", 1f);
	}
	
	// Update is called once per frame
	void Update () {

        GetComponent<Text>().text = numPoints.ToString();
        if (numPoints > 0)
            GetComponent<Text>().text = "+" + GetComponent<Text>().text;

        transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);

        transform.localScale = new Vector3(transform.localScale.x, 1.25f + Mathf.Sin((int)((Time.time * 1000)) * Mathf.Deg2Rad) * 0.25f);

       // if (fading)
         //   1;// transform.localScale
    }

    void startFading()
    {
        fading = true;
    }

    void selfDestruct()
    {
        GameObject.Destroy(this.gameObject);
    }
}
