namespace saimon3.blocks;

public class ServicePointBlock : ITickable
{
    public ITickable NextBlock { get; set; }
    public int WorkTicks { get; set; }
    private Request? _currentRequest;
    private State _currentState;
    private readonly bool _hasTimer;
    private readonly int _maxTimerTicks;
    private int _currentTimerTicks;
    private readonly double _probability;
    private readonly Random _random;
    public int BlockedTime = 0;
    
    public ServicePointBlock(int maxTimerTicks)
    {
        _hasTimer = true;
        _maxTimerTicks = maxTimerTicks;
        _currentTimerTicks = 0;
        _currentState = State.Open;
    }
    
    public ServicePointBlock(double probability, Random random)
    {
        _hasTimer = false;
        _probability = probability;
        _random = random;
        _currentState = State.Open;
    }
    

    public void NextTick()
    {
        switch (_currentState)
        {
            case State.Close:
            {
                _currentRequest.TimeInSystem++;
                WorkTicks++;
                
                if (_hasTimer)
                {
                    // если блок использует таймер и достиг максимального
                    // количества тактов, пытаемся отправить запрос
                    if (++_currentTimerTicks >= _maxTimerTicks) _trySend();
                }
                else
                {
                    // если блок не использует таймер,
                    // используем вероятность для отправки запроса
                    if (_random.NextDouble() > _probability) _trySend();
                }
                break;      
            }
            case State.Stop:
            {
                _currentRequest.TimeInSystem++;
                WorkTicks++;
                BlockedTime++;
                
                _trySend();  
                break;
            }
        }
    }

    private void _trySend()
    {
        if (NextBlock.CanAccept())
        {
            // отправляем текущий запрос следующему блоку,
            // устанавливаем состояние блока как открытое
            NextBlock.Accept(_currentRequest);
            _currentState = State.Open;
        }
        else
        {
            // если следующий блок не может принять запрос,
            // устанавливаем состояние блока как остановленное (Stop)
            _currentState = State.Stop;
        }
    }
    
    // принимаем новый запрос для обработки, устанавливаем состояние
    // блока как закрытое (Close), если блок использует таймер,
    // сбрасываем счетчик таймера
    public void Accept(Request req)
    {
        _currentRequest = req;
        _currentState = State.Close;
        if (_hasTimer) _currentTimerTicks = 0;
    }

    public bool CanAccept()
    {
        return _currentState == State.Open;
    }

    // возвращает количество запросов, которые могут быть обработаны блоком
    public int GetRequestCount() => _currentState != State.Open ? 1 : 0;
}

public enum State
{
    Open,
    Close,
    Stop
}