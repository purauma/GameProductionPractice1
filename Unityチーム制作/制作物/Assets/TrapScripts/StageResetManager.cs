using UnityEngine;
using System.Collections.Generic;

public class StageResetManager : MonoBehaviour
{
    public static StageResetManager Instance;

    private List<IResettable> resetObjects = new List<IResettable>();

    private void Awake()
    {
        Instance = this;
    }


    public void Register(IResettable obj)
    {
        resetObjects.Add(obj);
    }


    public void ResetStage()
    {
        foreach (IResettable obj in resetObjects)
        {
            obj.ResetObject();
        }
    }
}