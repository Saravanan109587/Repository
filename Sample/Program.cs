using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using BaseRepo;
using Dapper;
namespace Sample
{ 
    class Program
    {
        static void Main(string[] args)
        {
            BaseRepoTestRepo re = new BaseRepoTestRepo();
            BaseRepoTest da = new BaseRepoTest()
            {
                ID = 1,
                Name = "Saravananas"
            };
            re.Insert(da);

           var test=  re.get();

            var test2 = re.getPrecalcData();
        }
    }


    public class BaseRepoTest
    {
        public string Name { get; set; }
        public int ID { get; set; }
    }
 
    public class BaseRepoTestRepo : Repository<BaseRepoTest>
    {
        public BaseRepoTestRepo() :base("data source=PTQAASSRV03;initial catalog=AlternativeSoft_DB;persist security info=True;user id=asoft;Password=alter@123")
        {
             
        }

        public bool insertData(BaseRepoTest data)
        {
            base.Insert(data);
            return true;
        }


        public IEnumerable<BaseRepoTest> get()
        {
            Expression<Func<BaseRepoTest, bool>> FilterByID(string name)
            {
                return x => x.Name == name;
            }
           // return base.FindAll(null); ;
              return base.FindAll(FilterByID("Saravanan"));        
        }

        public  IEnumerable<BaseRepoTest> getPrecalcData()
        {
          SqlMapper.GridReader datareader =  base.ExecuteProcedureMultipleResult("USP_GetPrecalculationFunds", null, null);

            
            return datareader.Read<BaseRepoTest>();
            
        }
    }




}
