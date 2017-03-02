using UnityEngine;
using System.Collections;

public class CrateController : MonoBehaviour {

    GameObject gunText;

	// Use this for initialization
	void Start () {
        gunText = GameObject.Find("GunText");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                int newGunType = Random.Range(1, GunTypes.type.Length);
                while (newGunType == other.GetComponent<PlayerController>().gunType)
                    newGunType = Random.Range(1, GunTypes.type.Length);

                other.GetComponent<PlayerController>().gunType = newGunType;
                other.GetComponent<PlayerController>().currentAmmo = GunTypes.type[newGunType].ammo;

                gunText.GetComponent<GunTextController>().setText(transform.position.x, transform.position.y, GunTypes.type[newGunType].name);

                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
