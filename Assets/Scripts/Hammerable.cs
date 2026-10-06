using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.OpenXR.Input;

public class Hammerable : MonoBehaviour
{
    [SerializeField] GameObject point;
    [SerializeField] Rigidbody rb;
    [SerializeField] MeshFilter cube;
    [SerializeField] MeshCollider collid;

    Mesh alterMesh;
    public float toughness = 1.0f;


    void Update()
    {
        rb.AddForce(Vector3.zero);

        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            HammerThis(67f, point.transform.position, new Vector3(0, 1, 0));
        }*/
    }

    private void Start()
    {
        Configure();
    }

    public void Configure()
    {
        alterMesh = cube.mesh;
        rb.sleepThreshold = 0.0f;
    }



    public void HammerThis(float power, Vector3 pos, Vector3 fwd)
    {
        Debug.Log("kyaaaaaaaaa :: " + power);
        Deform(power, pos, fwd);
    }

    public void Deform(float pow, Vector3 pos, Vector3 fwd)
    {
        Vector3 locPos = pos;//this.transform.InverseTransformPoint(pos); //      we SHOULD be transforming the point itself, not every vertex, but. lol.    (this is way easier)

        Debug.Log("loc: " + locPos);
        Debug.Log("fwd: " + fwd);

        DeformGeometry(pow, pos, fwd.normalized);

        // Recalc collider
        alterMesh.RecalculateBounds();
        collid.sharedMesh = alterMesh;
    }

    void DeformGeometry(float pow,Vector3 pos, Vector3 fwd)
    {
        Debug.Log("--------START----------");

        List<Vector3> newV = new List<Vector3>();

        foreach (Vector3 v in alterMesh.vertices)
        {
            Vector3 vv = v;

            Vector3 check = transform.TransformPoint(v);
            float dist = Vector3.Distance(check, pos);

            if (dist < 0.05)
            {
                vv = vv + fwd * (0.05f - dist) * (pow + 1.5f);
                //Debug.Log("Change!");
            }

            newV.Add(vv);
        }


        alterMesh.vertices = newV.ToArray();
        alterMesh.RecalculateNormals();

        Debug.Log("----------END----------------");
    }
}
