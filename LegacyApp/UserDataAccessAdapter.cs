namespace LegacyApp;

public class UserDataAccessAdapter : IUserDataAccessAdapter
{
    public void AddUser(User user)
    {
        UserDataAccess.AddUser(user);
    }
}