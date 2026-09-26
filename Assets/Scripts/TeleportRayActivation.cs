using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportRayActivation : MonoBehaviour
{
    public GameObject teleportRay;
    public InputActionProperty input;

    public InputActionProperty cancelInput;

    // Update is called once per frame
    void Update()
    {
        teleportRay.SetActive(input.action.ReadValue<float>() > 0.1f && cancelInput.action.ReadValue<float>() < 0.1f);
    }
}
