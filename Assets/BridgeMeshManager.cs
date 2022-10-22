using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeMeshManager : MonoBehaviour
{
    [SerializeField]
    private GameObject BridgeMeshObject;
    [SerializeField]
    private MeshFilter BridgeMesh;

    [SerializeField]
    private Mesh[] bridgeMeshesArray;

    [SerializeField]
    private Transform[] meshPositions;

    private ObjectBuild objectBuild;


    private void OnEnable()
    {
        ObjectBuild.OnObjectLengthChange += ChangeBridgeMesh;
    }

    private void OnDisable()
    {
        ObjectBuild.OnObjectLengthChange += ChangeBridgeMesh;
    }


    private void Awake()
    {
        objectBuild = GetComponent<ObjectBuild>();
    }

    private void Start()
    {
        BridgeMesh.mesh = bridgeMeshesArray[0];
        BridgeMesh.transform.position = meshPositions[0].position;
    }
    public void ChangeBridgeMesh()
    {
        BridgeMesh.mesh = bridgeMeshesArray[objectBuild.extensionCount - 1];
        BridgeMesh.transform.position = meshPositions[objectBuild.extensionCount - 1].transform.position;
        
    }
}
