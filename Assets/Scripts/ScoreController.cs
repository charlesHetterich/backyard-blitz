using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController : MonoBehaviour {

    public int score = 0;
    int shownScore = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        if (score < 0)
            score = 0;

        if (Input.GetKeyDown(KeyCode.I))
        {
            score += Random.Range(100, 1000);
        }

        shownScore += (score - shownScore) / 10;

        if ((score - shownScore) / 10 == 0)
            shownScore = score;

        GetComponent<Text>().text = shownScore.ToString();
        while (GetComponent<Text>().text.Length < 10f)
        {
            GetComponent<Text>().text = "0" + GetComponent<Text>().text;
        }

        transform.localScale = new Vector3(1f, 1f + (score - shownScore) / 600f, transform.localScale.z);

    }
}
