using ScriptableObjects;
using UI;
using UnityEngine;

namespace Menu
{
    public class LevelSelector : MonoBehaviour
    {
        public Transform levelCardContainer;
        public LevelSet levelSet;
        public GameObject levelCardPrefab;

        private void Start()
        {
            Time.timeScale = 1;
            CreateLevelCards();
        }

        private void CreateLevelCards()
        {
            foreach (var level in levelSet.levels)
            {
                var go = Instantiate(levelCardPrefab, levelCardContainer);
                var card = go.GetComponent<LevelCard>();

                card.SetValues(level);
            }
        }
    }
}
