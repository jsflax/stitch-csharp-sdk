using System;
namespace Stitch
{
    public abstract class KeyValueStore
    {
        protected internal abstract string Get(string key, string def = "");

        protected internal abstract void Remove(string key);

        protected internal abstract void Put(string key, string val);

        protected internal abstract bool Contains(string key);
    }
}
