using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_11_json_and_database_01.Database
{
    class DB_test_Queries
    {

        public static List<string> dx_article_table_fields;

        static DB_test_Queries()
        {
            dx_article_table_fields = new List<string>();

            string str_dx_article_table_fields = "ID, SALES_UNIT_ID, ACTIVE, ARTICLE_TYPE_ID, CONSINGMENT, MODIF_DT, MODIF_USER_ID";
            dx_article_table_fields.AddRange(str_dx_article_table_fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            dx_article_table_fields = dx_article_table_fields.Select(s => s.Trim()).ToList();
        }

        public static bool Insert()
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO ");

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
