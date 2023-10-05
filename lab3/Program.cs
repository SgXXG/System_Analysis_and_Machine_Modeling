using System.Globalization;
using saimon3;
using saimon3.blocks;

// модель СМО
var simulationModel = new SimulationModel();
var random = new Random();

simulationModel.AddBlock(new ServicePointBlock(2));
simulationModel.AddBlock(new DropBlock(0.4, random));
simulationModel.AddBlock(new QueueBlock(2));
simulationModel.AddBlock(new DropBlock(1.0, random));

simulationModel.Compile();
simulationModel.RunSimulation(50000);

var values = simulationModel.GetSimulationValues();

// вывод значений
Console.WriteLine($"A : {values.A}");
Console.WriteLine($"Q : {values.Q}");
Console.WriteLine($"P отказа : {values.P1}");
Console.WriteLine($"P блокировки : {values.P2}");
Console.WriteLine($"L очереди : {values.L1}");
Console.WriteLine($"L заявок в системе : {values.L2}");
Console.WriteLine($"W очереди : {values.W1}");
Console.WriteLine($"W заявок в системе : {values.W2}");
Console.WriteLine($"K : {string.Join(", ", values.K.Select(x => x.ToString(CultureInfo.CurrentCulture)))}");