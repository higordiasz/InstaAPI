using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaAPI.Model.Return
{
    public class IReturn
    {
        public int Status { get; set; }
        public string Response { get; set; }
        public Exception Err { get; set; }
        public dynamic Json { get; set; }
        public IReturn (int status, string response)
        {
            Status = status;
            Response = response;
            Err = null;
            Json = null;
        }
    }

    public class IRelation
    {
        public int Status { get; set; }
        public string Response { get; set; }
        public Exception Err { get; set; }
        public Relation Relation { get; set; }
        public dynamic Json { get; set; }
        public IRelation(int status, string response)
        {
            Status = status;
            Response = response;
            Err = null;
            Json = null;
            Relation = null;
        }
    }
}
