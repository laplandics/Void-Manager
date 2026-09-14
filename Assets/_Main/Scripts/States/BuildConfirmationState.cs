using System.Collections.Generic;
using System.Linq;
using UIBinders;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using Grid = Tools.Grid;

namespace GameStates
{
    public class BuildConfirmationState : GameState
    {
        private const string BUILD_CONFIRMATION_STATE_HINTS = "RETURN - ESC; PLACE BUILDING - ARROWS UP/DOWN/LEFT/RIGHT";
        
        private Vector3 _cursorPosition;
        private int _selectedStationIndex;
        private string _hintsPanelId;
        private readonly List<string> _framesIds = new();
        
        public override void SetParameters(StateParameters parameters)
        { _selectedStationIndex = (int)parameters.Parameters[0]; }

        public override void OnEnter()
        {
            _hintsPanelId = G.Resolve<UI>().Add(nameof(GameUIHintsPanel), new UIInfo(BUILD_CONFIRMATION_STATE_HINTS));
            
            BuildSelectionFrames();
            G.Resolve<Inputs>().UISelectUp.performed += OnSelectUp;
            G.Resolve<Inputs>().UISelectDown.performed += OnSelectDown;
            G.Resolve<Inputs>().UISelectLeft.performed += OnSelectLeft;
            G.Resolve<Inputs>().UISelectRight.performed += OnSelectRight;
        }

        private void BuildSelectionFrames()
        {
            var cursor = G.Resolve<Entities>().GetEntities(nameof(Configs.EntityConfig.Cursor))[0];
            _cursorPosition = cursor.Actions.GetPosition();

            var minX = _cursorPosition.x - Grid.GRID_CELL_SIZE;
            var maxX = _cursorPosition.x + Grid.GRID_CELL_SIZE;
            
            var minY = _cursorPosition.y - Grid.GRID_CELL_SIZE;
            var maxY = _cursorPosition.y + Grid.GRID_CELL_SIZE;

            var iteration = -1;
            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    iteration++;
                    if (iteration is 0 or 2 or 6 or 8) continue;
                    
                    var position = new Vector3(x, y, 0);
                    if (position == _cursorPosition) continue;
                    
                    var frameData = Configs.EntityConfig.Frame(position);
                    _framesIds.Add(G.Resolve<Entities>().New(frameData).id);
                }
            }
        }

        private void OnSelectUp(InputAction.CallbackContext ctx) =>
            PlaceStation(new Vector3(_cursorPosition.x, _cursorPosition.y + Grid.GRID_CELL_SIZE, 0));

        private void OnSelectDown(InputAction.CallbackContext ctx) =>
            PlaceStation(new Vector3(_cursorPosition.x, _cursorPosition.y - Grid.GRID_CELL_SIZE, 0));

        private void OnSelectLeft(InputAction.CallbackContext ctx) =>
            PlaceStation(new Vector3(_cursorPosition.x - Grid.GRID_CELL_SIZE, _cursorPosition.y, 0));

        private void OnSelectRight(InputAction.CallbackContext ctx) =>
            PlaceStation(new Vector3(_cursorPosition.x + Grid.GRID_CELL_SIZE, _cursorPosition.y, 0));

        private void PlaceStation(Vector3 position)
        {
            var cellEntities = G.Resolve<Cells>().GetEntitiesByPosition(position);
            var cellEntitiesTypes = cellEntities.Select(e => e.type).ToArray();
            if (cellEntitiesTypes.Any(typeName => typeName.Contains("Station"))) return;
            
            var data = Configs.EntityConfig.StationByIndex(_selectedStationIndex);
            
            var station = G.Resolve<Entities>().New(data);
            station.Actions.SetPosition(position);
            station.AddComponent(Configs.ComponentConfig.OnBuild());
            
            G.Resolve<States>().ChangeState<ExplorationState>();
        }

        public override void OnKeyPressed(KeyboardKeys key)
        {
            switch (key)
            {
                case KeyboardKeys.Esc: EnterBuildState(); break;
            }
        }

        private void EnterBuildState() => G.Resolve<States>().ChangeState<BuildMenuState>();

        public override void OnExit()
        {
            G.Resolve<UI>().Remove(_hintsPanelId);
            
            _framesIds.ForEach(id => G.Resolve<Entities>().Delete(id));
            _framesIds.Clear();
            
            G.Resolve<Inputs>().UISelectUp.performed -= OnSelectUp;
            G.Resolve<Inputs>().UISelectDown.performed -= OnSelectDown;
            G.Resolve<Inputs>().UISelectLeft.performed -= OnSelectLeft;
            G.Resolve<Inputs>().UISelectRight.performed -= OnSelectRight;
        }
    }
}