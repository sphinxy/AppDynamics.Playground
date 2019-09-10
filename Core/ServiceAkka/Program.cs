using Common;

namespace ServiceAkka
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CustomWebHostService.Launch(args,59000);
        }
    }
}