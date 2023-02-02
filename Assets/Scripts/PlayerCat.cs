using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCat : MonoBehaviour
{
    // Start is called before the first frame update

    private float movementX;  // Tells if the cat has to move left or right

    private bool isGrounded;  // Tells if the cat can jump or not

    [SerializeField]
    private Rigidbody2D myBody;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private SpriteRenderer sprite_render;

    [SerializeField]
    private SpriteRenderer[] heart_sprites;

    public static short life;
    private bool canTakeDamage;

    // Variables for attacking
    public LayerMask enemyLayers;
    public Transform attackPoint;


    private Color red, originalColor, transparent;
    void Awake()
    {
        life = 6;
        red = new Color(1, 0, 0, 1);
        originalColor = new Color(1, 1, 1, 1);
        transparent = new Color(0, 0 ,0, 0);
        canTakeDamage = true;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();  // Moves the cat left, right
        AnimatePlayer();  // Walk animation


        // Checks if the cat has to attack depending on players input
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }


    // Deals with physics
    private void FixedUpdate()
    {
        PlayerJump();
        GoDown();
    }

    // Makes the cat change its position on the x axis, rigth or left
    private void PlayerMovement()
    {
        movementX = Input.GetAxisRaw("Horizontal");  // Checks if the left or  right arrows have been pressed
        transform.position += 8 * Time.deltaTime * new Vector3(movementX, 0, 0);  // Sets the cat on the new position
    }

    // Attacks the mice
    private void Attack()
    {
        SoundManagerScript.PlaySound("claw");  // Claw sound effect
        // Attack animation
        anim.SetBool("OnAir", false);  // To avoid conflict with jump animation
        anim.SetTrigger("Attack");  // Starts attack animation

        // Creates a circle of attack range and gets all the enemies that were affected by this attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, 0.5f, enemyLayers);

        // Tells all the mice that were attacked to die
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<MouseScript>().Die();
        }

    }


    /*
     * Works with the animator
     * Affects the animation of the cat
     */
    private void AnimatePlayer()
    {
        if (movementX > 0)
        {
            // Moving to the right
            anim.SetBool("Move", true);  // Changes the animator transition parameter, the cat has to move
            transform.localRotation = Quaternion.Euler(0, 180, 0);  // This method keeps the collider matching with the sprite, flips the element to the right

        } else if (movementX < 0)
        {
            // Moving to the left
            anim.SetBool("Move", true);  // Changes the animator transition parameter, the cat has to move
            transform.localRotation = Quaternion.Euler(0, 0, 0);  // This method keeps the collider matching with the sprite, flips the element to the left
        } else
        {
            // Stop
            anim.SetBool("Move", false);  // Changes the animator transition parameter, the cat has to idle
        }
    }


    /*
     Makes the cat jump
     Works with the upper arrow
     The cat most be over a ground collider
     */
    private void PlayerJump()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow)  && isGrounded)
        {
            myBody.AddForce(new Vector2(0, 6.5f), ForceMode2D.Impulse);  // Makes the cat up
            SoundManagerScript.PlaySound("jump");  // jump sound effect
            anim.SetBool("OnAir", true);  // Starts jump animation
            isGrounded = false;  // Tells it is on the air
        }
    }
    private bool IsOnAir()
    {
        /*Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.8f);
        return colliders.Length == 1;*/
        return Physics2D.RaycastAll(transform.position+ new Vector3(0.655f, 0, 0), -Vector2.up, 0.8f).Length == 1  &&
            Physics2D.RaycastAll(transform.position+ new Vector3(-0.655f, 0, 0), -Vector2.up, 0.8f).Length == 1;

    }

    /*
     * Makes the cat go down when the down key is pressed
     */
        private void GoDown()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (IsOnAir())
            {
                myBody.AddForce(new Vector2(0, -0.6f), ForceMode2D.Impulse);  // Makes the cat go down
                anim.SetBool("OnAir", false);
                anim.SetBool("Descend", true);
            }
        }

    }


    // Keeps checking if the cat is over the ground or not
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Checks if there is contact with something and if that something is ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("OnAir", false);  // To stop jump animation
            anim.SetBool("Descend", false);
        }

        // Checks if there is contact with an enemy
        if (collision.gameObject.CompareTag("Enemy") && canTakeDamage)
        {

            StartCoroutine(PaintDamage());
            life--;  // Takes damage
            if (life < 0) 
            {
                // There are no more lives left
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    public void Heal()
    {
        if(life < 6)
        {
            SoundManagerScript.PlaySound("heal");
            life++;
            heart_sprites[life].color = originalColor;
        }
    }


    IEnumerator PaintDamage()
    {
        // Cat scream sound effect
        SoundManagerScript.PlaySound("meow");

        // Tells that cannot be dameged during the hurt animation
        canTakeDamage = false;

        // Eraeases one heart icon
        heart_sprites[life].color = transparent;

        // Start red-normal-red hurt animation
        sprite_render.color = red;
        yield return new WaitForSeconds(0.2f);

        sprite_render.color = originalColor;
        yield return new WaitForSeconds(0.1f);

        sprite_render.color = red;
        yield return new WaitForSeconds(0.2f);

        sprite_render.color = originalColor;
        yield return new WaitForSeconds(0.1f);

        sprite_render.color = red;
        yield return new WaitForSeconds(0.2f);

        sprite_render.color = originalColor;  // Red-normal-red animation finished

        yield return new WaitForSeconds(0.3f);  // Extra inmunity time
        canTakeDamage = true;  // The cat can be hurt again

        yield break;
    }




}
