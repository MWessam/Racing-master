using UnityEngine;

namespace Cars
{
    [System.Serializable]
    public class CarData : GridData
    {
        public UpgradeableStat TopSpeed;
        public UpgradeableStat Handling;
        public UpgradeableStat Acceleration;
        public UpgradeableStat Nitro;
        public int WheelID;
        public int PaintID;
        public int Price;
        public int LevelData;
        [SerializeReference]
        public CarDataSO CarDataStatics;

        /// <summary>
        /// params order:
        /// top speed, handling, acceleration, nitro
        /// </summary>
        public CarData(int id, int price,int leveldata, CarDataSO carStatics, int wheelId, int paintID, bool isLocked = true, params UpgradeableStat[] stats)
        {
            ID = id;
            Price = price;
            IsPurchaseable = carStatics.IsPurchaseable;
            LevelData = leveldata;
            TopSpeed = stats[0];
            Handling = stats[1];
            Acceleration = stats[2];
            Nitro = stats[3];
            IsLocked = isLocked;
            CarDataStatics = carStatics;
            WheelID = wheelId;
            PaintID = paintID;
        }
    }
}