using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using ClingoHelperJSON;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
//q: [2 12 ][1 7 ][6 10 ][8 11 ][4 6 ][7 8 ][3 3 ][11 9 ][5 2 ][9 4 ][12 5 ][10 1 ]

public delegate void solverCallback(string clingoOutput);

public class ClingoSolver : MonoBehaviour
{

    public TextAsset aspFile;
    public string clingoExecutablePathMacOS = "Assets/ASP/ClingoBinaries/MacOS/clingo";
    public int maxDuration = 10; // in seconds
    public bool multipleSolution = false;
    public int numOfSolutionsWanted = 1; // set to 0 for all possible solution


    // Read Only
    private int randomSeed;
    private int totalSolutionsFound = -1;
    private bool moreSolutions = false; // Clingo's way to tell us there might be more solutions
    private double duration; // How long to run clingo
    private bool isSolverRunning = false;
    private string solutionOutput;
    private string clingoConsoleOutput;
    private string clingoConsoleError;

    public int Seed { get { return randomSeed; } }
    public bool MoreSolutions { get { return moreSolutions; } }
    public int SolutionsFound { get { return totalSolutionsFound; } }
    public double Duration { get { return duration; } }
    public bool IsSolverRunning { get { return isSolverRunning; } }
    public string SolutionOutput { get { return solutionOutput; } }
    public string ClingoConsoleOutput { get { return clingoConsoleOutput; } }
    public string ClingoConsoleError { get { return clingoConsoleError; } }

    // Private
    // The path has to be set in the main thread
    private string aspFilePath;
    private Thread thread;
    private static string[] trueArray = { "true" };

    public Dictionary<string, List<List<string>>> answerSet = new Dictionary<string, List<List<string>>>();
    public bool isSolved;

    // Call this before you call solve
    // This will kill the thread if you have one running;
    // There might be a better way to handle the thread than just aborting it and making a new one.
    // Returns false if aspfile or clingo are missing.
    public bool Reset() {
        if (thread != null)
        {
            thread.Abort();
        }
        if (aspFile == null) { print("aspFile is missing");  return false;  }
        if (!File.Exists(clingoExecutablePathMacOS)) { print("Clingo is missing"); return false; }


        thread = new Thread(MyThread);
        isSolverRunning = false;

        if (maxDuration < 1)
        {
            maxDuration = 10; // 10 sec
        }
        if (numOfSolutionsWanted < 0)
        {
            numOfSolutionsWanted = 1;
        }
        solutionOutput = "";
        clingoConsoleOutput = "";
        clingoConsoleError = "";
        duration = 0;
        totalSolutionsFound = -1;
        aspFilePath = AssetDatabase.GetAssetPath(aspFile);

        return true;
    }


    public void solveUsingThread()
    {
        if (!Reset()) { return; }
        if (!thread.IsAlive)
        {
            randomSeed = Random.Range(0, 1 << 30);
            thread.Start();
            isSolverRunning = true;
            UnityEngine.Debug.Log("Solver is running.");
        }
        else
        {
            UnityEngine.Debug.LogWarning("Solver is already running.");
        }
    }

    public void MyThread()
    {
        SolveNoReturn();
        solutionOutput = AnswerSetToString();
    }

    public string AnswerSetToString()
    {
        StringBuilder sb = new StringBuilder();

        List<string> keys = new List<string>(answerSet.Keys);
        foreach (string key in keys)
        {
            sb.Append(key + ": ");
            foreach (List<string> l in answerSet[key])
            {
                sb.Append("[");
                foreach (string s in l)
                {
                    sb.Append(s);
                    sb.Append(" ");
                }
                sb.Append("]");
            }
            sb.Append("\n");
        }
        return sb.ToString();
    }



    public void SolveNoReturn()
    {
        answerSet.Clear();
        if (File.Exists(clingoExecutablePathMacOS))
        {
            string arguments = " --outf=2 ";
            arguments += aspFilePath + " ";
            if (multipleSolution)
            {
                arguments += numOfSolutionsWanted.ToString() + " "; // 0 to show all answers
            }
            arguments += "--sign-def=rnd --seed=" + randomSeed;

            Process process = new Process();


            process.StartInfo.FileName = clingoExecutablePathMacOS;
            process.StartInfo.Arguments = arguments;

            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.Start();

            process.WaitForExit(maxDuration * 1000);


            clingoConsoleOutput = process.StandardOutput.ReadToEnd();
            clingoConsoleError = process.StandardError.ReadToEnd();

            print(clingoConsoleOutput);

            ClingoRoot clingoOutput = JsonUtility.FromJson<ClingoRoot>(clingoConsoleOutput);
            var values = clingoOutput.Call[0].Witnesses[0].Value;


            totalSolutionsFound = clingoOutput.Models.Number;
            moreSolutions = !clingoOutput.Models.More.Equals("no");
            duration = clingoOutput.Time.Total;


            foreach (string value in values)
            {
                int start = value.IndexOf('(');
                int end = value.IndexOf(')');

                if (start < 0 || end < 0)
                {
                    string key = value;
                    if (!answerSet.ContainsKey(key))
                    {
                        answerSet.Add(key, new List<List<string>>());
                    }

                    answerSet[key].Add(new List<string>(trueArray));
                }
                else
                {
                    string key = value.Substring(0, start);
                    string keyValue = value.Substring(start + 1, end - start - 1);

                    if (!answerSet.ContainsKey(key))
                    {
                        answerSet.Add(key, new List<List<string>>());
                    }

                    string[] body = keyValue.Split(',');
                    answerSet[key].Add(new List<string>(body));

                }
            }
            isSolved = true;
        }
        isSolverRunning = false;
        UnityEngine.Debug.Log("Solver is Done.");

    }

}
