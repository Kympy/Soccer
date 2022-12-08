using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private void FixedUpdate()
    {


    }
    private void Update()
    {
        Character_Movement();
        Character_Rotation();
        MovementAnimation();
        //Reset_Movement();
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerHandler.Instance._animator.SetTrigger("Pass");
        }
        Debug.DrawRay(transform.position + new Vector3(0f, 0.5f, 0f), transform.forward * 10f, Color.cyan);
        if (myBall.activeSelf == false)
        {
            if (Input.GetKey(KeyCode.D))
            {
                forwardShootPower += Time.deltaTime * Random.Range(20f, 30f);
                upwardShootPower += Time.deltaTime * Random.Range(4f, 10f);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                Debug.Log(forwardShootPower);
                if (forwardShootPower < 15f)
                {
                    forwardShootPower = 15f;
                    upwardShootPower = 0f;
                }
                PlayerHandler.Instance._animator.SetTrigger("Shoot");
            }
        }
    }
    private Vector3 moveDirection;
    private Vector3 finalMovement;
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;
    private float mySpeed = 0f;
    [SerializeField] private float accelRate = 10f;
    [SerializeField] private float rotateSpeed;
    private void Character_Movement()
    {
        if (PlayerHandler.Instance.IsMove == false) return;

        //mySpeed += Time.deltaTime * accelRate;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            PlayerHandler.Instance._animator.SetBool("IsSprint", true);
            //if (mySpeed > runSpeed)
            //{
            //    mySpeed = runSpeed;
            //}
        }
        else
        {
            PlayerHandler.Instance._animator.SetBool("IsSprint", false);
            //if (mySpeed > moveSpeed)
            //{
            //    mySpeed = moveSpeed;
            //}
        }
        //moveDirection = PlayerHandler.Instance.Horizontal * PlayerHandler.Instance.playerForward.right + 
        //    PlayerHandler.Instance.Vertical * PlayerHandler.Instance.playerForward.forward;
        moveDirection = new Vector3(PlayerHandler.Instance.Horizontal, 0f, PlayerHandler.Instance.Vertical);
        moveDirection.Normalize();
        //finalMovement = PlayerHandler.Instance._rigidbody.position + (mySpeed * Time.deltaTime * moveDirection);
        //PlayerHandler.Instance._rigidbody.MovePosition(finalMovement);
    }
    private void Reset_Movement()
    {
        if (PlayerHandler.Instance.IsMove == true) return;
            mySpeed = 0f;
    }
    private void Character_Rotation()
    {
        if (PlayerHandler.Instance.IsMove == false) return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * rotateSpeed);
    }
    private void MovementAnimation()
    {
        PlayerHandler.Instance._animator.SetBool("IsMove", PlayerHandler.Instance.IsMove);
    }
    public GameObject myBall;
    public Transform foot;
    public float forwardShootPower = 0f;
    public float upwardShootPower = 0f;
    public void Shoot()
    {
        myBall.SetActive(true);
        myBall.transform.position = foot.transform.position;
        myBall.GetComponent<Rigidbody>().AddForce((transform.forward * forwardShootPower + transform.up * upwardShootPower), ForceMode.Impulse);
        forwardShootPower = 0f;
        upwardShootPower = 0f;
    }
}
