using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
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

        public List<string> GetCategoryNames() =>
        _document.Settings.Categories
            .OfType<Category>()
            .Select(c => c.Name)
            .ToList();
    }
}
