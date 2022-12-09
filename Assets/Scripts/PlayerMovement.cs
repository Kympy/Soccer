using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float MinShootPower = 25f;
    private float MaxShootPower = 50f;
    private Vector3 shootVector;
    private void Update()
    {
        Character_Movement();
        MovementAnimation();

        Debug.DrawRay(transform.position + new Vector3(0f, 0.5f, 0f), transform.forward * 10f, Color.cyan);
        if (myBall.activeSelf == false)
        {
            if (PlayerHandler.Instance.isCharge)
            {
                //shootVector = new Vector3(PlayerHandler.Instance.Horizontal, 0f, PlayerHandler.Instance.Vertical);
                shootVector = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
                shootVector.Normalize();
                forwardShootPower += Time.deltaTime * Random.Range(20f, 30f);
                upwardShootPower += Time.deltaTime * Random.Range(4f, 10f);
                UIManager.Instance.UpdateBar(forwardShootPower, MaxShootPower);
            }
            else if (PlayerHandler.Instance.isShoot)
            {
                if (forwardShootPower < MinShootPower)
                {
                    forwardShootPower = MinShootPower;
                }
                else if(forwardShootPower > MaxShootPower)
                {
                    forwardShootPower = MaxShootPower;
                }
                if(upwardShootPower > MaxShootPower * 0.5f)
                {
                    upwardShootPower = MaxShootPower * 0.5f;
                }
                PlayerHandler.Instance._animator.SetTrigger("Shoot");
            }
        }
        Character_Rotation();
    }
    private Vector3 moveDirection;
    private Vector3 finalMovement;
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;
    private float mySpeed = 0f;
    [SerializeField] private float accelRate = 10f;
    [SerializeField] private float rotateSpeed;
    public bool isShooting = false;
    private void Character_Movement()
    {
        if (PlayerHandler.Instance.IsMove == false) return;

        if (Input.GetKey(KeyCode.E))
        {
            PlayerHandler.Instance._animator.SetBool("IsSprint", true);
        }
        else
        {
            PlayerHandler.Instance._animator.SetBool("IsSprint", false);
        }
        moveDirection = new Vector3(PlayerHandler.Instance.Horizontal, 0f, PlayerHandler.Instance.Vertical);
        moveDirection.Normalize();
    }
    private void Character_Rotation()
    {
        if (PlayerHandler.Instance.IsMove == false) return;
        if (isShooting) return;
        if (PlayerHandler.Instance.isCharge || PlayerHandler.Instance.isShoot)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * rotateSpeed * 0.1f);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * rotateSpeed);
        }
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
        if (PlayerHandler.Instance.isSpin)
        {
            Spin();
        }
        else if (PlayerHandler.Instance.isChip)
        {
            Chip();         
        }
        else
        {
            myBall.GetComponent<Rigidbody>().AddForce((shootVector * forwardShootPower + transform.up * upwardShootPower), ForceMode.Impulse);
        }
        forwardShootPower = 0f;
        upwardShootPower = 0f;
        UIManager.Instance.UpdateBar(forwardShootPower, MaxShootPower);
        isShooting = false;
    }
    public void Spin()
    {
        Debug.LogWarning("Spin!");
        if (shootVector.x > 0)
        {
            myBall.GetComponent<Ball>().isLeft = false;
        }
        else
        {
            myBall.GetComponent<Ball>().isLeft = true;
        }
        myBall.GetComponent<Rigidbody>().AddForce((shootVector * forwardShootPower * 0.9f + transform.up * upwardShootPower), ForceMode.Impulse);
        myBall.GetComponent<Ball>().isSpin = true;
    }
    public void Chip()
    {
        Debug.LogWarning("Chip!");
        forwardShootPower = 11f;
        upwardShootPower = 10f;
        myBall.GetComponent<Rigidbody>().AddForce((shootVector * forwardShootPower * 0.5f + transform.up * upwardShootPower), ForceMode.Impulse);
    }
    public void LockRotation()
    {
        isShooting = true;
    }
}
