using System;
using UnityEngine;

namespace Gun
{
    public class GunSpawn : MonoBehaviour
    {
        public Action<GunBehaviour> Spawned;

        public GameObject gunPrefab;

        private Transform _spawn;

        private void Start()
        {
            _spawn = FindObjectOfType<GunSpawnPoint>()?.transform;

            if (gunPrefab == null)
            {
                Debug.Log("No gun prefab supplied.  \nAdd a reference to a GunBehaviour in the GunSpawn component");
                return;
            }

            if (_spawn == null)
            {
                Debug.Log("No gun spawn point found.  \nAdd a GameObject with a GunSpawnPoint component attached");
                return;
            }

            var go = Instantiate(gunPrefab, _spawn.position, Quaternion.identity);
            var gun = go.GetComponent<GunBehaviour>();
            Spawned?.Invoke(gun);
        }
    }
}
