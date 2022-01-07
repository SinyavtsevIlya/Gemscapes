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
                ConvertCellEntity(converstionSystem, boardEntity, world, grid, cellTr, pos);
            }

            foreach (Transform pieceTr in _pieces.transform)
            {
                var pos = Vector3Int.RoundToInt(pieceTr.position) - bounds.min;
                ConvertPieceEntity(converstionSystem, world, grid, pieceTr, pos);
            }

            transform.Translate(-bounds.min);

            var halfSize = (Vector2)size / 2;
            Camera.main.transform.position = transform.position - Vector3.right * bounds.min.x / 2 - Vector3.forward;
            Camera.main.orthographicSize = halfSize.y;
        }

        private void ConvertPieceEntity(GameObjectConversionSystem converstionSystem, EcsConversionWorldWrapper world, Grid grid, Transform pieceTr, Vector3Int pos)
        {
            var pieceEntity = converstionSystem.Convert(pieceTr.gameObject, ConversionMode.ConvertAndDestroy);

            world.Add<CreatedEvent>(pieceEntity);
            world.Add<PieceTypeId>(pieceEntity).Value = _pieceTypeLookup[pieceTr.name];
            world.Add<Position>(pieceEntity).Value = new Vector2IntScaled(pos.x, pos.y, SimConstants.GridSubdivison);
            world.Add<Velocity>(pieceEntity).Value = new Vector2IntScaled(0, 0, SimConstants.GridSubdivison);
            world.Add<Grid>(pieceEntity) = grid;
            if (grid.TryGetCell((Vector2Int)pos, out var cellEntity))
            {
                world.Add<PieceLink>(cellEntity).Value = world.Dst.PackEntity(pieceEntity);
            }
            //world.Add<FallingTag>(pieceEntity);
        }

        private static void ConvertCellEntity(GameObjectConversionSystem converstionSystem, int boardEntity, EcsConversionWorldWrapper world, Grid grid, Transform cellTr, Vector3Int pos)
        {
            var cellEntity = converstionSystem.Convert(cellTr.gameObject, ConversionMode.Convert);
            world.Add<CreatedEvent>(cellEntity);
            world.Add<GravityDirection>(cellEntity).Value = new Vector2Int(0, -1);
            grid.Value[pos.x, pos.y] = cellEntity;
            world.Add<Grid>(cellEntity) = grid;
            world.Add<BoardLink>(cellEntity).Value = boardEntity;
            world.Add<CellPosition>(cellEntity).Value = new Vector2Int(pos.x, pos.y);

            if (pos.y == grid.Value.GetLength(1) - 1)
            {
                world.Add<GeneratorTag>(cellEntity);
            }

#if DEBUG
            
#endif
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
