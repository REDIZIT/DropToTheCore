using UnityEditor;
using UnityEngine;

namespace InGame.Level
{
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

            ClampSize();

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
        private void ClampSize()
        {
            if (float.IsNaN(size.x)) size.x = 100;
            if (float.IsNaN(size.y)) size.y = 100;
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