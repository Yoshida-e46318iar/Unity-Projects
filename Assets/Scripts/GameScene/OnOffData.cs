using UnityEngine;
[CreateAssetMenu(fileName = "OnOffDataObj", menuName = "Scriptable Objects/OnOffDataObj")]
public class OnOffData : ScriptableObject
{
    [SerializeField] public float preopen;
    [SerializeField] public float open;
    [SerializeField] public float close;
  
}
