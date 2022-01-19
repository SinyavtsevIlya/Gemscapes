using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using Nanory.Lex.UnityEditorIntegration;
#endif
namespace Client.Match
{
    public class BoardAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] GameObject[] _availablePieces;
        [SerializeField] Tilemap _cells;
        [SerializeField] Tilemap _pieces;

        private Dictionary<string, int> _pieceTypeLookup;

        public void Convert(int boardEntity, GameObjectConversionSystem converstionSystem)
        {
            var later = converstionSystem.GetCommandBufferFrom<BeginSimulationECBSystem>();
            var world = converstionSystem.World;
            transform.Translate(-(_cells.cellSize / 2));
            var bounds = GetBounds(_cells);
            var size = (Vector2Int)bounds.size;


            FillPieceTypesLookup(converstionSystem);

            var grid = new Grid(new int[size.x, size.y]);

            for (var row = 0; row < size.y; row++)
            {
                for (var column = 0; column < size.x; column++)
                {
                    grid.Value[column, row] = -1;
                }
            }

            world.Add<BoardTag>(boardEntity);
            world.Add<Grid>(boardEntity) = grid;
            ref var availablePiecesBuffer = ref world.Add<AvailablePieces>(boardEntity).Buffer;
            availablePiecesBuffer.Values = Buffer<int>.Pool.Pop();
            foreach (var pieceType in _pieceTypeLookup.Values)
            {
                availablePiecesBuffer.Values.Add(pieceType);
            }

            foreach (Transform cellTr in _cells.transform)
            {
                var pos = Vector3Int.RoundToInt(cellTr.position) - bounds.min;
                var cellEntity = converstionSystem.Convert(cellTr.gameObject, ConversionMode.Convert);
                CellAuthorizationUtility.Authorize(boardEntity, world.Dst, grid, (Vector2Int)pos, cellEntity);
                if (pos.y == grid.Value.GetLength(1) - 1)
                {
                    world.Add<GeneratorTag>(cellEntity);
                }
            }

            // Apply GravityInputDirection component datas
            foreach (Transform cellTr in _cells.transform)
            {
                var pos = (Vector2Int)(Vector3Int.RoundToInt(cellTr.position) - bounds.min);
                CellAuthorizationUtility.ApplyGravity(world.Dst, grid, pos);
            }

            foreach (Transform pieceTr in _pieces.transform)
            {
                var pos = Vector3Int.RoundToInt(pieceTr.position) - bounds.min;
                var pieceEntity = converstionSystem.Convert(pieceTr.gameObject, ConversionMode.ConvertAndDestroy);
                PieceAuthorizationUtility.Authorize(world, grid, (Vector2Int)pos, _pieceTypeLookup[pieceTr.name], pieceEntity);
            }

            transform.Translate(-bounds.min);

            var halfSize = (Vector2)size / 2;
            Camera.main.transform.position = bounds.center - bounds.min - Vector3.forward - Vector3.one / 2;
            Camera.main.orthographicSize = halfSize.y;
        }

        private void FillPieceTypesLookup(GameObjectConversionSystem converstionSystem)
        {
            _pieceTypeLookup = new Dictionary<string, int>();

            foreach (var piecePrefab in _availablePieces)
            {
                var pieceEntityPrefab = converstionSystem.Convert(piecePrefab);
                _pieceTypeLookup[piecePrefab.name] = pieceEntityPrefab;
            }
        }

        private BoundsInt GetBounds(Tilemap tilemap)
        {
            var xMin = int.MaxValue;
            var yMin = int.MaxValue;
            var xMax = int.MinValue;
            var yMax = int.MinValue;
            foreach (Transform cellTr in tilemap.transform)
            {
                if (cellTr.position.x < xMin) xMin = Mathf.RoundToInt(cellTr.position.x);
                if (cellTr.position.y < yMin) yMin = Mathf.RoundToInt(cellTr.position.y);
                if (cellTr.position.x > xMax) xMax = Mathf.RoundToInt(cellTr.position.x);
                if (cellTr.position.y > yMax) yMax = Mathf.RoundToInt(cellTr.position.y);
            }
            return new BoundsInt(xMin, yMin, 0, xMax - xMin + 1, yMax - yMin + 1, 0);
        }
    }
}
