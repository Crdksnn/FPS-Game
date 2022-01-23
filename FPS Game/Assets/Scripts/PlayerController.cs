using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float runSpeed = 12f;
    CharacterController charControl;
    private Vector3 moveInput;

    [Header("Gravity Settings")]
    private Vector3 velocity;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;

    [Header("Camera Settings")]
    [SerializeField] Transform camTransform;
    [SerializeField] float mouseSensivity = 3f;
    [SerializeField] bool invertX = false;
    [SerializeField] bool invertY = false;

    [Header("Jump Settings")]
    [SerializeField] float jumpHeight = 3f;
    bool canDoubleJump;

    [Header("Move - Run Animation")]
    [SerializeField] Animator anim;

    [Header("Gun Settings")]
    public Gun activeGun;
    public List<Gun> allGuns = new List<Gun>();
    public List<Gun> unlockableGuns = new List<Gun>();
    public int currentGun;

    [Header("ADS Setings")]
    public Transform adsPoint;
    public Transform gunHolder;
    public Transform gunStartPos;
    public float adsSpeed;

    public static PlayerController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(transform.gameObject);
            DontDestroyOnLoad(transform.gameObject);
        }
    }

    void Start()
    {
        charControl = GetComponent<CharacterController>();

        if(UIController.instance != null)
        {
            UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;
        }
       
        activeGun = allGuns[currentGun];
        activeGun.gameObject.SetActive(true);

    }

    
    void Update()
    {

        //Check ground control && Gravity
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        charControl.Move(velocity * Time.deltaTime);

        //Movement
        Vector3 verticalMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horizontalMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = verticalMove + horizontalMove;
        moveInput = Vector3.ClampMagnitude(moveInput, 1f);

        //Run Speed
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            moveInput = moveInput * runSpeed;
        }

        else
        {
            moveInput = moveInput * moveSpeed;
        }

        charControl.Move(moveInput * Time.deltaTime);

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * (-2f) * gravity);
            canDoubleJump = true;
        }
        //Double jump
        else if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * (-2f) * gravity);
            canDoubleJump = false;
        }

        //Control camera rotation
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"),Input.GetAxisRaw("Mouse Y")) * mouseSensivity;

        if (invertX)
        {
            mouseInput.x = -mouseInput.x;
        }

        if (invertY)
        {
            mouseInput.y = -mouseInput.y;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
        camTransform.rotation = Quaternion.Euler(camTransform.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));

        //Shooting
        //Handle Shots
        if (Input.GetMouseButtonDown(0) && activeGun.fireCounter <= 0)
        {
            CrossHairControl();
            FireShot();
        }

        //Repeating Shots
        if (Input.GetMouseButton(0) && activeGun.canAutoFire)
        {
            CrossHairControl();
            if (activeGun.fireCounter <= 0)
            {
                FireShot();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchGun();
        }

        if (Input.GetMouseButtonDown(1))
        {
            CameraController.instance.ZoomIn(activeGun.zoomAmount);
        }

        if (Input.GetMouseButton(1))
        {
            gunHolder.position = Vector3.MoveTowards(gunHolder.position, adsPoint.position, adsSpeed * Time.deltaTime);
        }
        else
        {
            gunHolder.position = Vector3.MoveTowards(gunHolder.position, gunStartPos.position, adsSpeed * Time.deltaTime);
        }

        if (Input.GetMouseButtonUp(1))
        {
            CameraController.instance.ZoomOut();
            
        }

        //Animation
        anim.SetFloat("moveSpeed", moveInput.magnitude);
        anim.SetBool("onGround", isGrounded);
    }

    public void FireShot()
    {
        if(activeGun.currentAmmo > 0)
        {
            //CrossHairControl();

            activeGun.currentAmmo--;
            if(UIController.instance != null)
            {
                UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;
            }
            Instantiate(activeGun.bullet, activeGun.firePoint.position, activeGun.firePoint.rotation);
            activeGun.fireCounter = activeGun.fireRate;
        }
    }

    private void CrossHairControl()
    {
        //Settings of aim and crosshair
        RaycastHit hit;

        //Physics.Raycast(startPosition, rotation, out interact object, distance)
        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, 50f))
        {
            if (Vector3.Distance(camTransform.position, hit.point) > 2f)
            {
                activeGun.firePoint.LookAt(hit.point);
            }
            activeGun.firePoint.LookAt(hit.point);
        }
        else
        {
            activeGun.firePoint.LookAt(camTransform.position + (camTransform.forward) * 30);
        }
    }

    public void SwitchGun()
    {

        activeGun.gameObject.SetActive(false);

        currentGun++;

        if(currentGun >= allGuns.Count)
        {
            currentGun = 0;
        }

        activeGun = allGuns[currentGun];
        activeGun.gameObject.SetActive(true);

        UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

    }

    public void AddGun(string gunToAdd)
    {
        bool gunUnlocked = false;

        if(unlockableGuns.Count > 0)
        {
            for(int i = 0; i < unlockableGuns.Count; i++)
            {
                if (unlockableGuns[i].gunName == gunToAdd)
                {
                    gunUnlocked = true;
                    allGuns.Add(unlockableGuns[i]);
                    unlockableGuns.RemoveAt(i);

                    i = unlockableGuns.Count;
                }
                    
            }
        }

        if (gunUnlocked)
        {
            currentGun = allGuns.Count - 2;
            SwitchGun();
        }

    }

}
