using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Managers
{
    public class ChunksGenerator
    {
        private const int VISIBLE_CHUNKS_RADIUS = 2;
        
        private bool _firstGeneration = true;
        private Vector2Int _currentChunk = new(0, 0);

        public void Launch()
        {
            G.Resolve<GameCamera>().OnCameraMoved += OnCameraMoved;
            OnCameraMoved(G.Resolve<GameCamera>().GetCameraPosition());
        }

        private void OnCameraMoved(Vector3 currentPosition)
        {
            var chunkIndex = G.Resolve<TileMap>().GetChunkIndex(currentPosition);
            if (chunkIndex == _currentChunk && !_firstGeneration) return;

            _firstGeneration = false;
            _currentChunk = chunkIndex;
            RegenerateChunks();
        }

        private void RegenerateChunks()
        {
            var existingChunks = G.Resolve<TileMap>().ChunksMap;
            var chunksIndexesToRemove = new List<Vector2Int>(existingChunks.Keys);
            
            for (var y = -VISIBLE_CHUNKS_RADIUS; y <= VISIBLE_CHUNKS_RADIUS; y++)
            {
                for (var x = -VISIBLE_CHUNKS_RADIUS; x <= VISIBLE_CHUNKS_RADIUS; x++)
                {
                    var newChunkX = _currentChunk.x + x;
                    var newChunkY  = _currentChunk.y + y;
                    var newChunkIndex = new Vector2Int(newChunkX, newChunkY);

                    if (existingChunks.ContainsKey(newChunkIndex))
                    { chunksIndexesToRemove.Remove(newChunkIndex); continue; }
                    
                    G.Resolve<TileMap>().AddChunk(newChunkIndex);
                }
            }

            for (var i = chunksIndexesToRemove.Count - 1; i >= 0; i--)
            { G.Resolve<TileMap>().RemoveChunk(chunksIndexesToRemove[i]); }
        }
    }
}