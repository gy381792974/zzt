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
        if (lastSwitch == null && sw.IsOn)
        {
            lastSwitch = sw;
        }
        else if (lastSwitch != sw && sw.IsOn)
        {
            lastSwitch.SetCloseState();
            lastSwitch = sw;
        }
    }

    public void SetLastSwitch(Switch sw)
    {
        if (sw.IsOn)
        {
            lastSwitch = sw;
        }
    }

}
