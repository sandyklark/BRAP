
namespace Consumables.Tokens
{
    public interface IToken
    {
        public int Deposit(int amount);
        public int Withdraw(int amount);
    }
}
