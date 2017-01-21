using System;
using Buldo.Ngb.FoxClient;

class Program
{

    static void Main(string[] args)
    {
        var client = new FoxClient("http://tomsk.redfoxkrsk.ru/");
        Console.WriteLine($"IsAuthenticated = {client.IsAuthenticated}");
        if (args.Length != 2)
        {
            Console.WriteLine("Недостаточно аргументов");
            Console.ReadLine();
            return;
        }

        var login = args[0];
        var password = args[1];

        try
        {
            client.SetCredentials(login, password);
            Console.WriteLine($"TeamName = {client.TeamName}");
            Console.WriteLine($"IsAuthenticated = {client.IsAuthenticated}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.ReadLine();
    }
}