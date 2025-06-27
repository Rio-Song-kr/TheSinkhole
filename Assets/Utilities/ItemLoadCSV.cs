using System.Collections.Generic;

public class ItemLoadCSV : LoadCSV<ItemFileData>
{
    public List<ItemFileData> ReadData(string path) => ReadDataFromLines(LoadFromCsv(path));

    protected override List<ItemFileData> ReadDataFromLines(string[] lines)
    {
        var dataList = new List<ItemFileData>();

        // for (int i = 0; i < lines.Length; i++)
        foreach (string line in lines)
        {
            string[] fields = line.Split(',');

            if (fields.Length >= 7)
            {
                var data = new ItemFileData
                {
                    ItemId = int.Parse(fields[0]),
                    ItemName = fields[1],
                    ItemEnName = fields[2],
                    ItemType = fields[3],
                    ItemMaxOwn = int.Parse(fields[4]),
                    EffectId = int.Parse(fields[5]),
                    ItemText = fields[6]
                };

                dataList.Add(data);
            }
        }

        return dataList;
    }
}