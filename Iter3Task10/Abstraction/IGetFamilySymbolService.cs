using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iter3Task10.Abstraction
{
    public interface IGetFamilySymbolsService
    {
        List<FamilySymbol> GetFamilySymbols(string categoryName);
    }
}
