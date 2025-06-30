using System;

/// <summary>
/// CSV에서 TileState 데이터를 읽어오기 위한 Struct
/// </summary>
[Serializable]
public struct TileStateFileData
{
    public int TileId;
    public int PosX;
    public int PosY;
    public string TileType;
    public string Description;

    public TileStateFileData(string[] fields)
    {
        TileId = int.Parse(fields[0]);
        PosX = int.Parse(fields[1]);
        PosY = int.Parse(fields[2]);
        TileType = fields[3];
        Description = fields[4];
    }
}