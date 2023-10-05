namespace saimon3;

// интерфейс представляет объект, способный выполнять
// тактовые операции в СМО
public interface ITickable
{
    public ITickable NextBlock { get; set; }
    public int WorkTicks { get; set; }
    // выполняет операции в текущем блоке на каждом такте моделирования
    public void NextTick();
    // принимает запрос и обрабатывает его в текущем блоке.
    // req - принимаемый запрос.
    public void Accept(Request req);
    // определяет, доступен ли текущий блок для принятия новых запросов.
    public bool CanAccept();
    // возвращает количество запросов, находящихся в текущем блоке.
    public int GetRequestCount();
}