using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorTestWindow : EditorWindow
{
    static List<GameObject> cubes = new List<GameObject>();
    [MenuItem("Test/TestWindow")]
    static void OpenWindow()
    {
        cubes.Clear();
        var window = EditorWindow.GetWindow<EditorTestWindow>("���Դ���");
        window.minSize = new Vector2(500, 500);
        window.Show(true);
    }
    string name = "";
    string name2 = "";
    private void OnGUI()
    {
        if (EditorApplication.isPlaying)
        {
            if (GUILayout.Button("��ť", GUILayout.Width(500)))
            {
                Debug.Log("�������������" + name);
                var gameobj = GameObject.Instantiate(Resources.Load("Cube"));
                /// cube1|0|0|0|type
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("���֣�");
            name = GUILayout.TextField(name);
            GUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.LabelField("Not playing");
        }
    }
}
