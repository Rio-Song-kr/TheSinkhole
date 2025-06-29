using System.Linq;
using UnityEngine;

public static class LoadCSV
{
    public static string[] LoadFromCsv(string fileName)
    {
        var csvFile = Resources.Load<TextAsset>($"CSVData/{fileName}");

        if (csvFile != null)
        {
            string[] lines = csvFile.text.Split('\n').Skip(3).ToArray();
            return lines;
        }

        Debug.LogWarning($"CSV File not Found: Resources/CSVData/{fileName}");
        return null;
    }
}