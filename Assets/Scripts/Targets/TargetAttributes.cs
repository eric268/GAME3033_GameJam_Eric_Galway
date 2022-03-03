using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAttributes : MonoBehaviour
{
    public bool isActive;
    public Animator animator;
    public int isActiveHash = Animator.StringToHash("isActive");
    // Start is called before the first frame update

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    public void SetIsActive(bool active)
    {
        isActive = active;
        animator.SetBool(isActiveHash, isActive);
    }
}
