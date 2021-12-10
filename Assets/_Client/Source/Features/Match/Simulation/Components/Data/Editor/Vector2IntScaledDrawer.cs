#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Nanory.Lex.UnityEditorIntegration.Inspectors
{
    sealed class Vector2IntScaledDrawer : IEcsComponentInspector
    {
        public Type GetFieldType()
        {
            return typeof(Vector2IntScaled);
        }

        public void OnGUI(string label, object value, EcsWorld world, int entityId)
        {
            EditorGUILayout.Vector2IntField(label, ((Vector2IntScaled)value).Value);
            EditorGUILayout.IntField(new GUIContent() { text = "Divisor" }, ((Vector2IntScaled)value).Divisor);
        }
    }
}
#endif