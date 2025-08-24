using UnityEngine;

[CreateAssetMenu(fileName = "SpecDataObj", menuName = "Scriptable Objects/SpecDataObj")]
public class SpecDataObj : ScriptableObject
{
    [SerializeField] public int number;
    [SerializeField] public int[] payouts;
    [SerializeField] public int[] fbunbo;
    [SerializeField] public int[] picWeight;
    [SerializeField] public int[] rounddatas;
    [SerializeField] public int[] jitandataL;
    [SerializeField] public int[] jitandataH;
    [SerializeField] public int machineType;
    [SerializeField] public float[] startKposOffsets;
    [SerializeField] public float[] haneKposOffsets;

}
