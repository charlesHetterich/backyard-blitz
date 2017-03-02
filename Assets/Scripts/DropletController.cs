using UnityEngine;
using System.Collections;

public class DropletController : MonoBehaviour
{

    DropletInfoController info;

    // Use this for initialization
    void Start()
    {
        info = GetComponent<DropletInfoController>();
    }

    void Target(GameObject t)
    {
        info.ignoreType = t;
    }

    // Update is called once per frame
    void Update()
    {

        info.velocity -= 0.007f;

        transform.position = new Vector3(transform.position.x + Mathf.Cos(info.angle) * info.velocity, transform.position.y + Mathf.Sin(info.angle) * info.velocity, transform.position.z);
        transform.rotation = Quaternion.AngleAxis(info.angle * Mathf.Rad2Deg - 90f, Vector3.forward);

        //destroy when stopped
        if (info.velocity < 0f)
            GameObject.Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (info.ignoreType != null)
        {
            if (!(other.tag == "Player" && tag == "Player Bullet") && !(other.tag == "Enemy" && tag == "Enemy Bullet") && other.tag != "Player Bullet" && other.tag != "Enemy Bullet" && other.tag != "Room")
            {
                if (!(other.tag == "Locked Fence" && !other.GetComponent<LockedFenceController>().locked))
                GameObject.Destroy(this.gameObject);
                if (other.tag == "Player" || other.tag == "Enemy")
                    other.gameObject.GetComponent<HealthController>().LoseHealth(10f);
            }
            else if (!(other.tag.Equals(info.ignoreType.tag)) && (other.tag == "Player" || other.tag == "Enemy"))
            {
            }
        }
    }
}
