using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCollider : MonoBehaviour
{
    [SerializeField]
    bool isEnable = false;

    [System.NonSerialized]
    public RaycastHit hit;

    public void JudgeCollision()
    {
        var scale = transform.lossyScale.x;

        var isHit = Physics.BoxCast(this.transform.position, Vector3.one * scale, this.transform.forward, out hit);
    }

    void OnDrawGizmos()
    {
        if (isEnable == false)
            return;

        var scale = transform.lossyScale.x * 0.5f;

        var isHit = Physics.BoxCast(this.transform.position, Vector3.one * scale, this.transform.forward, out hit, this.transform.rotation);
        if (isHit)
        {
            Gizmos.DrawRay(this.transform.position, this.transform.forward * hit.distance);
            Gizmos.DrawWireCube(this.transform.position + this.transform.forward * hit.distance, Vector3.one * scale * 2);
        }
        else
        {
            Gizmos.DrawRay(this.transform.position, this.transform.forward * 100);
        }
    }
}
