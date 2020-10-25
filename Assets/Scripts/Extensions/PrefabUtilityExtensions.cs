using UnityEngine;

public static class PrefabUtilityExtensions
{
    public static float GetHeight(this GameObject prefab)
    {
        Bounds bounds;
        Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();

        if (renderers.Length > 0)
        {
            bounds = renderers[0].bounds;
            for (int i = 1, ni = renderers.Length; i < ni; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }
        }
        else
        {
            bounds = new Bounds();
        }

        return bounds.size.y;
    }
}
