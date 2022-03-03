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
    // Start is called before the first frame update
    void Start()
    {
        targets = GetComponentsInChildren<TargetAttributes>();
        InvokeRepeating("ActivateTarget", 3.0f, timeToNextActiveTarget);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void ActivateTarget()
    {
        currentActiveTarget = Random.Range(0, targets.Length);
        targets[currentActiveTarget].SetIsActive(true);

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
        print(totalTargetsHit);
    }
}
