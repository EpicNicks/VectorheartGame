using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHealthBar : MonoBehaviour
{
    public GameObject mask;
    private float ratio;


    // Start is called before the first frame update
    //void Start()
    //{
    //    mask.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 80);
    //}
    private void Start()
    {
        ratio = 82f / 100f;
    }

    public void SetSize(float sizeNormalized)
    {
        mask.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 18f+ sizeNormalized * ratio);
    }
}
