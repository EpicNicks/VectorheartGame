using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesUI : MonoBehaviour
{
    public GameObject mask;
    private float TestTimer;
    private float TestCoolDown;

    //Test
    //void Start()
    //{
    //    mask.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 120);
    //}


    private void Update()
    {
        TestTimer += Time.deltaTime;
        TestCoolDown = TestTimer * 33f;
        mask.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100f+ TestCoolDown);
    }
    public void SetSize(float sizeNormalized)
    {
        mask.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100f + sizeNormalized);
    }
}
