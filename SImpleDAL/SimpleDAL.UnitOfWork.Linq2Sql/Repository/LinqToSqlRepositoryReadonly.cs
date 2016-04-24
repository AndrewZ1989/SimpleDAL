using SimpleDAL.Utility.Specifications;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace SimpleDAL.UnitOfWork.Linq2Sql
{
    public abstract class LinqToSqlRepositoryReadonly<TBusinesEntity, TStorageEntity, TDataContext> 
        : RepositoryBaseReadonly<TBusinesEntity, TStorageEntity>
		where TBusinesEntity : class
		where TStorageEntity : class
		where TDataContext : DataContext
	{
		#region fields

		protected TDataContext _privateDataContext;

		private Func<TDataContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> _mapperGenerator;

		private GetMapperDelegate<TDataContext, TBusinesEntity, TStorageEntity> _getMapperDelegate;

		#endregion

		#region .ctor

		public LinqToSqlRepositoryReadonly( Func<TDataContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> mapperGenerator )
			: base()
		{
			_mapperGenerator = mapperGenerator;
		}

		public LinqToSqlRepositoryReadonly( Func<TDataContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> mapperGenerator, TDataContext context )
			: this( mapperGenerator )
		{
			if ( context == null ) return;
			this._privateDataContext = context;
		}


		public LinqToSqlRepositoryReadonly( GetMapperDelegate<TDataContext, TBusinesEntity, TStorageEntity> getMapperDelegate ) : base()
		{
			_getMapperDelegate = getMapperDelegate;
		}

		public LinqToSqlRepositoryReadonly( GetMapperDelegate<TDataContext, TBusinesEntity, TStorageEntity> getMapperDelegate, TDataContext context )
			: this( getMapperDelegate )
		{
			if ( context == null ) return;
			this._privateDataContext = context;
		}

		#endregion

		#region Properties

		protected TDataContext DataContext
		{
			get
			{
				if ( _privateDataContext != null )
				{
					return _privateDataContext;
				}
				else
				{
					var uow = GetCurrentUnitOfWork<LinqToSqlUnitOfWork<TDataContext>>();
					if ( uow == null ) throw new Exception("Current unit of work not exists.");

					return uow.Context;
				}
			}
		}

		protected Table<TStorageEntity> Table
		{
			get
			{
				return DataContext.GetTable<TStorageEntity>();
			}
		}

		#endregion

		#region Abstract

		/// <summary>
		/// Дополнительная конфигурация сущности перед возвращением из запроса
		/// </summary>
		protected virtual TBusinesEntity ConfigureBeforeReturn( TBusinesEntity bEntity )
		{
			return bEntity;
		}

		#endregion

		#region Overrides of RepositoryBaseReadonly

		public override IEnumerable<TBusinesEntity> Query( ISpecification<TBusinesEntity> specification )
		{
			return QueryBase<List<TBusinesEntity>>( specification, new List<TBusinesEntity>(), ( counter, entity ) =>
			{
				entity = ConfigureBeforeReturn( entity );
				counter.Add( entity );
				return counter;
			} );
		}

		public override IEnumerable<TBusinesEntity> Query( ISpecification<TBusinesEntity> specification, IInclude<TBusinesEntity> include )
		{
			var result = Query( specification );

			foreach ( var item in result ) include.Include( item );

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

		#region Protected methods

		protected IRepositoryMapper<TBusinesEntity, TStorageEntity> GetMapper()
		{
			if ( _mapperGenerator != null ) return _mapperGenerator.Invoke( DataContext );

			if ( _getMapperDelegate != null ) return _getMapperDelegate.Invoke( () => DataContext );

			throw new Exception("Data mapping error.Could not create mapper." );
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
				var mapper = GetMapper();
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
			var mapper = GetMapper();

			foreach ( var item in Table )
			{
				var bEntity = mapper.MapStorageToBusines( item );
				if ( specification.IsSatisfiedBy( bEntity ) )
					yield return ConfigureBeforeReturn( bEntity );
			}
		}

		#endregion
	}
}
