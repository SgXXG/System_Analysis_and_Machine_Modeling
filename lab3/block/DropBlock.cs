using System.Diagnostics;

namespace saimon3.blocks;

public class DropBlock : ITickable
{
    public ITickable NextBlock { get; set; }
    public int WorkTicks { get; set; }
    private Request? _currentRequest;
    private readonly bool _hasTimer;
    private readonly int _maxTimerTicks;
    private int _currentTimerTicks;
    private readonly double _probability;
    private readonly Random _random;

    public DropBlock(int maxTimerTicks)
    {
        _hasTimer = true;
        _maxTimerTicks = maxTimerTicks;
        _currentTimerTicks = 0;
        _currentRequest = null;
    }
    
    public DropBlock(double probability, Random random)
    {
        _hasTimer = false;
        _probability = probability;
        _random = random;
        _currentRequest = null;
    }
    
    public void NextTick()
    {
        if (_currentRequest == null) return;
        
        _currentRequest.TimeInSystem++;
        WorkTicks++;
        
        if (_hasTimer)
        {
            // если есть таймер и достигнуто максимальное количество тактов,
            // выполняем отправку запроса
            if (++_currentTimerTicks >= _maxTimerTicks) _trySend();
        }
        else
        {
            // если нет таймера, выполняем отправку запроса с вероятностью
            if (_random.NextDouble() > _probability) _trySend();
        }
    }
    
    // попытка отправить запрос на следующий блок, если он доступен,
    // после отправки запроса устанавливаем текущий запрос как null
    private void _trySend()
    {
        if (NextBlock.CanAccept()) NextBlock.Accept(_currentRequest);
        _currentRequest = null;
    }

    // принятие запроса и установка его как текущего для обработки,
    // если есть таймер, сбрасываем текущее количество тактов
    public void Accept(Request req)
    {
        _currentRequest = req;
        if (_hasTimer) _currentTimerTicks = 0;
    }

    // возвращает true, если текущий запрос отсутствует (блок свободен)
    public bool CanAccept()
    {
        return _currentRequest == null;
    }

    // возвращает количество текущих запросов в блоке (0 или 1)
    public int GetRequestCount() => _currentRequest != null ? 1 : 0;
}