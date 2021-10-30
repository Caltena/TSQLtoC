  class cMSSQL
    {

        /// <summary>
        /// The string SQL
        /// </summary>
        public string strSQL = "";
        /// <summary>
        /// The connection
        /// </summary>
        private SqlConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="cMSSQL"/> class.
        /// </summary>
        public cMSSQL()
        {
            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();

            #if DEBUG
                    connectionString.DataSource = Properties.Settings.Default.DebugDBServer;
                    connectionString.InitialCatalog = Properties.Settings.Default.DebugDataBase;
                    connectionString.IntegratedSecurity = true;
            #else
                    connectionString.DataSource = Properties.Settings.Default.DBServer;
                    connectionString.InitialCatalog = Properties.Settings.Default.DataBase;
                    connectionString.IntegratedSecurity = true;
            #endif
            connection = new SqlConnection(connectionString.ConnectionString);
        }

        /// <summary>Databases the query array.</summary>
        /// <returns></returns>
        public string[] DBQueryArray()
        {
            bool boolDo;
            string[] stringArray = new string[50];
            try
            {
                if (OpenConnection())
                {
                    SqlDataReader record = DBQueryReader(strSQL, connection);
                    while (record.Read())
                    {
                        for (int i = 0; i < record.FieldCount; i++)
                            stringArray[i] = record.GetValue(i).ToString().Replace("'", "");
                    }
                    record.Close();
                    boolDo = DisConnection();
                }
                return stringArray;
            }
            catch (Exception x)
            {
                ShowError(x.Message);
                return stringArray;
            }
        }




        public List<cTIC_Not_Assigned> GetTIC_Not_Assigned()
        {
            List<cTIC_Not_Assigned> lcTIC_Not_AssignedL = new List<cTIC_Not_Assigned>();

            string strSQL = "    SELECT  TIC_Key , ADD_Match , TIC_Title FROM vTIC_not_assigned where tic_key = -1" ;
            if (OpenConnection())
            {
                try
                {
                    SqlDataReader reader = DBQueryReader(strSQL, connection);
                    while (reader.Read())
                    {
                        lcTIC_Not_AssignedL.Add(new cTIC_Not_Assigned(Convert.ToInt32(reader[0]),
                                              reader[1].ToString(),
                                              reader[2].ToString()));
                    }
                    reader.Close();
                    DisConnection();
                }
                catch (Exception e)
                {
                    ShowError(e.Message);
                }
            }

            return lcTIC_Not_AssignedL;
        }


        /// <summary>Databases the query.</summary>
        /// <param name="strTrenner">The string trenner.</param>
        /// <returns></returns>
        public string DBQuery(string strTrenner = "")
        {
            string str = "";

            if (String.IsNullOrEmpty(strSQL))
                return ("false");
            if (OpenConnection())
            {
                SqlDataReader reader = DBQueryReader(strSQL, connection);
                int intAnzahlCol = reader.VisibleFieldCount;
                while (reader.Read())
                {
                    for (int i = 0; i < intAnzahlCol; i++)
                        str += reader[i] + strTrenner;
                }
                reader.Close();
                if (DisConnection())
                    return (str);
                else
                    return ("false");
            }
            else
                return ("false");
        }

        /// <summary>Opens the connection.</summary>
        /// <returns></returns>
        private Boolean OpenConnection()
        {
            try
            {
                connection.Open();
                return (true);
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString());
                return (false);
            }
        }

        /// <summary>Dises the connection.</summary>
        /// <returns></returns>
        private Boolean DisConnection()
        {
            try
            {
                connection.Close();
                return (true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return (false);
            }
        }

        /// <summary>Databases the query reader.</summary>
        /// <param name="strSQL">The string SQL.</param>
        /// <param name="conn">The connection.</param>
        /// <returns></returns>
        private SqlDataReader DBQueryReader(string strSQL, SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand(strSQL, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            return (reader);
        }

        /// <summary>Ausfuehren eines SQL-Befehls</summary>
        /// <param name="strQuery">SQL - Befehl</param>
        public void Execute(string strQuery)
        {
            if (OpenConnection())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(strQuery, connection);
                    SqlDataReader reader = cmd.ExecuteReader();
                }
                catch (Exception e)
                {
                    ShowError(e.Message);
                }
                DisConnection();
            }
            else
            {
                ShowError("could not open Connection");

            }


        }


        /// <summary>
        /// Check the Documnet in
        /// </summary>
        /// <param name="CLU_Key">The cl u_ key.</param>
        /// <param name="DFV_Key">The df v_ key.</param>
        /// <returns></returns>
        public string CheckIn(int CLU_Key, int DFV_Key, string DDL_FileName, int DDL_Key )
        {

            if (OpenConnection())
            {
                #if DEBUG
                     string sFile = String.Format(Properties.Settings.Default.DebugFilePath, Environment.UserName , DDL_FileName);
                #else
                     string sFile = String.Format(Properties.Settings.Default.FilePath, Environment.UserName , DDL_FileName);
                #endif

                SqlCommand cmd = new SqlCommand("upDMS_CheckIn", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pintType = cmd.Parameters.Add("@pintType", SqlDbType.Int); pintType.Direction = ParameterDirection.Input; pintType.Value = 5;
                SqlParameter pstrFile = cmd.Parameters.Add("@pstrFile", SqlDbType.VarChar, 256); pstrFile.Direction = ParameterDirection.Input; pstrFile.Value = sFile;
                SqlParameter pCLU_Key = cmd.Parameters.Add("@pCLU_Key", SqlDbType.Int); pCLU_Key.Direction = ParameterDirection.Input; pCLU_Key.Value = CLU_Key;
                SqlParameter pDDL_Key = cmd.Parameters.Add("@pDDL_Key", SqlDbType.Int); pDDL_Key.Direction = ParameterDirection.Input; pDDL_Key.Value = DDL_Key;
                SqlParameter pflgDelete = cmd.Parameters.Add("@pflgDelete", SqlDbType.Int); pflgDelete.Direction = ParameterDirection.Input; pflgDelete.Value = -1;
                SqlParameter pDFV_Key = cmd.Parameters.Add("@pDFV_Key", SqlDbType.Int); pDFV_Key.Direction = ParameterDirection.Input; pDFV_Key.Value = DFV_Key;
                SqlParameter pDFT_Key = cmd.Parameters.Add("@pDFT_Key", SqlDbType.Int); pDFT_Key.Direction = ParameterDirection.Input; pDFT_Key.Value = DBNull.Value;
                SqlParameter pXXX_Key = cmd.Parameters.Add("@pXXX_Key", SqlDbType.Int); pXXX_Key.Direction = ParameterDirection.Input; pXXX_Key.Value = DBNull.Value;
                SqlParameter pDFV_Desc = cmd.Parameters.Add("@pDFV_Desc", SqlDbType.VarChar, 500); pDFV_Desc.Direction = ParameterDirection.Input; pDFV_Desc.Value = DBNull.Value;
                SqlParameter pDFV_Tags = cmd.Parameters.Add("@pDFV_Tags", SqlDbType.VarChar, 500); pDFV_Tags.Direction = ParameterDirection.Input; pDFV_Tags.Value = DBNull.Value;
SqlParameter ordQty = cmd.Parameters.Add("@ordQty", SqlDbType.Decimal);
ordQty.Precision = x; //Replace x with what you expect in Sql Sp
ordQty.Scale = y; //Replace y with what you expect in Sql Sp
ordQty.Value = 18; //Set value here
inscommand.Parameters.Add(ordQty);

                // output parm
                SqlParameter vendor = cmd.Parameters.Add("@rstrArray", SqlDbType.VarChar, 4000);
                vendor.Direction = ParameterDirection.Output;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    ShowError(e.Message);
                    connection.Close();
                    return "";
                }
                connection.Close();
                return vendor.Value.ToString();
            }
            return "";

        }
        public void ShowError(string strErrorMessage)
        {
            Error diaError = new Error();
            diaError.tberrortext.Text = strErrorMessage + " ";
            diaError.ShowDialog();
        }

private static void UpdateDemographics(Int32 customerID,
    string demoXml, string connectionString)
{
    // Update the demographics for a store, which is stored
    // in an xml column.
    string commandText = "UPDATE Sales.Store SET Demographics = @demographics "
        + "WHERE CustomerID = @ID;";

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        SqlCommand command = new SqlCommand(commandText, connection);
        command.Parameters.Add("@ID", SqlDbType.Int);
        command.Parameters["@ID"].Value = customerID;

        // Use AddWithValue to assign Demographics.
        // SQL Server will implicitly convert strings into XML.
        command.Parameters.AddWithValue("@demographics", demoXml);

        try
        {
            connection.Open();
            Int32 rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine("RowsAffected: {0}", rowsAffected);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

    }