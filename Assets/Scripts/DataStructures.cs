using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrackedObjectType
{
    Type0,
    Type1,
    Type2,
    Type3,
    Disabled,
    Count
}

public struct TrackedObjectData
{
    public Vector3 position;
    public float rotation;
    public TrackedObjectType type;
    public int id;
}