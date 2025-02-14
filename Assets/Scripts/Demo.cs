using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Demo : MonoBehaviour
{
    public void OnTryPlayReload(InputAction.CallbackContext context)
    {
        Debug.Log("Reload is Working");
    }
}
