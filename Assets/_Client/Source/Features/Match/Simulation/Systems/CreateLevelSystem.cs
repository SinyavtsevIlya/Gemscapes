using Nanory.Lex;
using Nanory.Lex.Conversion;
using UnityEngine;
using UnityEngine.Assertions;

namespace Client.Match
{
    [M3]
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public sealed class CreateLevelSystem : EcsSystemBase
    {
        protected override void OnCreate()
        {
            var board =  Object.FindObjectOfType<BoardAuthoring>().gameObject;
            World.Convert(board);
        }

        private void Generate()
        {
            var piecePrefab = Resources.Load<MovablePieceView>("piece");
            var cellPrefab = Resources.Load<CellView>("cell");

            var size = new Vector2Int(12, 12);

            Assert.IsTrue(SimConstants.GridSubdivison % 2 != 0, "Sub-grid size should be odd");

            var grid = new Grid(new int[size.x, size.y]);

            var boardEntity = World.NewEntity();
            Add<BoardTag>(boardEntity);
            Add<Grid>(boardEntity) = grid;

            for (int row = 0; row < size.y; row++)
            {
                for (int column = 0; column < size.x; column++)
                {
                    var cellEntity = World.NewEntity();
                    grid.Value[column, row] = cellEntity;
                    Add<CellPosition>(cellEntity).Value = new Vector2Int(column, row);
                    Add<GravityDirection>(cellEntity).Value = new Vector2Int(0, -1);
                    Add<Grid>(cellEntity) = grid;
                    Add<BoardLink>(cellEntity).Value = boardEntity;

                    var cellView = Object.Instantiate(cellPrefab);
                    cellView.SetLabel(cellEntity.ToString());
                    cellView.transform.position = new Vector3Int(column, row, 0);

                    if (Random.value > .95f)
                    {
                        continue;
                    }

                    if (row < size.y / 2 && Random.value > .15f)
                        continue;

                    var pieceEntity = World.NewEntity();
                    var pieceTypeId = Random.Range(0, 2);
                    Add<PieceTypeId>(pieceEntity).Value = pieceTypeId;
                    var pieceView = Object.Instantiate(Resources.Load<MovablePieceView>("piece" + (pieceTypeId == 0 ? "_book" : "_carrot")));
                    pieceView.SetLabel(pieceEntity.ToString());
                    pieceView.SetPosition(new Vector2Int(column, row));
                    Add<Position>(pieceEntity).Value = new Vector2IntScaled(column, row, SimConstants.GridSubdivison);
                    Add<Velocity>(pieceEntity).Value = new Vector2IntScaled(0, 0, SimConstants.GridSubdivison);
                    Add<Grid>(pieceEntity) = grid;
                    Add<PieceLink>(cellEntity).Value = World.PackEntity(pieceEntity);
                    Add<CreatedEvent>(pieceEntity);
                }
            }

            var halfSize = (Vector2)size / 2;
            Camera.main.transform.position = new Vector3(halfSize.x - .5f, halfSize.y - .5f, -25);
            Camera.main.orthographicSize = halfSize.y;
        }

        protected override void OnUpdate()
        {

        }
    }
}
