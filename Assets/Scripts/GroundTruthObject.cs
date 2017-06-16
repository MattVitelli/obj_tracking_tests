using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTruthObject : MonoBehaviour {
    public Renderer[] renderers = new Renderer[(int)TrackedObjectType.Count];
    public TrackedObjectType Type;

    // Use this for initialization
    void Start () {
        for(int it = 0; it < renderers.Length; it++)
        {
            if(renderers[it])
                renderers[it].enabled = (it == (int)Type);
        }
    }

    public TrackedObjectData GetTrackedObjectData()
    {
        TrackedObjectData tod = new TrackedObjectData();
        tod.position = this.transform.position;
        tod.rotation = this.transform.eulerAngles.y;
        tod.type = Type;
        return tod;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
