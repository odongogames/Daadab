using UnityEngine;

namespace Daadab
{
    public class PooledObject : MonoBehaviour
    {
        [SerializeField] private PooledObjectData data;

        private ObjectPool pool;
        private Transform originalParent;
        private Transform myTransform;
        private GameObject myGameObject;

        public ObjectPool Pool { get => pool; set => pool = value; }
        public PooledObjectData GetData() => data;
        public Transform Transform
        {
            get 
            {
                if (null == myTransform)
                {
                    myTransform = transform;
                }
                
                return myTransform;
            }
        }
        public GameObject GameObject
        {
            get 
            {
                if (null == myGameObject)
                {
                    myGameObject = gameObject;
                }
                
                return myGameObject;
            }
        }

        private void Awake()
        {
            SaveData();

            myTransform = transform;

            originalParent = myTransform.parent;
        }


        [ContextMenu("Save Data")]
        public void SaveData()
        {
            if (null == myTransform)
            {
                myTransform = transform;
            }

            data.Position = myTransform.localPosition;
            data.Rotation = myTransform.rotation;
            data.Scale = myTransform.localScale;
        }

        public void SetData(PooledObjectData newData)
        {
            data = newData;
        }

        public void ApplyData()
        {
            myTransform.localPosition = data.Position;
            myTransform.rotation = data.Rotation;
            myTransform.localScale = data.Scale;
        }

        public void Release()
        {
            pool.ReturnToPool(this);
            
            myTransform.SetParent(originalParent, worldPositionStays: false);
        }
    }
}
