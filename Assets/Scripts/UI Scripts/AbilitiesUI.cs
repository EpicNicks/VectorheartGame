using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesUI : MonoBehaviour
{
    public GameObject mask;
    private float TestTimer;
    private float TestCoolDown;
    //private float ratio;
    private void Start()
    {
        //ratio = 1/2;
    }
    public void SetSize(float sizeNormalized)
    {
        mask.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100f + sizeNormalized *100);
    }
}
