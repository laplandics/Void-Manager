using System.Collections.Generic;
using System.Linq;
using GameStates;
using UnityEngine.UIElements;
using Utils;

namespace UIBinders
{
    
    [UIBinder] public class GameUIBuildMenu : UIBinder
    {
        private const string BUILD_MENU_CONTAINER_NAME = "StationsBuildContainer";
        private const string BUILD_ENTRY_LABEL_USS_STYLE_CLASS_NAME = "build-menu-entry__label";
        private const string BUILD_ENTRY_LABEL_SELECTED_USS_STYLE_CLASS_NAME = "build-menu-entry__label-selected";

        private int _selectedIndex;
        private readonly List<Label> _stationLabels = new();
        
        public override void OnAdd()
        {
            _stationLabels.Clear();
            
            var root = G.Resolve<UI>().GetRoot();
            root.Add(Element.ElementInstance);
            
            FillBuildMenu();
            
            G.Resolve<States>().GetState<BuildMenuState>().OnSelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(BuildSelectionInfo selectionInfo)
        {
            if (_selectedIndex + selectionInfo.IndexChange >= _stationLabels.Count || 
                _selectedIndex + selectionInfo.IndexChange < 0)
                return;
            
            _stationLabels[_selectedIndex].RemoveFromClassList(BUILD_ENTRY_LABEL_SELECTED_USS_STYLE_CLASS_NAME);
            _selectedIndex += selectionInfo.IndexChange;
            _stationLabels[_selectedIndex].AddToClassList(BUILD_ENTRY_LABEL_SELECTED_USS_STYLE_CLASS_NAME);
            
            selectionInfo.SelectedIndex = _selectedIndex;
        }

        private void FillBuildMenu()
        {
            var container = Element.ElementInstance.Q<VisualElement>(BUILD_MENU_CONTAINER_NAME);
            var stations = Configs.EntityConfig.StationNames;
            var types = new string[stations.Length];
            for (var i = 0; i < stations.Length; i++)
            { types[i] = stations[i]; }

            var stationNames = types.Select(t => $"{t.Split("Station")[0]} Station").ToArray();
            foreach (var stationName in stationNames)
            {
                var label = new Label();
                label.text = stationName.ToUpper();
                label.AddToClassList(BUILD_ENTRY_LABEL_USS_STYLE_CLASS_NAME);
                container.Add(label);
                
                _stationLabels.Add(label);
            }
            _stationLabels[0].AddToClassList(BUILD_ENTRY_LABEL_SELECTED_USS_STYLE_CLASS_NAME);
        }
        
        public override void OnRemove()
        {
            var root = G.Resolve<UI>().GetRoot();
            root.Remove(Element.ElementInstance);
        }
    }
}