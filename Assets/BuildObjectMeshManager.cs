using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObjectMeshManager : GameBehaviour
{
    [SerializeField]
    private MeshFilter Mesh;
    [SerializeField]
    private Mesh[] bridgeMeshesArray;
    [SerializeField]
    private Transform[] meshPositions;
    [SerializeField]
    private ObjectBuild objectBuild;


    private void OnEnable()
    {
        ObjectBuild.OnObjectLengthChange += ChangeBridgeMesh;
    }

    private void OnDisable()
    {
        ObjectBuild.OnObjectLengthChange -= ChangeBridgeMesh;
    }

    private void Start()
    {
        Mesh.mesh = bridgeMeshesArray[0];
        Mesh.transform.position = meshPositions[0].position;
    }
    public void ChangeBridgeMesh()
    {
        Mesh.mesh = bridgeMeshesArray[objectBuild.objectLength - 1];
        Mesh.transform.position = meshPositions[objectBuild.objectLength - 1].transform.position;     
    }
}
