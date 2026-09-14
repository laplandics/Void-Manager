using Constants;
using UnityEngine;
using Utils;

namespace Generators
{
    public class StarfieldGenerator
    {
        private const int ATLAS_SIZE = 64;
        private const int BUFFER = 5;
        private const float PROBABILITY = 0.8f;

        private readonly GeneratorBase _generator;
        
        public StarfieldGenerator(string seed)
        {
            var starsContainer = new GameObject("Stars").transform;
            
            var starfieldSeeds = seed.Split(Separators.SEED_MAP_SEPARATOR)[0];
            var seeds = starfieldSeeds.Split(Separators.SEED_PART_SEPARATOR);
            var positionSeed = int.Parse(seeds[0]);
            var rotationSeed = int.Parse(seeds[1]);
            var spriteSeed = int.Parse(seeds[2]);

            var starsNames = new string[ATLAS_SIZE];
            for (var i = 0; i < ATLAS_SIZE; i++)
            { starsNames[i] = $"Stars {i}"; }

            _generator = new GeneratorBase(BUFFER, starsContainer, starsNames, positionSeed, rotationSeed, spriteSeed,
                PROBABILITY, Configs.EntityConfig.Star);
        }

        public void Launch() => G.Resolve<Coroutines>().Start(_generator.GenerationRoutine());
    }
}