using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using PathPaver.Application.Repository.Entities;
using PathPaver.Persistence.Context;
using PathPaver.Persistence.Repository;
namespace PathPaver.Persistence.Repository.Entities;

public sealed class GraphRepository(AppDbContext context) : BaseRepository<Graph>(context), IGraphRepository
{
    #region Overrided Methods from BaseRepository
    public Graph? GetGraphByName(string name)
    {
        return context.Graphs.FirstOrDefault(g => g.Name == name);
    }
    #endregion
}