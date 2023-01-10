using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace borra2.Skia.Tizen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new borra2.App());
            host.Run();
        }
    }
}
