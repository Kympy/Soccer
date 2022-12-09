using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
    }
    [SerializeField] private Image Z;
    [SerializeField] private Image D;
    [SerializeField] private Image Q;
    [SerializeField] private TextMeshProUGUI result;
    [SerializeField] private Image bar;
    private Color origin;
    private float alpha;
    private Color temp;
    private void Start()
    {
        origin = Z.color;
        alpha = origin.a;
        temp = new Color(Color.gray.r, Color.gray.g, Color.gray.b, alpha);
    }
    private void Update()
    {
        if (PlayerHandler.Instance.isSpin)
        {
            Z.color = temp;
        }
        else
        {
            Z.color = origin;
        }
        if (PlayerHandler.Instance.isChip)
        {
            Q.color = temp;
        }
        else
        {
            Q.color = origin;
        }
        if (PlayerHandler.Instance.isCharge)
        {
            D.color = temp;
        }
        else
        {
            D.color = origin;
        }
        if (PlayerHandler.Instance.isChip && PlayerHandler.Instance.isCharge)
        {
            result.text = "Chip";
        }
        else if (PlayerHandler.Instance.isSpin && PlayerHandler.Instance.isCharge)
        {
            result.text = "Spin";
        }
        else if (PlayerHandler.Instance.isCharge)
        {
            result.text = "Shoot";
        }
        else
        {
            result.text = "";
        }
    }
    public void UpdateBar(float current, float max)
    {
        bar.fillAmount = current / max;
    }
}
