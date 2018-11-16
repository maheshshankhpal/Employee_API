using System.Collections.Generic;
using System.Data;

namespace DynamicReportAPI.Models.Repository.LoginRepository
{
   public interface ILoginRepository
    {
       DataSet SelectLoginDetails(string paramString);
    }

}