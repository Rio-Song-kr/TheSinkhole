using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLoadCSV : LoadCSV<ItemFileData>
{
    public List<ItemFileData> ReadData(string path) => ReadDataFromLines(LoadFromCsv(path));

    protected override List<ItemFileData> ReadDataFromLines(string[] lines)
    {
        var dataList = new List<ItemFileData>();

        for (int i = 0; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split(',');

            if (fields.Length >= 6)
            {
                var data = new ItemFileData
                {
                    ItemId = int.Parse(fields[0]),
                    ItemName = fields[1],
                    ItemType = fields[2],
                    ItemMaxOwn = int.Parse(fields[3]),
                    EffectId = int.Parse(fields[4]),
                    ItemText = fields[5]
                };

                dataList.Add(data);
            }
        }

        return dataList;
    }
}