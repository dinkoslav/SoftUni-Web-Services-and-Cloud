namespace Problem2.DistanceCalculatorClient
{
    using CalcDistance;
    using System;
    class DistanceCalculatorClient
    {
        static void Main()
        {
            CalcDistanceClient calcDistanceClient = new CalcDistanceClient();
            double distance = calcDistanceClient.GetDistance(new Point() { X = 10, Y = 10 }, new Point() { X = 15, Y = 15 });
            Console.WriteLine(distance);
        }
    }
}
