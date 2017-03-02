using UnityEngine;
using System.Collections;

public class DropletCountController : MonoBehaviour {

    public GameObject droplet;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	


	}

    public void shootBullets(int numBullets)
    {
        for (int i = 0; i < numBullets; i++)
        {
            GameObject newDroplet = (GameObject)Instantiate(droplet, Camera.main.transform);
            newDroplet.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);

            float lightnes = Random.Range(0.5f, 0.9f);
            float angle = (Random.Range(-25f, 25f) + 90f) * Mathf.Deg2Rad;
            float force = Random.Range(100f, 150f);

            newDroplet.GetComponent<SpriteRenderer>().color = new Color(lightnes, lightnes, lightnes, 1f);
            newDroplet.GetComponent<Rigidbody2D>().AddForce(new Vector2(force * Mathf.Cos(angle), force * Mathf.Sin(angle)));
            newDroplet.GetComponent<Rigidbody2D>().AddTorque(Random.Range(0, 500));
        }
    }
}
