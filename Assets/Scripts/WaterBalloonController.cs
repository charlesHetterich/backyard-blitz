using UnityEngine;
using System.Collections;

public class WaterBalloonController : MonoBehaviour {

    DropletInfoController info;
    public GameObject droplet;
    public int numBullets;

    // Use this for initialization
    void Start()
    {
        info = GetComponent<DropletInfoController>();
        transform.localScale = new Vector3(2.5f, 2.5f);
    }

    void Target(GameObject t)
    {
        info.ignoreType = t;
    }

    // Update is called once per frame
    void Update()
    {

        info.velocity -= 0.0045f;

        transform.position = new Vector3(transform.position.x + Mathf.Cos(info.angle) * info.velocity / 2f, transform.position.y + Mathf.Sin(info.angle) * info.velocity / 2f, transform.position.z);
        transform.rotation = Quaternion.AngleAxis(info.angle * Mathf.Rad2Deg, Vector3.forward);

        //destroy when stopped
        if (info.velocity < 0f)
        {
            explode();
            GameObject.Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (info.ignoreType != null)
        {
            if (!(other.tag == "Player" && tag == "Player Bullet") && !(other.tag == "Enemy" && tag == "Enemy Bullet") && other.tag != "Player Bullet" && other.tag != "Enemy Bullet" && other.tag != "Room")
            {
                if (!(other.tag == "Locked Fence" && !other.GetComponent<LockedFenceController>().locked))
                {
                    explode();
                    GameObject.Destroy(this.gameObject);
                }
                if (other.tag == "Player" || other.tag == "Enemy")
                    other.gameObject.GetComponent<HealthController>().LoseHealth(10f);
            }
            else if (!(other.tag.Equals(info.ignoreType.tag)) && (other.tag == "Player" || other.tag == "Enemy"))
            {
            }
        }
    }

    void explode()
    {
        Camera.main.GetComponent<CameraController>().screenShake += 0.2f;

        if (Camera.main.GetComponent<CameraController>().screenShake > 0.2f)
            Camera.main.GetComponent<CameraController>().screenShake = 0.2f;

        //Vector3 relPos = target - transform.position;
        for (int i = 0; i < numBullets; i++)
        {
            GameObject newDroplet = Instantiate(GunTypes.type[0].bullet);
            newDroplet.GetComponent<DropletInfoController>().ignoreType = GameObject.FindGameObjectWithTag("Player");
            newDroplet.transform.position = new Vector3(transform.position.x, transform.position.y, newDroplet.transform.position.z);
            newDroplet.GetComponent<DropletInfoController>().angle = (i == 0) ? 0 : (float)((float)numBullets / i) * 360f;
            newDroplet.GetComponent<DropletInfoController>().velocity = Random.Range(0.2f - ((float)i * 0.005f), 0.2f);
            newDroplet.tag = tag;
        }
    }
}
