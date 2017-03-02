using UnityEngine;
using System.Collections;
public class SprinklerController : MonoBehaviour
{
    bool shootRoutine = false;
    bool rotateRoutine = false;
    public float range = .5f;
    public float delay = .1f;//arbitrary
    public float rotateAmount = Mathf.PI / 8; //radians
    float angle = 0f;
    Vector2 targetDist;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(angle);
        if (!shootRoutine)
        {
            shootRoutine = true;
            StartCoroutine("StartShooting");
        }
        if (!rotateRoutine)
        {
            rotateRoutine = true;
            StartCoroutine("StartRotating");
        }

        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg - 90f, Vector3.forward);
    }
    IEnumerator StartShooting()
    {
        yield return new WaitForSeconds(delay);
        Vector2 target = (Vector2)transform.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        GetComponent<ShootController>().Shoot(target, GunTypes.type[0].bullet, 0f, 0f, 1, "Enemy Bullet");
        shootRoutine = false;
    }
    IEnumerator StartRotating()
    {
        //Debug.Log("Here!");
        yield return new WaitForSeconds(delay);
        angle = (angle + rotateAmount) % (2f * Mathf.PI);
        rotateRoutine = false;
    }
}


