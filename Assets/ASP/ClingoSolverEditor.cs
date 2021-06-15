using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ClingoSolver))]
public class ClingoSolverEditor : Editor
{

    string solutionOutput = "";
    TextAsset aspFile;


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ClingoSolver solver = (ClingoSolver)target;

        EditorGUILayout.LabelField("Duration: ", solver.Duration.ToString());
        EditorGUILayout.LabelField("Seed: ", solver.Seed.ToString());
        EditorGUILayout.LabelField("Solutions Found: ", solver.SolutionsFound.ToString());
        EditorGUILayout.LabelField("More Solutions: ", solver.MoreSolutions.ToString());
        EditorGUILayout.LabelField("Is Solver Running: ", solver.IsSolverRunning.ToString());


        EditorGUILayout.PrefixLabel("File Contents");
        if (solver.aspFile == null)
        {
            EditorGUILayout.TextArea("");
        }
        else
        {
            EditorGUILayout.TextArea(solver.aspFile.text);
        }



        //if (GUILayout.Button("Solve"))
        //{
        //    solutionOutput = solver.SolverOutput();
        //}

        if (GUILayout.Button("Solve in Thread"))
        {
            //solver.MyThread();
            solver.solveUsingThread();
            //solutionOutput = solver.solutionOutput;
        }

        solutionOutput = solver.SolutionOutput;

        EditorGUILayout.PrefixLabel("Solution");
        EditorGUILayout.TextArea(solutionOutput);

        EditorGUILayout.PrefixLabel("Raw Clingo Output");
        EditorGUILayout.TextArea(solver.ClingoConsoleOutput);

        EditorGUILayout.PrefixLabel("Raw Clingo Error Output");
        EditorGUILayout.TextArea(solver.ClingoConsoleError);


    }

}
