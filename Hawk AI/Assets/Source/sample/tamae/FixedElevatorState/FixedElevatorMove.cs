
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixedElevatorMove : CStateBase<FixedElevator>
{
    public FixedElevatorMove(FixedElevator _cOwner) : base(_cOwner) { }

    private float m_fLerpVal;
    private int m_nMoveLength;  //到着マスまでの長さ

    public override void Enter()
    {
        this.m_cOwner.FixedElevatorState = FixedElevatorState.Move;
        m_fLerpVal = 0;
        m_nMoveLength = 0;
        SearchArrivedAtElevator();

        this.m_cOwner.StartCoroutine(MoveCoroutine());
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }

    private void SearchArrivedAtElevator()
    {
        int y = this.m_cOwner.Position.y;
        int x = this.m_cOwner.Position.x;

        for (int i = 0; i < this.m_cOwner.MapManager.BackMapData.Count; i++)
        {
            if (y != i)
            {//自分自身のy座標を外す
                if (this.m_cOwner.MapManager.BackMapData[i][x] == this.m_cOwner.CanNotUseNo)
                {
                    m_nMoveLength = y - i;
                    break;
                }
            }
        }
    }

    public IEnumerator MoveCoroutine()
    {
        Vector3 MyFloorPosition = Vector3.zero;
        Vector3 MyFloorPositionBuffer = Vector3.zero;
        Vector3 MyFloorArrivedPos = Vector3.zero;

        //自身の扉を閉める
        this.m_cOwner.ChangeState(0, FixedElevatorState.Close);

        while (this.m_cOwner.IsClose() == true)
        {
            yield return null;
        }

        //移動
        Vector3 ArrivedPos = new Vector3(this.m_cOwner.transform.position.x,
            this.m_cOwner.transform.position.y + m_nMoveLength,
            this.m_cOwner.EnterObject.transform.position.z);

        if (this.m_cOwner.IsFlg(FixedElevatorChildElement.Floor) == true)
        {
            //スタート位置
            MyFloorPositionBuffer = MyFloorPosition = 
                this.m_cOwner.gameObject.transform.
                GetChild((int)FixedElevatorChildElement.Floor).gameObject.transform.position;

            //到達地点
            MyFloorArrivedPos = new Vector3(MyFloorPosition.x,
              MyFloorPosition.y + m_nMoveLength,
              MyFloorPosition.z);

        }


        //this.m_cOwner.EnterObject.transform.position = ArrivedPos;
        while (m_fLerpVal <= 1.0f)
        {
            m_fLerpVal += (Time.deltaTime / this.m_cOwner.MoveTime);

            this.m_cOwner.EnterObject.transform.position
                = Vector3.Lerp(this.m_cOwner.EnterObject.transform.position, ArrivedPos, m_fLerpVal);

            if (this.m_cOwner.IsFlg(FixedElevatorChildElement.Floor) == true)
            {
                //スタート位置
                MyFloorPosition = this.m_cOwner.gameObject.transform.
                    GetChild((int)FixedElevatorChildElement.Floor).gameObject.transform.position;

         
                this.m_cOwner.gameObject.transform.
                    GetChild((int)FixedElevatorChildElement.Floor).gameObject.transform.position
                    = Vector3.Lerp(MyFloorPosition, MyFloorArrivedPos, m_fLerpVal);
            }

            yield return null;
        }

        //初期位置に戻す
        this.m_cOwner.gameObject.transform.
                    GetChild((int)FixedElevatorChildElement.Floor).gameObject.transform.position = MyFloorPositionBuffer;

        //エレベーターのマップ情報更新
        MapUpdate();

        //もう片方のエレベーターの扉を開ける
        PairToElevatorOpen();

    }


    #region エレベーターを上下に動かす場合、調整

    //public IEnumerator MoveCoroutine()
    //{
    //    float lerpVal = 0f;
    //    Vector3 ArrivedPos = Vector3.zero;
    //    ArrivedPos = new Vector3(this.m_cOwner.transform.position.x,
    //            this.m_cOwner.transform.position.y + m_nMoveLength,
    //            this.m_cOwner.transform.position.z);
    //    while (lerpVal <= 1f)
    //    {//動く時間の補間
    //        this.m_cOwner.transform.position = Vector3.Lerp(this.m_cOwner.transform.position, ArrivedPos, lerpVal);
    //        lerpVal += Time.deltaTime / this.m_cOwner.MoveSpeed;
    //        yield return null;
    //    }
    //    m_isArrived = true;
    //    this.m_cOwner.ChangeState(0, FixedElevatorState.Open);
    //}

    #endregion

    //エレベーターのマップ情報更新
    private void MapUpdate()
    {
        Vector2Int Start2Int = new Vector2Int((int)this.m_cOwner.Position.x, (int)(this.m_cOwner.Position.y));
        Vector2Int End2Int = new Vector2Int((int)this.m_cOwner.Position.x, (int)(this.m_cOwner.Position.y - m_nMoveLength));

        this.m_cOwner.MapManager.BackMapData[Start2Int.y][Start2Int.x] = this.m_cOwner.CanNotUseNo;
        this.m_cOwner.MapManager.BackMapData[End2Int.y][End2Int.x] = this.m_cOwner.CanUseNo;

    }

    //もう片方のエレベーターの扉を開ける
    private void PairToElevatorOpen()
    {
        const float Correction = 0.5f;                                                    //マス目の真ん中に補正
        RaycastHit hit;
        int Mask = LayerMask.GetMask(new string[] { "Elevator" });
        bool ishit;

        //レイの向きを決定
        if ((m_nMoveLength + Correction) > 0)
        {
            ishit = Physics.Raycast(this.m_cOwner.gameObject.transform.position, new Vector3(0f, m_nMoveLength, 0f),
                  out hit, Mathf.Infinity, Mask);
        }
        else
        {
            ishit = Physics.Raycast(this.m_cOwner.gameObject.transform.position, new Vector3(0f, m_nMoveLength, 0f),
                  out hit, Mathf.Infinity, Mask);

        }

        if (ishit == true)
        {
            //Debug.DrawRay(this.m_cOwner.gameObject.transform.position, new Vector3(0f, (m_nMoveLength + Correction), 0f));

            if (LayerMask.NameToLayer("Elevator") == hit.collider.transform.gameObject.layer)
            {
                hit.collider.transform.gameObject.GetComponent<FixedElevator>().EnterObject = this.m_cOwner.EnterObject;
                hit.collider.transform.gameObject.GetComponent<FixedElevator>().EnterObjectNo = this.m_cOwner.EnterObjectNo;

                ExecuteEvents.Execute<IFixedElevatorInterface>(
                    target: hit.collider.transform.gameObject,
                    eventData: null,
                    functor: (recieveTarget, y) => recieveTarget.Open());

                if (this.m_cOwner.IsUseIcon == true)
                {
                    //UIのアイコン
                    hit.collider.transform.gameObject.transform.
                        GetChild((int)FixedElevatorChildElement.ButtonIcon).gameObject.SetActive(true);
                }

                //Light
                if (this.m_cOwner.IsUseLight == true)
                {
                    hit.collider.transform.gameObject.transform.
                        GetChild((int)FixedElevatorChildElement.Light).gameObject.SetActive(true);
                }

                //EnterA
                if (this.m_cOwner.IsUseEnterIcon == true)
                {
                    hit.collider.transform.gameObject.transform.
                        GetChild((int)FixedElevatorChildElement.EnterIcon).gameObject.SetActive(true);
                }
            }
        }
        //else
        //{
        //    Debug.LogError("Not Found Elevator");
        //}

    }
}
