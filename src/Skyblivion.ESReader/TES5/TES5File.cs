using System.IO;

namespace Skyblivion.ESReader
{
    class TES5File//WTM:  Note:  Unused
    {
        private readonly string path;
        private readonly string name;
        //private masters;//WTM:  Change:  Commented since unused
        //private groups;//WTM:  Change:  Commented since unused
        /*
        * File constructor.
        */
        public TES5File(string path, string name)
        {
            this.path = path;
            this.name = name;
        }

        public void Load()
        {
            //WTM:  Change:  Commented since unused
            //string filepath = Path.Combine(this.path, this.name);
            //h = fopen(filepath, "rb");
        }
    }
}