using Common;

namespace ServiceAkkaFull
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CustomWebHostService.Launch(args,59001);
        }
    }
}