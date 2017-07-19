# NlayerAppCustomised
replace WCF by WEBAPI;

replace AutoMapper by original type;

replace Unity by autofac;

orm by using EF CodeFirst;

combine some Layers and interfaces;

Close The EF Configurations below for performance:

AutoDetectChangesEnabled = false;

LazyLoadingEnabled = false;

ProxyCreationEnabled = false;

ValidateOnSaveEnabled = false;

Add GetPaged Functions and Count Functions


use Include params when a query requires navigational properties:

GetAll(params string[] include)

GetFiltered<KProperty>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending, params string[] includeExpression)

GetPaged<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, bool>> filter,  Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending,string[] includeExpression)

GetCount(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter)

ex:

	_repository.Get(key,"column1","column2","column3"....);
        
	_repository.GetAll("column1","column2","column3"....).ToList();
        
	_repository.GetFiltered<TABLEOBJ>(t=>t.Id>10, t=>t.UpdateTime, false, "column1","column2","column3"....).ToList();

it's more easy to use and has a higher performance!

The NlayerApp original version is:

https://github.com/antgerasim/NLayerAppV2
