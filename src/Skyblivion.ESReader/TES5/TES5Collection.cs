using Skyblivion.ESReader.TES4;
using System.Collections.Generic;

namespace Skyblivion.ESReader
{
    class TES5Collection
    {
        private TES4Collection tes4Collection;//WTM:  Change:  Added to rectify problem in add method.
        private string path;
        private List<TES4File> files = new List<TES4File>();
        /*
        * TES5Collection constructor.
        */
        public TES5Collection(TES4Collection tes4Collection, string path)
        {
            this.tes4Collection = tes4Collection;
            this.path = path;
        }

        public void add(string name)
        {//WTM:  Change:  Added tes4Collection argument since the TES4File constructor requires it.
            this.files.Add(new TES4File(tes4Collection, this.path, name));
        }

        public void load(TES4FileLoadScheme loadScheme)//WTM:  Change:  Added loadScheme parameter since the file.load method requires it.
        {
            foreach (var file in this.files)
            {
                file.load(loadScheme);
            }
        }
    }
}