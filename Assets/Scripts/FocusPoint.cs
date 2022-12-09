using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusPoint : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    private void Update()
    {
        transform.position = PlayerHandler.Instance.transform.position + offset;
    }
}
