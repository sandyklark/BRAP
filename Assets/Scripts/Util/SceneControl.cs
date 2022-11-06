using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    public class SceneControl : MonoBehaviour
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
            SceneManager.LoadScene(0);
        }
    }
}
