﻿using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;

namespace Client
{
    public class GeneratorAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(int cellEntity, GameObjectConversionSystem converstionSystem)
        {
            converstionSystem.World.Add<GeneratorTag>(cellEntity);
        }
    }
}