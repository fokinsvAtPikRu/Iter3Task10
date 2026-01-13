using Autodesk.Revit.DB;
using Iter3Task10.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iter3Task10.Services
{
    public class GetLevelsService : IGetLevelsService
    {
        private readonly Document _document;

        public GetLevelsService(Document document)
        {
            _document = document;                
        }

        public List<Level> GetLevel()
        {
            return new FilteredElementCollector(_document)
                .OfClass(typeof(Level))
                .OfType<Level>()
                .ToList();
        }
    }
}
