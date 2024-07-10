using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NXOpen;

namespace DrawingDetailingModule
{
    public partial class MyProgram
    {
        // Terminate point
        public static int GetUnloadOption(string args)
        {
            int unloadOption;
            //unloadOption = System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);         //After executing
            unloadOption = System.Convert.ToInt32(Session.LibraryUnloadOption.AtTermination);     //When NX session terminates
            //unloadOption = System.Convert.ToInt32(Session.LibraryUnloadOption.Explicitly);        //Using File-->Unload

            return unloadOption;
        }
    }
}
