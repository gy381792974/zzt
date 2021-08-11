using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TNavMesh : MonoBehaviour
{
    // Start is called before the first frame update
    public NavMeshAgent a;

    void Start()
    {
        a = transform.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
             //Transform tf = GetCoinFromMouseClick(1);

            a.SetDestination(GetCoinFromMouseClick(1));
        }
    }

    Vector3 GetCoinFromMouseClick(int layout)
    {
        RaycastHit hitInfo = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(ray, out hitInfo, 1000, 1 << layout);
        if (hit)
        {
            return hitInfo.point;
        }

        return Vector3.zero;
    }


}
