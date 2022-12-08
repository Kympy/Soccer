using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardVector : MonoBehaviour
{
    [SerializeField] private Transform focusPoint = null;
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, focusPoint.eulerAngles.y, 0f);
    }
}
