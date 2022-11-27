using System;
using System.Data;
using System.Linq;

namespace TSQLtoC
{


    class BuildSP
    {

        public string sStoredProcedure = @"
/*______________________________________________
   Object:  StoredProcedure ###PREFIX######NAME###
________________________________________________*/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE ###SCHEMA###.###PREFIX######NAME###
        ( @ScopeID bigint OUTPUT
            ###VARIABLES###
        )
AS

        DECLARE @flgDebug    BIT          = 0
            , @strSPName VARCHAR(128)
            , @rstrOwn_Msg VARCHAR(500) = ''
            , @rstrSQL_Msg VARCHAR(500) = ''
            , @rstr_1 VARCHAR(100) = ''
            , @ret_1 VARCHAR(100)

BEGIN TRY
    SET @strSPName = OBJECT_NAME(@@PROCID)
    SET @flgDebug = 0

    ###INSERTUPDATE###

    RETURN -1
END TRY
BEGIN CATCH
    SET @rstrSQL_Msg = ERROR_MESSAGE()
    SET @rstr_1 = CONVERT(VARCHAR, ERROR_NUMBER())
    RETURN 0
END CATCH";

        public BuildSP()
        {
            ReplaceVARIABLES();
            ReplacePrimaryKey();
            ReplaceInsert();
            ReplaceUpdate();
            ReplaceNamet();
        }

        private void ReplaceNamet()
        {
            cField cFPrimary = Program.lcFields.Where(s => s.IS_PK != 1)
                                    .Select(s => s).FirstOrDefault();
            sStoredProcedure = sStoredProcedure.Replace("###NAME###", cFPrimary.TABLE_NAME);
            sStoredProcedure = sStoredProcedure.Replace("###SCHEMA###", cFPrimary.TABLE_SCHEMA);
            sStoredProcedure = sStoredProcedure.Replace("###PREFIX###", "up_");
        }

        private void ReplaceInsert()
        {
            string sInsert = "";


            try
            {
                cField cFPrimary = Program.lcFields.Where(s => s.IS_PK == 1)
                                        .Select(s => s).Single();

                sInsert = string.Format(@" INSERT INTO {0}.{1} ( {2}", cFPrimary.TABLE_SCHEMA, cFPrimary.TABLE_NAME, Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0} ", ex.Message);
            }



            var lsf = Program.lcFields.Where(s => s.IS_PK == 0)
                                    .Select(s => s);
            cField last = lsf.Last();
            foreach (cField cfFields in lsf)
            {
                sInsert += string.Format("      {0}", cfFields.COLUMN_NAME);
                if (cfFields.Equals(last) == false) sInsert += " , ";
                sInsert += string.Format("      {0}", Environment.NewLine);
            }
            sInsert += string.Format(" ) VALUES (     {0}", Environment.NewLine);
            foreach (cField cfFields in lsf)
            {
                sInsert += string.Format("      @{0}", cfFields.COLUMN_NAME);
                if (cfFields.Equals(last) == false) sInsert += " , ";
                sInsert += string.Format("      {0}", Environment.NewLine);
            }
            sInsert += string.Format(@" ) {0} If @@ROWCOUNT = 1 Select @ScopeID = SCOPE_IDENTITY() ", Environment.NewLine);
            sStoredProcedure = sStoredProcedure.Replace("###INSERT###", sInsert);
        }


        private void ReplaceUpdate()
        {
            string sUpdate = "";
            try
            {
                cField cFPrimary = Program.lcFields.Where(s => s.IS_PK == 1)
                                        .Select(s => s).Single();

                sUpdate = string.Format(@" UPDATE {0}.{1} SET {2}", cFPrimary.TABLE_SCHEMA, cFPrimary.TABLE_NAME, Environment.NewLine);

                var lsf = Program.lcFields.Where(s => s.IS_PK == 0)
                                        .Select(s => s);
                cField last = lsf.Last();
                foreach (cField cfFields in lsf)
                {
                    sUpdate += string.Format("      {0} = @{0}", cfFields.COLUMN_NAME);
                    if (cfFields.Equals(last) == false) sUpdate += " , ";
                    sUpdate += string.Format("      {0}", Environment.NewLine);
                }

                sUpdate += string.Format(@"  WHERE  [{0}] = @{0}  {1}", cFPrimary.COLUMN_NAME, Environment.NewLine);
                sUpdate += string.Format(@"  SELECT  @ScopeID = @{0}  {1}", cFPrimary.COLUMN_NAME, Environment.NewLine);
                sStoredProcedure = sStoredProcedure.Replace("###UPDATE###", sUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }

        private void ReplacePrimaryKey()
        {
            string sPrimaryKey = "";
            try
            {
                cField cF = Program.lcFields.Where(s => s.IS_PK == 1)
                .Select(s => s).Single();

                sPrimaryKey += string.Format(@" IF ISNULL( @{0} , 0 ) != 0  {1} " +
                                              "             BEGIN  {1}  " +
                                              "                 /* UPDATE */  {1}  " +
                                              "                 ###UPDATE###  {1}  " +
                                              "             END  {1} " +
                                              "         ELSE  {1} " +
                                              "             BEGIN  {1}  " +
                                              "                 /* INSERT */  {1}  " +
                                              "                 ###INSERT###  {1}  " +
                                              "             END  {1} ", cF.COLUMN_NAME, Environment.NewLine);

                sStoredProcedure = sStoredProcedure.Replace("###INSERTUPDATE###", sPrimaryKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ReplacePrimaryKey {0}", ex.Message);
            }

        }

        private void ReplaceVARIABLES()
        {
            string sVariables = "";

            foreach (cField cF in Program.lcFields)
            {
                sVariables += string.Format(@"      , @{0} {1}  {2}", cF.COLUMN_NAME, cF.SQL_STRING, Environment.NewLine);
            }

            sStoredProcedure = sStoredProcedure.Replace("###VARIABLES###", sVariables);
        }

    }
}
