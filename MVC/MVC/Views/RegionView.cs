namespace BasicConnectivity.Views;

public class RegionView : GeneralView
{
    public string InsertInput()
    {
        Console.WriteLine("Insert region name");
        var name = Console.ReadLine();

        return name;
    }

    public Region UpdateRegion()
    {
        Console.WriteLine("Insert region id");
        var id = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Insert region name");
        var name = Console.ReadLine();

        return new Region {
            Id = id,
            Name = name
        };
    }
}
