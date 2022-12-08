using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardEffect : MonoBehaviour
{
    private GameObject guard = null;

    private void Start()
    {
        guard = Resources.Load<GameObject>("Guard");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Instantiate(guard, collision.contacts[0].point, Quaternion.identity);
        }
    }
}
