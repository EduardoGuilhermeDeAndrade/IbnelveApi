using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbnelveApi.Application.Mappers.Interfaces
{
    

    /// <summary>
    /// Interface genérica para mapeamento entre tipos
    /// </summary>
    /// <typeparam name="TSource">Tipo de origem</typeparam>
    /// <typeparam name="TDestination">Tipo de destino</typeparam>
    public interface IMapper<in TSource, out TDestination>
    {
        /// <summary>
        /// Mapeia um objeto do tipo origem para o tipo destino
        /// </summary>
        /// <param name="source">Objeto de origem</param>
        /// <returns>Objeto mapeado para o tipo destino</returns>
        TDestination Map(TSource source);
    }

    /// <summary>
    /// Interface genérica para mapeamento bidirecional entre tipos
    /// </summary>
    /// <typeparam name="TFirst">Primeiro tipo</typeparam>
    /// <typeparam name="TSecond">Segundo tipo</typeparam>
    public interface IBidirectionalMapper<TFirst, TSecond> :
        IMapper<TFirst, TSecond>,
        IMapper<TSecond, TFirst>
    {
    }

    /// <summary>
    /// Interface para mapeamento de coleções
    /// </summary>
    /// <typeparam name="TSource">Tipo de origem</typeparam>
    /// <typeparam name="TDestination">Tipo de destino</typeparam>
    public interface ICollectionMapper<in TSource, out TDestination>
    {
        /// <summary>
        /// Mapeia uma coleção de objetos do tipo origem para o tipo destino
        /// </summary>
        /// <param name="source">Coleção de origem</param>
        /// <returns>Coleção mapeada para o tipo destino</returns>
        IEnumerable<TDestination> MapCollection(IEnumerable<TSource> source);
    }


}
