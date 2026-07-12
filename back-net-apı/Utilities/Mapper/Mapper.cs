using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;

namespace Utilitys.Mapper
{
    public class Mapper : Utilitys.Mapper.IMapper
    {
        public static List<TypePair> typePairs = new List<TypePair>();
        private AutoMapper.IMapper _mapper;

        public TDestination Map<TDestination, TSource>(TSource source, string? ignore = null)
        {
            Config<TDestination, TSource>(5, ignore);
            return _mapper.Map<TSource, TDestination>(source);
        }

        public IList<TDestination> Map<TDestination, TSource>(IList<TSource> source, string? ignore = null)
        {
            Config<TDestination, TSource>(5, ignore);
            return _mapper.Map<IList<TSource>, IList<TDestination>>(source);
        }

        public TDestination Map<TDestination>(object source, string? ignore = null)
        {
            Config<TDestination,object>(5, ignore);
            return _mapper.Map<TDestination>(source);
        }

        public IList<TDestination> Map<TDestination>(IList<object> source, string? ignore = null)
        {
            Config<TDestination, IList<object>>(5, ignore);
            return _mapper.Map<IList<TDestination>>(source);
        }

        public TDestination Map<TDestination, TSource>(TSource soruce, TDestination destination, string? ignore = null)
        {
            Config<TDestination, TSource>(5, ignore);
            return _mapper.Map(soruce,destination);
        }

        public IList<TDestination> Map<TDestination, TSource>(IList<TSource> soruce, IList<TDestination> destination, string? ignore = null)
        {
            Config<TDestination, TSource>(5, ignore);
            return _mapper.Map<IList<TSource>, IList<TDestination>>(soruce,destination);
        }

        protected void Config<TDestination,TSource>(int dept = 5, string? ignore = null)
        {
            var typePair = new TypePair(typeof(TSource), typeof(TDestination));
            if (typePairs.Any(x => x.DestinationType == typePair.DestinationType && x.SourceType == typePair.SourceType) && ignore is null)
                return;

            typePairs.Add(typePair);

            var config = new MapperConfiguration(cfg =>
            {
                foreach (var item in typePairs)
                {
                    if (ignore is not null)
                        cfg.CreateMap(item.SourceType, item.DestinationType).MaxDepth(dept).ForMember(ignore, x=> x.Ignore()).ReverseMap();
                    else
                        cfg.CreateMap(item.SourceType, item.DestinationType).MaxDepth(dept).ReverseMap();
                }
            });
            _mapper = config.CreateMapper();
        }
    }
}
