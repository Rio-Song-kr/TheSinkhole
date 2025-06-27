using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class LoadCSV<T>
{
    protected string[] LoadFromCsv(string fileName)
    {
        string filePath = Path.Combine(Application.dataPath, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"CSV file not found: {filePath}");
            return null;
        }

        string[] lines = File.ReadAllLines(filePath, Encoding.UTF8).Skip(3).ToArray();
        return lines;
    }

    protected abstract List<T> ReadDataFromLines(string[] lines);
}