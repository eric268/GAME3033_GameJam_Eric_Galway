using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public TargetAttributes[] targets;
    public int totalTargetsHit;
    public int currentActiveTarget;
    float timeToNextActiveTarget = 3.0f;
    float timeUntilDeactivateTarget = 2.5f;
    public float targetHitMoveAmount;
    public GameUIManager gameUIManager;
    // Start is called before the first frame update
    void Start()
    {
        targets = GetComponentsInChildren<TargetAttributes>();
        InvokeRepeating("ActivateTarget", 3.0f, timeToNextActiveTarget);
    }

    void ActivateTarget()
    {
        currentActiveTarget = Random.Range(0, targets.Length);
        targets[currentActiveTarget].SetIsActive(true);
        SoundEffectManager.PlaySound("ActivateTarget");
        Invoke("DeactivateTarget", timeUntilDeactivateTarget);
    }

    void DeactivateTarget()
    {
        targets[currentActiveTarget].SetIsActive(false);
    }

    public void TargetHit()
    {
        totalTargetsHit++;
        DeactivateTarget();
        CancelInvoke("DeactivateTarget");
        UpdateTargetTime();
        SoundEffectManager.PlaySound("Hit");
        gameUIManager.TargeHit(targetHitMoveAmount);
    }
    
    void UpdateTargetTime()
    {
        if (totalTargetsHit % 5 == 0)
        {
            if (timeUntilDeactivateTarget > 0.5f)
            {
                gameUIManager.TargeHit(targetHitMoveAmount);
                gameUIManager.boarderMovementSpeed += 0.05f;
                targetHitMoveAmount += 10.0f;
                CancelInvoke("ActivateTarget");
                timeToNextActiveTarget -= 0.1f;
                timeUntilDeactivateTarget -= 0.1f;
                InvokeRepeating("ActivateTarget", timeToNextActiveTarget, timeToNextActiveTarget);
            }
        }
    }
}
