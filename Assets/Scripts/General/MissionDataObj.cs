using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "MissionDataObj", menuName = "Scriptable Objects/MissionDataObj")]
public class MissionDataObj : ScriptableObject
{
    [SerializeField] public int number;
    [SerializeField] public string missionTitle;
    [SerializeField] public string missionTitleShort;
    [SerializeField] public int value;
    [SerializeField] public int prize;
    [SerializeField] public int itemNumber;





}
