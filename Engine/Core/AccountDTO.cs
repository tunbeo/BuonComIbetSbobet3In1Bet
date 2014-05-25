using System;
using System.Linq;
namespace iBet.DTO
{
    [System.Serializable]
    public class AccountDTO : BaseDTO
    {
        public string Name
        {
            get;
            set;
        }
        public string OutStanding
        {
            get;
            set;
        }
        public string AccountType
        {
            get;
            set;
        }
        public static AccountDTO SearchAccountByName(string accToSeach, System.Collections.Generic.List<AccountDTO> dataSource)
        {
            AccountDTO result;
            System.Collections.Generic.List<AccountDTO> list = (
                    from acc in dataSource
                    where acc.Name == accToSeach
                    select acc).ToList<AccountDTO>();
            if (list.Count == 1)
            {
                result = list[0];
            }
            else
            {
                result = null;
            }
            return result;
        }
        public static bool IsMasterOfAllAccount(string accToSearch, System.Collections.Generic.List<AccountDTO> dataSource)
        {
            System.Collections.Generic.List<AccountDTO> list = (
                    from acc in dataSource
                    where (acc.Name.Contains(accToSearch))
                    select acc).ToList<AccountDTO>();
            if (list.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
