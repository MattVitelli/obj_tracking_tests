using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedObjectManager : MonoBehaviour {
    public GameObject TrackedObjectPrefab;
    public int TrackedObjectPoolSize = 100;
    public Transform activeObjectParent;
    public Transform inactiveObjectParent;

    List<TrackedObject> trackedObjectsPool = new List<TrackedObject>();
    List<TrackedObject> activeObjects = new List<TrackedObject>();
    
    TrackedObject createTrackedObject()
    {
        GameObject obj = Instantiate(TrackedObjectPrefab);
        TrackedObject tobj = obj.GetComponent<TrackedObject>();
        tobj.Manager = this;
        tobj.SetupObject();
        return tobj;
    }

	// Use this for initialization
	void Start () {
		for(int it = 0; it < TrackedObjectPoolSize; it++)
        {
            TrackedObject tobj = createTrackedObject();
            tobj.transform.parent = inactiveObjectParent;
            tobj.gameObject.SetActive(false);
            trackedObjectsPool.Add(tobj);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RecycleTrackedObject(TrackedObject to)
    {
        activeObjects.Remove(to);
        to.transform.parent = inactiveObjectParent;
        to.gameObject.SetActive(false);
        trackedObjectsPool.Add(to);
    }

    public void OnTrackedObjectUpdate(List<TrackedObjectData> trackedObjects)
    {
        for(int it = 0; it < trackedObjects.Count; it++)
        {
            TrackedObjectData tod = trackedObjects[it];
            bool anyFound = false;
            for(int j = 0; j < activeObjects.Count; j++)
            {
                TrackedObject to = activeObjects[j];
                if(to.ID == tod.id)
                {
                    anyFound = true;
                    to.OnTrackedObjectUpdate(tod);
                    break;
                }
            }
            if(!anyFound)
            {
                TrackedObject to = null;
                if (trackedObjectsPool.Count > 0)
                {
                    int lastIdx = trackedObjectsPool.Count - 1;
                    to = trackedObjectsPool[lastIdx];
                    trackedObjectsPool.RemoveAt(lastIdx);
                    to.gameObject.SetActive(true);
                    Debug.Log("Recycling");
                }
                else
                {
                    Debug.Log("Created tracked object");
                    to = createTrackedObject();
                }
                to.ResetObject(tod.id);
                activeObjects.Add(to);
                to.transform.parent = activeObjectParent;
                to.OnTrackedObjectUpdate(tod);
                
            }
        }
    }
}
