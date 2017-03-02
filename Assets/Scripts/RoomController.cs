using UnityEngine;
using System.Collections;

public static class TileDimensions
{
    public static float PIXEL_SIZE = 32;
    public static float PIXELS_PER_UNIT = 50;
    public static float SIZE = PIXEL_SIZE / PIXELS_PER_UNIT;
}

public static class FenceDimensions
{
    public static float PIXEL_SIZE = 5;
    public static float PIXELS_PER_UNIT = 50;
    public static float SIZE = PIXEL_SIZE / PIXELS_PER_UNIT;
}

public static class RoomDimensions
{
    public static int BOARD_WIDTH = 17;
    public static int BOARD_HEIGHT = 10;
    public static float HALF_ROOM_WIDTH = (BOARD_WIDTH * TileDimensions.SIZE) / 2f;
    public static float HALF_ROOM_HEIGHT = (BOARD_HEIGHT * TileDimensions.SIZE) / 2f;
}

public class RoomController : MonoBehaviour {

    public GameObject roomReference;

    public GameObject player;

    public GameObject[] grass;
    public GameObject[] fence;
    public GameObject[] lockedFence;
    public GameObject enemy;
    public GameObject sprinkler;
    public GameObject[] props;
    public int roomType;
    bool hasBeenEntered = false;
    bool hasBeenExited = false;

    public GameObject[] backgrounds;
    public GameObject thisBackground;

    //crates
    public GameObject crate;
    bool spawnedCrate = false;

    int adj = 0;
    public static int numEnemies = 3 - 2;//2 less than actual value
    public static int numSprinklers = 1;//can be changed
    public static int numProps = 1;


    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	    if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0 && player.GetComponent<PlayerController>().isInRoom && !spawnedCrate)
        {
            spawnedCrate = true;
            GameObject newCrate = (GameObject)Instantiate(crate, transform);
            newCrate.transform.localPosition = new Vector3(RoomDimensions.HALF_ROOM_WIDTH,
                                                           RoomDimensions.HALF_ROOM_HEIGHT,
                                                           newCrate.transform.position.z);
        }
	}

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && other.transform.position.x > (transform.position.x + RoomDimensions.HALF_ROOM_WIDTH) && !hasBeenExited)
        {
            other.GetComponent<PlayerController>().isInRoom = false;
            hasBeenExited = true;
            GameObject[] myFences = GameObject.FindGameObjectsWithTag("Locked Fence");
            for (int i = 0; i < myFences.Length; i++)
                myFences[i].GetComponent<LockedFenceController>().permanentLock = true;
            Camera.main.GetComponent<CameraController>().destFocus = new Vector2(Camera.main.GetComponent<CameraController>().destFocus.x + 20, Camera.main.GetComponent<CameraController>().destFocus.y);
            GameObject newRoom = (GameObject)Instantiate(roomReference);
            newRoom.transform.localPosition = new Vector3(transform.position.x + 20, 0, newRoom.transform.position.z);
            newRoom.GetComponent<RoomController>().genMiddleRoom();
            newRoom.GetComponent<RoomController>().roomReference = roomReference;
            newRoom.GetComponent<RoomController>().player = player;
            newRoom.GetComponent<RoomController>().thisBackground = (GameObject)Instantiate(backgrounds[Random.Range(0, backgrounds.Length)], newRoom.transform);
            newRoom.GetComponent<RoomController>().thisBackground.transform.localPosition = new Vector3(8.5f, 3f, 10f);

            thisBackground.transform.position = new Vector3(thisBackground.transform.position.x, thisBackground.transform.position.y, thisBackground.transform.position.z + 1);
            Invoke("selfDestruct", 5f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !hasBeenEntered)
        {
            adj = DifficultyCalc(PlayerController.rating);
            if (adj < -2)
            {
                adj = -2;
            }
            numEnemies = numEnemies + adj;
            if (adj > 0)
            {
                if (Random.Range(1, 10) < adj)
                {
                    numSprinklers++;
                    if (numSprinklers > 5)
                    {
                        numSprinklers = 5;
                    }
                }
            }
            else
            {
                if (Random.Range(1, 10) < -adj)
                {
                    numSprinklers--;
                    if (numSprinklers < 0)
                    {
                        numSprinklers = 0;
                    }
                }
            }

            if (numEnemies <= 0)
            {
                numEnemies = 1;
            }
            if (numEnemies > 22)
            {
                numEnemies = 22;//max number of enemies
            }


            other.GetComponent<PlayerController>().isInRoom = true;
            hasBeenEntered = true;
            Camera.main.GetComponent<CameraController>().destFocus = new Vector2(transform.position.x + RoomDimensions.HALF_ROOM_WIDTH, Camera.main.GetComponent<CameraController>().destFocus.y);
            GameObject[] allLockedFences = GameObject.FindGameObjectsWithTag("Locked Fence");
            for (int i = 0; i < allLockedFences.Length; i++)
            {
                allLockedFences[i].GetComponent<LockedFenceController>().locked = true;
            }

            int[][] spawn = new int[RoomDimensions.BOARD_HEIGHT][];
            for (int i = 0; i < RoomDimensions.BOARD_HEIGHT; i++)
            {
                spawn[i] = new int[RoomDimensions.BOARD_WIDTH];
                for (int j = 0; j < RoomDimensions.BOARD_WIDTH; j++)
                {
                    spawn[i][j] = 0;
                }
            }
            for (int i = 0; i < numEnemies; i++)
            {
                int r = Random.RandomRange(0, RoomDimensions.BOARD_HEIGHT);
                int c = Random.RandomRange(0, RoomDimensions.BOARD_WIDTH);
                if (spawn[r][c] == 0)
                {
                    spawn[r][c] = 1;
                }
                else
                {
                    i--;
                }
            }
            for (int i = 0; i < numSprinklers; i++)
            {
                int r = Random.RandomRange(0, RoomDimensions.BOARD_HEIGHT);
                int c = Random.RandomRange(0, RoomDimensions.BOARD_WIDTH);
                if (spawn[r][c] == 0)
                {
                    spawn[r][c] = 2;
                }
                else
                {
                    i--;
                }
            }
            for (int i = 0; i < numProps; i++)
            {
                int r = Random.RandomRange(0, RoomDimensions.BOARD_HEIGHT);
                int c = Random.RandomRange(0, RoomDimensions.BOARD_WIDTH);
                if (spawn[r][c] == 0)
                {
                    spawn[r][c] = 3;
                }
                else
                {
                    i--;
                }
            }
            for (int i = 0; i < RoomDimensions.BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < RoomDimensions.BOARD_WIDTH; j++)
                {
                    if (spawn[i][j] == 1)
                    {
                        GameObject newEnemy = Instantiate(enemy);
                        newEnemy.GetComponent<EnemyController>().SetStats(adj);
                        newEnemy.transform.position = new Vector3(j * TileDimensions.SIZE + transform.position.x, i * TileDimensions.SIZE + transform.position.y, newEnemy.transform.position.z);
                    }
                    else if (spawn[i][j] == 2)
                    {
                        GameObject newSprinkler = (GameObject)Instantiate(sprinkler, transform);
                        newSprinkler.transform.localPosition = new Vector3(j * TileDimensions.SIZE, i * TileDimensions.SIZE, newSprinkler.transform.position.z);
                    }
                    else if (spawn[i][j] == 3)
                    {
                        GameObject prop = props[Random.Range(0, props.Length)];
                        GameObject newProp = (GameObject)Instantiate(prop, transform);
                        newProp.transform.localPosition = new Vector3(j * TileDimensions.SIZE, i * TileDimensions.SIZE, newProp.transform.position.z);
                    }
                }
            }
            PlayerController.rating = 0;
            numProps = Random.Range(0, 3);

        }
    }

    public void genLeftRoom()
    {
        //create grass for room
        for (int i = 0; i < RoomDimensions.BOARD_HEIGHT; i++)
        {
            for (int j = 0; j < RoomDimensions.BOARD_WIDTH; j++)
            {
                GameObject newGrass;// = new GameObject();
                //create & place new grass
                if (i == 0 && j == 0)
                    newGrass = (GameObject)Instantiate(grass[6], transform);
                else if (i == RoomDimensions.BOARD_HEIGHT - 1 && j == 0)
                    newGrass = (GameObject)Instantiate(grass[0], transform);
                else if (i == RoomDimensions.BOARD_HEIGHT - 1 && j == RoomDimensions.BOARD_WIDTH - 1)
                    newGrass = (GameObject)Instantiate(grass[2], transform);
                else if (i == 0 && j == RoomDimensions.BOARD_WIDTH - 1)
                    newGrass = (GameObject)Instantiate(grass[8], transform);
                else if (i == 0)
                    newGrass = (GameObject)Instantiate(grass[7], transform);
                else if (i == RoomDimensions.BOARD_HEIGHT - 1)
                    newGrass = (GameObject)Instantiate(grass[1], transform);
                else if (j == 0)
                    newGrass = (GameObject)Instantiate(grass[3], transform);
                else if (j == RoomDimensions.BOARD_WIDTH - 1)
                    newGrass = (GameObject)Instantiate(grass[5], transform);
                else
                    newGrass = (GameObject)Instantiate(grass[4], transform);

                newGrass.transform.localPosition = new Vector3(j * TileDimensions.SIZE, i * TileDimensions.SIZE);
                newGrass.transform.localScale = new Vector3(1.003f, 1.003f, newGrass.transform.localScale.z);
            }
        }

        //create fence for room around the grass
        for (int i = -1; i < RoomDimensions.BOARD_HEIGHT + 1; i++)
        {
            for (int j = -1; j < RoomDimensions.BOARD_WIDTH + 1; j++)
            {
                //create & place new fence
                //GameObject newFence;// = new GameObject();

                if (!((j == RoomDimensions.BOARD_WIDTH && i == 4) || (j == RoomDimensions.BOARD_WIDTH && i == 5)))
                {
                    if ((i == -1 || i == RoomDimensions.BOARD_HEIGHT) && (j == -1 || j == RoomDimensions.BOARD_WIDTH))      //corner
                    {
                        GameObject newFence = (GameObject)Instantiate(fence[4], transform);
                        float x = 0;
                        if (j == -1)
                            x = j * TileDimensions.SIZE + TileDimensions.SIZE / 2f - FenceDimensions.SIZE / 2f;
                        else
                            x = j * TileDimensions.SIZE - TileDimensions.SIZE / 2f + FenceDimensions.SIZE / 2f;
                        float y = 0;
                        if (i == -1)
                            y = i * TileDimensions.SIZE + TileDimensions.SIZE / 2f - FenceDimensions.SIZE / 2f;
                        else
                            y = i * TileDimensions.SIZE - TileDimensions.SIZE / 2f + FenceDimensions.SIZE / 2f;
                        newFence.transform.localPosition = new Vector3(x, y);
                        newFence.transform.localScale = new Vector3(1.003f, 1.003f, newFence.transform.localScale.z);
                    }
                    else if (i == -1)                                                                                       //bottom
                    {
                        GameObject newFence = (GameObject)Instantiate(fence[3], transform);
                        newFence.transform.localPosition = new Vector3(j * TileDimensions.SIZE, i * TileDimensions.SIZE + TileDimensions.SIZE / 2f - FenceDimensions.SIZE / 2f);
                        newFence.transform.localScale = new Vector3(1.003f, 1.003f, newFence.transform.localScale.z);
                    }
                    else if (i == RoomDimensions.BOARD_HEIGHT)                                                              //top
                    {
                        GameObject newFence = (GameObject)Instantiate(fence[2], transform);
                        newFence.transform.localPosition = new Vector3(j * TileDimensions.SIZE, i * TileDimensions.SIZE - TileDimensions.SIZE / 2f + FenceDimensions.SIZE / 2f);
                        newFence.transform.localScale = new Vector3(1.003f, 1.003f, newFence.transform.localScale.z);
                    }
                    else if (j == -1)                                                                                       //left
                    {
                        GameObject newFence = (GameObject)Instantiate(fence[0], transform);
                        newFence.transform.localPosition = new Vector3(j * TileDimensions.SIZE + TileDimensions.SIZE / 2f - FenceDimensions.SIZE / 2f, i * TileDimensions.SIZE);
                        newFence.transform.localScale = new Vector3(1.003f, 1.003f, newFence.transform.localScale.z);
                    }
                    else if (j == RoomDimensions.BOARD_WIDTH)                                                               //right
                    {
                        GameObject newFence = (GameObject)Instantiate(fence[1], transform);
                        newFence.transform.localPosition = new Vector3(j * TileDimensions.SIZE - TileDimensions.SIZE / 2f + FenceDimensions.SIZE / 2f, i * TileDimensions.SIZE);
                        newFence.transform.localScale = new Vector3(1.003f, 1.003f, newFence.transform.localScale.z);

                    }
                }
                else
                {
                    if (j == -1)                                                                                       //left
                    {
                        GameObject newFence = (GameObject)Instantiate(lockedFence[0], transform);
                        newFence.GetComponent<LockedFenceController>().room = this.gameObject;
                        newFence.transform.localPosition = new Vector3(j * TileDimensions.SIZE + TileDimensions.SIZE / 2f - FenceDimensions.SIZE / 2f, i * TileDimensions.SIZE);
                        newFence.transform.localScale = new Vector3(1.003f, 1.003f, newFence.transform.localScale.z);
                    }
                    else if (j == RoomDimensions.BOARD_WIDTH)                                                          //right
                    {
                        GameObject newFence = (GameObject)Instantiate(lockedFence[1], transform);
                        newFence.GetComponent<LockedFenceController>().room = this.gameObject;
                        newFence.transform.localPosition = new Vector3(j * TileDimensions.SIZE - TileDimensions.SIZE / 2f + FenceDimensions.SIZE / 2f, i * TileDimensions.SIZE);
                        newFence.transform.localScale = new Vector3(1.003f, 1.003f, newFence.transform.localScale.z);
                    }
                }
            }
        }
    }

    public void genMiddleRoom()
    {
        //create grass for room
        for (int i = 0; i < RoomDimensions.BOARD_HEIGHT; i++)
        {
            for (int j = 0; j < RoomDimensions.BOARD_WIDTH; j++)
            {
                GameObject newGrass;// = new GameObject();
                //create & place new grass
                if (i == 0 && j == 0)
                    newGrass = (GameObject)Instantiate(grass[6], transform);
                else if (i == RoomDimensions.BOARD_HEIGHT - 1 && j == 0)
                    newGrass = (GameObject)Instantiate(grass[0], transform);
                else if (i == RoomDimensions.BOARD_HEIGHT - 1 && j == RoomDimensions.BOARD_WIDTH - 1)
                    newGrass = (GameObject)Instantiate(grass[2], transform);
                else if (i == 0 && j == RoomDimensions.BOARD_WIDTH - 1)
                    newGrass = (GameObject)Instantiate(grass[8], transform);
                else if (i == 0)
                    newGrass = (GameObject)Instantiate(grass[7], transform);
                else if (i == RoomDimensions.BOARD_HEIGHT - 1)
                    newGrass = (GameObject)Instantiate(grass[1], transform);
                else if (j == 0)
                    newGrass = (GameObject)Instantiate(grass[3], transform);
                else if (j == RoomDimensions.BOARD_WIDTH - 1)
                    newGrass = (GameObject)Instantiate(grass[5], transform);
                else
                    newGrass = (GameObject)Instantiate(grass[4], transform);

                newGrass.transform.localPosition = new Vector3(j * TileDimensions.SIZE, i * TileDimensions.SIZE);
                newGrass.transform.localScale = new Vector3(1.003f, 1.003f, newGrass.transform.localScale.z);
            }
        }

        //create fence for room around the grass
        for (int i = -1; i < RoomDimensions.BOARD_HEIGHT + 1; i++)
        {
            for (int j = -1; j < RoomDimensions.BOARD_WIDTH + 1; j++)
            {
                //create & place new fence
                //GameObject newFence = null;

                if (!(i == 4 || i == 5))
                {
                    if ((i == -1 || i == RoomDimensions.BOARD_HEIGHT) && (j == -1 || j == RoomDimensions.BOARD_WIDTH))      //corner
                    {
                        GameObject newFence = (GameObject)Instantiate(fence[4], transform);
                        float x = 0;
                        if (j == -1)
                            x = j * TileDimensions.SIZE + TileDimensions.SIZE / 2f - FenceDimensions.SIZE / 2f;
                        else
                            x = j * TileDimensions.SIZE - TileDimensions.SIZE / 2f + FenceDimensions.SIZE / 2f;
                        float y = 0;
                        if (i == -1)
                            y = i * TileDimensions.SIZE + TileDimensions.SIZE / 2f - FenceDimensions.SIZE / 2f;
                        else
                            y = i * TileDimensions.SIZE - TileDimensions.SIZE / 2f + FenceDimensions.SIZE / 2f;
                        newFence.transform.localPosition = new Vector3(x, y);
                        newFence.transform.localScale = new Vector3(1.003f, 1.003f, newFence.transform.localScale.z);
                    }
                    else if (i == -1)                                                                                       //bottom
                    {
                        GameObject newFence = (GameObject)Instantiate(fence[3], transform);
                        newFence.transform.localPosition = new Vector3(j * TileDimensions.SIZE, i * TileDimensions.SIZE + TileDimensions.SIZE / 2f - FenceDimensions.SIZE / 2f);
                        newFence.transform.localScale = new Vector3(1.003f, 1.003f, newFence.transform.localScale.z);
                    }
                    else if (i == RoomDimensions.BOARD_HEIGHT)                                                              //top
                    {
                        GameObject newFence = (GameObject)Instantiate(fence[2], transform);
                        newFence.transform.localPosition = new Vector3(j * TileDimensions.SIZE, i * TileDimensions.SIZE - TileDimensions.SIZE / 2f + FenceDimensions.SIZE / 2f);
                        newFence.transform.localScale = new Vector3(1.003f, 1.003f, newFence.transform.localScale.z);
                    }
                    else if (j == -1)                                                                                       //left
                    {
                        GameObject newFence = (GameObject)Instantiate(fence[0], transform);
                        newFence.transform.localPosition = new Vector3(j * TileDimensions.SIZE + TileDimensions.SIZE / 2f - FenceDimensions.SIZE / 2f, i * TileDimensions.SIZE);
                        newFence.transform.localScale = new Vector3(1.003f, 1.003f, newFence.transform.localScale.z);
                    }
                    else if (j == RoomDimensions.BOARD_WIDTH)                                                               //right
                    {
                        GameObject newFence = (GameObject)Instantiate(fence[1], transform);
                        newFence.transform.localPosition = new Vector3(j * TileDimensions.SIZE - TileDimensions.SIZE / 2f + FenceDimensions.SIZE / 2f, i * TileDimensions.SIZE);
                        newFence.transform.localScale = new Vector3(1.003f, 1.003f, newFence.transform.localScale.z);
                    }
                }
                else
                {
                    if (j == -1)                                                                                       //left
                    {
                        GameObject newFence = (GameObject)Instantiate(lockedFence[0], transform);
                        newFence.GetComponent<LockedFenceController>().room = this.gameObject;
                        newFence.transform.localPosition = new Vector3(j * TileDimensions.SIZE + TileDimensions.SIZE / 2f - FenceDimensions.SIZE / 2f, i * TileDimensions.SIZE);
                        newFence.transform.localScale = new Vector3(1.003f, 1.003f, newFence.transform.localScale.z);
                    }
                    else if (j == RoomDimensions.BOARD_WIDTH)                                                          //right
                    {
                        GameObject newFence = (GameObject)Instantiate(lockedFence[1], transform);
                        newFence.GetComponent<LockedFenceController>().room = this.gameObject;
                        newFence.transform.localPosition = new Vector3(j * TileDimensions.SIZE - TileDimensions.SIZE / 2f + FenceDimensions.SIZE / 2f, i * TileDimensions.SIZE);
                        newFence.transform.localScale = new Vector3(1.003f, 1.003f, newFence.transform.localScale.z);
                    }
                }
            }
        }
    }

    public void genRightRoom()
    {
        //create grass for room

        for (int i = 0; i < RoomDimensions.BOARD_HEIGHT; i++)
        {
            for (int j = 0; j < RoomDimensions.BOARD_WIDTH; j++)
            {
                GameObject newGrass = new GameObject();
                //create & place new grass
                if (i == 0 && j == 0)
                    newGrass = (GameObject)Instantiate(grass[6], transform);
                else if (i == RoomDimensions.BOARD_HEIGHT - 1 && j == 0)
                    newGrass = (GameObject)Instantiate(grass[0], transform);
                else if (i == RoomDimensions.BOARD_HEIGHT - 1 && j == RoomDimensions.BOARD_WIDTH - 1)
                    newGrass = (GameObject)Instantiate(grass[2], transform);
                else if (i == 0 && j == RoomDimensions.BOARD_WIDTH - 1)
                    newGrass = (GameObject)Instantiate(grass[8], transform);
                else if (i == 0)
                    newGrass = (GameObject)Instantiate(grass[7], transform);
                else if (i == RoomDimensions.BOARD_HEIGHT - 1)
                    newGrass = (GameObject)Instantiate(grass[1], transform);
                else if (j == 0)
                    newGrass = (GameObject)Instantiate(grass[3], transform);
                else if (j == RoomDimensions.BOARD_WIDTH - 1)
                    newGrass = (GameObject)Instantiate(grass[5], transform);
                else
                    newGrass = (GameObject)Instantiate(grass[4], transform);

                newGrass.transform.localPosition = new Vector3(j * TileDimensions.SIZE, i * TileDimensions.SIZE);
                newGrass.transform.localScale = new Vector3(1.003f, 1.003f, newGrass.transform.localScale.z);
            }
        }

        //create fence for room around the grass
        for (int i = -1; i < RoomDimensions.BOARD_HEIGHT + 1; i++)
        {
            for (int j = -1; j < RoomDimensions.BOARD_WIDTH + 1; j++)
            {
                //create & place new fence
                GameObject newFence = new GameObject();
                newFence.transform.localScale = new Vector3(1.003f, 1.003f, newFence.transform.localScale.z);

                if (!((j == 0 && i == 4) || (j == 0 && i == 5)))
                {
                    if ((i == -1 || i == RoomDimensions.BOARD_HEIGHT) && (j == -1 || j == RoomDimensions.BOARD_WIDTH))      //corner
                    {
                        newFence = (GameObject)Instantiate(fence[4], transform);
                        float x = 0;
                        if (j == -1)
                            x = j * TileDimensions.SIZE + TileDimensions.SIZE / 2f - FenceDimensions.SIZE / 2f;
                        else
                            x = j * TileDimensions.SIZE - TileDimensions.SIZE / 2f + FenceDimensions.SIZE / 2f;
                        float y = 0;
                        if (i == -1)
                            y = i * TileDimensions.SIZE + TileDimensions.SIZE / 2f - FenceDimensions.SIZE / 2f;
                        else
                            y = i * TileDimensions.SIZE - TileDimensions.SIZE / 2f + FenceDimensions.SIZE / 2f;
                        newFence.transform.localPosition = new Vector3(x, y);
                    }
                    else if (i == -1)                                                                                       //bottom
                    {
                        newFence = (GameObject)Instantiate(fence[3], transform);
                        newFence.transform.localPosition = new Vector3(j * TileDimensions.SIZE, i * TileDimensions.SIZE + TileDimensions.SIZE / 2f - FenceDimensions.SIZE / 2f);
                    }
                    else if (i == RoomDimensions.BOARD_HEIGHT)                                                              //top
                    {
                        newFence = (GameObject)Instantiate(fence[2], transform);
                        newFence.transform.localPosition = new Vector3(j * TileDimensions.SIZE, i * TileDimensions.SIZE - TileDimensions.SIZE / 2f + FenceDimensions.SIZE / 2f);
                    }
                    else if (j == -1)                                                                                       //left
                    {
                        newFence = (GameObject)Instantiate(fence[0], transform);
                        newFence.transform.localPosition = new Vector3(j * TileDimensions.SIZE + TileDimensions.SIZE / 2f - FenceDimensions.SIZE / 2f, i * TileDimensions.SIZE);
                    }
                    else if (j == RoomDimensions.BOARD_WIDTH)                                                               //right
                    {
                        newFence = (GameObject)Instantiate(fence[1], transform);
                        newFence.transform.localPosition = new Vector3(j * TileDimensions.SIZE - TileDimensions.SIZE / 2f + FenceDimensions.SIZE / 2f, i * TileDimensions.SIZE);
                    }
                }
                else
                {
                    if (j == -1)                                                                                       //left
                    {
                        newFence = (GameObject)Instantiate(lockedFence[0], transform);
                        newFence.GetComponent<LockedFenceController>().room = this.gameObject;
                        newFence.transform.localPosition = new Vector3(j * TileDimensions.SIZE + TileDimensions.SIZE / 2f - FenceDimensions.SIZE / 2f, i * TileDimensions.SIZE);
                    }
                    else if (j == RoomDimensions.BOARD_WIDTH)                                                          //right
                    {
                        newFence = (GameObject)Instantiate(lockedFence[1], transform);
                        newFence.GetComponent<LockedFenceController>().room = this.gameObject;
                        newFence.transform.localPosition = new Vector3(j * TileDimensions.SIZE - TileDimensions.SIZE / 2f + FenceDimensions.SIZE / 2f, i * TileDimensions.SIZE);
                    }
                }
            }
        }
    }

    void selfDestruct()
    {
        while (transform.childCount > 0)
        {
            Transform c = transform.GetChild(0);
            c.SetParent(null); // become Batman
            Destroy(c.gameObject); // become The Joker
        }
        GameObject.Destroy(this.gameObject);
    }

    int DifficultyCalc(float n)
    {
        return 2 - ((int)n) / 50;
    }
}
