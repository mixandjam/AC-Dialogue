using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class VillagerAim : MonoBehaviour
{
    public Transform aimController;
    public Transform aimTarget;
    public MultiAimConstraint multiAim;

    void Update()
    {
        float weight = (aimTarget == null) ? 0 : 1f;
        Vector3 pos = (aimTarget == null) ? transform.position + transform.forward + Vector3.up : aimTarget.position + Vector3.up;

        multiAim.weight = Mathf.Lerp(multiAim.weight, weight, .05f);
        aimController.position = Vector3.Lerp(aimController.position, pos, .3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            aimTarget = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            aimTarget = null;
    }

}
