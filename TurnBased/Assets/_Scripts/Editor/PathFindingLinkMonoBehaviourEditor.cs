using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathFindingLinkMonoBehaviour))]
public class PathFindingLinkMonoBehaviourEditor : Editor
{

    private void OnSceneGUI()
    {
        PathFindingLinkMonoBehaviour pathFindingLinkMonoBehaviour = (PathFindingLinkMonoBehaviour)target;

        EditorGUI.BeginChangeCheck();

        Vector3 newLinkPositionA = Handles.PositionHandle(pathFindingLinkMonoBehaviour.LinkPositionA, Quaternion.identity);
        Vector3 newLinkPositionB = Handles.PositionHandle(pathFindingLinkMonoBehaviour.LinkPositionB, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(pathFindingLinkMonoBehaviour, "Change Link Position");

            pathFindingLinkMonoBehaviour.LinkPositionA = newLinkPositionA;
            pathFindingLinkMonoBehaviour.LinkPositionB = newLinkPositionB;
        }

    }

}
