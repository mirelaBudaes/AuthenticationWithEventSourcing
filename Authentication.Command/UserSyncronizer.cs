using Authentication.SqlStore.Models;

namespace Authentication.Command
{
    internal class UserSyncronizer
    {
        private readonly UserSqlRepository _userSqlRepository;

        public UserSyncronizer(UserSqlRepository userSqlRepository)
        {
            _userSqlRepository = userSqlRepository;
        }
        public void Save(User user)
        {
            if (user == null)
                return;

            _userSqlRepository.Save(user);
        }
    }
}
