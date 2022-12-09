using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private GoalKeeper goalKeeper;
    public bool isSpin = false;
    public bool isLeft = false;
    private Rigidbody rigid;
    [SerializeField] private Transform spinVec;
    private float timer = 0f;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        goalKeeper = GameObject.FindObjectOfType<GoalKeeper>();
        Hide();
    }
    private void OnEnable()
    {
        goalKeeper.Chase = true;
        Invoke(nameof(Hide), 3f);
    }
    private void Update()
    {
        if (isSpin == false) return;
        timer += Time.deltaTime;
        if (timer > 1.4f)
        {
            isSpin = false;
            return;
        }
        if(timer >= 0.4f)
        {
            if (isLeft == false)
            {
                rigid.AddForce(510f * Time.deltaTime * spinVec.right, ForceMode.Acceleration);

            }
            else
            {
                rigid.AddForce(510f * Time.deltaTime * -spinVec.right, ForceMode.Acceleration);
            }
        }
    }
    private void Hide()
    {
        isSpin = false;
        this.gameObject.SetActive(false);
        timer = 0f;
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
