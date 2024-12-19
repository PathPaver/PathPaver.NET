using Microsoft.EntityFrameworkCore;
using PathPaver.Application.Repository.Entities;
using PathPaver.Persistence.Context;
using PathPaver.Persistence.Repository;
namespace PathPaver.Persistence.Repository.Entities;

public sealed class GraphRepository(AppDbContext context) : BaseRepository<Graph>(context), IGraphRepository
{
    #region Overrided Methods from BaseRepository
    public async Task<IEnumerable<Graph>> GetGraphsAsync()
    {
        var data = await context.GraphData.ToListAsync();
        return data.AsEnumerable();
    }
    #endregion
}