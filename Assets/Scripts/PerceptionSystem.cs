using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionSystem : MonoBehaviour {
    public Transform worldParent;
    public TrackedObjectManager manager;
    public float updateFreq = 0.2f;
    float timeSinceLastUpdate = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate >= updateFreq)
        {
            timeSinceLastUpdate = 0.0f;
            float radius = this.transform.localScale.magnitude * 0.5f;
            GroundTruthObject[] objs = worldParent.GetComponentsInChildren<GroundTruthObject>();
            List<TrackedObjectData> tods = new List<TrackedObjectData>();
            int idx = 0;
            foreach(GroundTruthObject obj in objs)
            {
                if((obj.transform.position - this.transform.position).magnitude < radius)
                {
                    TrackedObjectData tod = obj.GetTrackedObjectData();
                    tod.id = idx;
                    idx++;
                    tods.Add(tod);
                }
            }
            manager.OnTrackedObjectUpdate(tods);
        }
	}
}
