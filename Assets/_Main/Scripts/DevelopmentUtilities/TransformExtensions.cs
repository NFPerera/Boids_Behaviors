using UnityEngine;

namespace _Main.Scripts.DevelopmentUtilities
{
    public static class TransformExtensions
    {
        public static void DeleteAllChild(this Transform p_transform)
        {
            foreach (Transform l_child in p_transform)
            {
                Object.Destroy(l_child.gameObject);
            }
        }
    }
}