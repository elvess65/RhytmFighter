namespace RhytmFighter.Objects
{
    public class DummyEnemyObject : iGridObject
    {
        public int ID { get; private set; }
        public ObjectTypes Type { get; private set; }


        public DummyEnemyObject(int id)
        {
            ID = id;
            Type = ObjectTypes.Enemy;
        }
    }
}
