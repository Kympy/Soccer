using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float MinShootPower = 25f;
    private float MaxShootPower = 50f;
    private Vector3 shootVector;
    [SerializeField] private GameObject arrow;
    private void Update()
    {
        Charge();
        Character_Movement();
        MovementAnimation();
        Character_Rotation();
    }
    private Vector3 moveDirection;
    [Header("Movement")]
    [SerializeField] private float rotateSpeed;
    private bool isShooting = false;
    private void Character_Movement()
    {
        if (PlayerHandler.Instance.IsMove == false) return;
        moveDirection = new Vector3(PlayerHandler.Instance.Horizontal, 0f, PlayerHandler.Instance.Vertical);
        moveDirection.Normalize();
        if (Input.GetKey(KeyCode.E))
        {
            PlayerHandler.Instance._animator.SetBool("IsSprint", true);
        }
        else
        {
            PlayerHandler.Instance._animator.SetBool("IsSprint", false);
        }
    }
    private void Character_Rotation()
    {
        if (PlayerHandler.Instance.IsMove == false) return;
        if (isShooting) return;
        if (PlayerHandler.Instance.isCharge || PlayerHandler.Instance.isShoot)
        {
            return;
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
    [SerializeField] private Transform leftUp;
    [SerializeField] private Transform rightUp;
    [SerializeField] private Transform leftDown;
    [SerializeField] private Transform rightDown;
    [SerializeField] private Transform middle;
    [SerializeField] private Transform leftMiddle;
    [SerializeField] private Transform rightMiddle;
    private void Charge()
    {
        Debug.DrawRay(transform.position + new Vector3(0f, 0.5f, 0f), transform.forward * 10f, Color.cyan);
        if (myBall.activeSelf == false)
        {
            if (PlayerHandler.Instance.isPrepare)
            {
                shootVector = middle.position - transform.position;
            }
            if (PlayerHandler.Instance.isCharge)
            {
                Vector3 direction;
                forwardShootPower += Time.deltaTime * Random.Range(20f, 30f);
                //shootVector = Vector3.Lerp(shootVector, new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")), Time.deltaTime * 4f);
                // Right
                if (PlayerHandler.Instance.Horizontal > 0f)
                {
                    // Right Up
                    if (PlayerHandler.Instance.Vertical > 0f)
                    {
                        direction = rightUp.position - transform.position;
                        upwardShootPower += Time.deltaTime * Random.Range(2f, 8f);
                    }
                    // RightMiddle
                    else if (PlayerHandler.Instance.Vertical == 0f)
                    {
                        direction = rightMiddle.position - transform.position;
                        upwardShootPower += Time.deltaTime * Random.Range(1f, 3f);
                    }
                    // RightDown
                    else
                    {
                        direction = rightDown.position - transform.position;
                        upwardShootPower = 0f;
                    }
                }
                // Left
                else if (PlayerHandler.Instance.Horizontal < 0f)
                {
                    // LeftUp
                    if (PlayerHandler.Instance.Vertical > 0f)
                    {
                        direction = leftUp.position - transform.position;
                        upwardShootPower += Time.deltaTime * Random.Range(2f, 8f);
                    }
                    // LeftMiddle
                    else if (PlayerHandler.Instance.Vertical == 0f)
                    {
                        direction = leftMiddle.position - transform.position;
                        upwardShootPower += Time.deltaTime * Random.Range(1f, 3f);
                    }
                    else
                    {
                        direction = leftDown.position - transform.position;
                        upwardShootPower = 0f;
                    }
                }
                else
                {
                    direction = middle.position - transform.position;
                    upwardShootPower += Time.deltaTime * Random.Range(1f, 3f);
                }
                shootVector = Vector3.Lerp(shootVector, direction, Time.deltaTime);
                shootVector.Normalize();
                Debug.DrawRay(transform.position, shootVector * 100f, Color.yellow);
                UIManager.Instance.UpdateBar(forwardShootPower, MaxShootPower);
            }
            if (PlayerHandler.Instance.isShoot)
            {
                if (forwardShootPower < MinShootPower)
                {
                    forwardShootPower = MinShootPower;
                }
                else if (forwardShootPower > MaxShootPower)
                {
                    forwardShootPower = MaxShootPower;
                }
                if (upwardShootPower > MaxShootPower * 0.5f)
                {
                    upwardShootPower = MaxShootPower * 0.5f;
                }
                PlayerHandler.Instance._animator.SetTrigger("Shoot");
            }
        }
    }
    private Vector3 tempMinus;
    private float tempDelta;
    private IEnumerator TurnDirectly()
    {
        Vector3 minus = shootVector - transform.forward;
        float degree = Mathf.Atan2(minus.y, minus.x) * Mathf.Rad2Deg;
        float sum = 0f;
        tempDelta = 0f;
        while (sum < degree)
        {
            tempMinus = shootVector - transform.forward;
            tempDelta = Mathf.Atan2(tempMinus.y, tempMinus.x) * Mathf.Rad2Deg;
            sum += tempDelta;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(shootVector), Time.deltaTime * rotateSpeed * 4f);
            yield return null;
        }
        Debug.Log("Clear");
    }
    public GameObject myBall;
    public Transform foot;
    private float forwardShootPower = 0f;
    private float upwardShootPower = 0f;
    public void Shoot()
    {
        myBall.SetActive(true);
        myBall.transform.position = foot.transform.position + Vector3.up * 0.2f;
        myBall.transform.rotation = Quaternion.identity;
        StartCoroutine(TurnDirectly());
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
            myBall.GetComponent<Rigidbody>().AddForce(shootVector * forwardShootPower + transform.up * upwardShootPower, ForceMode.Impulse);
        }
        PlayerHandler.Instance.isSpin = false;
        PlayerHandler.Instance.isChip = false;
        forwardShootPower = 0f;
        upwardShootPower = 0f;
        UIManager.Instance.UpdateBar(forwardShootPower, MaxShootPower);
        isShooting = false;
    }
    public void Spin()
    {
        Debug.LogWarning("Spin!");
        //Debug.Log(Vector3.Cross(transform.forward, shootVector).y);
        // Right
        if (Vector3.Cross(-middle.forward, shootVector).y > 0)
        {
            myBall.GetComponent<Ball>().isLeft = false;
        }
        else
        {
            myBall.GetComponent<Ball>().isLeft = true;
        }
        myBall.GetComponent<Rigidbody>().AddForce(0.9f * forwardShootPower * shootVector + transform.up * upwardShootPower, ForceMode.Impulse);
        myBall.GetComponent<Ball>().isSpin = true;
    }
    public void Chip()
    {
        Debug.LogWarning("Chip!");
        forwardShootPower = 11f;
        upwardShootPower = 10f;
        myBall.GetComponent<Rigidbody>().AddForce(0.5f * forwardShootPower * shootVector + transform.up * upwardShootPower, ForceMode.Impulse);
    }
    public void LockRotation()
    {
        isShooting = true;
    }
    public void UnLockRotation()
    {
        isShooting = false;
    }
}
