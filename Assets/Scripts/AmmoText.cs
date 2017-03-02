using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class AmmoText : MonoBehaviour
{
    float shownAmmo = 0f;

    //GameObject player;
    // Use this for initialization
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GameObject.FindGameObjectWithTag("Player"));
        int display = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().currentAmmo;
        if (display <= 0 || display == 999)
        {
            GetComponent<Text>().text = "---";
        }
        else
        {
            if ((int)shownAmmo > display)
            {
                shownAmmo += (display - shownAmmo) / 10;

                if ((display - shownAmmo) / 10 == 0)
                    shownAmmo = display;
            }
            else
                shownAmmo = display;

            GetComponent<Text>().text = ((int)shownAmmo).ToString();
            while (GetComponent<Text>().text.Length < 3f)
            {
                GetComponent<Text>().text = "0" + GetComponent<Text>().text;
            }

            transform.localScale = new Vector3(1f, 1f + (shownAmmo - display) / 20f, transform.localScale.z);
        }
    }
}


