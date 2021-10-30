using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSQLtoC
{
    public class cField
    {
        public string TABLE_CATALOG;
        public string TABLE_SCHEMA;
        public string COLUMN_NAME;
        public string TABLE_NAME;
        private string _DATA_TYPE;
        public string DATA_TYPE
        {
            get
            {
                return _DATA_TYPE;
            }
            set
            {
                _DATA_TYPE = value;
                ConvertField();
            }
        }
        public int CHARACTER_MAXIMUM_LENGTH;
        public int NUMERIC_PRECISION;
        public int NUMERIC_PRECISION_RADIX;
        public int IS_PK;
        public string PKNAME;
        public int NUMERIC_SCALE;
        public string C_STRING;
        public string SQL_STRING;


        private void ConvertField()
        {
            switch (_DATA_TYPE)
            {
                case "bigint":
                    C_STRING = "Int64";
                    SQL_STRING = "BigInt" ;
                    break;
                case "binary":
                    C_STRING = "Byte[]";
                    SQL_STRING = "Binary";
                    break;
                case "bit":
                    C_STRING = "Boolean";
                    SQL_STRING = "Bit" ;
                    break;
                case "char":
                    C_STRING = "String";
                    SQL_STRING = "NVarChar";
                    break;
                case "date":
                    C_STRING = "DateTime";
                    SQL_STRING = "Date";
                    break;
                case "datetime":
                    C_STRING = "DateTime";
                    SQL_STRING = "DateTime";
                    break;
                case "datetime2":
                    C_STRING = "DateTime";
                    SQL_STRING = "DateTime2";
                    break;
                case "datetimeoffset":
                    C_STRING = "DateTimeOffset";
                    SQL_STRING = "DateTimeOffset";
                    break;
                case "decimal":
                    C_STRING = "Decimal";
                    SQL_STRING = "Decimal";
                    break;
                case "filestream":
                    C_STRING = "Byte[]";
                    SQL_STRING = _DATA_TYPE;
                    break;
                case "float":
                    C_STRING = "Double";
                    SQL_STRING = "Float";
                    break;
                case "image":
                    C_STRING = "Byte[]";
                    SQL_STRING = "Int";
                    break;
                case "int":
                    C_STRING = "Int32";
                    SQL_STRING = "Int";
                    break;
                case "money":
                    C_STRING = "Decimal";
                    SQL_STRING = "Decimal";
                    break;
                case "nchar":
                    C_STRING = "String";
                    SQL_STRING = "NVarChar";
                    break;
                case "ntext":
                    C_STRING = "String";
                    SQL_STRING = "Text";
                    break;
                case "numeric":
                    C_STRING = "Decimal";
                    SQL_STRING = "Decimal";
                    break;
                case "nvarchar":
                    C_STRING = "String";
                    SQL_STRING = "NVarChar";
                    break;
                case "real":
                    C_STRING = "Single";
                    SQL_STRING = "Int";
                    break;
                case "rowversion":
                    C_STRING = "Byte[]";
                    SQL_STRING = "Binary";
                    break;
                case "smalldatetime":
                    C_STRING = "DateTime";
                    SQL_STRING = "DateTime";
                    break;
                case "smallint":
                    C_STRING = "Int16";
                    SQL_STRING = "Int";
                    break;
                case "smallmoney":
                    C_STRING = "Decimal";
                    SQL_STRING = "Decimal";
                    break;
                case "text":
                    C_STRING = "String";
                    SQL_STRING = "Text";
                    break;
                case "time":
                    C_STRING = "TimeSpan";
                    SQL_STRING = "Time";
                    break;

                case "tinyint":
                    C_STRING = "Byte";
                    SQL_STRING = "Byte";
                    break;
                case "uniqueidentifier":
                    C_STRING = "GUID";
                    SQL_STRING = "NVarChar";
                    break;
                case "varbinary":
                    C_STRING = "Byte[]";
                    SQL_STRING = "Byte";
                    break;
                case "varchar":
                    C_STRING = "String";
                    SQL_STRING = "NVarChar";
                    break;
                case "xml":
                    C_STRING = "Xml";
                    SQL_STRING = "Xml" ;
                    break;

            }
        }
    }
}
