using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachRoot : MonoBehaviour
{
    AttachPoint[] points;
    public List<Collider> accruedColliders = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        points = GetComponentsInChildren<AttachPoint>();
        foreach (AttachPoint point in points) 
        {
            point.root = this;
        }

        accruedColliders.Add(GetComponent<Collider>());
    }
}
