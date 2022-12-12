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
        // If ball isn't exist.
        if (myBall.activeSelf == false)
        {
            Debug.DrawRay(transform.position + rayOffset, transform.forward * 10f, Color.red);
            // Look player
            transform.LookAt(player.transform);
            return;
        }
        // If ball is active, calculate ball to keeper direction vector.
        direction = myBall.transform.position - (transform.position + rayOffset);
        // Normal state
        if (Dive == false && Chase == true)
        {
            if (direction.sqrMagnitude > chaseDistance)
            {
                return;
            }
            _rigidbody.MovePosition(_rigidbody.position + transform.forward * Time.deltaTime * keepSpeed);
            _animator.SetBool("IsMove", true);
            // If diving distance is enough.
            if (direction.sqrMagnitude <= diveDistance)
            {
                Dive = true;
                // Judge where to dive.
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
        // Ball is higher than keeper height
        if (ballToMe.y > rayOffset.y)
        {
            // Cross keeper's forward vector and ball direction vector.
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
        // Ball height is lower than keeper's height.
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
        else
        {
            _animator.SetBool("IsMove", false);
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
        _rigidbody.AddForce(Vector3.up * jumpPower * 0.5f, ForceMode.Impulse);
    }
    public void RightUp()
    {
        _rigidbody.AddForce(transform.right * jumpPower, ForceMode.Impulse);
        _rigidbody.AddForce(Vector3.up * jumpPower * 0.5f, ForceMode.Impulse);
    }
    public void UpJump()
    {
        _rigidbody.AddForce(Vector3.up * jumpPower * 0.7f, ForceMode.Impulse);
    }
}
