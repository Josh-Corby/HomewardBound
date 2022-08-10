using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : GameBehaviour
{
    public GameObject[] EntryTriggers;
    public GameObject[] ExitTriggers;

    public GameObject[] ObjectsToSpawn;

    public List<GameObject> spawnedObjectsList = new List<GameObject>();
}
