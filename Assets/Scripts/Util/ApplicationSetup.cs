using UnityEngine;

namespace Util
{
    public class ApplicationSetup : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 60;
        }
    }
}
