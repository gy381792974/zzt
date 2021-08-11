using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Await : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ExecuteAsync();
    }

    async void ExecuteAsync()
    {
        await Task.Run(() =>
        {
           System.Threading.Thread.Sleep(2000);
           Debug.Log("Async Executed");
       });
        
        Debug.Log("End");
    }


    // Update is called once per frame
    void Update()
    {
       // Debug.Log(1);
    }
}
