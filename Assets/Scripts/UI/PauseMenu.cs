using UnityEngine;
using Util;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pauseUI;

        public void SetPause(bool pause)
        {
            TimeControl.Instance.SetPause(pause);
            pauseUI.SetActive(pause);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                SetPause(true);
            }
        }
    }
}
