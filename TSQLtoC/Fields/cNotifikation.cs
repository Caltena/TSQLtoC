using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace TSQLtoC
{
    class cNotifikation
    {

        public string sNotifikation = @"
            using System;
            using System.ComponentModel;
            using System.Data.SqlClient;

            namespace ###NAMESPACE###
                {
                    class I###TABLENAME### : INotifyPropertyChanged
                    {

                        /// Defines the PropertyChanged
                        /// </summary>
                        public event PropertyChangedEventHandler PropertyChanged;
                        
                        ###CLASSES###

                        private void Notify(string argument)
                        {

                            if (this.PropertyChanged != null)
                            {
                                this.PropertyChanged(this, new PropertyChangedEventArgs(argument));
                            }
                        }
                    }
                }";

        public cNotifikation()
        {
            string sClasses = "";

            foreach ( cField cf in Program.lcFields )
            {
                sClasses += Environment.NewLine;
                sClasses += string.Format(@"        private {0} _{1};", cf.C_STRING, cf.COLUMN_NAME);
                sClasses += Environment.NewLine;
                sClasses += string.Format("         public {0} {1} {2} {3} ", cf.C_STRING, cf.COLUMN_NAME , Environment.NewLine , "{");
                sClasses += string.Format("             get {2}  return _{0}; {3} {1}", cf.COLUMN_NAME, Environment.NewLine,"{","}");
                sClasses += string.Format("             set {2}  _{0} =  value ;{1} Notify({4}{0}{4}); {3} {1}  {1}", cf.COLUMN_NAME, Environment.NewLine, "{", "}", "\"");
                sClasses += Environment.NewLine;
            }
            sNotifikation = sNotifikation.Replace("###CLASSES###", sClasses);
        }
    }



}
