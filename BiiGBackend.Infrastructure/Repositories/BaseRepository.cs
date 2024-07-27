using BiiGBackend.Infrastructure.Data;
using BiiGBackend.Infrastructure.Repositories.Interfaces;
using BiiGBackend.Models.Requests;
using BiiGBackend.Models.Responses;
using BiiGBackend.Models.SharedModels;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BiiGBackend.Infrastructure.Repositories
{
	public class BaseRepository<T> : IBaseRepository<T> where T : class
	{
		internal readonly DbSet<T> _dbSet;
		internal readonly ApplicationDbContext _context;
		public BaseRepository(ApplicationDbContext context)
		{
			_dbSet = context.Set<T>();
			_context = context;
		}

		public async Task<T> GetItem(Expression<Func<T, bool>> query, string? includeProperties = null)
		{
			var dbSetQueryable = _dbSet.AsQueryable();
			dbSetQueryable = IncludeProperties(dbSetQueryable, includeProperties);
			return await dbSetQueryable.Where(query).FirstOrDefaultAsync();

		}
		public async Task<IEnumerable<T>> GetItems(Expression<Func<T, bool>> query, string? includeProperties = null)
		{
			var dbSetQueryable = _dbSet.AsQueryable();
			dbSetQueryable = IncludeProperties(dbSetQueryable, includeProperties);
			return await dbSetQueryable.Where(query).ToListAsync(); ;
		}




		public async Task<PaginationResponse> GetPaginationItems(PaginationRequest request, Expression<Func<T, bool>> query, string? includeProperties = null)
		{
			try
			{
				var totalNumber = await _dbSet.Where(query).CountAsync();

				var limit = request.PageLimit;
				var page = request.PageNumber;
				var skipValue = limit * (page - 1);


				var totalPages = Math.Ceiling(totalNumber / (decimal)limit);
				var q = _dbSet.AsQueryable();
				q = IncludeProperties(q, includeProperties);
				var pagedValues = q.Where(query).Skip(skipValue).Take(limit).ToList();

				return new PaginationResponse()
				{
					Items = pagedValues,
					PageNumber = page,
					TotalPages = (int)totalPages,
					TotalItems = totalNumber
				};

			}
			catch (Exception ex)
			{
				throw new CustomException(ex.Message);
			}

		}


		public Task<PaginationResponse> GetPaginationItems(PaginationRequest request, ExpressionStarter<T> query, string? includeProperties = null)
		{
			try
			{
				var itemsQuery = _dbSet.AsExpandable().Where(query);


				var totalNumber = itemsQuery.Count();

				var limit = request.PageLimit;
				var page = request.PageNumber;
				var skipValue = limit * (page - 1);
				var totalPages = Math.Ceiling(totalNumber / (decimal)limit);
				var q = _dbSet.AsQueryable();
				q = IncludeProperties(q, includeProperties);
				var pagedValues = q.AsExpandable().Where(query).Skip(skipValue).Take(limit).ToList();

				return Task.FromResult(new PaginationResponse()
				{
					Items = pagedValues,
					PageNumber = page,
					TotalPages = (int)totalPages,
					TotalItems = totalNumber
				});

			}
			catch (Exception ex)
			{
				throw new CustomException(ex.Message);
			}

		}

		public async Task<bool> AddItem(T entity)
		{
			await _dbSet.AddAsync(entity);
			return 0 < await _context.SaveChangesAsync();
		}

		public async Task<bool> AddItems(IEnumerable<T> entities)
		{
			await _dbSet.AddRangeAsync(entities);
			return 0 < await _context.SaveChangesAsync();
		}


		public async Task<bool> DeleteItem(T entity)
		{
			_dbSet.Remove(entity);
			return 0 < await _context.SaveChangesAsync();
		}

		public async Task<bool> DeleteItems(IEnumerable<T> entities)
		{
			_dbSet.RemoveRange(entities);
			return 0 < await _context.SaveChangesAsync();

		}





		private IQueryable<T> IncludeProperties(IQueryable<T> dbSetQueryable, string includeProperties)
		{
			if (includeProperties != null)
			{
				var properties = includeProperties.Split(",", StringSplitOptions.RemoveEmptyEntries);
				foreach (var property in properties)
				{
					dbSetQueryable = dbSetQueryable.Include(property);
				}
			}
			return dbSetQueryable;
		}


	}
}
