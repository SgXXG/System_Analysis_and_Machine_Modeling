namespace saimon3.blocks;

public class StartBlock : ITickable
{
    public ITickable NextBlock { get; set; }
    public int WorkTicks { get; set; } = -1;
    public int GeneratedRequestsCount;

    public void NextTick()
    {
        if (!NextBlock.CanAccept()) return;
        NextBlock.Accept(new Request());
        GeneratedRequestsCount++;
    }

    public void Accept(Request req)
    {
        return;
    }

    public bool CanAccept()
    {
        return false;
    }

    public int GetRequestCount() => 0;
}