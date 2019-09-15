using System.Collections.Generic;

namespace FileCabinet
{
    public interface IStorage<T>
    {
        List<T> Import();

        void Export(List<T> list);
    }
}
