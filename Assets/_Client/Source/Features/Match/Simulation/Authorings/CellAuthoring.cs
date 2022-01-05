﻿using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;

namespace Client.Match
{
    public class CellAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(int cellEntity, GameObjectConversionSystem converstionSystem)
        {
            converstionSystem.World.Add<Mono<CellView>>(cellEntity).Value = GetComponent<CellView>();
        }
    }
}
