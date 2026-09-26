using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HandAnimator : MonoBehaviour
{
    public InputActionProperty triggerValue;
    public InputActionProperty gripValue;

    public Animator anim;

    int animTrigger = Animator.StringToHash("Trigger");
    int animGrip = Animator.StringToHash("Grip");


    // Update is called once per frame
    void Update()
    {
        float trigger = triggerValue.action.ReadValue<float>();
        float grip = gripValue.action.ReadValue<float>();

        anim.SetFloat(animTrigger, trigger);
        anim.SetFloat(animGrip, grip);
    }
}
