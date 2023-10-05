namespace saimon3.blocks;

public class QueueBlock : ITickable
{
    public ITickable NextBlock { get; set; }
    public int WorkTicks { get; set; } = -1;
    private readonly Queue<Request> _requestQueue;
    private readonly int _capacity;
    public readonly List<int> Snapshots;

    // инициализация
    public QueueBlock(int capacity)
    {
        _capacity = capacity;
        _requestQueue = new Queue<Request>();
        
        Snapshots = new List<int>();
    }

    public void NextTick()
    {
        Snapshots.Add(_requestQueue.Count);
        foreach (var request in _requestQueue)
        {
            request.TimeIQueue++;
            request.TimeInSystem++;
        }
        
        if (_requestQueue.Count == 0 || !NextBlock.CanAccept()) return;
        var req = _requestQueue.Dequeue();
        NextBlock.Accept(req);
    }

    public void Accept(Request req)
    {
        if (NextBlock.CanAccept())
            NextBlock.Accept(req);
        else
            _requestQueue.Enqueue(req);
    }

    public bool CanAccept() => _requestQueue.Count < _capacity;
    
    public int GetRequestCount() => _requestQueue.Count;
}