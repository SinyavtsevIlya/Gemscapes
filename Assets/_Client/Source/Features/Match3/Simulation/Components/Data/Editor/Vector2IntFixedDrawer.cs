#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using Nanory.Lex.UnityEditorIntegration;
using Nanory.Lex;

namespace Client.Match3.EditorIntegration
{
    sealed class Vector2IntFixedDrawer : IEcsComponentInspector
    {
        public Type GetFieldType()
        {
            return typeof(Vector2IntFixed);
        }

        public void OnGUI(string label, object value, EcsWorld world, int entityId)
        {
            EditorGUILayout.Vector2IntField(label, ((Vector2IntFixed)value).RawValue);
            EditorGUILayout.IntField(new GUIContent() { text = "Divisor" }, ((Vector2IntFixed)value).Divisor);
        }
    }
}
#endif