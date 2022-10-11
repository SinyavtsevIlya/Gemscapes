using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;
using Nanory.Lex.Lifecycle;
#if UNITY_EDITOR
using Nanory.Lex.UnityEditorIntegration;
#endif

namespace Client.Match3
{
    public class PieceAuthoring : MonoBehaviour, IConvertToEntity
    {
        public void Convert(int pieceEntity, ConvertToEntitySystem converstionSystem)
        {
            var world = converstionSystem.World;
            //pieceView.SetLabel(pieceEntity.ToString());

            var isPrefab = gameObject.scene.name == null;

            if (isPrefab)
            {
                world.Add<GameObjectReference>(pieceEntity).Value = gameObject;
            }
        }
    }

    public static class PieceAuthoringUtility
    {
        public static int Authorize(EcsConversionWorldWrapper world, Grid grid, Vector2Int pos, int pieceTypeId, int pieceEntity, bool isFalling)
        {
            world.Add<MovableTag>(pieceEntity);
            world.Add<CreatedEvent>(pieceEntity);
            world.Add<PieceTypeId>(pieceEntity).Value = pieceTypeId;
            world.Add<Position>(pieceEntity).Value = new Vector2IntFixed(pos.x, pos.y, SimConstants.GridSubdivison);
            world.Add<Velocity>(pieceEntity).Value = new Vector2IntFixed(0, 0, SimConstants.GridSubdivison);
            world.Add<Grid>(pieceEntity) = grid;
            if (grid.TryGetCell(pos, out var cellEntity))
            {
                world.Add<PieceLink>(cellEntity).Value = world.Dst.PackEntity(pieceEntity);
                world.Add<GravityDirection>(pieceEntity);

                if (isFalling)
                {
                    world.Add<FallingTag>(pieceEntity);
                    world.Add<FallingStartedEvent>(pieceEntity);
                }
            }

            return pieceEntity;
        }
    }
}
