public enum TileState
{
    PlainTile = 0, //개척 안된 타일
    Frontier, // 개척지 타일
    ChangingState,
    FarmTile, // 농사 가능한 타일
    DefenceArea, //설치 가능한 타일
    WaterTile
}