using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private GoalKeeper goalKeeper;
    private void Awake()
    {
        goalKeeper = GameObject.FindObjectOfType<GoalKeeper>();
        Hide();
    }
    private void OnEnable()
    {
        goalKeeper.Chase = true;
        Invoke(nameof(Hide), 3f);
    }
    private void Hide()
    {
        this.gameObject.SetActive(false);
        goalKeeper.KeeperReset();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GoalTrigger"))
        {
            goalKeeper.Goal();
        }
    }
}
