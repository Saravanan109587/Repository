using BaseRepo;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
namespace Sample
{
    public partial class ReturnsData
    {
        public Nullable<decimal> Returns { get; set; }
        public System.DateTime Date { get; set; }
        public string FundName { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            BaseRepoTestRepo re = new BaseRepoTestRepo();
            List<BaseRepoTest> listData = new List<BaseRepoTest>()
           {
               new BaseRepoTest()
               {
                   Name ="FirstUPdated",
                   Age=1
               },
                new BaseRepoTest()
               {
                   Name ="Second",
                   Age=2
               }
           };
   
          




            re.exeWithparams();
        }
    }


    public class BaseRepoTest
    {
        public string Name { get; set; }
        [ExplicitKey]
        public int Age { get; set; }
    }

    public class BaseRepoTestRepo : Repository 
    {
        public BaseRepoTestRepo() : base("data source=PTQAASSRV03;initial catalog=AlternativeSoft_DB;persist security info=True;user id=asoft;Password=alter@123")
        {

        }

        //public bool insertData(BaseRepoTest data)
        //{
        //    base.Insert(data, null, null);
        //    return true;
        //}
        //public bool insertDataBulk(List<BaseRepoTest> data)
        //{
        //    base.Insert(data[0], null, null);
        //    return true;
        //}
        //public bool Update(List<BaseRepoTest> data)
        //{
        //    base.Update(data[0]);
        //    return true;
        //}
        //public bool Delete(List<BaseRepoTest> data)
        //{
        //    base.Delete(data[0]);
        //    return true;
        //}


        //public IEnumerable<BaseRepoTest> get()
        //{
        //    Expression<Func<BaseRepoTest, bool>> FilterByID(string name)
        //    {
        //        return x => x.Name == name;
        //    }
        //   // return base.FindAll(null); ;
        //      return base.FindAll(FilterByID("Saravanan"));        
        //}



        public void exeWithparams()
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@fundId", 8877);
            var result = base.ExecuteProcedureSingleResult<DateTime>("GetDates");
        }
    }




}
