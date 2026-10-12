using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.OpenXR.Input;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

public class Hammerable : MonoBehaviour
{
    //[SerializeField] GameObject point;
    [SerializeField] Rigidbody rb;
    [SerializeField] MeshFilter cube;
    [SerializeField] MeshCollider collid;
    [SerializeField] bool interpolated = true;


    Mesh alterMesh;
    Mesh savedMesh;
    public float toughness = 1.0f;

    [Range(0, 1)]
    public float cubicity = 1;


    void Update()
    {
        rb.AddForce(Vector3.zero);

        //InterpolateGeometry(0f);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            HammerThis(5f, new Vector3(0, 1, 0), new Vector3(0, 1, 0));
        }
    }

    private void Start()
    {
        Configure();
    }

    public void Configure()
    {
        cubicity = 1.05f;
        savedMesh = Object.Instantiate(cube.mesh);
        alterMesh = cube.mesh;
        rb.sleepThreshold = 0.0f;
    }



    public void HammerThis(float power, Vector3 pos, Vector3 fwd)
    {
        Debug.Log("kyaaaaaaaaa :: " + power);

        if (interpolated)
            InterpolateGeometry(power);
        else
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

    void InterpolateGeometry(float pow)
    {
        Debug.Log("--------START----------");

        float val = pow / 100f;
        cubicity = Mathf.Max(0f, cubicity - val);
        List<Vector3> newV = new List<Vector3>();

        foreach (Vector3 v in savedMesh.vertices)
        { }

        foreach (Vector3 v in savedMesh.vertices)
        {
            Vector3 vv = v;
            Vector3 ingotFactors = Vector3.one;

            // spherize
            vv = v.normalized;

            // cubize
            int i;
            float r, a;
            // major axis and size
            a = Mathf.Abs(vv.x);
            r = a; i = 0;
            a = Mathf.Abs(vv.y); 
            if (r < a) { r = a; i = 1; }
            a = Mathf.Abs(vv.z); 
            if (r < a) { r = a; i = 2; }
            vv = vv / r;
            r *= 1.75f; 
            a = 4.0f * r / Mathf.PI;

            // convert of cube + linearization
            if (i == 0) { vv.y = a * Mathf.Atan(vv.y / r); vv.z = a * Mathf.Atan(vv.z / r); }
            else if (i == 1) { vv.x = a * Mathf.Atan(vv.x / r); vv.z = a * Mathf.Atan(vv.z / r); }
            else { vv.x = a * Mathf.Atan(vv.x / r); vv.y = a * Mathf.Atan(vv.y / r); }
            // just remedy boundaries after linearization
            if (vv.x < -1.0f) vv.x = -1.0f;
            if (vv.x > +1.0f) vv.x = +1.0f;
            if (vv.y < -1.0f) vv.y = -1.0f;
            if (vv.y > +1.0f) vv.y = +1.0f;
            if (vv.z < -1.0f) vv.z = -1.0f;
            if (vv.z > +1.0f) vv.z = +1.0f;

            //ingotize
            if (vv.y <= 1f && vv.y > 0.8f)
            {
                float factor = Mathf.Lerp(1f, 0.95f, (vv.y - 0.8f) / 0.2f);
                ingotFactors.x *= factor; //* Mathf.Sin(vv.y * 10f);
                ingotFactors.z *= factor;
            }
            else
            {
                float factor = Mathf.Lerp(1.3f, 1f, (vv.y + 1.0f) / 1.8f);
                ingotFactors.x *= factor; //* Mathf.Sin(vv.y * 10f);
                ingotFactors.z *= factor;
            }
            if (vv.y >= -1f && vv.y < -0.8f)
            {
                float factor = Mathf.Lerp(0.95f, 1f, (vv.y + 1.0f) / 0.2f);
                ingotFactors.x *= factor; //* Mathf.Sin(vv.y * 10f);
                ingotFactors.z *= factor;
            }

            vv.Scale(new Vector3(0.5f, 0.25f, 1f));
            vv.Scale(ingotFactors);
            vv = vv * 1.2f;


            // interpolate final vertex
            vv = Vector3.Lerp(v, vv, cubicity);
            newV.Add(vv);
        }


        alterMesh.vertices = newV.ToArray();
        alterMesh.RecalculateNormals();
        alterMesh.RecalculateBounds();
        collid.sharedMesh = alterMesh;

        Debug.Log("----------END----------------");
    }
}
