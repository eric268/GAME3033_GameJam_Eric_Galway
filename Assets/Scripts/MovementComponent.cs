using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{
    PlayerController controller;
    Animator gunAnimator;
    [SerializeField]
    public Vector2 lookInput;
    public float aimSensativity;
    private readonly int reloadHashValue = Animator.StringToHash("ReloadPressed");
    private readonly int fireHashValue = Animator.StringToHash("FirePressed");

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        gunAnimator = GetComponentInChildren<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation =  Quaternion.Euler(0, transform.eulerAngles.y + lookInput.x * aimSensativity, transform.eulerAngles.z + lookInput.y * aimSensativity);
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnFire(InputValue value)
    {
        if (controller.isFiring || controller.isReloading) return;
        controller.isFiring = value.isPressed;
        if (controller.isFiring)
            gunAnimator.SetTrigger(fireHashValue);
    }

    public void OnReload(InputValue value)
    {
        if (controller.isFiring || controller.isReloading) return;

        controller.isReloading = value.isPressed;
        gunAnimator.SetTrigger(reloadHashValue);
    }

    public void OnPause(InputValue value)
    {
        Time.timeScale = 0.0f;
    }
}
