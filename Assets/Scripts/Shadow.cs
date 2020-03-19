using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public Transform light = null;
    public Vector3 lightPos = Vector3.zero;

    public float max;
    private float min;

    public bool toggle = false;

    // Start is called before the first frame update
    void Start()
    {
        lightPos = light.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[CustomEditor(typeof(Shadow))]
public class ExampleEditor : Editor
{ 
    public override void OnInspectorGUI()
    {
        var instance = (Shadow)this.target;
        instance.toggle = GUILayout.Toggle(instance.toggle, "toggle me");
        if (instance.toggle == true)
        {
            EditorGUILayout.FloatField(instance.max);
        }
    }
}
