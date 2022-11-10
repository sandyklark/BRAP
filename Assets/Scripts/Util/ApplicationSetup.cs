using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    public class ApplicationSetup : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = 60;

            StartCoroutine(LoadMenu());
        }

        private IEnumerator LoadMenu()
        {
            yield return new WaitForSeconds(3f);

            SceneManager.LoadScene("MainMenu");
        }
    }
}
