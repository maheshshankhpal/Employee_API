using System.Data;

namespace employee_api.Models.Repository.LoginRepository
{
   public interface ILoginRepository
    {
       DataSet SelectLoginDetails(string paramString);
    }

}