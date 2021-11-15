using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHealthBar : MonoBehaviour
{
    public GameObject mask;
    // Start is called before the first frame update
    void Start()
    {
        mask.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 80);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
