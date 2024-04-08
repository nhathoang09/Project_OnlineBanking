using Project_OnlineBanking.Models;

namespace Project_OnlineBanking.Services;

public class RoleServiceImpl : RoleService
{
    private DatabaseContext db;

  

    public RoleServiceImpl(DatabaseContext _db)
    {
        db = _db;
    }

    public Role find(int? id)
    {
        return db.Roles.SingleOrDefault(r => r.RoleId == id);
    }

    public Role findByName(string name)
    {
        return db.Roles.SingleOrDefault(r => r.Name == name);
    }
}
