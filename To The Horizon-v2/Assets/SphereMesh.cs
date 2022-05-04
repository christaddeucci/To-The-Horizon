using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMesh : MonoBehaviour
{
    Mesh originalMesh;
    Mesh clonedMesh;
    MeshFilter meshFilter;

    [HideInInspector]
    public int targetIndex;

    [HideInInspector]
    public Vector3 targetVertex;

    [HideInInspector]
    public Vector3[] originalVertices;

    [HideInInspector]
    public Vector3[] modifiedVertices;

    [HideInInspector]
    public Vector3[] normals;

    [HideInInspector]
    public bool isMeshReady = false;
    public bool showTransformHandle = false;
    public List<int> selectedIndices = new List<int>();
    public float pickSize = 0.01f;


    public float radiusOfEffect = 0.3f; //1 
    public float pullValue = 0.3f; //2
    public float duration = 0.1f; //3
    int currentIndex = 0; //4
    bool isAnimate = false;
    float startTime = 0f;
    float runTime = 0f;


    void Start()
    {
        Init();
    }

    public void Init()
    {
        meshFilter = GetComponent<MeshFilter>();
        isMeshReady = false;

        currentIndex = 0;

        autoGenerate();

        originalMesh = meshFilter.mesh;
        originalVertices = originalMesh.vertices;
        normals = originalMesh.normals;
        modifiedVertices = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++)
        {
            modifiedVertices[i] = originalVertices[i];
        }
        StartDisplacement();
    }

    public void autoGenerate()
    {
        int size = UnityEngine.Random.Range(3, 10);

        for (int i = 0; i < size; i++)
        {
            selectedIndices.Add(UnityEngine.Random.Range(0, 510));

        }

        pullValue = UnityEngine.Random.Range(1.0f, 4.0f);
        radiusOfEffect = UnityEngine.Random.Range(0.0f, 1.0f);
    }

    public void StartDisplacement()
    {
        targetVertex = originalVertices[selectedIndices[currentIndex]]; //1
        startTime = Time.time; //2
        isAnimate = true;
        print("displaced");
    }

    protected void FixedUpdate()
    {
        if (!isAnimate) //2
        {
            return;
        }

        runTime = Time.time - startTime; //3

        if (runTime < duration)  //4
        {
            Vector3 targetVertexPos =
                meshFilter.transform.InverseTransformPoint(targetVertex);
            DisplaceVertices(targetVertexPos, pullValue, radiusOfEffect);
        }
        else //5
        {
            currentIndex++;
            if (currentIndex < selectedIndices.Count) //6
            {
                StartDisplacement();
            }
            else //7
            {
                originalMesh = GetComponent<MeshFilter>().mesh;
                isAnimate = false;
                isMeshReady = true;
            }
        }
    }


    void DisplaceVertices(Vector3 targetVertexPos, float force, float radius)
    {
        Vector3 currentVertexPos = Vector3.zero;
        float sqrRadius = radius * radius; //1

        for (int i = 0; i < modifiedVertices.Length; i++) //2
        {
            currentVertexPos = modifiedVertices[i];
            float sqrMagnitude = (currentVertexPos - targetVertexPos).sqrMagnitude; //3
            if (sqrMagnitude > sqrRadius)
            {
                continue; //4
            }
            float distance = Mathf.Sqrt(sqrMagnitude); //5
            float falloff = GaussFalloff(distance, radius);
            Vector3 translate = (currentVertexPos * force) * falloff; //6
            translate.z = 0f;
            Quaternion rotation = Quaternion.Euler(translate);
            Matrix4x4 m = Matrix4x4.TRS(translate, rotation, Vector3.one);
            modifiedVertices[i] = m.MultiplyPoint3x4(currentVertexPos);
        }
        originalMesh.vertices = modifiedVertices; //7
        originalMesh.RecalculateNormals();
    }

    public void ClearAllData()
    {
        selectedIndices = new List<int>();
        targetIndex = 0;
        targetVertex = Vector3.zero;
    }

    public Mesh SaveMesh()
    {
        Mesh nMesh = new Mesh();

        return nMesh;
    }

    static float LinearFalloff(float dist, float inRadius)
    {
        return Mathf.Clamp01(0.5f + (dist / inRadius) * 0.5f);
    }

    static float GaussFalloff(float dist, float inRadius)
    {
        return Mathf.Clamp01(Mathf.Pow(360, -Mathf.Pow(dist / inRadius, 2.5f) - 0.01f));
    }

    static float NeedleFalloff(float dist, float inRadius)
    {
        return -(dist * dist) / (inRadius * inRadius) + 1.0f;
    }
}
