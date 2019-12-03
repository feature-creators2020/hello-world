using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixedElevatorOpen : CStateBase<FixedElevator>
{
    public FixedElevatorOpen(FixedElevator _cOwner) : base(_cOwner) { }
 
    public override void Enter()
    {
        this.m_cOwner.PlayAnimation(FixedElevatorAnimation.Open);
        this.m_cOwner.FixedElevatorState = FixedElevatorState.Open;
    }

    public override void Execute()
    {
        if (this.m_cOwner.GetAnimation.isPlaying == false)
        {
            this.m_cOwner.ChangeState(0, FixedElevatorState.Stop);
        }
    }

    public override void Exit()
    {
        if (this.m_cOwner.EnterObject != null)
        {

            //TODO : スターまたはファンが出る処理を追加
            switch (this.m_cOwner.EnterObjectNo)
            {
                case ObjectNo.Star:

                    ExecuteEvents.Execute<IStarInterface>(
                        target: this.m_cOwner.EnterObject,
                        eventData: null,
                        functor: (recieveTarget, y) => recieveTarget.OnElevatorGetOff());

                    break;

                case ObjectNo.Paparazzi:

                            ExecuteEvents.Execute<Reboot.ILoiteringFanInterface>(
                        target: this.m_cOwner.EnterObject,
                        eventData: null,
                        functor: (recieveTarget, y) => recieveTarget.OnGetOff(m_cOwner.gameObject));

                    break;
            }

            //保持データ削除
            this.m_cOwner.EnterObjectNo = ObjectNo.None;
            this.m_cOwner.EnterObject = null;

        }
    }
}
