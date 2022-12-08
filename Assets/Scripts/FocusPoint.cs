using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusPoint : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private void Start()
    {
        transform.rotation = Quaternion.identity;
    }
    private void Update()
    {
        xRotation += PlayerHandler.Instance.MouseX * Time.deltaTime * GameManager.Instance.Mouse_X_Speed;
        yRotation += PlayerHandler.Instance.MouseY * Time.deltaTime * GameManager.Instance.Mouse_Y_Speed;
        transform.position = PlayerHandler.Instance.transform.position + offset;
        transform.rotation = Quaternion.Euler(new Vector3(-yRotation, xRotation, 0f));
    }
}
