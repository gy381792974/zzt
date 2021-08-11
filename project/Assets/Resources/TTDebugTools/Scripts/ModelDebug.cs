using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelDebug : MonoBehaviour
{
    // Start is called before the first frame update
    public static ModelDebug Instance;

    public List<GameObject> objModels;


    public List<Transform> modelRes;

    public List<Transform> createTfpos;


    public List<Transform> createModelRangPos;

    public GameObject eliminateBox;

    private List<Rigidbody> rigidbodies = new List<Rigidbody>();

    public PhysicMaterial physicMaterial;

    private void Awake()
    {
        Instance = this;
        rigidbodies.Clear();

        eliminateBox.SetActive(true);
    }

    private void Start()
    {
        AddModel(144);
    }

    public void AddModel(int num)
    {
        for (int i = 1; i <= num; i++)
        {
            int index = i % 6;

            Transform tf = modelRes[index];

            GameObject go = tf.GetChild(Random.Range(0, tf.childCount)).gameObject;

            GameObject itemObj = GameObject.Instantiate(go, createTfpos[index]);

            itemObj.gameObject.SetActive(true);

            float offx = Random.Range(createModelRangPos[0].transform.position.x, createModelRangPos[1].transform.position.x);
            float offz = Random.Range(createModelRangPos[2].transform.position.z, createModelRangPos[3].transform.position.z);
            float offy = Random.Range(createModelRangPos[4].transform.position.y, createModelRangPos[5].transform.position.y);

            //float offy = createModelRangPos[5].transform.position.y;

            itemObj.transform.position = new Vector3(offx, offy, offz);

            Rigidbody rigidbody = itemObj.GetComponent<Rigidbody>();

            rigidbody.drag = 50;
            rigidbody.mass = 10;

            rigidbodies.Add(rigidbody);

            //itemObj.transform.GetChild(0).GetComponent<Collider>().material = physicMaterial;
        }

        createModelRangPos[0].parent.gameObject.SetActive(false);

        StartCoroutine(SetRigidbody());
    }

    IEnumerator SetRigidbody()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < rigidbodies.Count; i++)
        {
            //if (i % 10 == 0)
            //{
            //    yield return new WaitForSeconds(0.1f);
            //}

            rigidbodies[i].drag = 5;
        }

        yield return new WaitForSeconds(2f);

        eliminateBox.SetActive(false);
    }

    public int GetModelCount()
    {
        int modelCount = 0;

        for (int i = 0; i < createTfpos.Count; i++)
        {
            modelCount += createTfpos[i].childCount;

        }

        return modelCount;
    }

}
