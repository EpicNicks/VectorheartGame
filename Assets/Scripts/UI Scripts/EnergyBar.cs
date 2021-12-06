using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour
{
    public GameObject mask;
    private float ratio;


    private void Start()
    {

        //energy ratio not sure
        ratio = 82f / 100f;
    }

    public void SetSize(float sizeNormalized)
    {
        mask.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 18f + sizeNormalized * ratio * 100);
    }
}