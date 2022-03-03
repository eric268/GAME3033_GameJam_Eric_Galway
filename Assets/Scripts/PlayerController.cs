using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerAttributes playerAttributes;
    public GameObject pauseCanvas;
    public GunController gunController;
    Animator gunAnimator;
    Camera mainCamera;
    public LayerMask targetLayerMask;
    [SerializeField]
    public TargetManager targetManager;
    public static bool gameIsPaused;
    [SerializeField]
    public Vector2 lookInput;
    public float aimSensativity;
    private readonly int reloadHashValue = Animator.StringToHash("ReloadPressed");
    private readonly int fireHashValue = Animator.StringToHash("FirePressed");

    private void Awake()
    {
        playerAttributes = GetComponent<PlayerAttributes>();
        gunAnimator = GetComponentInChildren<Animator>();
        gunController = GetComponentInChildren<GunController>();
        mainCamera = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
        pauseCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnFire(InputValue value)
    {
        if (gameIsPaused)
            return;

        if (!GameUIManager.gameActive || playerAttributes.isFiring || playerAttributes.isReloading) return;

        if (gunController.bulletsInClip <= 0)
        {
            SoundEffectManager.PlaySound("DryShot");
            playerAttributes.isReloading = true;
            gunAnimator.SetTrigger(reloadHashValue);
            return;
        }

        playerAttributes.isFiring = value.isPressed;
        if (playerAttributes.isFiring)
        {
            Fire();
            gunController.bulletsInClip--;
            gunAnimator.SetTrigger(fireHashValue);
        }

    }

    public void OnPause(InputValue value)
    {
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseCanvas.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void OnReload(InputValue value)
    {
        if (gameIsPaused)
            return;

        if (!GameUIManager.gameActive || playerAttributes.isFiring || playerAttributes.isReloading) return;

        if (gunController.bulletsInClip == gunController.maxClipSize) return;

        SoundEffectManager.PlaySound("StartReload");
        playerAttributes.isReloading = value.isPressed;
        gunAnimator.SetTrigger(reloadHashValue);
    }

    void RotatePlayer()
    {
        if (gameIsPaused)
            return;

        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + lookInput.x * aimSensativity, transform.eulerAngles.z + lookInput.y * aimSensativity);
        float angle = transform.eulerAngles.z;
        if (angle > 180 && angle < 300)
        {
            angle = 300.0f;
        }
        else if (angle < 180 && angle > 75)
        {
            angle = 75.0f;
        }
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, angle);
    }

    void Fire()
    {
        SoundEffectManager.PlaySound("Fire");
        Ray screenRay = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f));
        Vector3 hitLocation;
        if (Physics.Raycast(screenRay, out RaycastHit hit, 100, targetLayerMask))
        {
            hitLocation = hit.point;
            Vector3 hitDirection = hit.point - mainCamera.transform.position;
            //Debug.DrawRay(mainCamera.transform.position, hitDirection.normalized * 100, Color.green, 1);

           if (hit.collider.gameObject.GetComponentInParent<TargetAttributes>().isActive)
           {
                if (hit.collider.CompareTag("Head"))
                {
                    targetManager.TargetHit(true);
                    SoundEffectManager.PlaySound("Headshot");
                }
                else 
                    targetManager.TargetHit(false);
                return;
           }
        }
    }
}
