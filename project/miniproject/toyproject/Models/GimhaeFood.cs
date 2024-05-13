using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace toyproject.Models
{
    public class GimhaeFood
    {
        public string Idx {  get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Area { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Businesshour { get; set; }
        public string Holiday { get; set; }
        public string Menuprice { get; set; }
        public double Xposition { get; set; }
        public double Yposition { get; set; }

        public static readonly string INSERT_QUERY = @"INSERT INTO [dbo].[Food]
                                                               ([Idx]
                                                               ,[Name]
                                                               ,[Category]
                                                               ,[Area]
                                                               ,[Phone]
                                                               ,[Address]
                                                               ,[Businesshour]
                                                               ,[Holiday]
                                                               ,[Menuprice]
                                                               ,[Xposition]
                                                               ,[Yposition])
                                                         VALUES
                                                               (@Idx
                                                               ,@Name
                                                               ,@Category
                                                               ,@Area
                                                               ,@Phone
                                                               ,@Address
                                                               ,@Businesshour
                                                               ,@Holiday
                                                               ,@Menuprice
                                                               ,@Xposition
                                                               ,@Yposition)";

        public static readonly string SELECT_QUERY = @"SELECT [Idx],[Page]
                                                          ,[Name]
                                                          ,[Category]
                                                          ,[Area]
                                                          ,[Phone]
                                                          ,[Address]
                                                          ,[Businesshour]
                                                          ,[Holiday]
                                                          ,[Menuprice]
                                                          ,[Xposition]
                                                          ,[Yposition]
                                                      FROM [dbo].[Food]";

        public static readonly string Category_QUERY = @"SELECT Category
                                                           FROM Food
                                                          GROUP BY Category";
    }
}
