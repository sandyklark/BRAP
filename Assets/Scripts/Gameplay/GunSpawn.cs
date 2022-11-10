using System;
using UnityEngine;

namespace Gameplay
{
    public class GunSpawn : MonoBehaviour
    {
        public Action<GunBehaviour> Spawned;

        public GameObject gunPrefab;
        public Transform spawn;

        private void Start()
        {
            var go = Instantiate(gunPrefab, spawn.position, Quaternion.identity);
            var gun = go.GetComponent<GunBehaviour>();
            Spawned?.Invoke(gun);
        }
    }
}
