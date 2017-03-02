using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

    public GameObject room;
    public GameObject player;

	// Use this for initialization
	void Start () {

        //create player and first room
        GameObject newPlayer = (GameObject)Instantiate(player);
        newPlayer.transform.localPosition = new Vector3(2, 2, newPlayer.transform.position.z);
        GameObject newRoom = (GameObject)Instantiate(room);
        newRoom.transform.localPosition = new Vector3(0, 0, newRoom.transform.position.z);
        newRoom.GetComponent<RoomController>().genLeftRoom();
        newRoom.GetComponent<RoomController>().roomReference = room;
        newRoom.GetComponent<RoomController>().player = newPlayer;
        newRoom.GetComponent<RoomController>().thisBackground = (GameObject)Instantiate(newRoom.GetComponent<RoomController>().backgrounds[Random.Range(0, newRoom.GetComponent<RoomController>().backgrounds.Length)], transform);

        //re position camera on center of initial room
        Camera.main.GetComponent<CameraController>().focus = new Vector2(newRoom.transform.position.x + RoomDimensions.HALF_ROOM_WIDTH, newRoom.transform.position.y + RoomDimensions.HALF_ROOM_HEIGHT);
        Camera.main.GetComponent<CameraController>().destFocus = Camera.main.GetComponent<CameraController>().focus;
    }

    // Update is called once per frame
    void Update () {
	
	}
}