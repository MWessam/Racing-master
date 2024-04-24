using UnityEngine;

namespace UI.Car_Selection
{
    public abstract class ObjectDatabaseSO<T> : ScriptableObject where T : DatabaseObjectSO
    {
        [SerializeField] T[] _objects;
        private int _currentID;
        [HideInInspector]
        public T[] Objects;
        public void Initialize()
        {
            _currentID = 0;
            Objects = (T[])_objects.Clone();
            for (int i = 0; i < _objects.Length; ++i)
            {
                _objects[i].ID = _currentID++;
            }
        }
    }
}