using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour
{
    public GameObject score;
    public GameObject pointRef;

    public float maxDryness = 100f;
    [HideInInspector]
    public float dryness;

    bool drying = false;
    bool hit = false;

    // Use this for initialization
    void Start()
    {
        score = GameObject.Find("score");

        dryness = maxDryness;
    }

    // Update is called once per frame
    void Update()
    {
        if (!drying)
        {
            drying = true;
            StartCoroutine("DryOff");
        }
    }

    public void LoseHealth(float n)
    {
        if (tag == "Player")
        {
            GameObject newPoint = Instantiate(pointRef);
            newPoint.GetComponentInChildren<PointController>().numPoints = -125;
            newPoint.transform.position = new Vector3(transform.position.x, transform.position.y, newPoint.transform.position.z);
            score.GetComponent<ScoreController>().score -= 125;
        }

        dryness -= n;
        hit = true;
        if (dryness <= 0)
        {
            if (this.gameObject.tag.Equals("Player"))
            {
                PlayerController.rating = 0;
                RoomController.numEnemies = 3;
                RoomController.numSprinklers = 1;
                Application.LoadLevel("GameOver");//can change to game over later
            }
            else
            {
                GameObject newPoint = Instantiate(pointRef);
                newPoint.GetComponentInChildren<PointController>().numPoints = 500;
                newPoint.transform.position = new Vector3(transform.position.x, transform.position.y, newPoint.transform.position.z);
                score.GetComponent<ScoreController>().score += 500;

                Destroy(this.gameObject);
            }
        }

        if (this.gameObject.tag.Equals("Player"))
        {
            PlayerController.rating += (maxDryness - dryness);
        }
    }
    IEnumerator DryOff()
    {
        yield return new WaitForSeconds(0.2f);
        if (!hit)
        {
            if (dryness < maxDryness)
            {
                dryness += 1f;
            }
        }
        else
        {
            hit = false;
        }
        drying = false;
    }
}
