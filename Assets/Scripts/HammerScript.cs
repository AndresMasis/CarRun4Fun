using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*This script describes the hammer
 The hammer is an object itself
It is spawned by mice and destroyed when crashes with anything*/
public class HammerScript : MonoBehaviour
{
    private Rigidbody2D myBody;
    private CircleCollider2D myCollider;
    public float verticalForce, horizontalForce;
    public GameObject father;
    private Animator anim;
    private void Awake()
    {
        // Gets instances
        myBody = GetComponent<Rigidbody2D>();  
        anim = GetComponent<Animator>();
        myCollider = GetComponent<CircleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        myBody.AddForce(new Vector2(horizontalForce, verticalForce), ForceMode2D.Impulse);  // Throws the hammer
    }

  


    // Detect when the hammer touches something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Makes the validation the contact is not with the mouse who threw it
        if ( collision.gameObject != father)
        {
            // Crashed with any other object, gets destroyed
            StartCoroutine(DestroyHammer());
        }

    }

    IEnumerator DestroyHammer()
    {
        // Destroys the hammer

        anim.SetTrigger("Destroyed");  // Starts dust cloud animation
        // Destroys the collider and rigid body to make its physics disappear and only leaves the animation
        Destroy(myBody);
        Destroy(myCollider);

        // Gives time for the animation to end
        yield return new WaitForSeconds(0.5f);


        // Destroys the hammer object
        Destroy(gameObject);
        yield break;
    }
}
