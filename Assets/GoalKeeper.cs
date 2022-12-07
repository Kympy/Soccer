using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GoalKeeper : MonoBehaviour
{
    [SerializeField] private Transform startPos = null;
    [SerializeField] private GameObject myBall = null;
    private Rigidbody _rigidbody = null;
    private Animator _animator = null;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        Keep();
    }
    private Vector3 direction;
    public float keepSpeed = 5f;
    private float timer = 0f;
    public float startDelay = 0.3f;
    public float diveDistance = 1.2f;
    private bool Chase = false;
    public float diveHeight = 1f;
    private void Keep()
    {
        if (myBall.activeSelf == false)
        {
            transform.position = startPos.position;
            transform.rotation = startPos.rotation;
            timer = 0f;
            Chase = true;
            _animator.SetBool("IsMove", false);
            return;
        }

        timer += Time.deltaTime;
        if (timer > startDelay && Chase == true)
        {
            direction = myBall.transform.position - transform.position;
            Vector3 temp = direction;
            Debug.Log(direction.y);
            direction.y = 0f;
            _rigidbody.MovePosition(_rigidbody.position + direction * Time.deltaTime * keepSpeed);
            _animator.SetBool("IsMove", true);
            if (direction.sqrMagnitude < diveDistance)
            {
                if (temp.y > diveHeight)
                {
                    _animator.SetInteger("Dive", 1);
                }
                else if (temp.y < 0.1f)
                {
                    _animator.SetInteger("Dive", 4);
                }
                else
                {
                    if (Vector3.Cross(transform.forward, temp).y < 0)
                    {
                        _animator.SetInteger("Dive", 3);
                    }
                    else
                    {
                        _animator.SetInteger("Dive", 2);
                    }
                }
                _animator.SetBool("IsMove", false);
                Chase = false;
            }
        }
    }
}
