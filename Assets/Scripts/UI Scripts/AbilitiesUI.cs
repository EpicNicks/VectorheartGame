using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesUI : MonoBehaviour
{
    public GameObject mask;
    private float TestTimer;
    private float TestCoolDown;



    private void Update()
    {
        TestTimer += Time.deltaTime;
        TestCoolDown = TestTimer * 33f;
        mask.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100f+ TestCoolDown);

        //test code

        //if(isCoolDown == true)
        //{
        //    mask.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100f + RemainingCoolDown);
        //}else
        //{
        //    mask.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100f);
        //}

    }
    public void SetSize(float sizeNormalized)
    {
        mask.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100f + sizeNormalized);
    }
}
