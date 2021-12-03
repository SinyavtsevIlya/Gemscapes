using Nanory.Lex;
using Nanory.Lex.Conversion;
using UnityEngine;

namespace Client.Match
{
    [Battle]
    public sealed class CreateLevelSystem : EcsSystemBase
    {
        protected override void OnCreate()
        {
            //var level = Resources.Load<GameObject>("level");
            //World.Convert(level, ConversionMode.Convert);

            var piecePrefab = Resources.Load<MovablePieceView>("piece");

            var size = new Vector2Int(20, 30);
            var grid = new Grid(new int[size.x, size.y]);

            for (int row = 0; row < size.y; row++)
            {
                for (int column = 0; column < size.x; column++)
                {
                    var cellEntity = World.NewEntity();
                    grid.Value[column, row] = cellEntity;
                    Add<CellPosition>(cellEntity).Value = new Vector2Int(column, row);
                    Add<GravityDirection>(cellEntity).Value = Vector2Int.down;
                    Add<Grid>(cellEntity) = grid;

                    if (row > 5 && row <= 10)
                    {
                        continue;
                    }

                    var pieceView = Object.Instantiate(piecePrefab);
                    var pieceEntity = World.NewEntity();
                    pieceView.SetLabel(pieceEntity.ToString());
                    Add<Position>(pieceEntity).Value = new Vector2(column, row);
                    Add<Velocity>(pieceEntity);
                    Add<CellLink>(pieceEntity).Value = cellEntity;
                    Add<Grid>(pieceEntity) = grid;
                    Add<Mono<MovablePieceView>>(pieceEntity).Value = pieceView;
                    Add<PieceLink>(cellEntity).Value = World.PackEntity(pieceEntity);
                    
                    if (row == 11)
                    {
                        Add<FallingTag>(pieceEntity);
                    }
                }
            }

            var halfSize = (Vector2) size / 2;
            Camera.main.transform.position = new Vector3(halfSize.x, halfSize.y, -25);
            Camera.main.orthographicSize = halfSize.y;
        }

        protected override void OnUpdate()
        {
            
        }
    }
}
