using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    public class LoadMenu : MonoBehaviour
    {
        public void Load()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
