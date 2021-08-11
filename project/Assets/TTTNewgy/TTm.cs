using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTm : MonoBehaviour
{
    // Start is called before the first frame update


    int count1;
    int count2;
    void Update()
    {
        Debug.LogError("Update ");
    }

    private void FixedUpdate()
    {
        Debug.LogWarning("FixedUpdate ");
    }
}
