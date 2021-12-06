using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{

    public GameObject normalAttack;
    public GameObject chargedAttack;
    public ParticleSystem spinAttack;
    public GameObject ultimateAbility;
    public GameObject dash;
    public GameObject running;

    private CharacterInput character;

    public Transform leftFoot;
    public Transform rightFoot;
    public Transform feet;
    public Transform spinLocation;

    private Animator playerAnim;
    public AudioSource playerSound;
    public AudioClip spinSound;
    public AudioClip dashSound;
    public AudioClip attackSound;


    private void Awake()
    {
        playerAnim = GetComponent<Animator>();
        character = GetComponent<CharacterInput>();
    }

    public void SpinAttack()
    {
        spinAttack.Play();
        playerSound.clip = spinSound;
        playerSound.Play();
    }

    public void StartTrail()
    {
        if (character.Psm.isCharged)
        {
            chargedAttack?.SetActive(true);
        } else
        {
            normalAttack?.SetActive(true);
        }

    }

    public void EndTrail()
    {
        normalAttack?.SetActive(false);
        chargedAttack?.SetActive(false);
        ultimateAbility.SetActive(false);
    }

    public void Ultimate()
    {
        ultimateAbility.SetActive(true);
    }

    public void StartDashTrail()
    {
        dash?.SetActive(true);
        playerSound.clip = dashSound;
        playerSound.Play();
    }

    public void EndDashTrail()
    {
        dash?.SetActive(false);
    }

    public void RightFootStep()
    {

        Instantiate(running, rightFoot.position, Quaternion.identity);
    }

    public void LeftFootStep()
    {
        Instantiate(running, leftFoot.position, Quaternion.identity);
    }

    public void attackSoundFX()
    {
        playerSound.clip = attackSound;
        playerSound.Play();
    }
}
