using System; 
using System.IO;
using System.ComponentModel;
 

namespace GraphicResearchHuiZhao
{


    #region MeshSelection
    public class MeshSelectionRecord : BaseRecord
    {
        private string filename;
        private MeshSelection selection;

        public string Filename
        {
            get { return filename; }
        }
        [Browsable(false)]
        public MeshSelection Selection
        {
            get { return selection; }
        }

        public MeshSelectionRecord(string filename, MeshSelection selection)
        {
            this.filename = filename;
            this.selection = selection;
        }
        public override string ToString()
        {
            return Path.GetFileName(filename);
        }
    }
    #endregion

	
}
