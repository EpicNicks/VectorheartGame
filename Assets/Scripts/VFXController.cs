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
    private bool isCharged = false;

    private CharacterInput character;

    public Transform leftFoot;
    public Transform rightFoot;
    public Transform feet;
    public Transform spinLocation;


    private Animator playerAnim;
    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        character = GetComponent<CharacterInput>();
    }

    public void SpinAttack()
    {
        if(isCharged == true)
        {
            chargedSpinAttack.Play();
        } else
        {
            spinAttack.Play();
        }
       
    }

    public void StartTrail()
    {
        if (isCharged == true)
        {
            chargedAttack.SetActive(true);
        } else
        {
            normalAttack.SetActive(true);
        }

    }

    public void EndTrail()
    {
        normalAttack.SetActive(false);
        chargedAttack.SetActive(false);
    }

    public void Ultimate()
    {
        Instantiate(ultimateAbility, this.transform.position, this.transform.rotation);
    }

    public void StartDashTrail()
    {
        dash.SetActive(true);
    }

    public void EndDashTrail()
    {
        dash.SetActive(false);
    }

    public void RightFootStep()
    {

        Instantiate(running, rightFoot.position, new Quaternion(0, 0, 0, 1));
    }

    public void LeftFootStep()
    {
        Instantiate(running, leftFoot.position, new Quaternion(0, 0, 0, 1));
    }
}
