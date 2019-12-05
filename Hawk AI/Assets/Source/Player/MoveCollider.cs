using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCollider : MonoBehaviour
{
    [SerializeField]
    bool isEnable = false;

    [System.NonSerialized]
    public RaycastHit hit;

    public void JudgeCollision(Vector3 _vec)
    {
        var scale = transform.lossyScale.x * 0.5f;
        var scaley = transform.lossyScale.y * 0.2f;

        var isHit = Physics.BoxCast(this.transform.position, new Vector3(scale, scaley, scale) /*Vector3.one * scale*/, _vec, out hit);
    }


    void OnDrawGizmos()
    {
        if (isEnable == false)
            return;

        var scale = transform.lossyScale.x * 0.5f;
        var scaley = transform.lossyScale.y * 0.2f;

        var isHit = Physics.BoxCast(this.transform.position, new Vector3(scale, scaley, scale) /*Vector3.one * scale*/, this.transform.forward, out hit);
        if (isHit)
        {
            Gizmos.DrawRay(this.transform.position, this.transform.forward * hit.distance);
            Gizmos.DrawWireCube(this.transform.position + this.transform.forward * hit.distance, new Vector3(scale, scaley, scale)/*Vector3.one * scale * 2*/);
        }
        else
        {
            Gizmos.DrawRay(this.transform.position, this.transform.forward * 100);
        }
    }
}
