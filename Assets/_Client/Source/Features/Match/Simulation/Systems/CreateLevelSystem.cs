using Nanory.Lex;
using Nanory.Lex.Conversion;
using UnityEngine;

namespace Client.Match
{
    [Battle]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public sealed class CreateLevelSystem : EcsSystemBase
    {
        protected override void OnCreate()
        {
            //var level = Resources.Load<GameObject>("level");
            //World.Convert(level, ConversionMode.Convert);

            var piecePrefab = Resources.Load<MovablePieceView>("piece");
            var cellPrefab = Resources.Load<CellView>("cell");

            var size = new Vector2Int(11 , 11);
            var grid = new Grid(new int[size.x, size.y]);

            for (int row = 0; row < size.y; row++)
            {
                for (int column = 0; column < size.x; column++)
                {
                    var cellEntity = World.NewEntity();
                    grid.Value[column, row] = cellEntity;
                    Add<CellPosition>(cellEntity).Value = new Vector2Int(column, row);
                    Add<GravityDirection>(cellEntity).Value = new Vector2Int(0, -1);
                    Add<Grid>(cellEntity) = grid;

                    var cellView = Object.Instantiate(cellPrefab);
                    cellView.SetLabel(cellEntity.ToString());
                    cellView.transform.position = new Vector3Int(column, row, 0);

                    //if (Random.value > .5f)
                    //{
                    //    continue;
                    //}

                    if (row < size.y / 2)
                        continue;

                    var pieceView = Object.Instantiate(piecePrefab);
                    var pieceEntity = World.NewEntity();
                    pieceView.SetLabel(pieceEntity.ToString());
                    Add<Position>(pieceEntity).Value = new Vector2IntScaled(column, row, 5);
                    Add<Velocity>(pieceEntity).Value = new Vector2IntScaled(0, 0, 5);
                    Add<CellLink>(pieceEntity).Value = cellEntity;
                    Add<Grid>(pieceEntity) = grid;
                    Add<Mono<MovablePieceView>>(pieceEntity).Value = pieceView;
                    Add<PieceLink>(cellEntity).Value = World.PackEntity(pieceEntity);
                    Add<CreatedEvent>(pieceEntity);
                    
                    //Add<FallingTag>(pieceEntity);
                }
            }

            var halfSize = (Vector2) size / 2;
            Camera.main.transform.position = new Vector3(halfSize.x- .5f, halfSize.y - .5f, -25);
            Camera.main.orthographicSize = halfSize.y;
        }

        protected override void OnUpdate()
        {
            
        }
    }
}
