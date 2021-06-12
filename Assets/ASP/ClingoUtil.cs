﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ClingoUtil
{

    public static string DataFilePath = @"DataFiles/temp";

    public static string CreateFile(string content)
    {
        string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt";
        return CreateFile(content, FileName);
    }

    public static string CreateFile(string content, string filename)
    {
        string relativePath;
        if (Application.isEditor)
        {
            relativePath = Path.Combine("Assets", DataFilePath, filename);
        }
        else
        {
            relativePath = Path.Combine(DataFilePath, filename);
        }

        using (StreamWriter streamWriter = File.CreateText(relativePath))
        {
            streamWriter.Write(content);
        }
        return Path.Combine(DataFilePath, filename);
    }


}
