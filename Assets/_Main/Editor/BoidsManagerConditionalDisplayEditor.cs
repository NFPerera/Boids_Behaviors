

using _Main.Scripts.Managers;
using UnityEditor;

namespace _Main.Editor
{
    [CustomEditor(typeof(BoidsManager))]
    public class BoidsManagerConditionalDisplayEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            serializedObject.Update();

            SerializedProperty l_is2dProp = serializedObject.FindProperty("is2d");
            SerializedProperty l_spawnCenter3dProp = serializedObject.FindProperty("spawnCenter3d");
            SerializedProperty l_spawnArea3dHalfExtentProp = serializedObject.FindProperty("spawnArea3dHalfExtent");
            SerializedProperty l_spawnCenter2dProp = serializedObject.FindProperty("spawnCenter2d");
            SerializedProperty l_spawnArea2dHalfExtentProp = serializedObject.FindProperty("spawnArea2dHalfExtent");

            EditorGUILayout.PropertyField(l_is2dProp);

            if (l_is2dProp.boolValue)
            {
                EditorGUILayout.PropertyField(l_spawnCenter2dProp);
                EditorGUILayout.PropertyField(l_spawnArea2dHalfExtentProp);
            }
            else
            {
                EditorGUILayout.PropertyField(l_spawnCenter3dProp);
                EditorGUILayout.PropertyField(l_spawnArea3dHalfExtentProp);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}