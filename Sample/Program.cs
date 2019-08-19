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
    public enum SeriesType
    {
        Fund = 1,
        BM1 = 2,
        BM2 = 3,
        BM3 = 4,
        BM4 = 5,
        BM5 = 6
    }
    [Serializable]
    public class TimeSeries : ICloneable
    {
        public SeriesType SeriesType { get; set; }
        public int FundID { get; set; }
        public string FundName { get; set; }
        public Dictionary<DateTime, double> Returns { get; set; }
        public DateTime Start { get { return Returns.OrderByDescending(x => x.Key).LastOrDefault().Key; } }
        public DateTime End { get { return Returns.OrderByDescending(x => x.Key).FirstOrDefault().Key; } }
        public List<double> RetValue { get { return Returns.OrderBy(x => x.Key).Select(x => x.Value).ToList(); } }
        public object Clone()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return (TimeSeries)formatter.Deserialize(stream);
            }
        }
    }

    public class PrecalData
    {
        public List<PrecalculationFunds> Funds { get; set; }
        public List<StatInfo> Stats { get; set; }

    }
    public class StatInfo
    {
        public int StatID { get; set; }
        public string StatDisplayName { get; set; }
        public string StatDescription { get; set; }
        public int StatisticsGroupID { get; set; }
        public int StatisticsPeriodID { get; set; }
        public string Type { get; set; }
        public bool IsCalcLibraryStat { get; set; }
        public string Calculator { get; set; }
        public int DataPointsCount { get; set; }

        public string FunctionKey { get; set; }
        public string PreCaltable { get; set; }
        public string NodeName { get; set; }
    }

    public class PrecalculationFunds
    {
        public int FUNDID { get; set; }
        public int? Benchmark1 { get; set; }
        public int? Benchmark2 { get; set; }
        public int Frequency { get; set; }
        public int TimeSeriesDataID { get; set; }
        public System.DateTime ContextDate { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public List<TimeSeries> TsCollection { get; set; }
    }

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

        public  IEnumerable<PrecalculationFunds> getPrecalcData()
        {
          SqlMapper.GridReader datareader =  base.ExecuteProcedureMultipleResult("USP_GetPrecalculationFunds", null, null);

            
            return datareader.Read<PrecalculationFunds>();
            
        }
    }




}
