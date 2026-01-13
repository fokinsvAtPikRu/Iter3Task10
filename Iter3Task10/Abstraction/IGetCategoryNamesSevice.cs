using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace Iter3Task10.Abstraction
{
    public interface IGetCategoryNamesSevice
    {
        List<string> GetCategoryNames();
    }
}
