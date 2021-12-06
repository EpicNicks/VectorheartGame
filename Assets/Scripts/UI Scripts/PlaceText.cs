using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceText : MonoBehaviour
{
    public GameObject text1;
    public GameObject text2;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(placeText());
    }

    private IEnumerator placeText()
    {

            text1.SetActive(true);
            yield return new WaitForSeconds(3f);
            text1.SetActive(false);
            text2.SetActive(true);
            yield return new WaitForSeconds(3f);
            text2.SetActive(false);

    }
}
