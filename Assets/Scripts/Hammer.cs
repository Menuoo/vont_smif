using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public Rigidbody rb;
    public float velocityRequirement = 5f;
    

    public float cooldown = 0.1f;
    bool ready = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(rb.velocity.magnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("fuck");

        Hammerable obj = collision.gameObject.GetComponent<Hammerable>();
        float power = collision.GetContact(0).impulse.magnitude;

        if (obj != null && ready && power > velocityRequirement)
        {
            Debug.Log("Impulse: " + power);
            obj.HammerThis(5f, collision.GetContact(0).point, collision.GetContact(0).impulse.normalized);

            ready = false;
            Invoke("Reload", cooldown);
        }
    }

    public void Reload()
    {
        Debug.Log("trigger reload");
        ready = true;
    }

}
