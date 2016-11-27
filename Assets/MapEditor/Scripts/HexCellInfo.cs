using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class HexCellInfo
{
    public int elevation;

    [NonSerialized]
    public Color cellColor;
    
    public float[] _myColor = new float[4];

    public bool hasIncomingRiver, hasOutgoingRiver;
    public HexDirection incomingRiver, outgoingRiver;

    public void TransformColor()
    {
        _myColor[0] = cellColor.r;
        _myColor[1] = cellColor.g;
        _myColor[2] = cellColor.b;
        _myColor[3] = cellColor.a;
    }

    public void LoadColor()
    {
        cellColor.r = _myColor[0];
        cellColor.g = _myColor[1];
        cellColor.b = _myColor[2];
        cellColor.a = _myColor[3];
    }

}
