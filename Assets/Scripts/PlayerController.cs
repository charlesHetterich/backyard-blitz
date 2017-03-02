using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static float rating = 0f;

    GameObject dropletCount;
    public int currentAmmo = 0;
    float shootTimer = 0f;
    float shootWait = 0.1f;
    public int gunType = 2;
    public bool isInRoom = true;

    Vector2 velocity;
    Vector2 destVelocity;
    //Vector2 acceleration;
    float maxSpeed = 3f;
    float currentMax;

    float FRICTION = 0.1f;

    Vector3 lastMouse;

    // Use this for initialization
    void Start()
    {
        dropletCount = GameObject.Find("droplet_count");
        lastMouse = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        currentMax = (GetComponent<HealthController>().dryness / GetComponent<HealthController>().maxDryness) * maxSpeed;
        shootTimer += Time.deltaTime;


        RotatePlayer();

        //movement input
        if (isInRoom)
        {
            if (Input.GetMouseButton(0) && shootTimer > GunTypes.type[gunType].shootWait)
            {
                if (currentAmmo <= 0)
                    gunType = 0;
                if (gunType != 0)
                    currentAmmo -= GunTypes.type[gunType].numBullets;

                GetComponent<ShootController>().Shoot(Camera.main.ScreenToWorldPoint(Input.mousePosition), GunTypes.type[gunType].bullet, GunTypes.type[gunType].stray, GunTypes.type[gunType].shake, GunTypes.type[gunType].numBullets, "Player Bullet");
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x + Mathf.Cos((transform.eulerAngles.z - 90) * Mathf.Deg2Rad) * GunTypes.type[gunType].knockBack,
                                                                    GetComponent<Rigidbody2D>().velocity.y + Mathf.Sin((transform.eulerAngles.z - 90) * Mathf.Deg2Rad) * GunTypes.type[gunType].knockBack);

                dropletCount.GetComponent<DropletCountController>().shootBullets(GunTypes.type[gunType].numBullets);

                shootTimer = 0;
            }

            if (Input.GetKey(KeyCode.D))
            {
                destVelocity.x = currentMax;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                destVelocity.x = -currentMax;
            }
            else
                destVelocity.x = 0;

            if (Input.GetKey(KeyCode.W))
            {
                destVelocity.y = currentMax;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                destVelocity.y = -currentMax;
            }
            else
                destVelocity.y = 0;
        }
        else
        {
            if (transform.position.y > 3.2f)
                destVelocity.y = -currentMax;
            else if (transform.position.y < 2.8f)
                destVelocity.y = currentMax;
            else
                destVelocity.y = 0f;

            destVelocity.x = currentMax;
        }

        //movement by acceleration
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x + (destVelocity.x - GetComponent<Rigidbody2D>().velocity.x) / 5f,
                                                           GetComponent<Rigidbody2D>().velocity.y + (destVelocity.y - GetComponent<Rigidbody2D>().velocity.y) / 5f);
        velocity.x = velocity.x + (destVelocity.x - velocity.x) / 5f;
        velocity.y = velocity.y + (destVelocity.y - velocity.y) / 5f;

        //re-position player based on velocity
        //transform.position = new Vector3(transform.position.x + velocity.x, transform.position.y + velocity.y, transform.position.z);

        Camera.main.GetComponent<CameraController>().playerDestDamage = 1 - GetComponent<HealthController>().dryness / GetComponent<HealthController>().maxDryness;
    }

    void RotatePlayer()
    {
        Vector3 currentMouse = Input.mousePosition;
        lastMouse = currentMouse;
        Vector3 relPos = currentMouse - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(relPos.y, relPos.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //Quaternion rotation = Quaternion.LookRotation(relPos);
    }
}