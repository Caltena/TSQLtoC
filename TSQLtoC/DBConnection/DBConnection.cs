using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TSQLtoC
{

    public class cConnectDatabase : IDisposable
    {

        private SqlConnection connection = new SqlConnection();

        public cConnectDatabase()
        {
            this.UserID = "SQL_2016_Prod";
            this.Password = "anatevka";
            this.InitialCatalog = "test";
            this.DataSource = @"CALTENA-XPS13\SQLEXPRESS";
            this.IntegratedSecurity = true;

            this.Table = "Houses";
        }

        public void run()
        {
            BuildConnectionString();
            Program.lcFields = ReadShowInfo();
        }

        public string UserID { get; set; }
        public string Password { get; set; }
        public bool IntegratedSecurity { get; set; }
        public string InitialCatalog { get; set; }
        public string DataSource { get; set; }
        public string Table { get; set; }
        public string ConnectionString { get; set; }

        private void BuildConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.UserID = this.UserID;
            builder.Password = this.Password;
            builder.InitialCatalog = this.InitialCatalog;
            builder.DataSource = this.DataSource;
            builder.IntegratedSecurity = this.IntegratedSecurity;
            connection.ConnectionString = builder.ConnectionString;
            this.ConnectionString = connection.ConnectionString;
        }

        private List<cField> ReadShowInfo()
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = string.Format(@" 
                                    SELECT TABLE_CATALOG,
                                        TABLE_SCHEMA,
                                        COLUMN_NAME,
                                        DATA_TYPE,
                                        ISNULL(CHARACTER_MAXIMUM_LENGTH, 0),
                                        ISNULL(NUMERIC_PRECISION, 0),
                                        ISNULL(NUMERIC_PRECISION_RADIX, 0),
                                        CASE
                                            WHEN tz.PKNAME IS NULL THEN
                                                0
                                            ELSE
                                                1
                                        END,
                                        ISNULL(tz.PKNAME, ''),
                                        ISNULL(NUMERIC_SCALE, 0),
                                        Table_Name
                                FROM information_schema.columns
                                    LEFT JOIN
                                    (
                                        SELECT C.COLUMN_NAME AS PKNAME,
                                                C.TABLE_NAME AS TableName
                                        FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS T
                                            JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE C
                                                ON C.CONSTRAINT_NAME = T.CONSTRAINT_NAME
                                        WHERE T.CONSTRAINT_TYPE = 'PRIMARY KEY'
                                    ) tz
                                        ON tz.TableName = TABLE_NAME
                                            AND COLUMN_NAME = PKNAME
                                WHERE TABLE_NAME = '{0}'", this.Table);

            cmd.Connection = connection;
            List<cField> lT = new List<cField>();
            try
            {

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                // Call Read before accessing data.
                while (reader.Read())
                {
                    cField cT = new cField();
                    cT.TABLE_CATALOG = reader[0].ToString();
                    cT.TABLE_SCHEMA = reader[1].ToString();
                    cT.COLUMN_NAME = reader[2].ToString();
                    cT.CHARACTER_MAXIMUM_LENGTH = Convert.ToInt16(reader[4]);
                    cT.NUMERIC_PRECISION = Convert.ToInt16(reader[5]);
                    cT.NUMERIC_PRECISION_RADIX = Convert.ToInt16(reader[6]);
                    cT.IS_PK = Convert.ToInt16(reader[7]);
                    cT.PKNAME = reader[8].ToString();
                    cT.NUMERIC_SCALE = Convert.ToInt16(reader[9]);
                    cT.TABLE_NAME = reader[10].ToString();
                    cT.DATA_TYPE = reader[3].ToString();
                    lT.Add(cT);
                }

                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            return lT;
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                connection.Close();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public string Execute(string strQuery)
        {
            string strReturn = "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = strQuery;
            cmd.Connection = connection;

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Close();
            }
            catch (Exception e)
            {

                strReturn = e.Message;
            }
            finally
            {
                connection.Close();
            }
            return strReturn;
        }


    }



}
