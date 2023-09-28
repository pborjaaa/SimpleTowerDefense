using Events;

namespace Models
{
    public class PlayerState
    {
        public int Coins;
        
        public PlayerState(int coins, CurrencyChangedEvent CurrencyChangedEvent)
        {
            Coins = coins;
            CurrencyChangedEvent.Subscribe(OnCurrencyChangedEvent);
        }

        private void OnCurrencyChangedEvent(int amount)
        {
            Coins += amount;
        }
    }
}