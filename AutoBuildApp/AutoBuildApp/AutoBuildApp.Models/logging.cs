using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBuildApp.Models
{
    // this is the physical representation of out log table in the DB
    public class logging
    {

        /*
         * logID INTEGER NOT NULL IDENTITY(0030000,2), -- PRIMARY KEY,
         * creationDate DATETIME,
         * event Varchar(20), --example 192/168/1/1
         * message text, -- 
         * tag Varchar(20),
         */

        //there is more but this is for now.
        public int logID { get; }

        // when you see the library for DateTime it has a constructor: public DateTime(int year, int month, int day, int hour, int minute, int second);
        // this is to show how storing will be done 
        public DateTime CreationDate{ get; set; }
        public string Event { get; set; }
        public string Message { get; set; }
        public string tag{ get; set; }

        public logging()
        {
            // // Summary: (IN THE LIBRARY)
            //     Represents the smallest possible value of System.DateTime. This field is read-only. public static readonly DateTime MinValue;
            this.CreationDate = DateTime.MinValue;
            this.Event = "";
            this.Message = "";
            this.tag = "";


        }
        public logging(DateTime date, String eve, String message, String tag)
        {
            // // Summary: (IN THE LIBRARY)
            //     Represents the smallest possible value of System.DateTime. This field is read-only. public static readonly DateTime MinValue;
            this.CreationDate = date;
            this.Event = eve;
            this.Message = message;
            this.tag = tag;


        }



        public string logInfo
        {
            get
            {
                // this will return fname lname and (email)
                return $"{CreationDate} {Event} ({Message} {tag})";
            }
        }




    }
}
