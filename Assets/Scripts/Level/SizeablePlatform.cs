using UnityEditor;
using UnityEngine;

namespace InGame.Level
{
    [ExecuteInEditMode]
    public class SizeablePlatform : MonoBehaviour
    {
        public Vector2 size = new Vector2(-1, -1);

        public bool useNewWayToDetectPrefab;

        private SpriteRenderer spriteRenderer;
        private BoxCollider2D boxCollider;

        private bool isResizeScheduled;

        private void OnValidate()
        {
            isResizeScheduled = true;
            //Resize();
        }

        private void Start()
        {
            Resize();
        }

        private void LateUpdate()
        {
            if (isResizeScheduled)
            {
                isResizeScheduled = false;
                Resize();
            }
        }

        public void Resize()
        {
            if (IsPrefab(gameObject, useNewWayToDetectPrefab))
            {
                return;
            }

            if (spriteRenderer == null) GetComponents();

            if (size.x < 0 || size.y < 0) ImportSize();

            if (spriteRenderer != null) spriteRenderer.size = size;
            if (boxCollider != null) boxCollider.size = size;
        }

        private void GetComponents()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider = GetComponent<BoxCollider2D>();
        }

        private void ImportSize()
        {
            size = spriteRenderer.size;
        }

        public static bool IsPrefab(GameObject target, bool useNewWay = false)
        {
#if UNITY_EDITOR
            if (useNewWay) return PrefabUtility.GetPrefabType(target) == PrefabType.Prefab;
            return PrefabUtility.GetPrefabParent(target) == null && PrefabUtility.GetPrefabObject(target.transform) == null;
#endif
            return false;
        }
    }
}