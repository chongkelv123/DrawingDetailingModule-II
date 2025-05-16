using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingDetailingModule.Interfaces
{
    /// <summary>
    /// Abstract factory interface that combines all feature factory interfaces
    /// </summary>
    public interface IAbstractFeatureFactory:IFeatureFactory, IHoleFeatureFactory, IPocketFeatureFactory
    {
        // This interface aggregates all the other feature factory interfaces
        // No additional members are needed
    }
}
