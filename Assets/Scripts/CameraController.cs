using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float screenShake = 0f;
    public Vector2 focus = new Vector2(0, 0);
    public Vector2 destFocus = new Vector2(0, 0);
    float goToFocusSpeed = 0.1f;

    float playerDamage = 0f;
    public float playerDestDamage = 0f;

    public GameObject backgroundMusic;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        //go to x
        if (focus.x < destFocus.x)
            focus.x += goToFocusSpeed;
        else if (focus.x > destFocus.x)
            focus.x -= goToFocusSpeed;
        if (Mathf.Abs(focus.x - destFocus.x) < goToFocusSpeed)
            focus.x = destFocus.x;

        //go to y
        if (focus.y < destFocus.y)
            focus.y += goToFocusSpeed;
        else if (focus.y > destFocus.y)
            focus.y -= goToFocusSpeed;
        if (Mathf.Abs(focus.y - destFocus.y) < goToFocusSpeed)
            focus.y = destFocus.y;

        if (screenShake > 0)
            screenShake -= 0.01f;
        else
            screenShake = 0f;

        transform.position = new Vector3(focus.x + (Random.Range(-100, 100) / 100f) * screenShake, focus.y + (Random.Range(-100, 100) / 100f) * screenShake, transform.position.z);

        if (screenShake > 0)
        {
            GetComponent<BlurOptimized>().enabled = true;
            GetComponent<BlurOptimized>().blurIterations = (int)(screenShake * 40);
        }
        else
            GetComponent<BlurOptimized>().enabled = false;

        //damage screen effect
        playerDamage += (playerDestDamage - playerDamage) / 10f;

        //pitch
        backgroundMusic.GetComponent<AudioSource>().pitch = 1 - playerDamage / 2.5f;

        //contrast
        GetComponent<ContrastEnhance>().intensity = playerDamage;

        //fish eye
        GetComponent<Fisheye>().strengthX = Mathf.Sin(Time.time) * (playerDamage / 20f);
        GetComponent<Fisheye>().strengthY = Mathf.Sin(Time.time) * (playerDamage / 20f);

        //twirl
        GetComponent<Twirl>().angle = playerDamage * 50f;
        GetComponent<Twirl>().center.x = 0.5f + Mathf.Cos(Time.time) * 0.1f;
        GetComponent<Twirl>().center.y = 0.5f + Mathf.Sin(Time.time) * 0.1f;
    }
}
