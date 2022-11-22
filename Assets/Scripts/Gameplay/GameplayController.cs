using UnityEngine;

namespace Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        public float RemainingTimeSeconds
        {
            get
            {
                return 0;
                // if (levelDefinition == null || Time.time > _startTime + levelDefinition.timeLimitSeconds) return 0;
                // return levelDefinition == null ? 0 : _startTime + levelDefinition.timeLimitSeconds - Time.time;
            }
        }

        private float _startTime;
        private bool _isGameOver;

        private void Awake()
        {
            Debug.Log("Missing level information. \nPlease add a reference to a LevelDefinition to the GameplayController component.");
        }

        private void Start()
        {
            _startTime = Time.time;
        }

        private void Update()
        {
            if(_isGameOver) return;
            // if (RemainingTimeSeconds > 0f) return;
            //
            // _isGameOver = true;
            // Debug.Log("END");
        }
    }
}
