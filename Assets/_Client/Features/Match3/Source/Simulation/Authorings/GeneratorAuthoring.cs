﻿using UnityEngine;
using Nanory.Lex;
using Nanory.Lex.Conversion;

namespace Client.Match3
{
    public class GeneratorAuthoring : MonoBehaviour, IConvertToEntity
    {
        public void Convert(int cellEntity, ConvertToEntitySystem converstionSystem)
        {
            converstionSystem.World.Add<GeneratorTag>(cellEntity);
        }
    }
}
