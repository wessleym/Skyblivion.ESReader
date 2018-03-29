using System.IO;

namespace Skyblivion.ESReader
{
    class TES5File//WTM:  Note:  Unused
    {
        private string path;
        private string name;
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

        public void load()
        {
            string filepath = this.path+ Path.DirectorySeparatorChar + this.name;
            //h = fopen(filepath, "rb");//WTM:  Change:  Commented since unused
        }
    }
}