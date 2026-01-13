using Autodesk.Revit.DB;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iter3Task10.Abstraction
{
    public interface IPlaceService
    {
        Result Place(string categoryNameSelected, FamilySymbol familySymbol, Level level, int step, int count);
    }
}
