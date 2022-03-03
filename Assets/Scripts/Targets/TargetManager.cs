using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public TargetAttributes[] targets;
    public int totalTargetsHit;
    public int currentActiveTarget;
    float timeToNextActiveTarget = 2.5f;
    float timeUntilDeactivateTarget = 2.25f;
    float headShotMultiplier = 1.5f;
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

    public void TargetHit(bool headShot)
    {
        totalTargetsHit++;
        DeactivateTarget();
        CancelInvoke("DeactivateTarget");
        UpdateTargetTime();
        SoundEffectManager.PlaySound("Hit");

        if (headShot)
        {
            gameUIManager.TargeHit(targetHitMoveAmount * headShotMultiplier);
        }
        else
        {
            gameUIManager.TargeHit(targetHitMoveAmount);
        }

    }
    
    void UpdateTargetTime()
    {
        if (totalTargetsHit % 5 == 0)
        {
            if (timeUntilDeactivateTarget > 0.2f)
            {
                gameUIManager.TargeHit(targetHitMoveAmount);
                gameUIManager.boarderMovementSpeed += 0.05f;
                targetHitMoveAmount += 5.0f;
                CancelInvoke("ActivateTarget");
                timeToNextActiveTarget -= 0.1f;
                timeUntilDeactivateTarget -= 0.1f;
                InvokeRepeating("ActivateTarget", timeToNextActiveTarget, timeToNextActiveTarget);
            }
        }
    }
}
