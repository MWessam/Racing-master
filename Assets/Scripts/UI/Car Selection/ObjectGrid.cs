using System.Collections.Generic;
using Nova;
using UnityEngine;

namespace UI.Car_Selection
{
    public class ObjectGrid : MonoBehaviour
    {
        [SerializeField] GridView GridV;
        [SerializeField] List<CategoryTabContent> contents;
        private void Start()
        {
            GridV.AddDataBinder<CategoryTabContent, CategoryTabVisuals>(BindContact);
            GridV.SetDataSource(contents);
        }

        private void BindContact(Data.OnBind<CategoryTabContent> evt, CategoryTabVisuals visuals, int index)
        {
            CategoryTabContent contact = evt.UserData;
        }
    }
}
