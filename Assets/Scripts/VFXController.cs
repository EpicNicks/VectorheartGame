using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{

    public GameObject normalAttack;
    public GameObject spinAttack;
    public GameObject ultimateAbility;
    public GameObject dash;
    public GameObject running;

    public Transform feet;
    public Transform spinLocation;


    private Animator playerAnim;
    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
    }

    public void SpinAttack()
    {
        Instantiate(spinAttack, spinLocation.position, this.transform.rotation);
    }

    public void StartTrail()
    {
        normalAttack.SetActive(true);
    }

    public void EndTrail()
    {
        normalAttack.SetActive(false);
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

    public void Running()
    {
        Instantiate(running, feet.position, this.transform.rotation);
    }
}
