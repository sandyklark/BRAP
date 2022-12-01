using System;

namespace Consumables.Tokens
{
    public abstract class AbstractToken : IToken
    {
        public abstract string Name { get; }
        public virtual int Limit { get; set; }
        public int Amount { get; protected set; }

        public int Deposit(int amount)
        {
            var initial = Amount;
            var isLimited = Limit > 0;
            var value = initial + amount;
            var limited = Math.Clamp(value, 0, Limit);

            Amount = isLimited ? limited : value;

            return Amount - initial;
        }

        public int Withdraw(int amount)
        {
            var initial = Amount;
            var value = initial - amount;
            if (value < 0) value = 0;

            Amount = value;

            return Amount + initial;
        }
    }
}
