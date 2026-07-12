using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilitys.Mapper
{
    public interface IMapper
    {
        TDestination Map<TDestination, TSource>(TSource soruce, string? ignore = null);

        IList<TDestination> Map<TDestination, TSource>(IList<TSource> source, string? ignore = null);

        TDestination Map<TDestination>(object soruce, string? ignore = null);
        IList<TDestination> Map<TDestination>(IList<object> source, string? ignore = null);

        TDestination Map<TDestination, TSource>(TSource soruce, TDestination destination, string? ignore = null);

        IList<TDestination> Map<TDestination, TSource>(IList<TSource> soruce, IList<TDestination> destination, string? ignore = null);
    }
}
