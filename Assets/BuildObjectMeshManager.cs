using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObjectMeshManager : GameBehaviour
{
    [SerializeField]
    private MeshFilter _mesh;
    [SerializeField]
    private Mesh[] _bridgeMeshesArray;
    [SerializeField]
    private Transform[] _meshPositions;
    [SerializeField]
    private ObjectBuild _objectBuild;

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
        _mesh.mesh = _bridgeMeshesArray[0];
        _mesh.transform.position = _meshPositions[0].position;
    }
    public void ChangeBridgeMesh()
    {
        _mesh.mesh = _bridgeMeshesArray[_objectBuild.ObjectLength - 1];
        _mesh.transform.position = _meshPositions[_objectBuild.ObjectLength - 1].transform.position;     
    }
}
