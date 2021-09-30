using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace WorldBuilder
{
    public class SaveUtility : Utility
    {
        public static string DataFilePath = @"WorldBuilder/WorldJson";
        public static string[] getFileNames()
        {
            string[] allFiles = Directory.GetFiles(System.IO.Path.Combine("Assets", DataFilePath));

            List<string> files = new List<string>();

            for (int i = 0; i < allFiles.Length; i++)
            {
                string file = allFiles[i];

                if (!file.Contains(".meta"))
                {
                    file = file.Replace("Assets\\", "");
                    file = file.Replace("Assets/", "");
                    files.Add(file);
                }
            }

            return GetArray(files);
        }

        public static string GetFile(string name)
        {
            string path = Application.dataPath + "/" + name;
            StreamReader reader = new StreamReader(path);

            string contents = reader.ReadToEnd();

            return contents;
        }

        public static void SaveWorld(World world, string name)
        {

            string json = JsonUtility.ToJson(world, true);


            int counter = 0;
            string testName = name;
            string dataFilePath = Application.isEditor ? System.IO.Path.Combine("Assets", DataFilePath) : DataFilePath;
            while (File.Exists(System.IO.Path.Combine(dataFilePath, testName + ".txt")))
            {
                counter += 1;
                testName = name + " " + counter;
            }

            Debug.Log(System.IO.Path.Combine(dataFilePath, testName + ".txt"));
            CreateFile(json, $"{testName}.txt");


        }

        public static string CreateFile(string content, string filename)
        {
            //string relativePath;
            //if (Application.isEditor)
            //{
            //    if (!Directory.Exists(System.IO.Path.Combine("Assets", DataFilePath)))
            //    {
            //        Directory.CreateDirectory(System.IO.Path.Combine("Assets", DataFilePath));
            //    }

            //    relativePath = System.IO.Path.Combine("Assets", DataFilePath, filename);
            //}
            //else
            //{
            //    if (!Directory.Exists(DataFilePath))
            //    {
            //        Directory.CreateDirectory(DataFilePath);
            //    }
            //    relativePath = System.IO.Path.Combine(DataFilePath, filename);
            //}
            //Debug.Log(relativePath);
            //using (StreamWriter streamWriter = File.CreateText(relativePath))
            //{
            //    streamWriter.Write(content);
            //}
            //return System.IO.Path.Combine(DataFilePath, filename);
            return CreateFile(content, filename, DataFilePath);
        }

        public static string CreateFile(string content, string filename, string DataFilePath)
        {
            string relativePath;
            if (Application.isEditor)
            {
                if (!Directory.Exists(System.IO.Path.Combine("Assets", DataFilePath)))
                {
                    Directory.CreateDirectory(System.IO.Path.Combine("Assets", DataFilePath));
                }

                relativePath = System.IO.Path.Combine("Assets", DataFilePath, filename);
            }
            else
            {
                if (!Directory.Exists(DataFilePath))
                {
                    Directory.CreateDirectory(DataFilePath);
                }
                relativePath = System.IO.Path.Combine(DataFilePath, filename);
            }
            Debug.Log(relativePath);
            using (StreamWriter streamWriter = File.CreateText(relativePath))
            {
                streamWriter.Write(content);
            }
            return System.IO.Path.Combine(DataFilePath, filename);
        }

        public static string CreateCSV(string[,] table)
        {
            int x = table.GetUpperBound(0)+1;
            int y = table.GetUpperBound(1)+1;
            Debug.Log($"{x},{y}");
            string tableCSV = "";
            for (int j = 0; j < y; j++)
            {
                string row = "";
                for (int i = 0; i < x; i++)
                {
                    row += table[i, j] + ",";
                }
                tableCSV += row + "\n";
            }
            
            return tableCSV;
        }
    }
}

