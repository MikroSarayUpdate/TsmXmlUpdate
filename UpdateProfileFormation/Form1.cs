using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UpdateProfileFormation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static string conString = ("Server=94.103.41.146,1343;DataBase=workcube_mikrosaray;User Id=sa;Password=_GbY159!TzP41_;Integrated Security=False;TransparentNetworkIPResolution = False");
        //Bu veritabanına bağlanmak için gerekli olan bağlantı cümlemiz.
        SqlConnection baglanti = new SqlConnection(conString);
        //bağlantı cümlemizi kullanarak bir SqlConnection bağlantısı oluşturuyoruz.

        private void kayitGetir()
        {
            baglanti.Open();
            string kayit = "SELECT PRO_SERIAL_NO,APPLICATOR_COMP_NAME from workcube_mikrosaray_1.SERVICE";
            SqlCommand komut = new SqlCommand(kayit, baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();
        }

        public object GetTSMREPORTS([FromBody] UserRequest userRequest)
        {

            string tsmSunucu = ConfigurationManager.AppSettings.Get("TsmSunucu");
            string url = string.Format(tsmSunucu + "/get_reports?vkntckn=" + userRequest.Tcknvkn + "&sc=" + userRequest.SerialNumber + "&start_date=" + userRequest.StartDate + "&end_date=" + userRequest.EndDate + "");


            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            //request.Headers.Add("X-Metabase-Session: " + token);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            //returned as serialized JSON, need to convert if plan to use
            var serverResponse = reader.ReadToEnd();
            // clean up.
            reader.Close();
            dataStream.Close();
            response.Close();

            IEnumerable<TsmReportsResult> result = JsonConvert.DeserializeObject<IEnumerable<TsmReportsResult>>(serverResponse);


            return result;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            kayitGetir();
        }
    }
}
