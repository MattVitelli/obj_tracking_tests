using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedObject : MonoBehaviour {
    public Renderer[] renderers = new Renderer[(int)TrackedObjectType.Count];
    public float[] scores = new float[(int)TrackedObjectType.Count];
    public TrackedObjectType bestClassification = TrackedObjectType.Disabled;
    public Material TrackedObjectMaterial;
    public TrackedObjectManager Manager;
    public int ID = -1;

    public float TimeFadeAmount = 0.0f;
    public float TimeSinceLastUpdate = 0.0f;
    const float TimeTillFade = 1.0f;
    const float TimeTillInactive = TimeTillFade + 2.0f;

    public void ResetObject(int id)
    {
        if (scores.Length != (int)TrackedObjectType.Count)
            scores = new float[(int)TrackedObjectType.Count];

        for(int it = 0; it < scores.Length; it++)
        {
            scores[it] = 0;
        }

        bestClassification = TrackedObjectType.Disabled;
        TimeSinceLastUpdate = 0.0f;
        TimeFadeAmount = TimeTillFade;
        ID = id;
    }

    public void OnTrackedObjectUpdate(TrackedObjectData data)
    {
        TimeSinceLastUpdate = 0;
        TimeFadeAmount = 0;// Mathf.Max(0.0f, TimeFadeAmount -= Time.deltaTime);
        scores[(int)data.type] += 1;

        for(int it = 0; it < scores.Length; it++)
        {
            if(scores[it] > scores[(int)bestClassification])
            {
                bestClassification = (TrackedObjectType)it;
            }
        }

        for(int it = 0; it < renderers.Length; it++)
        {
            if(renderers[it] != null)
            {
                renderers[it].enabled = (it == (int)bestClassification);
            }
        }

        this.transform.position = data.position;
        Vector3 eulerAngles = this.transform.eulerAngles;
        eulerAngles.y = data.rotation;
        this.transform.eulerAngles = eulerAngles;
    }

    void SetupMaterials()
    {
        for (int it = 0; it < renderers.Length; it++)
        {
            if(renderers[it] != null)
                renderers[it].material = Instantiate(TrackedObjectMaterial);
        }
    }
    public void SetupObject()
    {
        SetupMaterials();
        ResetObject(-1);
    }
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        TimeSinceLastUpdate += Time.deltaTime;
        TimeFadeAmount = Mathf.Min(TimeTillFade, TimeFadeAmount + Time.deltaTime);
        if (TimeSinceLastUpdate >= TimeTillInactive)
        {
            Manager.RecycleTrackedObject(this);
        }

        int objIdx = (int)bestClassification;
        if(renderers[objIdx])
        {
            float alpha = Mathf.Clamp01(1.0f - (TimeSinceLastUpdate - TimeTillFade) / (TimeTillInactive - TimeTillFade));
            renderers[objIdx].material.SetFloat(Shader.PropertyToID("_Confidence"), alpha);
        }
	}
}
