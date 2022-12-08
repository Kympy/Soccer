using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GoalKeeper : MonoBehaviour
{
    [SerializeField] private Transform startPos = null;
    [SerializeField] private GameObject myBall = null;
    private Rigidbody _rigidbody = null;
    public Animator _animator = null;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F2))
        //{
        //    Time.timeScale = 1f;
        //}
        Keep();
    }
    private Vector3 direction;
    public float keepSpeed = 5f;
    public float diveDistance = 1.2f;
    public bool Dive = false;
    public bool Chase = false;
    public Vector3 rayOffset;
    public GameObject player;
    public float chaseDistance = 5f;
    private void Keep()
    {
        if (Vector3.Distance(transform.position + rayOffset, player.transform.position) < diveDistance)
        {
            Debug.DrawLine(transform.position + rayOffset, player.transform.position, Color.magenta);
        }

        Debug.DrawRay(transform.position + rayOffset, transform.right * 2f, Color.red);
        Debug.DrawRay(transform.position + rayOffset, -transform.right * 2f, Color.red);
        if (myBall.activeSelf == false)
        {
            Debug.DrawRay(transform.position + rayOffset, transform.forward * 10f, Color.red);
            transform.LookAt(player.transform);
            return;
        }

        //timer += Time.deltaTime;
        direction = myBall.transform.position - (transform.position + rayOffset);

        if (Dive == false && Chase == true)
        {
            //Vector3 lookBall = myBall.transform.position;
            //lookBall.y = 0f;
            //transform.LookAt(lookBall);
            Debug.Log("Distance = " + direction.sqrMagnitude);
            if (direction.sqrMagnitude > chaseDistance)
            {
                return;
            }
            _rigidbody.MovePosition(_rigidbody.position + transform.forward * Time.deltaTime * keepSpeed);
            _animator.SetBool("IsMove", true);
            if (direction.sqrMagnitude < diveDistance)
            {
                Dive = true;
                Judgement();
            }
        }
    }
    public float jumpPower = 50f;
    public void Judgement()
    {
        Debug.DrawRay(transform.position + rayOffset, transform.forward * 5f, Color.magenta, 5f);
        Vector3 ballToMe = myBall.transform.position - (transform.position + rayOffset);
        Debug.DrawLine(transform.position + rayOffset, myBall.transform.position, Color.yellow, 5f);
        Debug.Log("Cross Y : " + Vector3.Cross(transform.forward, ballToMe).y);
        //Time.timeScale = 0f;
        if (ballToMe.y > rayOffset.y)
        {
            if (Vector3.Cross(transform.forward, ballToMe).y > 0.35f)
            {
                Debug.Log("UpRight");
                _animator.SetInteger("Dive", 6);
            }
            else if (Vector3.Cross(transform.forward, ballToMe).y < -0.35f)
            {
                Debug.Log("UpLeft");
                _animator.SetInteger("Dive", 5);
            }
            else
            {
                Debug.Log("Up");
                _animator.SetInteger("Dive", 1);
            }
        }
        else if (ballToMe.y <= rayOffset.y)
        {
            if (Vector3.Cross(transform.forward, ballToMe).y > 0.35f) // right
            {
                Debug.Log("DownRight");
                _animator.SetInteger("Dive", 3);
            }
            else if (Vector3.Cross(transform.forward, ballToMe).y < -0.35f) // left
            {
                Debug.Log("DownLeft");
                _animator.SetInteger("Dive", 2);
            }
            else 
            {
                Debug.Log("Down");
                _animator.SetInteger("Dive", 4);
            }
        }
    }
    public void Reset()
    {
        _animator.SetInteger("Dive", 0);
        _animator.SetBool("IsMove", false);
    }
    public void KeeperReset()
    {
        transform.position = startPos.position;
        transform.rotation = startPos.rotation;
        _animator.SetBool("IsMove", false);
        Dive = false;
    }
    public void Goal()
    {
        _animator.SetInteger("Dive", 0);
        _animator.SetBool("IsMove", false);
        Chase = false;
    }
    public void LeftDownJump()
    {
        _rigidbody.AddForce(-transform.right * jumpPower, ForceMode.Impulse);
        _rigidbody.AddForce(Vector3.up * jumpPower * 0.3f, ForceMode.Impulse);
    }
    public void RightDownJump()
    {
        _rigidbody.AddForce(transform.right * jumpPower, ForceMode.Impulse);
        _rigidbody.AddForce(Vector3.up * jumpPower * 0.3f, ForceMode.Impulse);
    }
    public void LeftUp()
    {
        _rigidbody.AddForce(-transform.right * jumpPower, ForceMode.Impulse);
        _rigidbody.AddForce(Vector3.up * jumpPower * 0.6f, ForceMode.Impulse);
    }
    public void RightUp()
    {
        _rigidbody.AddForce(transform.right * jumpPower, ForceMode.Impulse);
        _rigidbody.AddForce(Vector3.up * jumpPower * 0.6f, ForceMode.Impulse);
    }
    public void UpJump()
    {
        _rigidbody.AddForce(Vector3.up * jumpPower * 0.8f, ForceMode.Impulse);
    }
}
