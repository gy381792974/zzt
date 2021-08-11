using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadingFlag : Singleton<SceneLoadingFlag>
{
    public bool LoadedGameMgr = true;
    public bool LoadedUIRoot = true;
}
