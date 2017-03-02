

using UnityEngine;
using System.Collections;
public class EnemyController : MonoBehaviour
{
    float maxDryness = 100f;
    float dryness;
    bool shootRoutine = false;

    public int gunType = 0;

    Vector2 velocity;
    Vector2 destVelocity;
    Vector2 targetDist;
    float currentMax;
    float maxSpeed;
    float maxSpeedRange = .65f;
    float minSpeedRange = .50f;
    float minRange = 1f;//for now
    float maxRange = 1f;//for now
    float minStray = 5f;
    float maxStray = 5f;
    public float minTime = 1.0f;
    public float maxTime = 3.0f;
    public int minBullets = 1;
    public int maxBullets = 3;

    public Sprite[] sprites; 

    // Use this for initialization
    void Start()
    {
        dryness = maxDryness;
        maxSpeed = Random.Range(minSpeedRange, maxSpeedRange);
        this.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
        //velocity = new Vector2(0f, 0f);//for stationary
        //acceleration = new Vector2(0f, 0f);//for stationary
    }
    public void SetStats(int adj)
    {
        if (adj > 0)
        {
            maxSpeedRange += (adj * 0.1f);
            maxRange += (adj * 0.2f);
            minStray -= (adj * 0.5f);
            if (minStray < 0f)
            {
                minStray = 0f;
            }
            minTime -= (adj * 0.1f);
            if (minTime < 0f)
            {
                minTime = 0f;
            }
        }
        else
        {
            minSpeedRange += (adj * 0.1f);
            if (minSpeedRange < 0f)
            {
                minSpeedRange = 0f;
            }
            minRange += (adj * 0.2f);
            if (minRange < .5f)
            {
                minRange = .5f;
            }
            maxStray -= (adj * 0.5f);
            maxTime -= (adj * 0.1f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        RotateEnemy();
        if (dryness <= 0f)
        {
            Destroy(this.gameObject);
        }
        targetDist = (Vector2)(GameObject.FindGameObjectWithTag("Player").transform.position - transform.position);
        if (!shootRoutine && targetDist.magnitude <= Random.Range(minRange, maxRange))
        {
            StartCoroutine("StartShooting");
            shootRoutine = true;
        }
        currentMax = (GetComponent<HealthController>().dryness / GetComponent<HealthController>().maxDryness) * maxSpeed;
        if (targetDist.magnitude > Random.Range(minRange, maxRange))
        {
            velocity = currentMax * targetDist / targetDist.magnitude;
        }
        else
        {
            velocity = -currentMax * targetDist / targetDist.magnitude;
        }
        GetComponent<Rigidbody2D>().velocity = velocity;
    }
    void RotateEnemy()
    {
        Vector3 currentPlayer = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 relPos = currentPlayer - transform.position;
        float angle = Mathf.Atan2(relPos.y, relPos.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //Quaternion rotation = Quaternion.LookRotation(relPos);
    }
    IEnumerator StartShooting()
    {
        yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        GetComponent<ShootController>().Shoot(GameObject.FindGameObjectWithTag("Player").transform.position, GunTypes.type[gunType].bullet, Random.Range(minStray, maxStray), .1f, Random.Range(minBullets, maxBullets + 1), "Enemy Bullet");
        shootRoutine = false;
    }
}