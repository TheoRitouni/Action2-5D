using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CalculShadow : MonoBehaviour
{
    private GameObject player;

    public List<Vector3> tempPos = new List<Vector3>();
    private List<Vector3> finalPos = new List<Vector3>();

    private Vector3 lastLocalScale = Vector3.zero;

    public bool isCube = false;
    public bool isSphere = false;

    public Transform directional = null;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (isCube)
        {
            SetBasicCubePos();
            LocalScaleCubeChange();
        }
    }

    void Update()
    {
        if (isCube)
        {
            if (lastLocalScale != transform.localScale)
                LocalScaleCubeChange();

            CheckCubeShadows();
        }
    }

    #region CUBE

    private void CheckCubeShadows()
    {
        RaycastHit hit;

        for (int i = 0; i < finalPos.Count; i++)
        {
            if (Physics.Raycast(transform.position + finalPos[i], directional.forward, out hit))
            {
                Debug.DrawRay(transform.position + finalPos[i], hit.point - (transform.position + finalPos[i]), Color.green);
            }
        }
        
    }

    public void SetBasicCubePos()
    {
        tempPos.Clear();
        if (directional.forward.z > 0)
        {
            tempPos.Add(new Vector3(0.5f, -0.5f, 0.5f));
            tempPos.Add(new Vector3(-0.5f, -0.5f, 0.5f));

            tempPos.Add(new Vector3(0.5f, 0.5f, -0.5f));
            tempPos.Add(new Vector3(-0.5f, 0.5f, -0.5f));
        }
        else
        {
            tempPos.Add(new Vector3(0.5f, 0.5f, 0.5f));
            tempPos.Add(new Vector3(-0.5f, 0.5f, 0.5f));
            tempPos.Add(new Vector3(0.5f, -0.5f, 0.5f));
            tempPos.Add(new Vector3(-0.5f, -0.5f, 0.5f));

            tempPos.Add(new Vector3(0.5f, 0.5f, -0.5f));
            tempPos.Add(new Vector3(-0.5f, 0.5f, -0.5f));
            tempPos.Add(new Vector3(0.5f, -0.5f, -0.5f));
            tempPos.Add(new Vector3(-0.5f, -0.5f, -0.5f));
        }
    }

    private void LocalScaleCubeChange()
    {
        if (finalPos.Count > 0)
        {
            for (int i = 0; i < tempPos.Count; i++)
                finalPos[i] = new Vector3(tempPos[i].x * transform.localScale.x, tempPos[i].y * transform.localScale.y, tempPos[i].z * transform.localScale.z);
        }
        else
        {
            for (int i = 0; i < tempPos.Count; i++)
                finalPos.Add(new Vector3(tempPos[i].x * transform.localScale.x, tempPos[i].y * transform.localScale.y, tempPos[i].z * transform.localScale.z));
        }

        lastLocalScale = transform.localScale;
    }

    #endregion
}

[CustomEditor(typeof(CalculShadow)), CanEditMultipleObjects]
public class CalculShadowEditor : Editor
{
    private bool showList = false;

    public override void OnInspectorGUI()
    {
        var script = (CalculShadow)target;
        GUILayout.BeginHorizontal();
        GUILayout.Label("Is Cube", GUILayout.Width(70));
        script.isCube = EditorGUILayout.Toggle(script.isCube);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Is Sphere", GUILayout.Width(70));
        script.isSphere = EditorGUILayout.Toggle(script.isSphere);
        GUILayout.EndHorizontal();

        if (script.isSphere && script.isCube)
        {
            GUILayout.Space(5);
            EditorGUILayout.HelpBox("Both is true, you need to chose which one you would use", MessageType.Error);
            return;
        }

        if (script.isCube)
        {
            if (script.tempPos.Count == 0)
                script.SetBasicCubePos();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            script.directional = EditorGUILayout.ObjectField("Light Transform", script.directional, typeof(Transform), true) as Transform;
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Space(10);
            showList = EditorGUILayout.Foldout(showList, "Positions");
            GUILayout.EndHorizontal();

            if (showList)
            {
                for (int i = 0; i < script.tempPos.Count; i++)
                {
                    GUILayout.Space(5);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(35);
                    EditorGUILayout.Vector3Field(i.ToString(), script.tempPos[i]);
                    GUILayout.EndHorizontal();
                }
            }
        }
        else
        {
            if (script.tempPos.Count > 0)
                script.tempPos.Clear();
        }
    }
}