using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedBoard
{
   public List<SerializedTileInfo> tiles; 
}

[Serializable]
public class SerializedTileInfo
{
    public string type;
}

[Serializable]
public class SerializedCulture
{

}
