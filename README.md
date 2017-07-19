# NlayerAppCustomised
Replace WCF by WEBAPI;

Replace AutoMapper by original type;

Replace Unity by autofac;

ORM by using EF CodeFirst;

Combine some Layers and interfaces;

Close these EF Configurations below for performance:

	AutoDetectChangesEnabled = false;

	LazyLoadingEnabled = false;

	ProxyCreationEnabled = false;

	ValidateOnSaveEnabled = false;

Add GetPaged Functions and Count Functions.

use Include params when a query requires navigational properties:

	GetAll(params string[] include);

	GetFiltered<KProperty>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending, params string[] includeExpression);

	GetPaged<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, bool>> filter,  Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending,string[] includeExpression);

	GetCount(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter);

ex:

	_repository.Get(key,"column1","column2","column3"....);
        
	_repository.GetAll("column1","column2","column3"....).ToList();
        
	_repository.GetFiltered<TABLEOBJ>(t=>t.Id>10, t=>t.UpdateTime, false, "column1","column2","column3"....).ToList();

it's more easy to use and more efficient!

The NlayerApp original version is:

https://github.com/antgerasim/NLayerAppV2
