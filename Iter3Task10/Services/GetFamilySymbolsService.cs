using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Iter3Task10.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iter3Task10.Services
{
    public class GetFamilySymbolsService : IGetFamilySymbolsService
    {
        private Document _document;

        public GetFamilySymbolsService(Document document)
        {
            _document = document;
        }

        public List<FamilySymbol> GetFamilySymbols(string categoryName) =>
             new FilteredElementCollector(_document)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .Where(fs => fs.Category != null && fs.Category.Name == categoryName)                
                .ToList();
    }
}
