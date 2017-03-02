using UnityEngine;
using System.Collections;

public class LockedFenceController : MonoBehaviour {

    public bool locked = false;
    public bool permanentLock = false;
    public GameObject room;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (locked && GameObject.FindGameObjectsWithTag("Enemy").Length <= 0 && transform.position.x > room.transform.position.x + RoomDimensions.HALF_ROOM_WIDTH)
            locked = false;

        GetComponent<BoxCollider2D>().isTrigger = !(locked || permanentLock);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (locked || permanentLock) ? 1 : 0);
    }
}
