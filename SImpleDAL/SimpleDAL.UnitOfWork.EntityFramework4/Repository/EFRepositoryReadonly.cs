using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using SimpleDAL.Utility.Specifications;

namespace SimpleDAL.UnitOfWork.EntityFramework
{
	public abstract class EFRepositoryReadonly<TBusinesEntity, TStorageEntity, TContext> : RepositoryBaseReadonly<TBusinesEntity, TStorageEntity>
		where TBusinesEntity : class
		where TStorageEntity : class
		where TContext : DbContext
	{
		#region .ctor

		public EFRepositoryReadonly( Func<TContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> mapperGenerator )
			: base()
		{
			_mapperGenerator = mapperGenerator;
		}

		public EFRepositoryReadonly( Func<TContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> mapperGenerator, TContext context )
			: this( mapperGenerator )
		{
			if ( context == null ) return;
			this._privateDataContext = context;
		}

		#endregion

		#region Fields

		protected TContext _privateDataContext;
		protected Func<TContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> _mapperGenerator;

		#endregion

		#region Properties

		protected TContext DataContext
		{
			get
			{
				return _privateDataContext ?? GetCurrentUnitOfWork<EFUnitOfWork<TContext>>().Context;
			}
		}

		protected IQueryable<TStorageEntity> Table
		{
			get
			{
				var objectContext = ( DataContext as IObjectContextAdapter ).ObjectContext;
				return objectContext.CreateQuery<TStorageEntity>( objectContext.CreateObjectSet<TStorageEntity>().EntitySet.Name );
			}
		}

		#endregion

		#region Virtual

		/// <summary>
		/// Дополнительная конфигурация сущности перед возвращением из запроса
		/// </summary>
		protected virtual TBusinesEntity ConfigureBeforeReturnInner( TBusinesEntity bEntity )
		{
			return bEntity;
		}

		#endregion

		#region RepositoryBaseReadonly

		public override IEnumerable<TBusinesEntity> Query( ISpecification<TBusinesEntity> specification )
		{
			return QueryBase<List<TBusinesEntity>>( specification, new List<TBusinesEntity>(), ( counter, entity ) => { counter.Add( entity ); return counter; } );
		}

		public override IEnumerable<TBusinesEntity> Query( ISpecification<TBusinesEntity> specification, IInclude<TBusinesEntity> include )
		{
			var result = Query( specification );

			foreach ( var item in result )
				include.Include( item );

			return result;
		}

		public override IEnumerable<TBusinesEntity> LazyQuery( ISpecification<TBusinesEntity> specification )
		{
			return QueryBase( specification );
		}

		public override IEnumerable<TBusinesEntity> LazyQuery( ISpecification<TBusinesEntity> specification, IInclude<TBusinesEntity> include )
		{
			foreach ( var item in QueryBase( specification ) )
			{
				include.Include( item );
				yield return item;
			}
		}

		public override long QueryCount( ISpecification<TBusinesEntity> specification )
		{
			return QueryBase( specification ).Count();
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Базовый метод запроса
		/// </summary>
		/// <typeparam name="TQueryResult">Тип результата запроса</typeparam>
		/// <param name="specification">Условие отбора</param>
		/// <param name="startValue">Начальное значение результата запроса</param>
		/// <param name="resultModifier">Функция обновления результатов запроса</param>
		/// <returns></returns>
		private TQueryResult QueryBase<TQueryResult>( ISpecification<TBusinesEntity> specification, TQueryResult startValue, Func<TQueryResult, TBusinesEntity, TQueryResult> resultModifier )
		{
			TQueryResult resultValue = startValue;
			using ( var enumerator = Table.GetEnumerator() )
			{
				var mapper = _mapperGenerator( DataContext );
				while ( enumerator.MoveNext() )
				{
					var bEntity = mapper.MapStorageToBusines( enumerator.Current );
					if ( specification.IsSatisfiedBy( bEntity ) )
					{
						resultValue = resultModifier( resultValue, bEntity );
					}
				}
			}
			return resultValue;
		}

		/// <summary>
		/// оптимизация
		/// </summary>
		private IEnumerable<TBusinesEntity> QueryBase( ISpecification<TBusinesEntity> specification )
		{
			var mapper = _mapperGenerator( DataContext );

			foreach ( var item in Table )
			{
				var bEntity = mapper.MapStorageToBusines( item );
				if ( specification.IsSatisfiedBy( bEntity ) )
				{
					( DataContext as IObjectContextAdapter ).ObjectContext.Detach( item );
					yield return bEntity;
				}
			}
		}

		#endregion
	}
}
