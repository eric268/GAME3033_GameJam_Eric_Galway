using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{
    PlayerController controller;
    [SerializeField]
    public Vector2 lookInput;
    public float aimSensativity;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
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
        print(lookInput);
    }

    public void OnFire(InputValue value)
    {
        controller.isFiring = value.isPressed;
    }

    public void OnReload(InputValue value)
    {
        controller.isReloading = value.isPressed;
    }

    public void OnPause(InputValue value)
    {
        Time.timeScale = 0.0f;
    }
}
