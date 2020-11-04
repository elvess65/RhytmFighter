namespace RhytmFighter.UI.Bar
{
    public class SimpleBarStrategy : AbstractBarStrategy
    {
        public SimpleHealthBar Bar;

        public override void SetProgress(int cur, int max)
        {
            Bar.UpdateBar(cur, max);
        }
    }
}
