using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace TSQLtoC
{

    
    class Program
    {
        public static List<cField> lcFields = new List<cField>();

        static void Main(string[] args)
        {
            cConnectDatabase cDB = new cConnectDatabase();
            cDB.run();
            BuildSP cSP = new BuildSP();
            cNotifikation cNO = new cNotifikation();
            cStoredProcedure cCP = new cStoredProcedure();

            Console.WriteLine(cSP.sStoredProcedure);
            Console.WriteLine(cNO.sNotifikation);

            Console.WriteLine(cCP.sStoredProcedure);


        }




    }


}
