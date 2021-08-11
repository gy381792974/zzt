using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MyTest
{
     void  AA();
}


public class UpdateMgr : MonoBehaviour, MyTest
{
    private MyTest myTest;

    private void Start()
    {
        myTest = new UpdateMgr();
    }
    void Update()
    {

        //Update_FullFire();
        //Update_HalfSecond();
        //Update_OneSecond();
        //Update_TwoSecond();

        myTest.AA();
    }
    
    public virtual void Update_FullFire()
    {
       //Debug.Log(1);
    }

    public virtual void Update_HalfSecond()
    {
        //Debug.Log(2);
    }

    public virtual void Update_OneSecond()
    {
        
    }

    public virtual void Update_TwoSecond()
    {
       
    }

    public void AA()
    {
        
    }
}
