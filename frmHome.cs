using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Combinatorics;
using System.Web.Script.Serialization;
using System.Net;
using Newtonsoft.Json;
using System.Globalization;

namespace Billion
{
    public partial class frmHome : Form
    {

        DataTable dtMatches = null;

        private const string key = "BB2SUrTRrFfjHtmBHq3yee4cndC3";

        private static string sCustomFormat = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern.ToString();

        public frmHome()
        {
            InitializeComponent();
            
            tsProgress.Visible = false;
            tslblMessage.Visible = false;
        }
        
        #region "Class Matches"

        public partial class MatchObject
        {
            [JsonProperty("matches")]
            public Match[] Matches { get; set; }

            [JsonProperty("ttl")]
            public long Ttl { get; set; }

            [JsonProperty("creditsLeft")]
            public long CreditsLeft { get; set; }
        }

        public partial class Match
        {
            [JsonProperty("unique_id")]
            public long UniqueId { get; set; }

            [JsonProperty("team-2")]
            public string Team2 { get; set; }

            [JsonProperty("team-1")]
            public string Team1 { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("date")]
            public DateTimeOffset Date { get; set; }

            [JsonProperty("dateTimeGMT")]
            public DateTimeOffset DateTimeGmt { get; set; }

            [JsonProperty("squad")]
            public bool Squad { get; set; }

            [JsonProperty("toss_winner_team", NullValueHandling = NullValueHandling.Ignore)]
            public string TossWinnerTeam { get; set; }

            [JsonProperty("winner_team", NullValueHandling = NullValueHandling.Ignore)]
            public string WinnerTeam { get; set; }

            [JsonProperty("matchStarted")]
            public bool MatchStarted { get; set; }
        }

        #endregion

        #region "Class Matches"

        public class Player
        {
            public int pid { get; set; }
            public string name { get; set; }
        }

        public class Squad
        {
            public string name { get; set; }
            public List<Player> players { get; set; }
        }

        public class SquadObject
        {
            public List<Squad> squad { get; set; }
            public bool cache { get; set; }
            public string v { get; set; }
            public int ttl { get; set; }
            public int creditsLeft { get; set; }
        }

        #endregion

        private void frmHome_Load(object sender, EventArgs e)
        {
            try
            {
                dtpMatchDate.CustomFormat = sCustomFormat;

                tsProgress.Visible = true;
                tslblMessage.Visible = true;
                tslblMessage.Text = "Getting match list...";
                tsProgress.Style = ProgressBarStyle.Marquee;
                bgwMatches.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bgwMatches_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                
                string apiUrl = "http://cricapi.com/api/matches";
                object input = new
                {
                    apikey = key
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string json = client.UploadString(apiUrl, inputJson);

                var jsonObj = JsonConvert.DeserializeObject<MatchObject>(json);

                dtMatches = new DataTable();
                dtMatches.Columns.Add("UniqueID", typeof(string));
                dtMatches.Columns.Add("Date", typeof(DateTime));
                dtMatches.Columns.Add("Time", typeof(string));
                dtMatches.Columns.Add("Team1", typeof(string));
                dtMatches.Columns.Add("Team2", typeof(string));
                dtMatches.Columns.Add("Squad", typeof(bool));

                DataRow drRow = null;

                foreach (var obj in jsonObj.Matches)
                {
                    drRow = dtMatches.NewRow();
                    drRow["UniqueID"] = obj.UniqueId;
                    drRow["Date"] = obj.DateTimeGmt.ToLocalTime().ToString(sCustomFormat);
                    drRow["Time"] = obj.DateTimeGmt.ToLocalTime().ToString("hh:mm tt");
                    drRow["Team1"] = obj.Team1;
                    drRow["Team2"] = obj.Team2;
                    drRow["Squad"] = obj.Squad;

                    dtMatches.Rows.Add(drRow);
                }

                if (dtMatches != null && dtMatches.Rows.Count > 0)
                {
                    this.Invoke((MethodInvoker)delegate
                    {

                        DataView view = new DataView(dtMatches);
                        view.RowFilter = "Date = '" + dtpMatchDate.Value.ToString(sCustomFormat) + "'";
                        dgvMatches.DataSource = view.ToTable().DefaultView;
                    });

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void bgwMatches_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                tsProgress.Visible = false;
                tslblMessage.Visible = false;

                object result = e.Result;
                if (result != null && Convert.ToBoolean(result) == true)
                {

                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cmsPlayers_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = (dgvMatches.Rows.Count == 0 || dgvMatches.SelectedRows.Count == 0);
        }

        private void getSquadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow dgvSelected = (DataGridViewRow)dgvMatches.SelectedRows[0];

                int UniqueID = Convert.ToInt32(dgvSelected.Cells["colUniqueID"].Value.ToString());
                string Date = Convert.ToDateTime(dgvSelected.Cells["colDate"].Value).ToString(sCustomFormat);
                string TeamA = dgvSelected.Cells["colTeam1"].Value.ToString();
                string TeamB = dgvSelected.Cells["colTeam2"].Value.ToString();

                new frmTeam(UniqueID,Date,TeamA,TeamB).ShowDialog();

            }
            catch (Exception ex)
            {

            }
        }

        private void dgvMatches_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                int UniqueID = 0;
                DataGridView dgv = sender as DataGridView;
                if (dgv != null && dgv.SelectedRows.Count == 1)
                {
                    DataGridViewRow dgvRow = ((DataGridViewRow)dgvMatches.SelectedRows[0]);
                    UniqueID = Convert.ToInt32(dgvRow.Cells["colUniqueID"].Value.ToString());
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dtpMatchDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if(dtMatches != null && dtMatches.Rows.Count > 0)
                {
                    DataView dv = new DataView(dtMatches);
                    dv.RowFilter = "Date = '"+  dtpMatchDate.Value.ToString(sCustomFormat) +"'";
                    dgvMatches.DataSource = dv.ToTable().DefaultView;
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }

}
