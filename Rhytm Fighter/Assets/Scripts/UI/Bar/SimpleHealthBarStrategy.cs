namespace RhytmFighter.UI.Bar
{
    public class SimpleHealthBarStrategy : AbstractBarStrategy
    {
        public SimpleHealthBar Bar;

        public override void SetProgress(int cur, int max)
        {
            Bar.UpdateBar(cur, max);
        }
    }
}
