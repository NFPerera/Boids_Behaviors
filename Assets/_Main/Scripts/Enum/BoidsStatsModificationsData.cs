using UnityEngine;

namespace _Main.Scripts.Enum
{
    [CreateAssetMenu(fileName = "BoidsStatsModificationsData", menuName = "main/StatsMod")]
    public class BoidsStatsModificationsData : ScriptableObject
    {
        public BoidsStatsIds boidsStatsIds;
        public float modification;
    }
}