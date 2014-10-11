namespace KarelTheRobot
{
    public partial class ExtendedKarel : Karel
    {
        protected override void run()
        {
            placeBeeper();
            move();
            run(); // recursion test :P
        }
    }
}
