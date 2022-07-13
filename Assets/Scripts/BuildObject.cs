using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : GameBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
            BM.canBuild = false;
        
    }
    private void OnTriggerExit(Collider other)
    {
            BM.canBuild = true;
      
    }
}
