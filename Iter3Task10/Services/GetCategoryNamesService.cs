using Autodesk.Revit.DB;
using CSharpFunctionalExtensions;
using Iter3Task10.Abstraction;
using System.Collections.Generic;
using System.Linq;

namespace Iter3Task10.Services
{
    public class GetCategoryNameService : IGetCategoryNamesSevice
    {
        private Document _document;

        public GetCategoryNameService(Document document)
        {
            _document = document;
        }

        public List<string> GetCategoryNames()
        {
            var familySymbols = new FilteredElementCollector(_document)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .Where(fs => fs.Category != null)                
                .ToList();
            return familySymbols
                .Select(fs => fs.Category.Name)
                .Distinct()
                .OrderBy(name=>name)
                .ToList();
        }
    }
}
