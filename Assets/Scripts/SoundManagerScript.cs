using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 This class manages the sound effects
It plays an specific sound effect according a given situation
 */
public class SoundManagerScript : MonoBehaviour
{
    private static AudioClip catAttackSound, jumpSound, meowSound, mouseAppearSound, mouseDeathSound, healSoundEffect;  // All the sound effects we have
    private static AudioSource audioSrc; // Instance to the audio source

    // Start is called before the first frame update
    void Start()
    {
        // Gives value to variables
        catAttackSound = Resources.Load<AudioClip>("ClawEffect");
        jumpSound = Resources.Load<AudioClip>("JumpEffect");
        meowSound = Resources.Load<AudioClip>("MeowEffect");
        mouseDeathSound = Resources.Load<AudioClip>("EnemyKillEffect");
        mouseAppearSound = Resources.Load<AudioClip>("SqueakSoundEffect");
        healSoundEffect = Resources.Load<AudioClip>("Heal Sound Effect");

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     Is called by other classes
    THe string parameter tells which audio effect we want to play
     */
    public static void PlaySound(string clip)
    {
        switch(clip)
        {
            case "claw":
                audioSrc.PlayOneShot(catAttackSound);
                break;

            case "jump":
                audioSrc.PlayOneShot(jumpSound);
                break;

            case "meow":
                audioSrc.PlayOneShot(meowSound);
                break;

            case "mouse die":
                audioSrc.PlayOneShot(mouseDeathSound);
                break;

            case "mouse appear":
                audioSrc.PlayOneShot(mouseAppearSound);
                break;

            case "heal":
                audioSrc.PlayOneShot(healSoundEffect);
                break;

            default:
                break;
        }
    }
}
