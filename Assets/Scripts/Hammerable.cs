using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;
using UnityEngine.XR.OpenXR.Input;

public class Hammerable : MonoBehaviour
{
    //[SerializeField] GameObject point;
    //[SerializeField] Rigidbody rb;
    [SerializeField] MeshFilter cube;
    [SerializeField] MeshCollider collid;

    [Header("Deform Settings")]
    [SerializeField] bool interpolated = true;
    [SerializeField] float seed = 14658.4564654564f;
    [SerializeField] float scale = 10f;
    [SerializeField] int bumpAmount = 10;
    [SerializeField] float bumpSize = 0.1f;
    [SerializeField] float maxDeform = 0.5f;

    [Header("Other")]
    [SerializeField] Vector3 debugPos = Vector3.zero;
    [Range(0, 1)]
    public float cubicity = 1;

    Heatable heatable;
    Rigidbody rb;

    Mesh alterMesh;
    Mesh savedMesh;
    public float toughness = 1.0f;


    void Update()
    {
        //InterpolateGeometry(0f);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            HammerThis(5f, debugPos, new Vector3(0, 1, 0));
        }
    }

    private void FixedUpdate()
    {
        if (rb != null)
            rb.AddForce(Vector3.zero);
    }

    private void Start()
    {
        heatable = GetComponent<Heatable>();
        rb = GetComponent<Rigidbody>();
        Configure();
    }

    public void Configure()
    {
        cubicity = 1.0f;
        savedMesh = Instantiate(cube.mesh);
        alterMesh = cube.mesh;

        if (rb != null)
            rb.sleepThreshold = 0.0f;

        if (interpolated)
        {
            InterpolateGeometry(0f);
        }
        else
        {
            DeformGeometry(0f, Vector3.zero, Vector3.zero, true);
        }
    }



    public void HammerThis(float power, Vector3 pos, Vector3 fwd)
    {
        Debug.Log("kyaaaaaaaaa i am [" + gameObject.name + "] :: " + power);

        if (interpolated)
            InterpolateGeometry(power);
        else
            Deform(power, pos, fwd);
    }

    public void Deform(float pow, Vector3 pos, Vector3 fwd)
    {
        Vector3 locPos = pos; //this.transform.InverseTransformPoint(pos); //      we SHOULD be transforming the point itself, not every vertex, but. lol.    (this is way easier)

        //Debug.Log("loc: " + locPos);
        //Debug.Log("fwd: " + fwd);

        DeformGeometry(maxDeform, locPos, fwd.normalized, false); // pow excluded for now, pain in the ass (replaced with maxDeform)

        /*
        alterMesh.RecalculateBounds();
        collid.sharedMesh = alterMesh;*/
    }

    void DeformGeometry(float pow, Vector3 pos, Vector3 fwd, bool init)
    {
        //Debug.Log("--------START----------");

        List<Vector3> newV = new List<Vector3>();

        int i = 0;
        foreach (Vector3 v in savedMesh.vertices)
        {
            Vector3 vv = alterMesh.vertices[i];

            if (!init)
            {
                float diff = Vector3.Distance(vv, v);

                Vector3 check = transform.TransformPoint(vv);
                float dist = Vector3.Distance(check, pos);

                //Debug.Log(dist);

                if (dist < maxDeform * 3f)
                {
                    //vv = vv + fwd * (0.05f - dist) * (pow + 1.5f);
                    //vv *= 0.9f;
                    //Debug.Log("Change!");

                    float final = pow * (((maxDeform * 3f - dist) / (maxDeform * 3f)) + 0.3f);

                    if (heatable != null)
                        final *= (heatable.temperature * heatable.temperature);

                    //final = 0.5f * maxDeform;

                    if (diff - final > 0.001f)
                        final = 1f - (diff - final) / diff;
                    else
                        final = 1f;

                    vv = Vector3.Lerp(vv, v, final);
                }
            }

            newV.Add(vv);
            i++;
        }

        if (init)
        {
            int bumps = 0;
            while (bumps < bumpAmount)
            {
                int randInd = (int)(Mathf.Min(UnityEngine.Random.value * savedMesh.vertices.Length, savedMesh.vertices.Length - 1));
                Vector3 vv = newV[randInd];
                Vector3 nrml = savedMesh.normals[randInd];

                for (int j = 0; j < newV.Count; j++)
                {
                    float dist = Vector3.Distance(vv, newV[j]);
                    if (dist < bumpSize)
                    {
                        newV[j] += nrml * ((bumpSize - dist) / bumpSize) * maxDeform;
                    }
                }
                bumps++;
            }
        }


        alterMesh.vertices = newV.ToArray();
        alterMesh.RecalculateNormals();
        alterMesh.RecalculateBounds();
        collid.sharedMesh = alterMesh;

        //Debug.Log("----------END----------------");
    }




    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////





    void InterpolateGeometry(float pow)
    {
        Debug.Log("--------START----------");
        float heatFactor = 1f;
        if (heatable != null)
        {
            heatFactor = Mathf.Max(heatable.temperature * heatable.temperature, 0f) * 1f;
        }

        float val = pow / 100f * heatFactor;
        cubicity = Mathf.Max(0f, cubicity - val);


        List<Vector3> newV = new List<Vector3>();

        float[] bounds = { 10000f, -10000f, 10000f, -10000f, 10000f, -10000f }; // min and max bounds for x, y, z
        float maxBound = 0f;
        Vector3 sphereScale = Vector3.one;
        Vector3 midPoint = Vector3.zero;

        // get spherization parameters
        foreach (Vector3 v in savedMesh.vertices)
        {
            if (v.x < bounds[0])
                bounds[0] = v.x;
            if (v.x > bounds[1])
                bounds[1] = v.x;

            if (v.y < bounds[2])
                bounds[2] = v.y;
            if (v.y > bounds[3])
                bounds[3] = v.y;

            if (v.z < bounds[4])
                bounds[4] = v.z;
            if (v.z > bounds[5])
                bounds[5] = v.z;

            if (bounds[1] - bounds[0] > bounds[3] - bounds[2])
                maxBound = bounds[1] - bounds[0];
            else
                maxBound = bounds[3] - bounds[2];
            if (bounds[5] - bounds[4] > maxBound)
                maxBound = bounds[5] - bounds[4];
        }
        sphereScale = new Vector3(1f / ((bounds[1] - bounds[0]) / maxBound), 1f / ((bounds[3] - bounds[2]) / maxBound), 1f / ((bounds[5] - bounds[4]) / maxBound));
        //sphereScale *= 2f;
        midPoint = new Vector3((bounds[1] + bounds[0]) / 2f,(bounds[3] + bounds[2]) / 2f, (bounds[5] + bounds[4]) / 2f);

        Debug.Log(bounds[0] + " " + bounds[1] + " " + bounds[2] + " " + bounds[3] + " " + bounds[4] + " " + bounds[5] + " ");
        Debug.Log(sphereScale);
        Debug.Log(midPoint);

        foreach (Vector3 v in savedMesh.vertices)
        {
            Vector3 vv = v;

            // spherize
            vv = v;// + midPoint;
            vv = vv * (Mathf.Log10(1f / (vv.magnitude + 0.05f)) + 2f);
            //vv.Scale(sphereScale);
            vv = vv.normalized;


            // cubize
            vv = Cubize(vv);
            vv = vv.normalized;
            vv = Cubize(vv);
            vv = vv.normalized;
            vv = Cubize(vv);
            vv = vv.normalized;
            vv = Cubize(vv);
            vv = vv.normalized;
            vv = Cubize(vv);

            //ingotize
            vv = Ingotize(vv);


            vv.Scale(new Vector3(0.5f, 0.25f, 1f)); // rectangular shape
            vv = vv * 0.5f + midPoint;

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

    Vector3 Cubize(Vector3 vv)
    {
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

        Vector3 perfect = new Vector3(Mathf.Round(vv.x), Mathf.Round(vv.y), Mathf.Round(vv.z));
        //vv = Vector3.Lerp(vv, perfect, cubicity);

        return vv;
    }

    Vector3 Ingotize(Vector3 vv)
    {
        Vector3 ingotFactors = Vector3.one;
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
        vv.Scale(ingotFactors);

        return vv;
    }




}