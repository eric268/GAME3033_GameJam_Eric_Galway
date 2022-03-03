using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{
    PlayerController controller;
    GunController gunController;
    Animator gunAnimator;
    Camera mainCamera;
    public LayerMask targetLayerMask;

    [SerializeField]
    public TargetManager targetManager;

    [SerializeField]
    public Vector2 lookInput;
    public float aimSensativity;
    private readonly int reloadHashValue = Animator.StringToHash("ReloadPressed");
    private readonly int fireHashValue = Animator.StringToHash("FirePressed");

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        gunAnimator = GetComponentInChildren<Animator>();
        gunController = GetComponentInChildren<GunController>();
        mainCamera = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
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
        if (controller.isFiring || controller.isReloading) return;

        if (gunController.bulletsInClip <= 0)
        {
            controller.isReloading = true;
            gunAnimator.SetTrigger(reloadHashValue);
            return;
        }

        controller.isFiring = value.isPressed;
        if (controller.isFiring)
        {
            Fire();
            gunController.bulletsInClip--;
            gunAnimator.SetTrigger(fireHashValue);
        }

    }

    public void OnReload(InputValue value)
    {
        if (controller.isFiring || controller.isReloading) return;

        if (gunController.bulletsInClip == gunController.maxClipSize) return;

        controller.isReloading = value.isPressed;
        gunAnimator.SetTrigger(reloadHashValue);
    }

    public void OnPause(InputValue value)
    {
        Time.timeScale = 0.0f;
    }

    void RotatePlayer()
    {
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
        Ray screenRay = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f));
        Vector3 hitLocation;
        if (Physics.Raycast(screenRay, out RaycastHit hit, 100, targetLayerMask))
        {
            hitLocation = hit.point;
            Vector3 hitDirection = hit.point - mainCamera.transform.position;
            Debug.DrawRay(mainCamera.transform.position, hitDirection.normalized * 100, Color.green, 1);

           if (hit.collider.gameObject.GetComponentInParent<TargetAttributes>().isActive)
           {
                targetManager.TargetHit();
                return;
           }
        }
        else if (Physics.Raycast(screenRay, out hit, 100))
        {
            hitLocation = hit.point;
            Vector3 hitDirection = hit.point - mainCamera.transform.position;
            Debug.DrawRay(mainCamera.transform.position, hitDirection.normalized * 100, Color.red, 1);
        }
    }
}
