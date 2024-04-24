using Game_Manager.Mediator;
using Nova;
using UnityEngine;
namespace UI
{
    public class MoneyView : MonoBehaviour, IStartable, IOnDisabled
    {
        [SerializeField]
        private TextBlock _moneyText;

        public void OnDisable()
        {
            PersistantPlayerDataBase.Instance.OnMoneyChange -= UpdateMoney;
        }
        private void UpdateMoney()
        {
            _moneyText.Text = PersistantPlayerDataBase.Instance.Money.ToString();
        }
        public void Start()
        {
            PersistantPlayerDataBase.Instance.OnMoneyChange += UpdateMoney;
            UpdateMoney();
        }
    }
}
