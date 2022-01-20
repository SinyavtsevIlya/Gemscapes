using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;
#if UNITY_EDITOR
using Nanory.Lex.UnityEditorIntegration;
#endif

namespace Client.Match
{
    public class PieceAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(int pieceEntity, GameObjectConversionSystem converstionSystem)
        {
            var world = converstionSystem.World;
            //pieceView.SetLabel(pieceEntity.ToString());
            world.Add<MovableTag>(pieceEntity);

            var isPrefab = gameObject.scene.name == null;

            if (isPrefab)
            {
                world.Add<GameObjectReference>(pieceEntity).Value = gameObject;
            }
        }
    }

    public static class PieceAuthorizationUtility
    {
        public static void Authorize(EcsConversionWorldWrapper world, Grid grid, Vector2Int pos, int pieceTypeId, int pieceEntity)
        {
            world.Add<CreatedEvent>(pieceEntity);
            world.Add<PieceTypeId>(pieceEntity).Value = pieceTypeId;
            world.Add<Position>(pieceEntity).Value = new Vector2IntScaled(pos.x, pos.y, SimConstants.GridSubdivison);
            world.Add<Velocity>(pieceEntity).Value = new Vector2IntScaled(0, 0, SimConstants.GridSubdivison);
            world.Add<Grid>(pieceEntity) = grid;
            if (grid.TryGetCell(pos, out var cellEntity))
            {
                world.Add<PieceLink>(cellEntity).Value = world.Dst.PackEntity(pieceEntity);
                world.Add<GravityCellLink>(pieceEntity).Value = cellEntity;

                if (!world.Has<GeneratorTag>(cellEntity))
                {
                    //world.Add<FallingTag>(pieceEntity);
                }
            }

            
        }

        public static void ApplyFalling(EcsWorld world, Grid grid, Vector2Int pos)
        {
            var cellEntity = grid.Value[pos.x, pos.y];
            if (grid.TryGetCell(pos + world.Get<GravityDirection>(cellEntity).Value, out var tendingCellEntity))
            {
                if (!world.Has<PieceLink>(tendingCellEntity))
                {
                    if (!world.Has<GeneratorTag>(cellEntity))
                    {
                        if (world.TryGet<PieceLink>(cellEntity, out var pieceLink))
                        {
                            if (pieceLink.Value.Unpack(world, out var pieceEntity))
                            {
                                world.Add<FallingTag>(pieceEntity);
                            }
                        }
                    }
                }
            }
        }
    }
}
