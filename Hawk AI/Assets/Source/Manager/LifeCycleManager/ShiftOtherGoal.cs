using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/****シングルトン化****/
public class ShiftOtherGoal : SingletonMonoBehaviour<ShiftOtherGoal>
{
    [SerializeField]
    private List<GameObject> GoalObj;
    private List<int> Numbers = new List<int>();

    void Start()
    {
        for(int i = 0; i < GoalObj.Count;i++)
        {
            GoalObj[i].SetActive(false);
        }

        for (int i = 0; i < 2; i++)
        {
            int index = Random.Range(0, GoalObj.Count);

            foreach (var val in Numbers)
            {
                while (val == index)
                {
                    index = Random.Range(0, GoalObj.Count);
                }
            }

            Numbers.Add(index);
            GoalObj[index].SetActive(true);
                       
        }
        //CursorManager.Instance.SetCheeseActive();
    }

    public void Shift(GameObject Cheese)
    {
        for(int i = 0; i < GoalObj.Count; i++)
        {
            if (GoalObj[i] == Cheese)
            {
                Numbers.Remove(i);

                ExecuteEvents.Execute<ICheeseEffect>(
                target: Cheese.GetComponent<CheeseScript>().m_cCheeseEffects,
                eventData: null,
                functor: (recieveTarget, y) => recieveTarget.Stop());


                Cheese.SetActive(false);
            }
        }
        if(Numbers.Count == 0)
        {
            //for(int i = 0; i < GoalObj.Count; i++)
            //{
            //    GoalObj[i].SetActive(true);
            //    Numbers.Add(i);
            //}
            //for(int i = 0; i < GoalObj.Count - 2; i++)
            //{
            //    int index = Random.Range(0, Numbers.Count);
            //    GoalObj[Numbers[index]].SetActive(false);
            //    Numbers.RemoveAt(index);
            //}


            Numbers = new List<int>();

            for (int i = 0; i < GoalObj.Count; i++)
            {
                GoalObj[i].SetActive(false);
            }

            for (int i = 0; i < 2; i++)
            {
                int index = Random.Range(0, GoalObj.Count);

                foreach (var val in Numbers)
                {
                    while (val == index)
                    {
                        index = Random.Range(0, GoalObj.Count);
                    }
                }

                Numbers.Add(index);
                GoalObj[index].SetActive(true);

            }


        }
        //CursorManager.Instance.SetCheeseActive();
    }

    public List<GameObject> GetGoalObj()
    {
        List<GameObject> GoalObj = new List<GameObject>();

        for(int i =0; i < Numbers.Count; i++)
        {
            GoalObj.Add(this.GoalObj[Numbers[i]]);
        }

        return GoalObj;
    }
}
