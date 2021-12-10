using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace Client.Match
{
    public class BoardAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] GameObject[] _availablePieces;
        [SerializeField] Tilemap _cells;
        [SerializeField] Tilemap _pieces;
        
        public void Convert(int boardEntity, GameObjectConversionSystem converstionSystem)
        {
            var world = converstionSystem.World;
            var size = (Vector2Int) _cells.cellBounds.size;

            var pieceTypeLookup = new Dictionary<string, int>();

            foreach (var piecePrefab in _availablePieces)
            {
                var pieceEntityPrefab = converstionSystem.Convert(piecePrefab);
                pieceTypeLookup[piecePrefab.name] = pieceEntityPrefab;
            }

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
            foreach (var posRaw in _cells.cellBounds.allPositionsWithin)
            {
                var pos = posRaw - _cells.cellBounds.min;
                var cellEntity = world.NewEntity();
                world.Add<GravityDirection>(cellEntity).Value = new Vector2Int(0, -1);
                grid.Value[pos.x, pos.y] = cellEntity;
                world.Add<Grid>(cellEntity) = grid;
                world.Add<BoardLink>(cellEntity).Value = boardEntity;
                world.Add<CellPosition>(cellEntity).Value = new Vector2Int(pos.x, pos.y);
            }

            foreach (Transform pieceTr in _pieces.transform)
            {
                var pos = _pieces.WorldToCell(pieceTr.position);
                var pieceEntity = converstionSystem.Convert(pieceTr.gameObject);

                world.Add<CreatedEvent>(pieceEntity);
                world.Add<PieceTypeId>(pieceEntity).Value = pieceTypeLookup[pieceTr.name];
                world.Add<Position>(pieceEntity).Value = new Vector2IntScaled(pos.x, pos.y, SimConstants.GridSubdivison);
                world.Add<Velocity>(pieceEntity).Value = new Vector2IntScaled(0, 0, SimConstants.GridSubdivison);
                world.Add<Grid>(pieceEntity) = grid;
                if (grid.TryGetCell((Vector2Int)pos, out var cellEntity))
                {
                    world.Add<CellLink>(pieceEntity).Value = cellEntity;
                    world.Add<PieceLink>(cellEntity).Value = world.Dst.PackEntity(pieceEntity);
                }
            }

            var halfSize = (Vector2)size / 2;
            Camera.main.transform.position = _cells.cellBounds.center;
            Camera.main.orthographicSize = halfSize.y;
        }
    }
}
