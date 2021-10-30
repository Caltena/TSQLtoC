using System;
using System.Linq;

namespace TSQLtoC
{
    public class cStoredProcedure
    {
        public string sStoredProcedure = string.Format(@"
public string up###NAME###( ###VALUES###)
{1}
if (OpenConnection())
{1}
    SqlCommand cmd = new SqlCommand({0}up###NAME###{0}, connection);
    cmd.CommandType = CommandType.StoredProcedure;
    ###PARAM###
    // output parm
    SqlParameter vendor = cmd.Parameters.Add( {0}@ScopeID{0}, SqlDbType.Bigint );
    vendor.Direction = ParameterDirection.Output;
    try
    {1}
        cmd.ExecuteNonQuery();
    {2}
    catch (Exception e)
    {1}
        connection.Close();

    {2}
    connection.Close();
    return vendor.Value.ToString();
{2}
return '';

    {2}", "\"", "{", "}");


        public cStoredProcedure()
        {
            string sPARAM = "";
            string sVALUES = "";
            cField cfTe = new cField();

            foreach (cField cf in Program.lcFields)
            {

                sPARAM += string.Format(@"              SqlParameter p{1} = cmd.Parameters.Add({0}@{1}{0}, SqlDbType.{2} );", "\"", cf.COLUMN_NAME, cf.SQL_STRING);
                sPARAM += Environment.NewLine;
                if (cf.CHARACTER_MAXIMUM_LENGTH != 0)
                {
                    sPARAM += string.Format("            p{1}.Precision = {0} ;", cf.CHARACTER_MAXIMUM_LENGTH, cf.COLUMN_NAME);
                    sPARAM += Environment.NewLine;
                }
                if (cf.NUMERIC_SCALE != 0)
                {
                    sPARAM += string.Format("           p{1}.Scale = {0} ;", cf.NUMERIC_SCALE, cf.COLUMN_NAME);
                    sPARAM += Environment.NewLine;
                }
                sPARAM += string.Format("           p{1}.Value = _{0} ;", cf.COLUMN_NAME, cf.COLUMN_NAME);
                sPARAM += Environment.NewLine;
                cfTe = cf;
            }
            sStoredProcedure = sStoredProcedure.Replace("###PARAM###", sPARAM);
            sStoredProcedure = sStoredProcedure.Replace("###NAME###", cfTe.TABLE_NAME.ToUpper());

            foreach (cField cf in Program.lcFields)
            {
                sVALUES += string.Format(" {0} _{1} ", cf.C_STRING, cf.COLUMN_NAME);
                if (cf != Program.lcFields.Last())
                { sVALUES += ","; }
                sVALUES += Environment.NewLine;

            }
            sStoredProcedure = sStoredProcedure.Replace("###VALUES###", sVALUES);
        }

    }
}
