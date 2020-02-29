using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    private Quaternion _noRotation;
    private void Start()
    {
        _noRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.rotation = _noRotation;
    }
}
