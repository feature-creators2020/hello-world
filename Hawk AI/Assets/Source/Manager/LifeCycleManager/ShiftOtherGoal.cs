using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/****シングルトン化****/
public class ShiftOtherGoal : SingletonMonoBehaviour<ShiftOtherGoal>
{
    [SerializeField]
    private List<GameObject> GoalObj;
    private List<int> Numbers = new List<int>();

    void Start()
    {
        for (int i = 0; i < GoalObj.Count; i++)
        {
            Numbers.Add(i);
        }
        for(int i = 0; i < GoalObj.Count - 2; i++)
        {
            int index = Random.Range(0, Numbers.Count);
            GoalObj[Numbers[index]].SetActive(false);
            Numbers.RemoveAt(index);
        }
        //CursorManager.Instance.SetCheeseActive();
    }

    public void Shift(GameObject Cheese)
    {
        for(int i = 0; i < GoalObj.Count; i++)
        {
            if (GoalObj[i] == Cheese)   Numbers.Remove(i);
            Cheese.SetActive(false);
        }
        if(Numbers.Count == 0)
        {
            for(int i = 0; i < GoalObj.Count; i++)
            {
                GoalObj[i].SetActive(true);
                Numbers.Add(i);
            }
            for(int i = 0; i < GoalObj.Count - 2; i++)
            {
                int index = Random.Range(0, Numbers.Count);
                GoalObj[Numbers[index]].SetActive(false);
                Numbers.RemoveAt(index);
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
