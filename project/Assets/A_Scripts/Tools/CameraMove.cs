using UnityEngine;
using EazyGF;

public class CameraMove : MonoBehaviour
{
    public Transform left;
    public Transform right;
    public Transform up;
    public Transform down;
    public float speed;

    void Start()
    {
        
    }

    // Update is called once per frame

    float lastX;
    float lastZ;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastX = Input.mousePosition.x;
            lastZ = Input.mousePosition.y;
        }

        if (Input.GetMouseButton(0) && UIPanelManager.Instance.GetDicShowingUI() < 2)
        {
            MoveCamera2();
        }
    }

    public void MoveCamera()
    {
        float offx = Input.mousePosition.x - lastX;

        float endX = transform.position.x + (offx * speed * Time.deltaTime);

        if (endX < left.position.x || endX > right.position.x)
        {
            endX = transform.position.x;
        }

        float offZ = Input.mousePosition.y - lastZ;
        float endZ = transform.position.z + (offZ * speed * Time.deltaTime);
        if (endZ < down.position.z || endZ > up.position.z)
        {
            endZ = transform.position.z;
        }

        Vector3 pos = transform.position;
        pos.x = endX;
        pos.z = endZ;

        //if (Mathf.Abs(offx) > Mathf.Abs(offY))
        //{
        //    pos.x = endX;

        //}
        //else
        //{
        //    pos.z = endZ;
        //}

        //Debug.LogError("px  " + transform.position.x  + " pz \n" + transform.position.z +
        //    " ox " +  offx + " oz " + offZ  + "\n l " + left.position.x + " r " + right.position.x + " u " + down.position.x + " d " + up.position.x);

        transform.position = pos;

        lastX = Input.mousePosition.x;
        lastZ = Input.mousePosition.y;
    }

    public void MoveCamera2()
    {
        float offx = lastX - Input.mousePosition.x;
        float offZ = lastZ -Input.mousePosition.y;

        float y = transform.position.y;

        Vector3 dirFor= transform.up * offZ * speed * Time.deltaTime;

        Vector3 dirRight = transform.right * offx * speed * Time.deltaTime;
        Vector3 pos = transform.position + dirFor + dirRight;
        pos.y = y;

        if (pos.x < left.position.x || pos.x > right.position.x || pos.z < down.position.z || pos.z > up.position.z)
        {
            pos = transform.position;
        }

        transform.position = pos;

        lastX = Input.mousePosition.x;
        lastZ = Input.mousePosition.y;
    }
}
