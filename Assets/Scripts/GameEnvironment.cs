using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class GameEnvironment
{
    private static GameEnvironment instance;
    private List<GameObject> checkpoints = new List<GameObject>();
    private GameObject safePoint;
    public List<GameObject> Checkpoints
    {
        get { return checkpoints; }
    }

    public GameObject SafePoint
    {
        get { return safePoint; }
    }

    public static GameEnvironment Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new GameEnvironment();
                instance.Checkpoints.AddRange(GameObject.FindGameObjectsWithTag("marker"));
                instance.checkpoints = instance.checkpoints.OrderBy(x => x.name).ToList();
                instance.safePoint = GameObject.FindGameObjectWithTag("SafePoint");
            }
            return instance;
        }
    }
}