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
        public void InitializeGenerator(ASPGeneratorBehavior generatorBehavior)
        {
            this.generatorBehavior = generatorBehavior;
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

        protected override void finalizeGenerator()
        {
            if (generatorBehavior == ASPGeneratorBehavior.saveToFile)
            {
                saveToJson();
            }
            base.finalizeGenerator();
        }

        

        protected virtual void generateFromJson(string jsonFilename)
        {
            string answersetJson = WorldBuilder.SaveUtility.GetFile(jsonFilename);
            Clingo.AnswerSet answerSet = JsonUtility.FromJson<Clingo.AnswerSet>(answersetJson);
            satifiableCallBack(answerSet,"");
        }

        protected virtual void saveToJson()
        {
            string answersetJson = JsonUtility.ToJson(solver.answerSet);
            WorldBuilder.SaveUtility.CreateFile(answersetJson, jsonFilename);
        }
    }
}