using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{

    public GameObject normalAttack;
    public GameObject chargedAttack;
    public ParticleSystem chargedSpinAttack;
    public ParticleSystem spinAttack;
    public GameObject ultimateAbility;
    public GameObject chargedDash;
    public GameObject dash;
    public GameObject running;

    private CharacterInput character;

    public Transform leftFoot;
    public Transform rightFoot;
    public Transform feet;
    public Transform spinLocation;

    private Animator playerAnim;

    private void Awake()
    {
        playerAnim = GetComponent<Animator>();
        character = GetComponent<CharacterInput>();
    }

    public void SpinAttack()
    {
        if(character.Psm.isCharged)
        {
            chargedSpinAttack.Play();
        }
        else
        {
            spinAttack.Play();
        }
       
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
    }

    public void Ultimate()
    {
        Instantiate(ultimateAbility, transform.position, transform.rotation);
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
}
