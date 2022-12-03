using System;
using Global;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LevelCard : MonoBehaviour
    {
        public Action<string> Selected;

        public Image thumbnailImage;
        public TextMeshProUGUI descriptionText;
        public Button button;

        private Animator _animator;
        private string _targetScene;
        private bool _isSelected;
        private LevelDefinition _level;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            var s = button.GetComponent<SelectableButton>();
            button.onClick.AddListener(() =>
            {
                if (_isSelected)
                {
                    GlobalData.CurrentLevel = _level;
                    SceneManager.LoadScene(_targetScene);
                }
                else
                {
                    _animator.Play("Selected");
                    _isSelected = true;
                }
            });

            s.Deselected += () =>
            {
                if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Selected")) _animator.Play("Deselected");
                _isSelected = false;
            };
        }

        public void SetValues(LevelDefinition levelDefinition)
        {
            _level = levelDefinition;
            _targetScene = _level.sceneName;
            descriptionText.text = _level.title;
            thumbnailImage.sprite = _level.thumbnail;
        }
    }
}
