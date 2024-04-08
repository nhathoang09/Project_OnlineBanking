using Project_OnlineBanking.Models;

namespace Project_OnlineBanking.Services;

public interface RoleService
{
    public Role find(int? id);

    public Role findByName(string name);
}
