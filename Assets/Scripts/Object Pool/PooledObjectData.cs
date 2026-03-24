using System;
using UnityEngine;

namespace Daadab
{
    [Serializable]
    public class PooledObjectData
    {
        public PooledObjectVariable Variable;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
    }
}