using LiteDB;

namespace CartService.DataStorage;

public class LiteDbContext
{
    public LiteDatabase db { get; init; }

    public LiteDbContext()
    {
        
    }
}
