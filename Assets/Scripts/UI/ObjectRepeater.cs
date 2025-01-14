using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.UI
{
    public class ObjectRepeater : MonoBehaviour
    {
        public GameObject Prefab;
        public int Count = 1;
        public Vector3 Offset = new Vector3(0, 0, 0);

        public List<GameObject> Objects = new List<GameObject>();

        void Start()
        {
            Objects.Clear();

            for (int i = 1; i < Count; i++)
            {
                Vector3 position = Prefab.transform.position + i * Offset;
                var prefab = Instantiate(Prefab, position, Quaternion.identity, transform);

                Objects.Add(prefab);
            }
        }

        public void ApplyOffsetChanges()
        {
            for (int i = 1; i < Count; i++)
            {
                var prefab = Objects[i - 1];
                prefab.transform.position = Prefab.transform.position + i * Offset;
            }
        }
    }
}