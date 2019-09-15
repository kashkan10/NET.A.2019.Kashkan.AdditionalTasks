using System.Collections.Generic;

namespace FileCabinet
{
    public interface IStorage
    {
        List<User> Import();

        void Export(List<User> list);
    }
}
