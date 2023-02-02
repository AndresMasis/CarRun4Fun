using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Makes the actions of a mouse
The mouse is an active object itself but also a spawner of other kind of objects
It is an enemy that can be killed by the cat if attacked
Also spawns hammers
 */

public class MouseScript : MonoBehaviour
{
    [SerializeField]
    private GameObject hammerReference;  // Reference to the kind of object that will spawn, in this case hammers

    private GameObject spawnedHammer;  // The individual hammer to spawn at a moment

    public int index;  // Tells in which position of the spawning array this mouse was created

    public bool isFlipped, isTop;  // Validation variables
    public float verticalForce, horizontalForce;  // Throwing variables
    private int waitTime;  // Time between a throw and the next

    // Start is called before the first frame update
    void Start()
    {
        // Assigns value to verticalForce
        if(isTop)
        {
            // The hammer may crash, low vertical force
            verticalForce = Random.Range(0, 1);
        } else
        {
            // No restriction on vertical force
            verticalForce = Random.Range(2, 9);
        }

        // Assigns value to horizontalForce
        horizontalForce = Random.Range(2, 8);
        if (!isFlipped)
        {
            // Throws to the other side
            horizontalForce *= -1;
        }

        // Assigns a constant wait time between a throw and the next one
        waitTime = Random.Range(1, 4);

        SoundManagerScript.PlaySound("mouse appear");  // Sound effect

        // Starts throwing hammers
        StartCoroutine(SpawnHammer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     The mouse has to die
    It is called by the cat when it attacks the mouse
     */
    public void Die()
    {
        SpawnerScript.taken[index] = false;  // Tells the spawner class that this position now is free
        SoundManagerScript.PlaySound("mouse die");  // Mouse destroyed sound effect
        Destroy(gameObject);  // This mouse is destroyed
    }

    /*
     This coroutine spawns a new hammer after a certain period of time
     */
    IEnumerator SpawnHammer()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            spawnedHammer = Instantiate(hammerReference);  // Creates a new hammer
            spawnedHammer.transform.position = gameObject.transform.position;  // Gives the hammer a starting position at the mouse hand

            // Throws the hammer
            spawnedHammer.GetComponent<HammerScript>().verticalForce = verticalForce;
            spawnedHammer.GetComponent<HammerScript>().horizontalForce = horizontalForce;

            spawnedHammer.GetComponent<HammerScript>().father = gameObject;  // Needed for a validation to get a correct throw
        }

    }
}
