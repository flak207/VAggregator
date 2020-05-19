using KEA.VAggregator.StdLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KEA.VAggregator.StdLib.Services
{
    public interface IVideoService
    {
        IEnumerable<Category> GetCategories();
    }
}
