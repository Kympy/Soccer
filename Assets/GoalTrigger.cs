using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private GameObject effect;
    private void Start()
    {
        effect = Resources.Load<GameObject>("Goal");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Instantiate(effect, other.ClosestPoint(transform.position), Quaternion.identity);
        }
    }
}
