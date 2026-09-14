using Constants;
using UnityEngine;
using Utils;

namespace Generators
{
    public class AsteroidsGenerator
    {
        private const int BUFFER = 5;
        private const int ASTEROIDS_SPRITES_COUNT = 8;
        private const float PROBABILITY = 0.1f;
        
        private readonly GeneratorBase _generator;
        
        public AsteroidsGenerator(string seed)
        {
            var container = new GameObject("Asteroids").transform;
            
            var asteroidSeeds = seed.Split(Separators.SEED_MAP_SEPARATOR)[1];
            var seeds = asteroidSeeds.Split(Separators.SEED_PART_SEPARATOR);
            var positionSeed = int.Parse(seeds[0]);
            var rotationSeed = int.Parse(seeds[1]);
            var spriteSeed = int.Parse(seeds[2]);

            var asteroidsNames = new string[ASTEROIDS_SPRITES_COUNT];
            for (var i = 0; i < ASTEROIDS_SPRITES_COUNT; i++)
            { asteroidsNames[i] = $"Asteroid {i}"; }
            
            _generator = new GeneratorBase(BUFFER, container, asteroidsNames, positionSeed, rotationSeed, spriteSeed,
                PROBABILITY, Configs.EntityConfig.Asteroid);
        }
        
        public void Launch() => G.Resolve<Coroutines>().Start(_generator.GenerationRoutine());
    }
}