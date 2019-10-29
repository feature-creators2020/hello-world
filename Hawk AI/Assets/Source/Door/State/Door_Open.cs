using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Open : CStateBase<Door>
{
    public Door_Open(Door _cOwner) : base(_cOwner) { }

    // Start is called before the first frame update
    public override void Enter()
    {
        this.m_cOwner.isOpening = true;
        this.m_cOwner.StartCoroutine(OpenDoorCoroutine(this.m_cOwner.StartAngle, new Vector3(0f, this.m_cOwner.MaxOpenRadian, 0f)));

    }
    public override void Execute()
    {

    }

    public override void Exit()
    {

    }

    public virtual IEnumerator OpenDoorCoroutine(Vector3 _StartAngle, Vector3 _EndAngle)
    {
        float lerpVal = 0f;

        while (lerpVal <= 1f)
        {//開ける時間補間
            this.m_cOwner.transform.rotation = Quaternion.Euler(
                Vector3.Lerp(_StartAngle, _EndAngle, lerpVal));
            lerpVal += Time.deltaTime / this.m_cOwner.OpenSpeed;
            yield return null;
        }

        if (_StartAngle != _EndAngle)
        {//強硬手段
            this.m_cOwner.transform.rotation = Quaternion.Euler(_EndAngle);
        }
        this.m_cOwner.isClosing = false;

    }
}
