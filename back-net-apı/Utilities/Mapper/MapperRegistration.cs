using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Mapper;

namespace Utilities.Mapper
{
    public static class MapperRegistration
    {
        public static void AddMapperApplication(this IServiceCollection services)
        {
            services.AddSingleton<IMapper, Utilities.Mapper.Mapper>();
        }
    }
}