﻿using Nanory.Lex;
using Nanory.Lex.Conversion;
using UnityEngine;
using UnityEngine.Assertions;

namespace Client.Match
{
    [Battle]
    //[UpdateInGroup(typeof(InitializationSystemGroup))]
    public sealed class CreateLevelSystem : EcsSystemBase
    {
        protected override void OnCreate()
        {
            //var level = Resources.Load<GameObject>("level");
            //World.Convert(level, ConversionMode.Convert);

            var piecePrefab = Resources.Load<MovablePieceView>("piece");
            var cellPrefab = Resources.Load<CellView>("cell");

            var size = new Vector2Int(11 , 25);
            var subGridSize = 25;

            Assert.IsTrue(subGridSize % 2 != 0, "Sub-grid size should be odd");

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

                    if (Random.value > .95f)
                    {
                        continue;
                    }

                    if (row < size.y / 2 && Random.value > .15f)
                        continue;

                    var pieceView = Object.Instantiate(piecePrefab);
                    var pieceEntity = World.NewEntity();
                    pieceView.SetLabel(pieceEntity.ToString());
                    pieceView.SetPosition(new Vector2Int(column, row));
                    Add<Position>(pieceEntity).Value = new Vector2IntScaled(column, row, subGridSize);
                    Add<Velocity>(pieceEntity).Value = new Vector2IntScaled(0, 0, subGridSize);
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
