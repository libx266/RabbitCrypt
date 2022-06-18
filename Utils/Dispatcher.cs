using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrypt.Utils
{
    internal static class Dispatcher
    {
        internal static async Task Multithread(int count, Action<int> foo) =>
            await Task.WhenAll(Enumerable.Range(0, count).Select(i => Task.Run(() => foo(i))));
    }
}
