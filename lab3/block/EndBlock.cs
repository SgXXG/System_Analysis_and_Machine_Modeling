namespace saimon3.blocks;

public class EndBlock : ITickable
{
    public ITickable NextBlock { get; set; }
    public int WorkTicks { get; set; } = -1;
    public int RequestCount;
    public readonly List<int> SystemSnapshots = new();
    public readonly List<int> QueueSnapshots = new();

    public void NextTick() {}

    public void Accept(Request req)
    {
        RequestCount++;
        SystemSnapshots.Add(req.TimeInSystem);
        QueueSnapshots.Add(req.TimeIQueue);
    }

    public bool CanAccept() => true;

    public int GetRequestCount() => 0;
}