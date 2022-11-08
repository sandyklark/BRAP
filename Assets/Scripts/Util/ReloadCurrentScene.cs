using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    public class ReloadCurrentScene : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }

        public void Reload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
