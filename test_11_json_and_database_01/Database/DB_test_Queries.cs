using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_11_json_and_database_01.Database
{
    class DB_test_Queries
    {

        public static List<string> dx_article_Columns_canBeNull;
        public static List<string> dx_article_attribute_Columns_canBeNull;
        public static List<string> dx_article_attribute_translation_Columns_canBeNull;
        public static List<string> dx_article_prices_Columns_canBeNull;

        static DB_test_Queries()
        {
            dx_article_Columns_canBeNull = new List<string>();
            dx_article_Columns_canBeNull.AddRange("SALES_UNIT_ID, ACTIVE, CONSINGMENT, MODIF_DT, MODIF_USER_ID, LOCK_INFO, LOCK_ID".Split(
                new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            dx_article_Columns_canBeNull = dx_article_Columns_canBeNull.Select(s => s.Trim()).ToList();

            dx_article_attribute_Columns_canBeNull = new List<string>();
            dx_article_attribute_Columns_canBeNull.AddRange("ORD, ART_ATTRIBUTE_ID, PUBLIC".Split(
                new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            dx_article_attribute_Columns_canBeNull = dx_article_attribute_Columns_canBeNull.Select(s => s.Trim()).ToList();

            dx_article_attribute_translation_Columns_canBeNull = new List<string>();
            dx_article_attribute_translation_Columns_canBeNull.AddRange("NOTES, MODIF_DT, MODIF_USER_ID, LOCK_INFO, LOCK_ID".Split(
                new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            dx_article_attribute_translation_Columns_canBeNull = dx_article_attribute_translation_Columns_canBeNull.Select(s => s.Trim()).ToList();

            dx_article_prices_Columns_canBeNull = new List<string>();
            dx_article_prices_Columns_canBeNull.AddRange("MODIF_DT, MODIF_USER_ID, LOCK_INFO, LOCK_ID".Split(
                new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            dx_article_prices_Columns_canBeNull = dx_article_prices_Columns_canBeNull.Select(s => s.Trim()).ToList();
        }

        public static void Test_01()
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = @"SELECT * FROM `test_json_data_01`.`dx_article` WHERE ";
        }

        /// <summary>
        /// Please note that parameters should have same name as table columns (but with '@' sign before them of course)
        /// </summary>
        private static bool CheckIfThoseParametersCanBeNullInTable(List<string> listOfTableFields, MySqlParameterCollection parametersCollection)
        {
            foreach (MySqlParameter param in parametersCollection)
            {
                if (param.Value == null &&
                    listOfTableFields.Contains(
                        param.ParameterName.Substring(1, param.ParameterName.Length - 1) // remove '@' character
                    , StringComparer.OrdinalIgnoreCase) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool InsertDataFromJson_1(JsonFile_1.Data jsonData)
        {
            MySqlCommand cmd;

            DBConnect.Instance.OpenConnection();
            DBConnect.Instance.TransactionBegin();

            try
            {
                cmd = new MySqlCommand();
                cmd.CommandText = @"INSERT INTO `test_json_data_01`.`dx_article` 
(`ID`, `ARTICLE_TYPE_ID`, `CONSINGMENT`) 
VALUES 
(@ID, @ARTICLE_TYPE_ID, @CONSINGMENT)";

                cmd.Parameters.AddWithValue("@ID", jsonData.article_id);
                cmd.Parameters.AddWithValue("@ARTICLE_TYPE_ID", jsonData.article_type_id);
                cmd.Parameters.AddWithValue("@CONSINGMENT", jsonData.consingment);

                // check (if 'value = null' but that field can not be null inside Database, then 'return false')
                if (CheckIfThoseParametersCanBeNullInTable(dx_article_Columns_canBeNull, cmd.Parameters) == false)
                {
                    DBConnect.Instance.TransactionRollback();
                    return false;
                }


                if (DBConnect.Instance.Insert(cmd) == false)
                {
                    DBConnect.Instance.TransactionRollback();
                    return false;
                }

                foreach (var attribute in jsonData.attributes)
                {
                    cmd = new MySqlCommand();
                    cmd.CommandText = @"INSERT INTO `test_json_data_01`.`dx_article_attribute` 
(`ARTICLE_ID`, `ART_ATTRIBUTE_ID`,`ATTRIBUTE_ID`, `ORD`, `PUBLIC`) 
VALUES 
(@ARTICLE_ID, @ART_ATTRIBUTE_ID, @ATTRIBUTE_ID, @ORD, @PUBLIC)";

                    cmd.Parameters.AddWithValue("@ARTICLE_ID", jsonData.article_id);
                    cmd.Parameters.AddWithValue("@ART_ATTRIBUTE_ID", attribute.article_attribute_id);
                    cmd.Parameters.AddWithValue("@ATTRIBUTE_ID", attribute.attribute_id);
                    cmd.Parameters.AddWithValue("@ORD", attribute.ord);
                    cmd.Parameters.AddWithValue("@PUBLIC", attribute.is_public);

                    if (CheckIfThoseParametersCanBeNullInTable(dx_article_attribute_Columns_canBeNull, cmd.Parameters) == false)
                    {
                        DBConnect.Instance.TransactionRollback();
                        return false;
                    }


                    if (DBConnect.Instance.Insert(cmd) == false)
                    {
                        DBConnect.Instance.TransactionRollback();
                        return false;
                    }

                    foreach (var note in attribute.notes)
                    {
                        cmd = new MySqlCommand();
                        cmd.CommandText = @"INSERT INTO `test_json_data_01`.`dx_article_attribute_translation` 
(`ARTICLE_ATTRIBUTE_ARTICLE_ID`, `ARTICLE_ATTRIBUTE_ATTRIBUTE_ID`,`LANGUAGE_ID`, `NOTES`, `VALUE`) 
VALUES 
(@ARTICLE_ATTRIBUTE_ARTICLE_ID, @ARTICLE_ATTRIBUTE_ATTRIBUTE_ID, @LANGUAGE_ID, @NOTES, @VALUE)";

                        cmd.Parameters.AddWithValue("@ARTICLE_ATTRIBUTE_ARTICLE_ID", jsonData.article_id);
                        cmd.Parameters.AddWithValue("@ARTICLE_ATTRIBUTE_ATTRIBUTE_ID", attribute.article_attribute_id);
                        cmd.Parameters.AddWithValue("@LANGUAGE_ID", note.language_id);
                        cmd.Parameters.AddWithValue("@NOTES", note.txt);

                        string valueField = String.Empty;

                        if (attribute.values == null && attribute.value == null)
                        {
                            valueField = "{\"val\": " + "null}";
                        }
                        else
                        {
                            if (attribute.values != null)
                            {
                                valueField = JsonConvert.SerializeObject(attribute.values);
                            }
                            else
                            {
                                if (attribute.value is string)
                                {
                                    valueField = "{\"val\": " + '\"' + attribute.value + '\"' + "}";

                                }
                                else
                                {
                                    valueField = "{\"val\": " + attribute.value + "}";

                                }
                            }
                        }

                        cmd.Parameters.AddWithValue("@VALUE", valueField);

                        if (CheckIfThoseParametersCanBeNullInTable(dx_article_attribute_translation_Columns_canBeNull, cmd.Parameters) == false)
                        {
                            DBConnect.Instance.TransactionRollback();
                            return false;
                        }


                        if (DBConnect.Instance.Insert(cmd) == false)
                        {
                            DBConnect.Instance.TransactionRollback();
                            return false;
                        }


                    }
                }

                foreach(var price in jsonData.prices)
                {
                    cmd = new MySqlCommand();
                    cmd.CommandText = @"INSERT INTO `test_json_data_01`.`dx_article_prices` 
(`ID`, `ARTICLE_ID`, `SALES_CHANNEL_ID`, `VALID_FROM`, `TAX_CODE_ID`, `PURCHASE`, `COST`, `LIST`, `RETAIL`, `REDUCTION`) 
VALUES 
(@ID, @ARTICLE_ID, @SALES_CHANNEL_ID, @VALID_FROM, @TAX_CODE_ID, @PURCHASE, @COST, @LIST, @RETAIL, @REDUCTION)";

                    cmd.Parameters.AddWithValue("@ID", price.id);
                    cmd.Parameters.AddWithValue("@ARTICLE_ID", jsonData.article_id);
                    cmd.Parameters.AddWithValue("@SALES_CHANNEL_ID", price.sales_channel_id);
                    cmd.Parameters.AddWithValue("@VALID_FROM", price.valid_from);
                    cmd.Parameters.AddWithValue("@TAX_CODE_ID", price.tax_code_id);
                    cmd.Parameters.AddWithValue("@PURCHASE", price.purchase);
                    cmd.Parameters.AddWithValue("@COST", price.cost);
                    cmd.Parameters.AddWithValue("@LIST", price.list);
                    cmd.Parameters.AddWithValue("@RETAIL", price.retail);
                    cmd.Parameters.AddWithValue("@REDUCTION", price.reduction);


                    if (CheckIfThoseParametersCanBeNullInTable(dx_article_prices_Columns_canBeNull, cmd.Parameters) == false)
                    {
                        DBConnect.Instance.TransactionRollback();
                        return false;
                    }


                    if (DBConnect.Instance.Insert(cmd) == false)
                    {
                        DBConnect.Instance.TransactionRollback();
                        return false;
                    }
                }

                DBConnect.Instance.TransactionCommit();

            }
            catch (MySqlException ex)
            {
                DBConnect.Instance.TransactionRollback();

                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                DBConnect.Instance.CloseConnection();
            }

            return true;
        }


        //public static List<Country> SelectAllCountries()
        //{
        //    MySqlCommand cmd = new MySqlCommand("SELECT countries.ID, countries.NAME, countries.SHORT_NAME FROM countries");

        //    DataTable result = DBConnect.Instance.Select(cmd);

        //    if (result == null)
        //    {
        //        return null;
        //    }

        //    var countriesList = new List<Country>();

        //    for (int i = 0; i < result.Rows.Count; i++)
        //    {
        //        int countryID = Int32.Parse(result.Rows[i].ItemArray[0].ToString());
        //        string countryName = result.Rows[i].ItemArray[1].ToString();
        //        string countryShortName = result.Rows[i].ItemArray[2].ToString();

        //        var country = new Country(countryID, countryName, countryShortName);
        //        countriesList.Add(country);
        //    }

        //    return countriesList;
        //}

        //public static List<City> SelectAllCitiesByCountryID(int _countryID)
        //{
        //    MySqlCommand cmd = new MySqlCommand("SELECT cities.ID, cities.NAME, cities.AMOUNT_OF_PEOPLE FROM cities WHERE COUNTRY_ID=@country_id");
        //    cmd.Parameters.AddWithValue("@country_id", _countryID);

        //    DataTable result = DBConnect.Instance.Select(cmd);

        //    if (result == null)
        //    {
        //        return null;
        //    }

        //    var citiesList = new List<City>();

        //    for (int i = 0; i < result.Rows.Count; i++)
        //    {
        //        int cityID = Int32.Parse(result.Rows[i].ItemArray[0].ToString());
        //        string cityName = result.Rows[i].ItemArray[1].ToString();
        //        int cityAmountOfPeople = Int32.Parse(result.Rows[i].ItemArray[2].ToString());

        //        var city = new City(cityID, cityName, cityAmountOfPeople);
        //        citiesList.Add(city);
        //    }

        //    return citiesList;
        //}

    }
}
