using System.Linq.Expressions;
using saimon3.blocks;

namespace saimon3;

public class SimulationModel
{
    private readonly List<ITickable> _blocks = new();
    private List<ITickable> _reverseBlocks;
    private int _tickCount;
    private readonly List<int> _snapshots = new();

    public SimulationModel() => AddBlock(new StartBlock());

    public void Compile()
    {
        AddBlock(new EndBlock());
        _reverseBlocks = Enumerable.Reverse(_blocks).ToList();
    }

    // добавление блока в список
    public void AddBlock(ITickable block)
    {
        if (_blocks.Count != 0) _blocks.Last().NextBlock = block; 
        _blocks.Add(block);
    }

    public void RunSimulation(int requestCount)
    {
        _tickCount = requestCount;
        for (var i = 0; i < requestCount; i++)
        {
            foreach (var block in _reverseBlocks)
            {
                block.NextTick();
            }
            _snapshots.Add(_blocks
                .Select(block => block.GetRequestCount())
                .Aggregate((sum, count) => sum + count));
        }
    }

    public SimulationValues GetSimulationValues()
    {
        // абсолютная пропускная способность
        var A = (_blocks.Last() as EndBlock)!.RequestCount / (double)_tickCount;
        // относительная пропускная способность
        var Q = (_blocks.Last() as EndBlock)!.RequestCount / (double)(_blocks.First() as StartBlock)!.GeneratedRequestsCount;
        // вероятность отакза
        var P1 = 1 - Q;
        
        // вероятность блокировки
        double P2 = 0; 
        foreach (var block in _blocks)
        {
            if (block is ServicePointBlock stopBlock) P2 += stopBlock.BlockedTime;
        }
        P2 /= _tickCount;
        
        var ld = 0.5*(1-P2);
        
        var accumulatorSnapshots = new List<List<int>>();
        foreach (var block in _blocks)
        {
            if (block is QueueBlock accumulator)
            {
                accumulatorSnapshots.Add(accumulator.Snapshots);
            }
        }

        double L1 = 0; 
        if (accumulatorSnapshots.Count > 0) L1 = accumulatorSnapshots.ToSumList().Average();

        var L2 = _snapshots.Average()-1;
        
        double W1 = L1 / ld;
        double W2 = L2 / ld;
        
        // коэффициент загрузки канала
        var KList = _blocks
            .Select((block) => block.WorkTicks / (double)_tickCount)
            .Where(x => x >= 0)
            .Skip(1)
            .ToList();
        
        // А – абсолютная пропускная способность (среднее число заявок,
        // обслуживаемых системой в единицу времени, т.е.
        // интенсивность потока заявок на выходе системы);
        // Q – относительная пропускная способность (вероятность того,
        // что заявка, сгенерированная источником, будет в конечном
        // итоге обслужена системой); 
        // Ротк – вероятность отказа (вероятность того, что заявка,
        // сгенерированная источником, не будет в конечном итоге обслужена
        // системой);  
        // Рбл – вероятность блокировки (вероятность застать источник или
        // канал в состоянии блокировки);
        // Lоч – средняя длина очереди; 
        // Lc – среднее число заявок, находящихся в системе; 
        // Wоч – среднее время пребывания заявки в очереди; 
        // Wс – среднее время пребывания заявки в системе; 
        // Kкан – коэффициент загрузки канала (вероятность занятости канала,
        // рассчитывается для каждого канала по отдельности).
        return new SimulationValues
        {
            A = A,
            Q = Q,
            P1 = P1,
            P2 = P2,
            L1 = L1,
            L2 = L2,
            W1 = W1,
            W2 = W2,
            K = KList,
        };
    }
}

// А – абсолютная пропускная способность (среднее число заявок,
// обслуживаемых системой в единицу времени, т.е. интенсивность потока
// заявок на выходе системы);
// Q – относительная пропускная способность (вероятность того, что заявка,
// сгенерированная источником, будет в конечном итоге обслужена системой); 
// Ротк – вероятность отказа (вероятность того, что заявка, сгенерированная
// источником, не будет в конечном итоге обслужена системой);  
// Рбл – вероятность блокировки (вероятность застать источник или канал в
// состоянии блокировки);
// Lоч – средняя длина очереди; 
// Lc – среднее число заявок, находящихся в системе; 
// Wоч – среднее время пребывания заявки в очереди; 
// Wс – среднее время пребывания заявки в системе; 
// Kкан – коэффициент загрузки канала (вероятность занятости канала,
// рассчитывается для каждого канала по отдельности).
public struct SimulationValues
{
    public double A;
    public double Q;
    public double P1;
    public double P2;
    public double L1;
    public double L2;
    public double W1;
    public double W2;
    public List<double> K;
}