using System;
using System.Collections.Generic;

namespace TSQLtoC
{


    class Program
    {
        public static List<cField> lcFields = new List<cField>();

        static void Main(string[] args)
        {
            cConnectDatabase cDB = new cConnectDatabase();
            cDB.DataSource = @"127.0.0.1";
            cDB.InitialCatalog = "caltena";
            cDB.Table = "Houses";
            cDB.run();
            BuildSP cSP = new BuildSP();
            cNotifikation cNO = new cNotifikation();
            cStoredProcedure cCP = new cStoredProcedure();
            // TEST TEST TEST TEST
            Console.WriteLine("{0}-------------------------------------------{0}", Environment.NewLine);
            Console.WriteLine(cSP.sStoredProcedure);
            Console.WriteLine("{0}-------------------------------------------{0}", Environment.NewLine);
            Console.WriteLine(cNO.sNotifikation);
            Console.WriteLine("{0}-------------------------------------------{0}", Environment.NewLine);
            Console.WriteLine(cCP.sStoredProcedure);
            Console.WriteLine("{0}-------------------------------------------{0}", Environment.NewLine);
            Console.WriteLine(cDB.ConnectionString);
            Console.ReadKey();
        }



    }


}
