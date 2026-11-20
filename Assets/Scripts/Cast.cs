using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cast : MonoBehaviour
{
    public CastArea dada;

    public Collider coll;
    public GameObject lavaMetal;

    public Mesh[] meshes;
    public Renderer[] prefabs;
    public CastType castType;
    public bool canAttach = true;

    void Start()
    {
        Cook(0f);
        MeshFilter mesh = GetComponent<MeshFilter>();
        mesh.mesh = meshes[(int)castType];

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
            transform.parent.gameObject.AddComponent<Rigidbody>();
    }

    public Renderer GetCastObject()
    {
        return prefabs[(int)castType];
    }

    public void Unparent()
    {
        /*Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
            transform.parent.gameObject.AddComponent<Rigidbody>();*/

        if (dada != null)
        {
            Debug.Log("dada :(");

            transform.SetParent(null);
            GetComponent<Collider>().isTrigger = false;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
            dada.currentCast = null;
            dada = null;
        }
    }

    public void CanAttach(bool val)
    {
        Debug.Log("selected");
        canAttach = val;
    }

    public void Cook(float alpha)
    {
        lavaMetal.transform.localPosition = Vector3.up * Mathf.Lerp(-0.5f, 0.98f, alpha);
    }
}

public enum CastType { Axe, Handle, Guard, Blade, Shield }