using UnityEngine;

namespace _Main.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "FlockData", menuName = "main/FlockData", order = 0)]
    public class FlockData : ScriptableObject
    {
        [field : SerializeField] public string Id { get; private set; }
        [field : SerializeField] public Mesh BoidsMesh { get; private set; }
        [field : SerializeField] public Material DefaultMaterial { get; private set; }
        [field : SerializeField] public Material SelectedMaterial { get; private set; }
        [field: SerializeField] public BoidsData BoidsData{ get; private set; }

    }
}