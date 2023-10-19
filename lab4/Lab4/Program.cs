using System;
using System.Linq;

class Program
{
    static Random _random = new Random();
    // Интенсивность входного потока заявок (заявок в час)
    static double _arrivalRate = 2; 
    // Среднее время обслуживания (в часах)
    static double _serviceTime = 0.7;
    // Количество обслуживающих каналов
    static int _channelCount = 3; 

    static void Main(string[] args)
    {
        while (true)
        {
            // Общее количество заявок для имитации
            int totalRequests = 1000000;
            // Количество отклоненных заявок
            int rejectedRequests = 0; 

            // Время завершения обслуживания для каждого канала
            double[] channelFinishTimes = new double[_channelCount]; 
            // Суммарное время, когда хотя бы один канал был занят
            double totalBusyTime = 0; 

            // Текущее время в имитационной модели
            double currentTime = 0; 

            for (int i = 0; i < totalRequests; i++)
            {
                // Генерация времени между поступлениями заявок
                double interArrivalTime = Exponential(_arrivalRate); 
                // Время поступления новой заявки
                double arrivalTime = currentTime + interArrivalTime; 

                bool isRejected = true;
                double nextAvailableTime = channelFinishTimes.Min();

                for (int j = 0; j < _channelCount; j++)
                {
                    if (channelFinishTimes[j] <= arrivalTime && nextAvailableTime == channelFinishTimes[j])
                    {
                        // Обслуживание заявки на канале j
                        channelFinishTimes[j] = arrivalTime + Exponential(_serviceTime); 
                        isRejected = false;
                        // Обновляем суммарное время занятости каналов
                        totalBusyTime += channelFinishTimes[j] - arrivalTime; 
                        break;
                    }
                }

                if (isRejected)
                {
                    // Если нет свободных каналов, заявка отклоняется
                    rejectedRequests++; 
                }

                // Обновляем текущее время
                currentTime = arrivalTime; 
            }

            double rejectionProbability = (double)rejectedRequests / totalRequests;
            double serviceProbability = 1 - rejectionProbability;
            double throughput = serviceProbability * _arrivalRate;
            // Рассчет средней загрузки каналов
            double averageUtilization = totalBusyTime / currentTime; 

            Console.WriteLine($"Вероятность обслуживания: {serviceProbability}");
            Console.WriteLine($"Средняя загрузка каналов: {averageUtilization}");
            Console.WriteLine($"Абсолютная пропускная способность: {throughput}");

            Console.ReadLine();
        }
    }

    // Генерация случайной величины с экспоненциальным распределением
    static double Exponential(double rate)
    {
        return -Math.Log(_random.NextDouble()) / rate;
    }
}
