using System;
using ScriptableObjects;
using UnityEngine;

namespace Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        public Action GameOver;

        private LevelDefinition _level;
        private bool _hasLevel;

        public float RemainingTimeSeconds
        {
            get
            {
                if (!_hasLevel || Time.time > _startTime + _level.timeLimitSeconds) return 0;
                return _startTime + _level.timeLimitSeconds - Time.time;
            }
        }

        private float _startTime;
        private bool _isGameOver;

        private void Awake()
        {
            _level = FindObjectOfType<LevelReference>()?.level;

            _hasLevel = _level != null;

            if (!_hasLevel)
            {
                Debug.Log("Missing level information. \nPlease add a reference to a LevelDefinition to the GameplayController component.");
            }
        }

        private void Start()
        {
            _startTime = Time.time;
        }

        private void Update()
        {
            if(_isGameOver) return;

            if (RemainingTimeSeconds > 0f) return;

            _isGameOver = true;

            GameOver?.Invoke();
        }
    }
}
