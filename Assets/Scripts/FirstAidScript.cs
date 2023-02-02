using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(Random.Range(-8.5f, 8.4f), -4.3f, 0);
    }

    // Tells if the cat touched it
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Checks if there is contact with something and if that something is ground
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerCat>().Heal();
            Destroy(gameObject);
        }
    }

}
