namespace saimon3;

// представляет собой модель запроса в системе моделирования
public class Request
{
    public int id;
    public int TimeIQueue = 0;
    public int TimeInSystem = 0;
    
    public Request()
    {
        id = new Random().Next();
    }
}