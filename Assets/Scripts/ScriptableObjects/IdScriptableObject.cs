using UnityEngine;
using Util;

namespace ScriptableObjects
{
    public class IdentifiableScriptableObject : ScriptableObject
    {
        [ScriptableObjectId]
        public string id;
    }
}
