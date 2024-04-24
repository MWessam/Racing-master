using System.Collections.Generic;
using Game_Manager.Mediator;
using Nova;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Car_Selection
{
    public class UIBlockCategoriesBarComponent : MonoBehaviour, IStartable
    {
        [SerializeField] ListView _categoriesBar;
        [SerializeField] List<CategoryTabContent> _contents;
        [FormerlySerializedAs("_persistantCarDataBase")]
        [SerializeField] PersistantPlayerDataBase persistantPlayerDataBase;
        [SerializeField] CarSelectionItems _carSelectionItems;
        public int CurrentIndex { get; private set; } = -1;
        public void Start()
        {
            _categoriesBar.AddGestureHandler<Gesture.OnClick, CategoryTabVisuals>(HandleSettingsTabClicked);
            _categoriesBar.AddDataBinder<CategoryTabContent, CategoryTabVisuals>(BindContent);
            _categoriesBar.SetDataSource(_contents);
            persistantPlayerDataBase = PersistantPlayerDataBase.Instance;
        }
        private void BindContent(Data.OnBind<CategoryTabContent> evt, CategoryTabVisuals visuals, int index)
        {
            CategoryTabContent content = evt.UserData;
            visuals.MainTabBlock.Color = visuals.DefaultColor;
            visuals.TabIcon.SetImage(content.TabIcon);
        }
        private void HandleSettingsTabClicked(Gesture.OnClick evt, CategoryTabVisuals button, int index)
        {
            SelectTab(button, index);
        }
        private void SelectTab(CategoryTabVisuals button, int index)
        {
            if (index == CurrentIndex)
            {
                return;
            }

            if (CurrentIndex >= 0)
            {
                if (_categoriesBar.TryGetItemView(CurrentIndex, out ItemView selectedTab))
                {
                    CategoryTabVisuals selected = selectedTab.Visuals as CategoryTabVisuals;
                    selected.IsSelected = false;
                }
            }
            CurrentIndex = index;
            button.IsSelected = true;
            switch (index)
            {
                case 0:
                    _carSelectionItems.SetDataSource(persistantPlayerDataBase.Cars);
                    break;
                case 1:
                    _carSelectionItems.SetDataSource(persistantPlayerDataBase.Wheels);
                    break;
                 case 2:
                     _carSelectionItems.SetDataSource(persistantPlayerDataBase.PaintData);
                     break;
                default:
                    break;
            }
        }
    }
}
