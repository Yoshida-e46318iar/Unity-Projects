using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using static UnityEngine.Rendering.DebugUI;
using System;

public class DataDisplayManager : MonoBehaviour
{
    [SerializeField] TMP_Text[] displayTexts;
    int currentValue = 0;
    int addRemains = 0;
    bool isCoroutinDoing = false;
    bool isMissionAlmostComp = false;
    string[] ketasu = { "D7", "D4", "D4", "D4", "D6", "D6" , "D4" };

    List<int> valueList= new List<int>();
    int currentListLength = 0;

    void Start()
    {
        addRemains = 0;
        currentValue = 0;

        //à√çÜóp
        if (GeneralManager.currentMachineNum == 6)
        {
            UpdateDisplayText(5, 29934, false);
            UpdateDisplayText(6, 1576, false);

        }
    }


    public void UpdateDisplayText(int index, int value,bool iscolorChange)
    {

        if(isMissionAlmostComp)
        {
            switch (GeneralManager.currentMissionNum)
            {
                case 4:
                case 9:
                case 8:
                    if (index == 0)
                        iscolorChange = true;
                    break;
                case 5:
                case 7:
                    if (index == 4)
                        iscolorChange = true;
                    break;
                case 3:
                case 6:
                
                    if (index == 3)
                        iscolorChange = true;
                    break;
            }

        }


        if (iscolorChange)
        {
            displayTexts[index].color = new Color(255, 0, 0);
        }
        else
            displayTexts[index].color = new Color(0,255, 252);






        displayTexts[index].text = value.ToString(ketasu[index]);


    }



    public void MissionAlmostComp(bool status)
    {
        isMissionAlmostComp = status;
    }

    public void AddValue(int value)
    {
        if (value != 0)
        {
            valueList.Add(value);
            currentListLength = valueList.Count;
            MochidamaAddAnime();
        }


    }

    public void MochidamaAddAnime()
    {

        if (!isCoroutinDoing)
        {
            isCoroutinDoing = true;
            StartCoroutine(AddAnime());
            
        }

    }
    IEnumerator AddAnime()
    {
        float countupInterval = 0.05f;

        currentValue = int.Parse(displayTexts[0].text);
        for (int i = 0; i < currentListLength; i++)
        {
            addRemains = valueList[0];
            if (addRemains < 0)
            {
                UpdateDisplayText(0, GeneralManager.mochidama - 1,false);
            }

            else
            {
                while (addRemains > 0)
                {
                    if (addRemains >= 100)
                        countupInterval = 0.005f;

                    yield return new WaitForSeconds(countupInterval);

                    if (addRemains > 100)
                    {
                        currentValue+= addRemains;
                        addRemains =0;
                    }
                    else { 
                        addRemains -= 1;
                        currentValue++;
                    }
                    UpdateDisplayText(0, currentValue, false);

                }

            }
            valueList.RemoveAt(0);
        }

        isCoroutinDoing=false;
        if (valueList.Count > 0)
        {
            currentListLength = valueList.Count;
            MochidamaAddAnime();
        }

    }


}
