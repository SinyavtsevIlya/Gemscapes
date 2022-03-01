using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;
using Nanory.Lex.Lifecycle;

namespace Client.Match3
{
    public class CellAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(int cellEntity, GameObjectConversionSystem converstionSystem)
        {
            converstionSystem.World.Add<Mono<CellView>>(cellEntity).Value = GetComponent<CellView>();
        }
    }

    public static class CellAuthoringUtility
    {
        public static void Authorize(int boardEntity, EcsWorld world, Grid grid, Vector2Int pos, int cellEntity, bool addGravity = false)
        {
            world.Add<CreatedEvent>(cellEntity);
            if (addGravity)
            {
                world.Add<GravityDirection>(cellEntity).Value = new Vector2Int(0, -1);
            }
            grid.Value[pos.x, pos.y] = cellEntity;
            world.Add<Grid>(cellEntity) = grid;
            world.Add<BoardLink>(cellEntity).Value = boardEntity;
            world.Add<CellPosition>(cellEntity).Value = new Vector2Int(pos.x, pos.y);
        }

        public static void BuildGravityGraph(EcsWorld world, Grid grid, Vector2Int pos)
        {
            var cellEntity = grid.Value[pos.x, pos.y];

            if (world.TryGet<Buffer<GravityDirection>>(cellEntity, out var gravityDirections))
            {
                for (int idx = 0; idx < gravityDirections.Values.Count; idx++)
                {
                    var gravityDirection = gravityDirections.Values[idx];

                    if (grid.TryGetCell(pos + gravityDirection.Value, out var tendingCellEntity))
                    {
                        if (!world.Has<Buffer<GravityInputLink>>(tendingCellEntity))
                        {
                            world.AddBuffer<GravityInputLink>(tendingCellEntity);
                        }

                        world.Get<Buffer<GravityInputLink>>(tendingCellEntity).Values.Add(new GravityInputLink()
                        {
                            Value = cellEntity
                        });

                        if (!world.Has<Buffer<GravityOutputLink>>(cellEntity))
                        {
                            world.AddBuffer<GravityOutputLink>(cellEntity);
                        }

                        world.Get<Buffer<GravityOutputLink>>(cellEntity).Values.Add(new GravityOutputLink()
                        {
                            Value = tendingCellEntity
                        });
                    }
                }
            }
        }
    }
}
