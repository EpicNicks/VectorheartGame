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
    public AudioSource enemyShootSound;
    public AudioClip shootSound;
    public AudioSource enemyMeleeSound;
    public AudioClip hurtSound;
    public AudioClip hurtSoundMelee;
    public AudioClip enemyMeleeSwordSound;
    public AudioClip playerHurt;
    public AudioClip playerUlt;

    public AudioSource playerHurtSource;



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

    public void doUltimate()
    {
        ultimateAbility.SetActive(true);
    }

    public void StartDashTrail()
    {
        dash?.SetActive(true);
        
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

    public void dashSoundFX()
    {
        playerSound.clip = dashSound;
        playerSound.Play();
    }

    public void enemyShootFX()
    {
        enemyShootSound.clip = shootSound;
        enemyShootSound.Play();
    }

    public void enemyShootHitFX()
    {
        enemyShootSound.clip = hurtSound;
        enemyShootSound.Play();
    }

    public void enemyMeleeSoundHitFX()
    {
        enemyMeleeSound.clip = hurtSoundMelee;
        enemyMeleeSound.Play();
    }

    public void enemyMeleeSoundFX()
    {
        enemyMeleeSound.clip = enemyMeleeSwordSound;
        enemyMeleeSound.Play();
    }

    public void ultSoundFX()
    {
        playerSound.clip = playerUlt;
        playerSound.Play();
    }

    public void playerHurtSoundFX()
    {
        playerHurtSource.clip = playerHurt;
        playerHurtSource.Play();
    }
}
