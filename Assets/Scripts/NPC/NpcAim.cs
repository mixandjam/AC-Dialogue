using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NpcAim : MonoBehaviour
{
    public Transform aimController;
    private Transform aimTarget;
    public MultiAimConstraint multiAim;

    void Update()
    {
        float weight = (aimTarget == null) ? 0 : 1;
        Vector3 pos = (aimTarget == null) ? transform.position + transform.forward : aimTarget.position;

        multiAim.weight = Mathf.Lerp(multiAim.weight, weight, .1f);
        aimController.position = Vector3.Lerp(aimController.position, pos, .1f);
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
