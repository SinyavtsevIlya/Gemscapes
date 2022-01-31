using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;
using UnityEngine.Serialization;
using System.Collections.Generic;

namespace Client.Match3.EditorIntegration
{
    public static class BoardCompression
    {
        /// <summary>
        /// Finds all convertible gameobjects and replaces them with a compressed data structure.
        /// Then creates a copy of the scene to be added in the build. 
        /// All convertibles implemented <see cref="IPreserveCompression"/> will be ignored.
        /// </summary>
        /// <param name="boardGameobject"></param>
        public static void CompressMatch3Level(GameObject boardGameobject)
        {
            var output = string.Empty;

            var cellsRoot = boardGameobject.transform.GetChild(0);

            foreach (var cellAuthoring in cellsRoot.GetComponentsInChildren<CellAuthoring>())
            {
            }

            var piececRoot = boardGameobject.transform.GetChild(1);

        }

        // TODO: this is an example of a compressed
        // cell data layout to be done
        // {3,12, 0, -1} - cell with a stretched gravity
        // {1, 4} - cell with a default gravity
        public struct CellData
        {
            public Vector2Int Position;
            public Vector2Int GravityDirection;
        }

        public interface IPreserveCompression
        {
            void DeclarePreservedCompressionGameobjects(List<GameObject> list);
        }
    }
}
