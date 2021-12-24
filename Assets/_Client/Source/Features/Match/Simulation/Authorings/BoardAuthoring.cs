using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
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
            var size = (Vector2Int)_cells.cellBounds.size;

            transform.Translate(-(_cells.cellBounds.min + _cells.cellSize / 2));

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

            foreach (var posRaw in _cells.cellBounds.allPositionsWithin)
            {
                var pos = posRaw - _cells.cellBounds.min;
                ConvertCellEntity(boardEntity, world, grid, pos);
            }

            foreach (Transform pieceTr in _pieces.transform)
            {
                var pos = _pieces.WorldToCell(pieceTr.position) - _cells.cellBounds.min;
                ConvertPieceEntity(converstionSystem, world, grid, pieceTr, pos);
            }

            var halfSize = (Vector2)size / 2;
            Camera.main.transform.position = _cells.cellBounds.center + Vector3.up * (_cells.cellSize / 2).y - Vector3.forward;
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
                world.Add<CellLink>(pieceEntity).Value = cellEntity;
                world.Add<PieceLink>(cellEntity).Value = world.Dst.PackEntity(pieceEntity);
            }
            world.Add<FallingTag>(pieceEntity);
        }

        private static void ConvertCellEntity(int boardEntity, EcsConversionWorldWrapper world, Grid grid, Vector3Int pos)
        {
            var cellEntity = world.NewEntity();
            world.Add<GravityDirection>(cellEntity).Value = new Vector2Int(0, -1);
            grid.Value[pos.x, pos.y] = cellEntity;
            world.Add<Grid>(cellEntity) = grid;
            world.Add<BoardLink>(cellEntity).Value = boardEntity;
            world.Add<CellPosition>(cellEntity).Value = new Vector2Int(pos.x, pos.y);

            if (pos.y == grid.Value.GetLength(1) - 1)
            {
                world.Add<GeneratorTag>(cellEntity);
            }
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
    }
}
