using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private void Start()
    {
        Hide();
    }
    private void OnEnable()
    {
        Invoke(nameof(Hide), 3f);
    }
    private void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
