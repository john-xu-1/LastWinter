using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASPGenerator{
    public class ASPFileGenerator : ASPGenerator
    {
        [SerializeField] protected ASPGeneratorBehavior generatorBehavior;
        [SerializeField] protected string jsonFilename = "answersetJsonTest1.txt";

        public enum ASPGeneratorBehavior
        {
            generate,
            saveToFile,
            generateFromFile
        }

        protected override void startGenerator()
        {
            if (generatorBehavior == ASPGeneratorBehavior.generate || generatorBehavior == ASPGeneratorBehavior.saveToFile)
                base.startGenerator();
            else
            {
                generateFromJson(WorldBuilder.SaveUtility.DataFilePath + "/" + jsonFilename);
            }
        }

        protected override void SATISFIABLE()
        {

            base.SATISFIABLE();
            if (generatorBehavior == ASPGeneratorBehavior.saveToFile)
            {
                saveToJson();
            }

        }

        protected virtual void generateFromJson(string jsonFilename)
        {
            string answersetJson = WorldBuilder.SaveUtility.GetFile(jsonFilename);
            Clingo.AnswerSet answerSet = JsonUtility.FromJson<Clingo.AnswerSet>(answersetJson);
            map.DisplayMap(answerSet, mapKey);
        }

        protected virtual void saveToJson()
        {
            string answersetJson = JsonUtility.ToJson(solver.answerSet);
            WorldBuilder.SaveUtility.CreateFile(answersetJson, jsonFilename);
        }
    }
}