using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingDetailingModule.Model;

namespace DrawingDetailingModule.Controller
{
    public class Control
    {
        NXDrawing drawing;
        public Control()
        {
            drawing = new NXDrawing(this);
        }
    }
}
