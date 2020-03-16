namespace RhytmFighter.Objects
{
    public interface iGridObject
    {
        int ID { get; }
        ObjectTypes Type { get; }
        
    }

    public enum ObjectTypes
    {
        Item,
        Enemy
    }
}
