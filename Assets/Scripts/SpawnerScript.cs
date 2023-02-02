using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 This class is a spawner
 Continously generates new enemies after a period of time
 It spawns mice on specific points of the map
 Also, randomly spawns first aids at any point of the map
 */

public class SpawnerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseReference, firstAidReference;  // Reference the object that will spawn, in this case mice and first aid

    [SerializeField]
    private Transform[] positions;  // Gets all the possible positions where a new mouse can be spawned

    private int positionsLength;  // Takes the lenght of the previous array


    public static bool[] taken;  // Avoids overlapping, it must be static

    private GameObject spawnedMouse, spawnedFirstAid;  // Needed to spawn a mouse or a first aid at the moment


    // Start is called before the first frame update
    void Start()
    {
        positionsLength = positions.Length;  // Gets how mnany possible positions we have
        taken = new bool[positionsLength];  // Initializes the array

        StartCoroutine(SpawnMouse());  // Starts the spawning coroutines
        StartCoroutine(SpawnFirstAid());
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
     Periodically spawns a new mouse
     Wait n seconds and checks if can create a new mouse instantiate
     */
    IEnumerator SpawnMouse()
    {
        // Declaration of needed variables before entering the loop
        int randomPosition;  // Tells in which position the mouse must be spawned
        float waitTime = 2;  // Tells how much time it has to wait to spawn the next mouse


        while (true)  // Needed for continous spawning
        {
            yield return new WaitForSeconds(waitTime);

            randomPosition = Random.Range(0, positionsLength);  // Select a random position in the plane to spawn the mouse

            if (taken[randomPosition])  // Checks if there is already a mouse in the selected spawn point, to avoid overlapping
            {
                // Tries again, the selected spawn location is already taken
                waitTime = 2;
                continue;
            }


            // Can spawn the mouse in that location, it was free
            spawnedMouse = Instantiate(mouseReference);
            taken[randomPosition] = true;  // Tells that now there is a mouse in that position

            // Positions the mouse on the plane
            spawnedMouse.transform.position = positions[randomPosition].position;
            spawnedMouse.GetComponent<MouseScript>().index = randomPosition;  // Tells the what index of the array corresponds to its position


            // Checks if the mouse has to be flipped according its position
            if (randomPosition < 4)
            {
                // Extreme left

                // Has to flip the mouse
                spawnedMouse.transform.localScale = new Vector3(-1, 1, 1);  // Moves the mouse to the other side
                spawnedMouse.GetComponent<MouseScript>().isFlipped = true;  // Tells to throw the hammer to the other side
                spawnedMouse.GetComponent<MouseScript>().isTop = false;
            }
            else if (randomPosition < 12)
            {
                // General case
                if (Random.Range(0, 1000) < 500)
                {
                    // Randomly flips the mouse
                    spawnedMouse.transform.localScale = new Vector3(-1, 1, 1);  // Moves the mouse to the other side
                    spawnedMouse.GetComponent<MouseScript>().isFlipped = true;    // Tells to throw the hammer to the other side
                }
                else
                {
                    spawnedMouse.GetComponent<MouseScript>().isFlipped = false;
                }

                spawnedMouse.GetComponent<MouseScript>().isTop = false;
            }
            else
            {
                // Extreme right, Also extreme top
                spawnedMouse.GetComponent<MouseScript>().isFlipped = false;
                spawnedMouse.GetComponent<MouseScript>().isTop = true;
            }

            // Random time for the next spawning
            waitTime = Random.Range(3, 9);
        }

    }


    /*
     Periodically spawns a new first aid
     Wait n seconds and checks if can create a new mouse instantiate
     */
    IEnumerator SpawnFirstAid()
    {
        while (true)  // Needed for continous spawning
        {
            yield return new WaitForSeconds(Random.Range(9, 20));

            if (PlayerCat.life < 5)
            {
                // Can spawn the mouse in that location, it was free
                spawnedMouse = Instantiate(firstAidReference);
            }

        }

    }
}
