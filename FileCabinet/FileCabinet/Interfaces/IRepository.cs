using System.Collections;
using System.Collections.Generic;

namespace FileCabinet
{
    public interface IRepository : IEnumerable
    {
        User this[int id] { get; set; }

        List<User> this[string name] { get; }

        List<User> this[string name, string lastName] { get; }

        void Create(User user);

        void Remove(int id);

        int Stat();

        void Purge();

        void Export(IStorage storage);

        void Import(IStorage storage);
    }
}
