using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswersetJson : MonoBehaviour
{
    [SerializeField] protected ASPMap.ASPMap map;
    [SerializeField] protected ASPMap.ASPMapKey mapKey;
    [SerializeField] protected string answersetJsonFilename = "answersetJsonTest1.txt";
    // Start is called before the first frame update
    void Start()
    {
        Display(WorldBuilder.SaveUtility.DataFilePath + "/" + answersetJsonFilename, map, mapKey);
    }

    public void Display(string answersetJsonFilename, ASPMap.ASPMap map, ASPMap.ASPMapKey mapKey)
    {
        string answersetJson = WorldBuilder.SaveUtility.GetFile(answersetJsonFilename);
        Clingo.AnswerSet answerSet = JsonUtility.FromJson<Clingo.AnswerSet>(answersetJson);
        map.DisplayMap(answerSet, mapKey);
    }
}
