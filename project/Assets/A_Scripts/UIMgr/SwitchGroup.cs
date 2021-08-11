using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGroup : MonoBehaviour
{
    List<Switch> Switches = new List<Switch>();
    Switch lastSwitch;
    public void AddSwitch(Switch sw)
    {
        Switches.Add(sw);
    }
    public List<Switch> GetSwitches()
    {
        return Switches;
    }

    public void SwitchesController(Switch sw)
    {
        if (lastSwitch == null || lastSwitch == sw)
        {
            lastSwitch = sw;
            return;
        }
        else
        {
            lastSwitch.SetCloseState();
        }
        //sw.isOn = true;

        //for (int i = 0; i < Switches.Count; i++)
        //{
        //    if (Switches[i].isOn)
        //    {
        //        Switches[i].isOn = false;
        //        break;
        //    }
        //}
    }

}
