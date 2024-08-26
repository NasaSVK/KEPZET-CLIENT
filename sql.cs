using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace BarcodeReader_ce_CF2
{
    class sql
    {
        private string connString = "server=10.0.0.32,53262;database=zkwPBL;User Id=nasask;Password=nasask";

        fs FS = new fs();

        public sql()
        {
            FS.readConfig();
            connString = FS.config[FS.dbConnStr];
        }

        #region private methods

        private string[] readerQuery(string query, int retValCount)
        {
            SqlConnection DBConnection = new SqlConnection(connString);
            try
            {
                string[] retValues;
                retValues = new string[retValCount];
                if (DBConnection.State != ConnectionState.Closed)
                {
                    DBConnection.Close();
                    FS.logData("dbconnection closed(" + DBConnection.State.ToString() + ")");
                }
                SqlDataReader dr = null;
                SqlCommand tq = new SqlCommand(query, DBConnection);
                DBConnection.Open();

                dr = tq.ExecuteReader();

                while (dr.Read())
                {
                    for (int i = 0; i < retValCount; i++)
                    { retValues[i] = dr[i].ToString(); }

                }
                DBConnection.Close();
                dr.Close();
                dr = null;
                tq.Dispose();


                return retValues;
            }
            catch (Exception e)
            {
                //DBConnection.Close();
                FS.logData("readerQuery:" + query + "\nError:" + e.ToString());
                return null;
            }
            finally
            {
                DBConnection.Close();
                DBConnection.Dispose();
            }
        }

        ///<summary>
        ///query w return values and multiple rows
        ///</summary>
        private string[] readerQuery(string query, int retValCount, int retRowCount)
        {
            SqlConnection DBConnection = new SqlConnection(connString);
            try
            {
                List<string> retValues = new List<string>();
                if (DBConnection.State != ConnectionState.Closed)
                {
                    DBConnection.Close();
                    FS.logData("dbconnection closed(" + DBConnection.State.ToString() + ")");
                }
                SqlDataReader dr = null;
                SqlCommand tq = new SqlCommand(query, DBConnection);
                DBConnection.Open();

                dr = tq.ExecuteReader();

                do
                {
                    int count = dr.FieldCount;
                    while (dr.Read())
                    {
                        for (int i = 0; i < count; i++)
                        {
                            retValues.Add(dr.GetValue(i).ToString().Trim());

                        }
                    }
                } while (dr.NextResult());

                DBConnection.Close();
                dr.Close();
                dr = null;
                tq.Dispose();

                return retValues.ToArray();
            }
            catch (Exception e)
            {
                //DBConnection.Close();
                FS.logData("readerQuery2:" + query + "\nError:" + e.ToString());
                return null;
            }
            finally
            {
                DBConnection.Close();
                DBConnection.Dispose();
            }
        }

        ///<summary>
        ///query w/o return values
        ///</summary>
        private int readerQuery(string query)
        {
            SqlConnection DBConnection = new SqlConnection(connString);
            try
            {
                int retValue = -1;
                if (DBConnection.State != ConnectionState.Closed)
                {
                    DBConnection.Close();
                    FS.logData("dbconnection closed(" + DBConnection.State.ToString() + ")");
                }
                SqlCommand tq = new SqlCommand(query, DBConnection);
                DBConnection.Open();

                tq.ExecuteNonQuery();

                DBConnection.Close();
                tq.Dispose();
                retValue = 0;
                return retValue;
            }
            catch (Exception e)
            {
                //DBConnection.Close();
                FS.logData("readerQuery3:" + query + "\nError:" + e.ToString());
                return -2;
            }
            finally
            {
                DBConnection.Close();
                DBConnection.Dispose();
            }
        }

        #endregion

        #region public methods

        ///<summary>
        ///returns object array of values for searched pallet
        ///</summary>
        public object[] searchRecord(string paletteNr)
        {
            object[] o = readerQuery("DECLARE @pack int, @x int, @y int, @pallet nchar(20); SET @pallet = N'" + paletteNr + "';" +
                "SELECT @x = [posX],@y = [posY] FROM [dbo].[warehouseDB] WHERE [paletteNr] = @pallet;" +
                "SELECT @pack = [pack] FROM [dbo].[layout] WHERE [posX] = @x and [posY] = @y;" +
                "WITH temp AS(" +
                "SELECT ROW_NUMBER() OVER (ORDER BY [fifoDatetime],[id]) AS row , [id], [paletteNr],[posX],[posY],[partNr],[channel]" +
                "FROM [dbo].[warehouseDB] WHERE [posX] = @x and [posY] = @y)" +
                "SELECT [posX], [posY], row, [partNr], [channel], @pack AS pack FROM [temp] WHERE [paletteNr] = @pallet", 6);
            //object[] o = readerQuery("SELECT * FROM [dbo].[warehouseDB] WHERE [paletteNr] = N'12345672'", 10);
            string[] test = (string[])o;
            return o;

        }

        ///<summary>
        ///returns object array of values for selected position of selected type
        ///</summary>
        public object[] getPos(string item, string x, string y)
        {
            try
            {
                object[] a = readerQuery("SELECT COUNT(*) FROM [dbo].[warehouseDB] WHERE [posX] = " + x + " AND [posY] = " + y, 1);
                int count = Convert.ToInt32(a[0]);
                object[] o = readerQuery("SELECT [" + item + "] FROM [dbo].[warehouseDB] WHERE [posX] = " + x + " AND [posY] = " + y + " order by [FIFODatetime],[id]", 1, count);
                string[] test = (string[])o;
                return o;
            }
            catch (Exception ex)
            {
                FS.logData("getPos(" + item + "," + x.ToString() + "," + y.ToString() + ") Error: " + ex.ToString());
                return null;
            }

        }

        ///<summary>
        ///returns object array of distinct part numbers
        ///</summary>
        public string[] getPnList()
        {
            try
            {
                object[] a = readerQuery("SELECT COUNT (DISTINCT [partNr]) FROM [dbo].[layout]", 1);
                int count = Convert.ToInt32(a[0]);
                object[] o = readerQuery("SELECT DISTINCT [partNr] FROM [dbo].[layout] ORDER BY [partNr]", 1, count);
                string[] test = (string[])o;
                return test;
            }
            
            catch (Exception ex)
            {
                FS.logData("getPnList() Error: " + ex.ToString());
                return null;
            }

        }

        ///<summary>
        ///returns object array of values for selected position
        ///</summary>
        public object[] getPos(string x, string y)
        {
            object[] o = readerQuery("SELECT [partNr], [channel], [pack], [aLTime], [aLCheck], [aLBPTime], [aLBPStart], [aLBP], [maxcount] FROM [dbo].[layout] WHERE [posX] = " + x + " AND [posY] = " + y, 9);
            string[] test = (string[])o;
            return o;

        }

        ///<summary>
        ///returns object array of values for selected position
        ///</summary>
        public object[] getPosAge(string pn)
        {
            object[] o = readerQuery("SELECT TOP(1) [aLBPTime],[aLTime],[aLCheck] FROM [dbo].[layout] WHERE [partNr] LIKE '" + pn + "%';", 3);
            string[] test = (string[])o;
            return o;

        }

        ///<summary>
        ///clear record by paletteNr
        ///</summary>
        public int clearRecord(string paletteNr)
        {
            
            string query = "declare @posX int, @posY int, @count int; set @posX = 0; set	@posY = 0; set @count = 0; "
                    + "SELECT @posX = [posX], @posY = [posY] FROM [dbo].[warehouseDB] WHERE [paletteNr] = N'" + paletteNr + "' DELETE from [dbo].[warehouseDB]"
                    + " where [paletteNr] = N'" + paletteNr + "' SELECT @count = COUNT(*) FROM [dbo].[warehouseDB] WHERE [posX] like @posX and [posY] like @posY"
                    + " UPDATE [dbo].[layout] SET [count] = @count WHERE [posX] like @posX and [posY] like @posY";
            int retVal = readerQuery(query);
            if (checkRecord(paletteNr))
            {
                FS.logData("clearRecordError(" + paletteNr + ")");
                return -3;
            }

            else
            {
                FS.logData("sql.clearRecord:" + paletteNr);
                return retVal;
            }

        }

        ///<summary>
        ///update record by paletteNr
        ///</summary>
        public int updateRecordByPalletNr(string currentPalletNr, string newPalletNr, string newFifoDatetime)
        {
            return readerQuery("UPDATE [dbo].[warehouseDB]\nSET [paletteNr] = N'" + newPalletNr + "',\n[FIFODatetime] = '" + newFifoDatetime + "'\nWHERE [paletteNr] = N'" + currentPalletNr + "'");

        }

        ///<summary>
        ///update position by position coordinates
        ///</summary>
        public int updatePosByPos(string x, string y, string newPartNr, string newChannel, string newPack, string maxcount)
        {
            //return readerQuery("UPDATE [dbo].[layout]\nSET [partNr] = N'" + newPartNr + "'\n,[channel] = " + newChannel + "\n,[pack] = " + newPack + "\nWHERE [posX] = " + x + " AND [posY] = " + y + "");
            return readerQuery("declare @pack int;\nset @pack = " + newPack + ";\nUPDATE[dbo].[layout]\nSET[partNr] = N'" + newPartNr + "'\n,[channel] = " + newChannel + "\n,[pack] = @pack\n,[maxcount] = " + maxcount + "\nWHERE[posX] = " + x + " AND[posY] = " + y);
        }

        ///<summary>
        ///update position by position coordinates
        ///</summary>
        public int updatePosAgeByPn(string pn, string aLTime, bool aLCheck, string aLBPTime)
        {
            //return readerQuery("UPDATE [dbo].[layout]\nSET [partNr] = N'" + newPartNr + "'\n,[channel] = " + newChannel + "\n,[pack] = " + newPack + "\nWHERE [posX] = " + x + " AND [posY] = " + y + "");
            return readerQuery("UPDATE [dbo].[layout] SET [aLTime] = " + aLTime + " ,[aLCheck] = '" + aLCheck + "' ,[aLBPTime] = " + aLBPTime + " WHERE partNr LIKE '" + pn + "%';");
        }

        ///<summary>
        ///set age lock bypass timer
        ///</summary>
        public int alBPUpdate(string pn, string aLBP)
        {
            return readerQuery("UPDATE[dbo].[layout] SET[aLBP] = '" + aLBP + "' ,[aLBPStart] = GETDATE() WHERE [partNr] LIKE '" + pn + "%'");
        }

        ///<summary>
        ///get age lock bypass timer remaining time
        ///</summary>
        public string aLBPRemainingTime(string pn)
        {
            object[] o = readerQuery("DECLARE @time int, @remainingTime int, @start datetime, @en bit; SELECT @time = [aLBPTime] ,@start = [aLBPStart] ,@en = [aLBP] FROM [dbo].[layout] WHERE [partNr] LIKE '"+pn+"%'; set @remainingTime = @time - DATEDIFF(minute,@start,GETDATE()); IF(@remainingTime>0 AND @en='True') SELECT @remainingTime as rt; ELSE SELECT NULL as rt;", 1);
            string[] test = (string[])o;
            string rv = test[0];
            return rv;

        }

        ///<summary>
        ///check record in db
        ///</summary>
        public bool checkRecord(string paletteNr)
        {
            int count = 0;

            if (paletteNr == "")
                return false;

            object[] o = readerQuery("SELECT count(*) FROM [dbo].[warehouseDB] WHERE [paletteNr] = N'" + paletteNr + "'", 1);
            try
            {
                count = Convert.ToInt32(o[0].ToString());
            }
            catch (Exception ex)
            {
                FS.logData("checkRecord(" + paletteNr + ")=" + count.ToString() + " error:" + ex.ToString());
            }
            if (count >= 1)
                return true;
            else
                return false;

        }

        #endregion

        #region helper methods
        //public string getSQLDateTimeFromFIFO(string fifo)
        //{
        //    //20160422 73335
        //    //20160202 35 00:00:35

        //    string retVal = fifo, tempDate = null, year = null, month = null, day = null, hour = null, minute = null, second = null;
        //    try
        //    {
        //        year = fifo.Substring(0, 4);
        //        month = fifo.Substring(4, 2);
        //        day = fifo.Substring(6, 2);
        //        //73335
        //        second = fifo.Substring(fifo.Length - 2);
        //        minute = fifo.Substring(fifo.Length - 4, 2);
        //        hour = fifo.Substring(fifo.Length - 6, 2);
        //    }
        //    catch (Exception ex)
        //    {
        //        logData("sqldatetime conversion error(" + fifo + "):" + ex.ToString());

        //    }


        //    retVal = year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;


        //    return retVal;
        //}
        #endregion

    }


}
