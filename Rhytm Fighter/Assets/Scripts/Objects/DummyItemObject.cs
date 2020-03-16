namespace RhytmFighter.Objects
{
    public class DummyItemObject : iGridObject
    {
        public int ID { get; private set; }
        public ObjectTypes Type { get; private set; }

        
        public DummyItemObject(int id)
        {
            ID = id;
            Type = ObjectTypes.Item;
        }
    }
}
