using UnityEngine;
using System.Collections;

public class Gun
{
    public string name;

    public GameObject bullet;

    public float shootWait;
    public int numBullets;
    public float stray;
    public float shake;
    public float knockBack;
    public int ammo;
}

public static class GunTypes
{
    public static GameObject droplet;

    public static Gun[] type = {
        new Gun {name = "Water Pistol", shootWait = .4f, numBullets = 1, stray = 10f, shake = 0.1f, knockBack = 2f, ammo = 999},
        new Gun {name = "Rapid Water Gun", shootWait = 0.1f, numBullets = 1, stray = 10f, shake = 0.06f, knockBack = 2f, ammo = 100},
        new Gun {name = "Flash Flood", shootWait = 1f, numBullets = 15, stray = 20f, shake = 0.2f,   knockBack = 4f, ammo = 150},
        new Gun {name = "Hose", shootWait = 0.02f, numBullets = 1, stray = 10f, shake = 0.06f, knockBack = 3f, ammo = 150},
        new Gun {name = "Water Bomb", shootWait = 1.2f, numBullets = 30, stray = 180f, shake = 0.2f, knockBack = 3f, ammo = 600},
        new Gun {name = "Water Spear", shootWait = 1.2f, numBullets = 1, stray = 0f, shake = 0.1f, knockBack = 3f, ammo = 20},
        new Gun {name = "Water Balloon", shootWait = 1f, numBullets = 1, stray = 0f, shake = 0.2f, knockBack = 3f, ammo = 10},

    };
}

public class ShootController : MonoBehaviour
{

    public GameObject droplet;
    public GameObject dropletSpear;
    public GameObject waterBalloon;

    // Use this for initialization
    void Start()
    {
        GunTypes.type[0].bullet = droplet;
        GunTypes.type[1].bullet = droplet;
        GunTypes.type[2].bullet = droplet;
        GunTypes.type[3].bullet = droplet;
        GunTypes.type[4].bullet = droplet;
        GunTypes.type[5].bullet = dropletSpear;
        GunTypes.type[6].bullet = waterBalloon;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Shoot(Vector3 target, GameObject bullet, float stray, float shake, int bullets, string newTag)
    {
        if (Camera.main.GetComponent<CameraController>().screenShake < shake)
            Camera.main.GetComponent<CameraController>().screenShake += shake;
        Vector3 relPos = target - transform.position;
        for (int i = 0; i < bullets; i++)
        {
            GameObject newDroplet = Instantiate(bullet);
            newDroplet.GetComponent<DropletInfoController>().ignoreType = this.gameObject;
            newDroplet.transform.position = new Vector3(transform.position.x, transform.position.y, newDroplet.transform.position.z);
            newDroplet.GetComponent<DropletInfoController>().angle = (Mathf.Atan2(relPos.y, relPos.x) * Mathf.Rad2Deg + Random.Range(-stray, stray)) * Mathf.PI / 180;
            newDroplet.GetComponent<DropletInfoController>().velocity = Random.Range(0.2f - ((float)i * 0.01f), 0.2f);
            newDroplet.tag = newTag;
        }
    }
}
