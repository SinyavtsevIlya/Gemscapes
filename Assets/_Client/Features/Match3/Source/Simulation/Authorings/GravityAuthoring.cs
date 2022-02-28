using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;

namespace Client.Match3
{
    [ExecuteInEditMode]
    public class GravityAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private Vector2Int _value;
        [SerializeField] private Vector2Int[] _values;
        
        public void Convert(int cellEntity, GameObjectConversionSystem converstionSystem)
        {
            converstionSystem.World.Add<GravityDirection>(cellEntity).Value = _value;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            //var direction = (Vector3)(Vector2)_value;
            //DrawArrow.ForGizmo(transform.position, direction, arrowPosition: 1f);

            for (var idx = 0; idx < _values.Length; idx++)
            {
                var dir = (Vector3)(Vector2)_values[idx];
                DrawArrow.ForGizmo(transform.position, dir, GetColorByGravityIndex(idx), arrowPosition: 1f);
            }
        }

        private Color GetColorByGravityIndex(int gravityIndex)
        {
            switch (gravityIndex)
            {
                case 0: return Color.cyan;
                case 1: return Color.red;
                case 2: return Color.green;
                default:
                    return Color.blue;
            }
        }

        public static class DrawArrow
        {
            public static void ForGizmo(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f, float arrowPosition = 0.5f)
            {
                ForGizmo(pos, direction, Color.cyan, arrowHeadLength, arrowHeadAngle, arrowPosition);
            }

            public static void ForGizmo(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f, float arrowPosition = 0.5f)
            {
                Gizmos.color = color;
                Gizmos.DrawRay(pos, direction);
                DrawArrowEnd(pos, direction, color, arrowHeadLength, arrowHeadAngle, arrowPosition);
            }

            private static void DrawArrowEnd(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f, float arrowPosition = 0.5f)
            {
                Vector3 right = (Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back) * arrowHeadLength;
                Vector3 left = (Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back) * arrowHeadLength;
                Vector3 up = (Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back) * arrowHeadLength;
                Vector3 down = (Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back) * arrowHeadLength;

                Vector3 arrowTip = pos + (direction * arrowPosition);

                Gizmos.color = color;
                Gizmos.DrawRay(arrowTip, right);
                Gizmos.DrawRay(arrowTip, left);
                Gizmos.DrawRay(arrowTip, up);
                Gizmos.DrawRay(arrowTip, down);
            }
        }
#endif
    }
}
