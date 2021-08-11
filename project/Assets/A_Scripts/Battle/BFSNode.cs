using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSNode : MonoBehaviour
{
    //public Transform trans;
    public Vector3 Pos;
    public bool finded;

    public void Init()
    {
        Pos = transform.position;
        finded = false;
    }
}
