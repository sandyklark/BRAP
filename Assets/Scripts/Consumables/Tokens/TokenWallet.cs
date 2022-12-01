using System;
using UnityEngine;

namespace Consumables.Tokens
{
    public class TokenWallet : MonoBehaviour
    {
        private GoldToken _gold;
        private EnergyToken _energy;
        private GunToken _gunToken;

        private void Start()
        {
            _gold = new GoldToken();
            _energy = new EnergyToken
            {
                Limit = 15
            };
            _gunToken = new GunToken
            {
                Limit = 100
            };
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _gold.Deposit(10);
                _energy.Deposit(2);
                _gunToken.Deposit(1);

                // LogTokens();
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                _gold.Withdraw(3);
                _energy.Withdraw(1);
                _gunToken.Withdraw(1);

                // LogTokens();
            }
        }

        private void LogTokens()
        {
            Debug.Log($"Gold: {_gold.Amount} - Energy: {_energy.Amount}/{_energy.Limit} - Gun Tokens: {_gunToken.Amount}");
        }
    }
}
