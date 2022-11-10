using System;
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
        private string _targetSceneName;
        private bool _isSelected;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            var s = button.GetComponent<SelectableButton>();
            button.onClick.AddListener(() =>
            {
                if (_isSelected)
                {
                    SceneManager.LoadScene(_targetSceneName);
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
            _targetSceneName = levelDefinition.sceneName;
            descriptionText.text = levelDefinition.title;
            thumbnailImage.sprite = levelDefinition.thumbnail;
        }
    }
}
