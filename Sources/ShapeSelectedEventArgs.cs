using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScatterTangram
{
    public class ShapeSelectedEventArgs : EventArgs
    {
        private TangramShape selectedShape;

        public ShapeSelectedEventArgs(TangramShape selectedShape)
        {
            if (selectedShape == null) throw new NullReferenceException();
            this.selectedShape = selectedShape;
        }

        public TangramShape Shape
        {
            get { return this.selectedShape; }
        }	
    }
}
