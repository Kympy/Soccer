using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private static PlayerHandler _instance = null;
    public static PlayerHandler Instance
    {
        get
        {
            _instance ??= GameObject.FindObjectOfType<PlayerHandler>();
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(_instance);
        }
        _instance = this;
    }
    public GameObject myPlayer { get; private set; } = null;
    public Rigidbody _rigidbody { get; private set; } = null;
    public Animator _animator { get; private set; } = null;
    public Transform playerForward { get; private set; } = null;
    private void Start()
    {
        myPlayer ??= this.gameObject;
        playerForward ??= GameObject.FindGameObjectWithTag("Forward").transform;
        if (myPlayer == null)
        {
            Debug.LogError("## Cannot find my Player ##");
            return;
        }
        if (myPlayer.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
        {
            _rigidbody = rigidbody;
        }
        if (myPlayer.TryGetComponent<Animator>(out Animator animator))
        {
            _animator = animator;
        }
    }
    public float Horizontal { get; private set; } = 0f;
    public float Vertical { get; private set; } = 0f;
    public bool IsMove { get; private set; } = false;
    public float MouseX { get; private set; } = 0f;
    public float MouseY { get; private set; } = 0f;

    public bool isSpin = false;
    public bool isChip = false;
    public bool isCharge = false;
    public bool isShoot = false;

    private void Update()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");

        IsMove = !Mathf.Approximately(Horizontal, 0f) || !Mathf.Approximately(Vertical, 0f);

        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");

        isCharge = Input.GetKey(KeyCode.D);
        isShoot = Input.GetKeyUp(KeyCode.D);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            isSpin = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            isChip = true;
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            isSpin = false;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            isChip = false;
        }
    }
}
