using System;

public enum EWorldStatus
{
    Ghost,
    Living
}

static class EWorldStatusMethods
{
    public static EWorldStatus Advance(this EWorldStatus worldStatus)
    {
        switch (worldStatus)
        {
            case EWorldStatus.Living:
                return EWorldStatus.Ghost;
            case EWorldStatus.Ghost:
                return EWorldStatus.Living;
            default:
                throw new NotSupportedException("Unknown world status" + worldStatus);
        }
    }
}
