using Data;
using Utils;

namespace Managers
{
    public class CursorSpawner
    {
        private readonly EntityData _cursorData;
        private string _cursorId;
        
        public CursorSpawner(EntityData[] entities)
        {
            foreach (var entityData in entities)
            {
                if (entityData.type != nameof(Configs.EntityConfig.Cursor)) continue;
                _cursorData = entityData;
            }
        }
        
        public void Launch()
        {
            var cursor = G.Resolve<Entities>().New(_cursorData);
            _cursorId = cursor.id;
        }
    }
}
