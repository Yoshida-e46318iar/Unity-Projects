using UnityEngine;
using UnityEngine.UI;

public class CountDotCtrl : MonoBehaviour
{
    [SerializeField] GameObject[] Dots;
    [SerializeField] Sprite[] sprites;



    void Start()
    {
        AllClear();
    }



    public void UpdateOnOff(int index,int mode)
    {
        int dotnum = index;
        if (dotnum > 9)
            dotnum = 9;

        Dots[dotnum].GetComponent<Image>().sprite = sprites[mode];
    }

    public void AllClear()
    {
        for (int i = 0; i < Dots.Length; i++)
        {
            UpdateOnOff(i, 0);
        }
    }

}
