using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SQLite;
using Newtonsoft.Json;

namespace Billion
{
    public partial class frmTeam : Form
    {

        #region "Excel Closing"

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        /// <summary> Tries to find and kill process by hWnd to the main window of the process.</summary>
        /// <param name="hWnd">Handle to the main window of the process.</param>
        /// <returns>True if process was found and killed. False if process was not found by hWnd or if it could not be killed.</returns>
        public static bool TryKillProcessByMainWindowHwnd(int hWnd)
        {
            uint processID;
            GetWindowThreadProcessId((IntPtr)hWnd, out processID);
            if (processID == 0) return false;
            try
            {
                Process.GetProcessById((int)processID).Kill();
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (Win32Exception)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            return true;
        }
        public static void KillProcessByMainWindowHwnd(int hWnd)
        {
            uint processID;
            GetWindowThreadProcessId((IntPtr)hWnd, out processID);
            if (processID == 0)
                throw new ArgumentException("Process has not been found by the given main window handle.", "hWnd");
            Process.GetProcessById((int)processID).Kill();
        }

        #endregion

        #region "DeserializeObject"
        public class TeamPreviewArtwork
        {
            public string src { get; set; }
        }

        public class TeamCriteria
        {
            public int totalCredits { get; set; }
            public int maxPlayerPerSquad { get; set; }
            public int totalPlayerCount { get; set; }
        }

        public class Artwork
        {
            public string src { get; set; }
        }

        public class Role
        {
            public int id { get; set; }
            public List<Artwork> artwork { get; set; }
            public string color { get; set; }
            public string name { get; set; }
            public double pointMultiplier { get; set; }
            public string shortName { get; set; }
        }

        public class Artwork2
        {
            public string src { get; set; }
        }

        public class PlayerType
        {
            public int id { get; set; }
            public string name { get; set; }
            public int minPerTeam { get; set; }
            public int maxPerTeam { get; set; }
            public string shortName { get; set; }
            public List<Artwork2> artwork { get; set; }
        }

        public class Flag
        {
            public string src { get; set; }
        }

        public class Squad
        {
            public List<Flag> flag { get; set; }
            public int id { get; set; }
            public string jerseyColor { get; set; }
            public string name { get; set; }
            public string shortName { get; set; }
        }

        public class Artwork3
        {
            public string src { get; set; }
        }

        public class Squad2
        {
            public int id { get; set; }
            public string name { get; set; }
            public string jerseyColor { get; set; }
            public string shortName { get; set; }
        }

        public class Type
        {
            public int id { get; set; }
            public int maxPerTeam { get; set; }
            public int minPerTeam { get; set; }
            public string name { get; set; }
            public string shortName { get; set; }
        }

        public class Player
        {
            public List<Artwork3> artwork { get; set; }
            public Squad2 squad { get; set; }
            public double credits { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public double points { get; set; }
            public Type type { get; set; }
            public bool isSelected { get; set; }
            public object role { get; set; }
        }

        public class Match
        {
            public int id { get; set; }
            public string guru { get; set; }
            public List<Squad> squads { get; set; }
            public DateTime startTime { get; set; }
            public string status { get; set; }
            public List<Player> players { get; set; }
        }

        public class Tour
        {
            public Match match { get; set; }
        }

        public class Site
        {
            public string name { get; set; }
            public List<TeamPreviewArtwork> teamPreviewArtwork { get; set; }
            public TeamCriteria teamCriteria { get; set; }
            public List<Role> roles { get; set; }
            public List<PlayerType> playerTypes { get; set; }
            public Tour tour { get; set; }
        }

        public class Me
        {
            public bool isGuestUser { get; set; }
        }

        public class Data
        {
            public Site site { get; set; }
            public Me me { get; set; }
        }

        public class RootObject
        {
            public Data data { get; set; }
        }

        #endregion

        int UniqueID = 0;
        string Date = string.Empty;
        string TeamA = string.Empty;
        string TeamB = string.Empty;

        DataTable dtPlayer = new DataTable();

        DataTable dtCombinationType = null;
        DataRow drRow = null;

        CheckBox headerCheckBox = new CheckBox();

        IEnumerable<DataGridViewRow> comboSelected = null;

        public frmTeam()
        {
            InitializeComponent();
            
        }

        public frmTeam(int UniqueID,string Date,string TeamA,string TeamB)
        {
            InitializeComponent();
            this.UniqueID = UniqueID;
            this.Date = Date;
            this.TeamA = TeamA.ToUpper();
            this.TeamB = TeamB.ToUpper();
            
            tsProgress.Visible = false;
            tslblMessage.Visible = false;

            dtCombinationType = new DataTable();
            dtCombinationType.Columns.Add("COMBO", typeof(string));
            dtCombinationType.Columns.Add("WK",typeof(string));
            dtCombinationType.Columns.Add("BAT", typeof(string));
            dtCombinationType.Columns.Add("ALL", typeof(string));
            dtCombinationType.Columns.Add("BOWL", typeof(string));
            dtCombinationType.Columns.Add("TeamCount", typeof(int));
            dtCombinationType.Columns.Add("TeamCountCV", typeof(int));
            dtCombinationType.Columns.Add("Team", typeof(string));
            dtCombinationType.Columns.Add("RowID", typeof(int));

            drRow = dtCombinationType.NewRow();
            drRow["COMBO"] = Combination.COMBO1;
            drRow["WK"] = 1;
            drRow["BAT"] = 5;
            drRow["ALL"] = 2;
            drRow["BOWL"] = 3;
            drRow["TeamCount"] = 0;
            drRow["TeamCountCV"] = 0;
            drRow["Team"] = "1WK,5BAT,2ALL,3BOWL";
            dtCombinationType.Rows.Add(drRow);

            drRow = dtCombinationType.NewRow();
            drRow["COMBO"] = Combination.COMBO2;
            drRow["WK"] = 1;
            drRow["BAT"] = 5;
            drRow["ALL"] = 1;
            drRow["BOWL"] = 4;
            drRow["TeamCount"] = 0;
            drRow["TeamCountCV"] = 0;
            drRow["Team"] = "1WK,5BAT,1ALL,4BOWL";
            dtCombinationType.Rows.Add(drRow);

            drRow = dtCombinationType.NewRow();
            drRow["COMBO"] = Combination.COMBO3;
            drRow["WK"] = 1;
            drRow["BAT"] = 4;
            drRow["ALL"] = 1;
            drRow["BOWL"] = 5;
            drRow["TeamCount"] = 0;
            drRow["TeamCountCV"] = 0;
            drRow["Team"] = "1WK,4BAT,1ALL,5BOWL";
            dtCombinationType.Rows.Add(drRow);

            drRow = dtCombinationType.NewRow();
            drRow["COMBO"] = Combination.COMBO4;
            drRow["WK"] = 1;
            drRow["BAT"] = 4;
            drRow["ALL"] = 2;
            drRow["BOWL"] = 4;
            drRow["TeamCount"] = 0;
            drRow["TeamCountCV"] = 0;
            drRow["Team"] = "1WK,4BAT,2ALL,4BOWL";
            dtCombinationType.Rows.Add(drRow);

            drRow = dtCombinationType.NewRow();
            drRow["COMBO"] = Combination.COMBO5;
            drRow["WK"] = 1;
            drRow["BAT"] = 4;
            drRow["ALL"] = 3;
            drRow["BOWL"] = 3;
            drRow["TeamCount"] = 0;
            drRow["TeamCountCV"] = 0;
            drRow["Team"] = "1WK,4BAT,3ALL,3BOWL";
            dtCombinationType.Rows.Add(drRow);

            drRow = dtCombinationType.NewRow();
            drRow["COMBO"] = Combination.COMBO6;
            drRow["WK"] = 1;
            drRow["BAT"] = 3;
            drRow["ALL"] = 2;
            drRow["BOWL"] = 5;
            drRow["TeamCount"] = 0;
            drRow["TeamCountCV"] = 0;
            drRow["Team"] = "1WK,3BAT,2ALL,5BOWL";
            dtCombinationType.Rows.Add(drRow);

            drRow = dtCombinationType.NewRow();
            drRow["COMBO"] = Combination.COMBO7;
            drRow["WK"] = 1;
            drRow["BAT"] = 3;
            drRow["ALL"] = 3;
            drRow["BOWL"] = 4;
            drRow["TeamCount"] = 0;
            drRow["TeamCountCV"] = 0;
            drRow["Team"] = "1WK,3BAT,3ALL,4BOWL";
            dtCombinationType.Rows.Add(drRow);

            dgvCombination.DataSource = dtCombinationType.DefaultView;

            
            //Place the Header CheckBox in the Location of the Header Cell.
            Rectangle rect = this.dgvCombination.GetCellDisplayRectangle(0, -1, true);
            headerCheckBox.Size = new Size(18, 18);
            rect.Offset(50, 2);
            headerCheckBox.Location = rect.Location;

            //Assign Click event to the Header CheckBox.
            headerCheckBox.Click += new EventHandler(HeaderCheckBox_Clicked);
            dgvCombination.Controls.Add(headerCheckBox);

            //Add a CheckBox Column to the DataGridView at the first position.
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "";
            checkBoxColumn.Width = 30;
            checkBoxColumn.Name = "checkBoxColumn";
            dgvCombination.Columns.Insert(0, checkBoxColumn);

            //Assign Click event to the DataGridView Cell.
            dgvCombination.CellContentClick += new DataGridViewCellEventHandler(DataGridView_CellClick);

            
        }

        private void HeaderCheckBox_Clicked(object sender, EventArgs e)
        {
            //Necessary to end the edit mode of the Cell.
            dgvCombination.EndEdit();

            //Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
            foreach (DataGridViewRow row in dgvCombination.Rows)
            {
                DataGridViewCheckBoxCell checkBox = (row.Cells["checkBoxColumn"] as DataGridViewCheckBoxCell);
                checkBox.Value = headerCheckBox.Checked;
            }
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Check to ensure that the row CheckBox is clicked.
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                //Loop to verify whether all row CheckBoxes are checked or not.
                bool isChecked = true;
                foreach (DataGridViewRow row in dgvCombination.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["checkBoxColumn"].EditedFormattedValue) == false)
                    {
                        isChecked = false;
                        break;
                    }
                }
                headerCheckBox.Checked = isChecked;
            }
        }

        private void frmDreamXI_Load(object sender, EventArgs e)
        {
            try
            {
               
                LoadPlayerDetail();
                
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex.ToString());
            }
            finally
            {
                
            }
            
        }
        
        public class PlayerDetail 
        {
            public int id { get; set; }
            public string name { get; set; }
            public string team { get; set; }
            public decimal credits { get; set; }
            public bool must { get; set; }
            public string player_type { get; set; }
            public bool c { get; set; }
            public bool vc { get; set; }
            public int Combo { get; set; }
        }

        public static class Combination
        {
            public static string COMBO1 = "COMBO1";
            public static string COMBO2 = "COMBO2";
            public static string COMBO3 = "COMBO3";
            public static string COMBO4 = "COMBO4";
            public static string COMBO5 = "COMBO5";
            public static string COMBO6 = "COMBO6";
            public static string COMBO7 = "COMBO7";
        }
        
        public class TeamWithCombo
        {

            public TeamWithCombo(string Combo, List<PlayerDetail> team,decimal Credits)
            {
                this.Combo = Combo;
                this.team = team;
                this.Credits = Credits;
            }

            public string Combo { get; set; }
            public List<PlayerDetail> team { get; set; }
            public decimal Credits { get; set; }
        }

        private void btnGenerateTeam_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    List<PlayerDetail> lstPlayer = new List<PlayerDetail>();
                    PlayerDetail obj = null;

                    List<int> lstWK = new List<int>();
                    List<int> lstBAT = new List<int>();
                    List<int> lstALL = new List<int>();
                    List<int> lstBOWL = new List<int>();

                    List<int> lstWKMust = new List<int>();
                    List<int> lstBATMust = new List<int>();
                    List<int> lstARMust = new List<int>();
                    List<int> lstBOWLMust = new List<int>();
                    
                    IEnumerable<DataGridViewRow> dgvWKSelected = dgvWK.Rows.Cast<DataGridViewRow>().Where(r => (r.Cells["WKSelected"].Value != DBNull.Value && r.Cells["WKSelected"].Value != null && Convert.ToBoolean(r.Cells["WKSelected"].Value) == true));
                    IEnumerable<DataGridViewRow> dgvBATSelected = dgvBAT.Rows.Cast<DataGridViewRow>().Where(r => (r.Cells["BATSelected"].Value != DBNull.Value && r.Cells["BATSelected"].Value != null && Convert.ToBoolean(r.Cells["BATSelected"].Value) == true));
                    IEnumerable<DataGridViewRow> dgvALLSelected = dgvALL.Rows.Cast<DataGridViewRow>().Where(r => (r.Cells["ALLSelected"].Value != DBNull.Value && r.Cells["ALLSelected"].Value != null && Convert.ToBoolean(r.Cells["ALLSelected"].Value) == true));
                    IEnumerable<DataGridViewRow> dgvBOWLSelected = dgvBOWL.Rows.Cast<DataGridViewRow>().Where(r => (r.Cells["BOWLSelected"].Value != DBNull.Value && r.Cells["BOWLSelected"].Value != null && Convert.ToBoolean(r.Cells["BOWLSelected"].Value) == true));
                    IEnumerable<DataGridViewRow> comboSelected = dgvCombination.Rows.Cast<DataGridViewRow>().Where(r => (r.Cells["checkBoxColumn"].Value != DBNull.Value && r.Cells["checkBoxColumn"].Value != null && Convert.ToBoolean(r.Cells["checkBoxColumn"].Value) == true));

                    #region "Validation"

                    if (dgvWKSelected == null || dgvWKSelected.Count() == 0)
                    {
                        tabPlayer.SelectTab("tabWK");
                        MessageBox.Show("Every team needs atleast 1 Wicket-Keeper");
                        return;
                    }
                    if (dgvBATSelected == null || dgvBATSelected.Count() == 0)
                    {
                        tabPlayer.SelectTab("tabBAT");
                        MessageBox.Show("Every team needs atleast 3 Batsman");
                        return;
                    }
                    if (dgvALLSelected == null || dgvALLSelected.Count() == 0)
                    {
                        tabPlayer.SelectTab("tabALL");
                        MessageBox.Show("Every team needs atleast 1 All-Rounder");
                        return;
                    }
                    if (dgvBOWLSelected == null || dgvBOWLSelected.Count() == 0)
                    {
                        tabPlayer.SelectTab("tabBOWL");
                        MessageBox.Show("Every team needs atleast 3 Bowlers");
                        return;
                    }
                    if (comboSelected == null || comboSelected.Count() == 0)
                    {
                        MessageBox.Show("Please select combination..!");
                        return;
                    }

                    #endregion

                    #region "Adding Player"

                    foreach (var row in dgvWKSelected)
                    {
                        lstWK.Add(Convert.ToInt32(row.Cells["WKPlayerID"].Value));
                        obj = new PlayerDetail();
                        obj.id = Convert.ToInt32(row.Cells["WKPlayerID"].Value);
                        obj.name = row.Cells["WKName"].Value.ToString();
                        obj.team = row.Cells["WKTeam"].Value.ToString();
                        obj.credits = Convert.ToDecimal(row.Cells["WKCredits"].Value);
                        obj.must = (row.Cells["WKMust"].Value != DBNull.Value && row.Cells["WKMust"].Value != null && Convert.ToBoolean(row.Cells["WKMust"].Value) == true) ? true : false;
                        obj.player_type = "WK";
                        obj.vc = (row.Cells["WKVC"].Value != DBNull.Value && row.Cells["WKVC"].Value != null && Convert.ToBoolean(row.Cells["WKVC"].Value) == true) ? true : false;
                        obj.c = (row.Cells["WKC"].Value != DBNull.Value && row.Cells["WKC"].Value != null && Convert.ToBoolean(row.Cells["WKC"].Value) == true) ? true : false;
                        lstPlayer.Add(obj);
                    }

                    foreach (var row in dgvBATSelected)
                    {
                        lstBAT.Add(Convert.ToInt32(row.Cells["BATPlayerID"].Value));
                        obj = new PlayerDetail();
                        obj.id = Convert.ToInt32(row.Cells["BATPlayerID"].Value);
                        obj.name = row.Cells["BATName"].Value.ToString();
                        obj.team = row.Cells["BATTeam"].Value.ToString();
                        obj.credits = Convert.ToDecimal(row.Cells["BATCredits"].Value);
                        obj.must = (row.Cells["BATMust"].Value != DBNull.Value && row.Cells["BATMust"].Value != null && Convert.ToBoolean(row.Cells["BATMust"].Value) == true) ? true : false;
                        obj.player_type = "BAT";
                        obj.vc = (row.Cells["BATVC"].Value != DBNull.Value && row.Cells["BATVC"].Value != null && Convert.ToBoolean(row.Cells["BATVC"].Value) == true) ? true : false;
                        obj.c = (row.Cells["BATC"].Value != DBNull.Value && row.Cells["BATC"].Value != null && Convert.ToBoolean(row.Cells["BATC"].Value) == true) ? true : false;
                        lstPlayer.Add(obj);
                    }

                    foreach (var row in dgvALLSelected)
                    {
                        lstALL.Add(Convert.ToInt32(row.Cells["ALLPlayerID"].Value));
                        obj = new PlayerDetail();
                        obj.id = Convert.ToInt32(row.Cells["ALLPlayerID"].Value);
                        obj.name = row.Cells["ALLName"].Value.ToString();
                        obj.team = row.Cells["ALLTeam"].Value.ToString();
                        obj.credits = Convert.ToDecimal(row.Cells["ALLCredits"].Value);
                        obj.must = (row.Cells["ALLMust"].Value != DBNull.Value && row.Cells["ALLMust"].Value != null && Convert.ToBoolean(row.Cells["ALLMust"].Value) == true) ? true : false;
                        obj.player_type = "ALL";
                        obj.vc = (row.Cells["ALLVC"].Value != DBNull.Value && row.Cells["ALLVC"].Value != null && Convert.ToBoolean(row.Cells["ALLVC"].Value) == true) ? true : false;
                        obj.c = (row.Cells["ALLC"].Value != DBNull.Value && row.Cells["ALLC"].Value != null && Convert.ToBoolean(row.Cells["ALLC"].Value) == true) ? true : false;
                        lstPlayer.Add(obj);
                    }

                    foreach (var row in dgvBOWLSelected)
                    {
                        lstBOWL.Add(Convert.ToInt32(row.Cells["BOWLPlayerID"].Value));
                        obj = new PlayerDetail();
                        obj.id = Convert.ToInt32(row.Cells["BOWLPlayerID"].Value);
                        obj.name = row.Cells["BOWLName"].Value.ToString();
                        obj.team = row.Cells["BOWLTeam"].Value.ToString();
                        obj.credits = Convert.ToDecimal(row.Cells["BOWLCredits"].Value);
                        obj.must = (row.Cells["BOWLMust"].Value != DBNull.Value && row.Cells["BOWLMust"].Value != null && Convert.ToBoolean(row.Cells["BOWLMust"].Value) == true) ? true : false;
                        obj.player_type = "BOWL";
                        obj.vc = (row.Cells["BOWLVC"].Value != DBNull.Value && row.Cells["BOWLVC"].Value != null && Convert.ToBoolean(row.Cells["BOWLVC"].Value) == true) ? true : false;
                        obj.c = (row.Cells["BOWLC"].Value != DBNull.Value && row.Cells["BOWLC"].Value != null && Convert.ToBoolean(row.Cells["BOWLC"].Value) == true) ? true : false;
                        lstPlayer.Add(obj);
                    }

                    #endregion

                    var groupedTeam = from player in lstPlayer
                                      group player by player.team into team
                                      orderby team.Key ascending
                                      select team;

                    this.TeamA = groupedTeam.ElementAt(0).Key;
                    this.TeamB = ((groupedTeam.ElementAtOrDefault(1) != null) ? groupedTeam.ElementAt(1).Key : null);

                    if(string.IsNullOrEmpty(this.TeamB))
                    {
                        MessageBox.Show("Need to choose players from both teams..!",ProductName);
                        return;
                    }

                    return;

                    #region "Background Worker"
                    string sExportFilePath = string.Empty;
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.FileName = string.Format("{0}_{1}({2})",this.TeamA, this.TeamB, string.Join("_", this.Date.Split(Path.GetInvalidFileNameChars())));
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        sExportFilePath = string.Format("{0}.xlsx",sfd.FileName);
                    }

                    if (string.IsNullOrEmpty(sExportFilePath))
                        return;

                    tsProgress.Visible = true;
                    tslblMessage.Visible = true;
                    tslblMessage.Text = "Getting team ...";
                    tsProgress.Style = ProgressBarStyle.Marquee;

                    if (File.Exists(sExportFilePath))
                    {
                        File.Delete(sExportFilePath);
                    }

                    object[] parameters = new object[] { lstPlayer , sExportFilePath };
                    bgwGenerateTeam.RunWorkerAsync(parameters);

                    #endregion

                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex.ToString());
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex.ToString());
            }
            
        }

        private void bgwGenerateTeam_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] parameters = e.Argument as object[];
                List<PlayerDetail> lstPlayer = ((List<PlayerDetail>)parameters[0]);
                string sExportFilePath = parameters[1].ToString();

                List<PlayerDetail> player;
                decimal Credits = 0;
                int TeamACount = 0;
                int TeamBCount = 0;
                int ViceCaptainCount = 0;

                List<int> listWK = lstPlayer.Where(item => item.player_type == "WK").Select(n => n.id).ToList();
                List<int> listBAT = lstPlayer.Where(item => item.player_type == "BAT").Select(n => n.id).ToList();
                List<int> listALL = lstPlayer.Where(item => item.player_type == "ALL").Select(n => n.id).ToList();
                List<int> listBOWL = lstPlayer.Where(item => item.player_type == "BOWL").Select(n => n.id).ToList();

                List<int> listWKMust = lstPlayer.Where(item => item.player_type == "WK" && item.must == true).Select(n => n.id).ToList();
                List<int> listBATMust = lstPlayer.Where(item => item.player_type == "BAT" && item.must == true).Select(n => n.id).ToList();
                List<int> listALLMust = lstPlayer.Where(item => item.player_type == "ALL" && item.must == true).Select(n => n.id).ToList();
                List<int> listBOWLMust = lstPlayer.Where(item => item.player_type == "BOWL" && item.must == true).Select(n => n.id).ToList();

                Combinations<int> combWK = null;

                Combinations<int> combBAT3 = null;
                Combinations<int> combBAT4 = null;
                Combinations<int> combBAT5 = null;

                Combinations<int> combALL1 = null;
                Combinations<int> combALL2 = null;
                Combinations<int> combALL3 = null;

                Combinations<int> combBOWL3 = null;
                Combinations<int> combBOWL4 = null;
                Combinations<int> combBOWL5 = null;

                List<TeamWithCombo> teams = new List<TeamWithCombo>();
                TeamWithCombo objCombo = null;
                
                #region "WK Combinations"

                if (listWK.Count() >= 1)
                {
                    combWK = new Combinations<int>(listWK, 1, GenerateOption.WithoutRepetition);
                }

                #endregion

                #region "BAT Combinations"

                if (listBAT.Count >= 3)
                {
                    combBAT3 = new Combinations<int>(listBAT, 3, GenerateOption.WithoutRepetition);
                }

                if (listBAT.Count >= 4)
                {
                    combBAT4 = new Combinations<int>(listBAT, 4, GenerateOption.WithoutRepetition);
                }

                if (listBAT.Count >= 5)
                {
                    combBAT5 = new Combinations<int>(listBAT, 5, GenerateOption.WithoutRepetition);
                }

                #endregion

                #region "ALL Combinations"

                if (listALL.Count >= 1)
                {
                    combALL1 = new Combinations<int>(listALL, 1, GenerateOption.WithoutRepetition);
                }

                if (listALL.Count >= 2)
                {
                    combALL2 = new Combinations<int>(listALL, 2, GenerateOption.WithoutRepetition);
                }

                if (listALL.Count >= 3)
                {
                    combALL3 = new Combinations<int>(listALL, 3);
                }

                #endregion

                #region "BOWL Combinations"

                if (listBOWL.Count >= 3)
                {
                    combBOWL3 = new Combinations<int>(listBOWL, 3, GenerateOption.WithoutRepetition);
                }

                if (listBOWL.Count >= 4)
                {
                    combBOWL4 = new Combinations<int>(listBOWL, 4, GenerateOption.WithoutRepetition);
                }

                if (listBOWL.Count >= 5)
                {
                    combBOWL5 = new Combinations<int>(listBOWL, 5, GenerateOption.WithoutRepetition);
                }

                #endregion

                #region "Capatain & Vice Captain"

                System.Drawing.Color colorHeading = System.Drawing.ColorTranslator.FromHtml("#AAFF00");
                System.Drawing.Color colSummary = System.Drawing.ColorTranslator.FromHtml("#46F0FF");

                int iCurrentRow = 1;
                int iRowStart = 0;

                #region "Excel object Declaration"

                Excel.Application xlApp = null;
                Excel.Workbook xlWorkBook = null;
                Excel.Worksheet xlWorkSheet;
                Excel.Range range = null;
                Excel.Range findRange = null;
                object misValue = System.Reflection.Missing.Value;
                int hWnd = 0;

                #endregion

                #region "Excel object Initialization"

                xlApp = new Excel.Application();
                xlApp.Visible = false;
                xlApp.DisplayAlerts = false;
                hWnd = xlApp.Application.Hwnd;
                xlWorkBook = xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                xlApp.StandardFont = "Calibri";
                xlApp.StandardFontSize = 11;

                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet.Name = "Player Details";

                #endregion

                #region "Player List"

                iCurrentRow++;

                xlWorkSheet.Cells[iCurrentRow, 1] = "WK";
                xlWorkSheet.Cells[iCurrentRow, 2] = "BAT";
                xlWorkSheet.Cells[iCurrentRow, 3] = "ALL";
                xlWorkSheet.Cells[iCurrentRow, 4] = "BOWL";

                range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "D" + iCurrentRow.ToString());
                range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colSummary);
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                range = null;

                iCurrentRow++;

                iRowStart = iCurrentRow;

                var maxRow = lstPlayer.GroupBy(x => x.player_type).Max(t => t.Count());

                iCurrentRow = iRowStart;
                foreach (PlayerDetail item in lstPlayer.Where(r => r.player_type == "WK"))
                {
                    xlWorkSheet.Cells[iCurrentRow, 1] = item.name;

                    if (item.must)
                    {
                        range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "A" + iCurrentRow.ToString());
                        range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                        range = null;
                    }

                    iCurrentRow++;
                }

                iCurrentRow = iRowStart;
                foreach (PlayerDetail item in lstPlayer.Where(r => r.player_type == "BAT"))
                {
                    xlWorkSheet.Cells[iCurrentRow, 2] = item.name;

                    if (item.must)
                    {
                        range = xlWorkSheet.Rows.get_Range("B" + iCurrentRow.ToString(), "B" + iCurrentRow.ToString());
                        range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                        range = null;
                    }

                    iCurrentRow++;
                }

                iCurrentRow = iRowStart;
                foreach (PlayerDetail item in lstPlayer.Where(r => r.player_type == "ALL"))
                {
                    xlWorkSheet.Cells[iCurrentRow, 3] = item.name;

                    if (item.must)
                    {
                        range = xlWorkSheet.Rows.get_Range("C" + iCurrentRow.ToString(), "C" + iCurrentRow.ToString());
                        range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                        range = null;
                    }

                    iCurrentRow++;
                }

                iCurrentRow = iRowStart;
                foreach (PlayerDetail item in lstPlayer.Where(r => r.player_type == "BOWL"))
                {
                    xlWorkSheet.Cells[iCurrentRow, 4] = item.name;

                    if (item.must)
                    {
                        range = xlWorkSheet.Rows.get_Range("D" + iCurrentRow.ToString(), "D" + iCurrentRow.ToString());
                        range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                        range = null;
                    }

                    iCurrentRow++;
                }

                iCurrentRow = iRowStart + (maxRow - 1);
                
                range = xlWorkSheet.Rows.get_Range("A" + iRowStart.ToString(), "D" + iCurrentRow.ToString());
                range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                range = null;

                #endregion

                try
                {
                    IEnumerable<int> NotMatching = null;
                    
                    if (combWK != null && combWK.Count() > 0)
                    {
                        foreach (IList<int> permWK in combWK)
                        {
                            var wk = lstPlayer.FirstOrDefault(i => i.id == permWK.ElementAt(0));

                            #region "Combination1 - 5BAT,2ALL,3BOWL"

                            comboSelected = dgvCombination.Rows.Cast<DataGridViewRow>().Where(r => (r.Cells["checkBoxColumn"].Value != DBNull.Value && r.Cells["checkBoxColumn"].Value != null && r.Cells["dgvCombo"].Value.ToString() == Combination.COMBO1 && Convert.ToBoolean(r.Cells["checkBoxColumn"].Value) == true));

                            if (comboSelected != null && comboSelected.Count() > 0)
                            {
                                if (combBAT5 != null && combBAT5.Count() > 0)
                                {
                                    foreach (IList<int> permBAT5 in combBAT5)
                                    {
                                        var bat1 = lstPlayer.FirstOrDefault(i => i.id == permBAT5.ElementAt(0));
                                        var bat2 = lstPlayer.FirstOrDefault(i => i.id == permBAT5.ElementAt(1));
                                        var bat3 = lstPlayer.FirstOrDefault(i => i.id == permBAT5.ElementAt(2));
                                        var bat4 = lstPlayer.FirstOrDefault(i => i.id == permBAT5.ElementAt(3));
                                        var bat5 = lstPlayer.FirstOrDefault(i => i.id == permBAT5.ElementAt(4));

                                        NotMatching = from i in listBATMust
                                                      where !permBAT5.Contains(i)
                                                      select i;

                                        if (NotMatching != null && NotMatching.Count() > 0)
                                            continue;

                                        #region "2 ALL"
                                        if (combALL2 != null && combALL2.Count() > 0)
                                        {
                                            foreach (IList<int> permALL2 in combALL2)
                                            {
                                                var all1 = lstPlayer.FirstOrDefault(i => i.id == permALL2.ElementAt(0));
                                                var all2 = lstPlayer.FirstOrDefault(i => i.id == permALL2.ElementAt(1));

                                                NotMatching = from i in listALLMust
                                                              where !permALL2.Contains(i)
                                                              select i;

                                                if (NotMatching != null && NotMatching.Count() > 0)
                                                    continue;

                                                #region "3 BOWL"
                                                if (combBOWL3 != null && combBOWL3.Count() > 0)
                                                {
                                                    foreach (IList<int> permBOWL3 in combBOWL3)
                                                    {
                                                        var bowl1 = lstPlayer.FirstOrDefault(i => i.id == permBOWL3.ElementAt(0));
                                                        var bowl2 = lstPlayer.FirstOrDefault(i => i.id == permBOWL3.ElementAt(1));
                                                        var bowl3 = lstPlayer.FirstOrDefault(i => i.id == permBOWL3.ElementAt(2));

                                                        NotMatching = from i in listBOWLMust
                                                                      where !permBOWL3.Contains(i)
                                                                      select i;

                                                        if (NotMatching != null && NotMatching.Count() > 0)
                                                            continue;

                                                        player = new List<PlayerDetail> { wk, bat1, bat2, bat3, bat4, bat5, all1, all2, bowl1, bowl2, bowl3 };
                                                        Credits = player.Select(n => n.credits).Sum();
                                                        TeamACount = player.Where(item => item.team == this.TeamA).Count();
                                                        TeamBCount = player.Where(item => item.team == this.TeamB).Count();

                                                        if ((Credits <= 100) && (TeamACount <= 7 && TeamBCount <= 7) && (TeamACount + TeamBCount) == 11)
                                                        {
                                                            //if ((rbTeamA.Checked || rbTeamB.Checked) && ((rbTeamA.Checked ? TeamACount : TeamBCount) != 7))
                                                            //    continue;

                                                            teams.Add(new TeamWithCombo(Combination.COMBO1, player, Credits));
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }

                                        }
                                        #endregion
                                    }
                                }
                            }

                            #endregion

                            #region "Combination2 - 5BAT,1ALL,4BOWL"

                            comboSelected = dgvCombination.Rows.Cast<DataGridViewRow>().Where(r => (r.Cells["checkBoxColumn"].Value != DBNull.Value && r.Cells["checkBoxColumn"].Value != null && r.Cells["dgvCombo"].Value.ToString() == Combination.COMBO2 && Convert.ToBoolean(r.Cells["checkBoxColumn"].Value) == true));

                            if (comboSelected != null && comboSelected.Count() > 0)
                            {
                                if (combBAT5 != null && combBAT5.Count() > 0)
                                {
                                    foreach (IList<int> permBAT5 in combBAT5)
                                    {
                                        var bat1 = lstPlayer.FirstOrDefault(i => i.id == permBAT5.ElementAt(0));
                                        var bat2 = lstPlayer.FirstOrDefault(i => i.id == permBAT5.ElementAt(1));
                                        var bat3 = lstPlayer.FirstOrDefault(i => i.id == permBAT5.ElementAt(2));
                                        var bat4 = lstPlayer.FirstOrDefault(i => i.id == permBAT5.ElementAt(3));
                                        var bat5 = lstPlayer.FirstOrDefault(i => i.id == permBAT5.ElementAt(4));

                                        NotMatching = from i in listBATMust
                                                      where !permBAT5.Contains(i)
                                                      select i;

                                        if (NotMatching != null && NotMatching.Count() > 0)
                                            continue;

                                        #region "1 ALL"
                                        if (combALL1 != null && combALL1.Count() > 0)
                                        {
                                            foreach (IList<int> permALL1 in combALL1)
                                            {
                                                var all1 = lstPlayer.FirstOrDefault(i => i.id == permALL1.ElementAt(0));

                                                NotMatching = from i in listALLMust
                                                              where !permALL1.Contains(i)
                                                              select i;

                                                if (NotMatching != null && NotMatching.Count() > 0)
                                                    continue;

                                                #region "4 BOWL"
                                                if (combBOWL4 != null && combBOWL4.Count() > 0)
                                                {
                                                    foreach (IList<int> permBOWL4 in combBOWL4)
                                                    {
                                                        var bowl1 = lstPlayer.FirstOrDefault(i => i.id == permBOWL4.ElementAt(0));
                                                        var bowl2 = lstPlayer.FirstOrDefault(i => i.id == permBOWL4.ElementAt(1));
                                                        var bowl3 = lstPlayer.FirstOrDefault(i => i.id == permBOWL4.ElementAt(2));
                                                        var bowl4 = lstPlayer.FirstOrDefault(i => i.id == permBOWL4.ElementAt(3));

                                                        NotMatching = from i in listBOWLMust
                                                                      where !permBOWL4.Contains(i)
                                                                      select i;

                                                        if (NotMatching != null && NotMatching.Count() > 0)
                                                            continue;

                                                        player = new List<PlayerDetail> { wk, bat1, bat2, bat3, bat4, bat5, all1, bowl1, bowl2, bowl3, bowl4 };
                                                        Credits = player.Select(n => n.credits).Sum();
                                                        TeamACount = player.Where(item => item.team == this.TeamA).Count();
                                                        TeamBCount = player.Where(item => item.team == this.TeamB).Count();
                                                        ViceCaptainCount = player.Where(item => item.vc == true).Count();

                                                        if ((Credits <= 100) && (TeamACount <= 7 && TeamBCount <= 7) && (TeamACount + TeamBCount) == 11)
                                                        {
                                                            teams.Add(new TeamWithCombo(Combination.COMBO2, player, Credits));
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }

                            #endregion

                            #region "Combination3 - 4BAT,1ALL,5BOWL"

                            comboSelected = dgvCombination.Rows.Cast<DataGridViewRow>().Where(r => (r.Cells["checkBoxColumn"].Value != DBNull.Value && r.Cells["checkBoxColumn"].Value != null && r.Cells["dgvCombo"].Value.ToString() == Combination.COMBO3 && Convert.ToBoolean(r.Cells["checkBoxColumn"].Value) == true));

                            if (comboSelected != null && comboSelected.Count() > 0)
                            {
                                if (combBAT4 != null && combBAT4.Count() > 0)
                                {
                                    foreach (IList<int> permBAT4 in combBAT4)
                                    {
                                        var bat1 = lstPlayer.FirstOrDefault(i => i.id == permBAT4.ElementAt(0));
                                        var bat2 = lstPlayer.FirstOrDefault(i => i.id == permBAT4.ElementAt(1));
                                        var bat3 = lstPlayer.FirstOrDefault(i => i.id == permBAT4.ElementAt(2));
                                        var bat4 = lstPlayer.FirstOrDefault(i => i.id == permBAT4.ElementAt(3));

                                        NotMatching = from i in listBATMust
                                                      where !permBAT4.Contains(i)
                                                      select i;

                                        if (NotMatching != null && NotMatching.Count() > 0)
                                            continue;

                                        #region "1 ALL"
                                        if (combALL1 != null && combALL1.Count() > 0)
                                        {
                                            foreach (IList<int> permALL1 in combALL1)
                                            {
                                                var all1 = lstPlayer.FirstOrDefault(i => i.id == permALL1.ElementAt(0));

                                                NotMatching = from i in listALLMust
                                                              where !permALL1.Contains(i)
                                                              select i;

                                                if (NotMatching != null && NotMatching.Count() > 0)
                                                    continue;

                                                #region "5 BOWL"
                                                if (combBOWL5 != null && combBOWL5.Count() > 0)
                                                {
                                                    foreach (IList<int> permBOWL5 in combBOWL5)
                                                    {
                                                        var bowl1 = lstPlayer.FirstOrDefault(i => i.id == permBOWL5.ElementAt(0));
                                                        var bowl2 = lstPlayer.FirstOrDefault(i => i.id == permBOWL5.ElementAt(1));
                                                        var bowl3 = lstPlayer.FirstOrDefault(i => i.id == permBOWL5.ElementAt(2));
                                                        var bowl4 = lstPlayer.FirstOrDefault(i => i.id == permBOWL5.ElementAt(3));
                                                        var bowl5 = lstPlayer.FirstOrDefault(i => i.id == permBOWL5.ElementAt(4));

                                                        NotMatching = from i in listBOWLMust
                                                                      where !permBOWL5.Contains(i)
                                                                      select i;

                                                        if (NotMatching != null && NotMatching.Count() > 0)
                                                            continue;

                                                        player = new List<PlayerDetail> { wk, bat1, bat2, bat3, bat4, all1, bowl1, bowl2, bowl3, bowl4, bowl5 };
                                                        Credits = player.Select(n => n.credits).Sum();
                                                        TeamACount = player.Where(item => item.team == this.TeamA).Count();
                                                        TeamBCount = player.Where(item => item.team == this.TeamB).Count();
                                                        ViceCaptainCount = player.Where(item => item.vc == true).Count();

                                                        if ((Credits <= 100) && (TeamACount <= 7 && TeamBCount <= 7) && (TeamACount + TeamBCount) == 11)
                                                        {
                                                            teams.Add(new TeamWithCombo(Combination.COMBO3, player, Credits));
                                                        }

                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion

                                    }
                                }
                            }

                            #endregion

                            #region "Combination4 - 4BAT,2ALL,4BOWL"

                            comboSelected = dgvCombination.Rows.Cast<DataGridViewRow>().Where(r => (r.Cells["checkBoxColumn"].Value != DBNull.Value && r.Cells["checkBoxColumn"].Value != null && r.Cells["dgvCombo"].Value.ToString() == Combination.COMBO4 && Convert.ToBoolean(r.Cells["checkBoxColumn"].Value) == true));

                            if (comboSelected != null && comboSelected.Count() > 0)
                            {
                                if (combBAT4 != null && combBAT4.Count() > 0)
                                {
                                    foreach (IList<int> permBAT4 in combBAT4)
                                    {
                                        var bat1 = lstPlayer.FirstOrDefault(i => i.id == permBAT4.ElementAt(0));
                                        var bat2 = lstPlayer.FirstOrDefault(i => i.id == permBAT4.ElementAt(1));
                                        var bat3 = lstPlayer.FirstOrDefault(i => i.id == permBAT4.ElementAt(2));
                                        var bat4 = lstPlayer.FirstOrDefault(i => i.id == permBAT4.ElementAt(3));

                                        NotMatching = from i in listBATMust
                                                      where !permBAT4.Contains(i)
                                                      select i;

                                        if (NotMatching != null && NotMatching.Count() > 0)
                                            continue;

                                        #region "2 ALL"
                                        if (combALL2 != null && combALL2.Count() > 0)
                                        {
                                            foreach (IList<int> permALL2 in combALL2)
                                            {
                                                var all1 = lstPlayer.FirstOrDefault(i => i.id == permALL2.ElementAt(0));
                                                var all2 = lstPlayer.FirstOrDefault(i => i.id == permALL2.ElementAt(1));

                                                NotMatching = from i in listALLMust
                                                              where !permALL2.Contains(i)
                                                              select i;

                                                if (NotMatching != null && NotMatching.Count() > 0)
                                                    continue;

                                                #region "4 BOWL"
                                                if (combBOWL4 != null && combBOWL4.Count() > 0)
                                                {
                                                    foreach (IList<int> permBOWL4 in combBOWL4)
                                                    {
                                                        var bowl1 = lstPlayer.FirstOrDefault(i => i.id == permBOWL4.ElementAt(0));
                                                        var bowl2 = lstPlayer.FirstOrDefault(i => i.id == permBOWL4.ElementAt(1));
                                                        var bowl3 = lstPlayer.FirstOrDefault(i => i.id == permBOWL4.ElementAt(2));
                                                        var bowl4 = lstPlayer.FirstOrDefault(i => i.id == permBOWL4.ElementAt(3));

                                                        NotMatching = from i in listBOWLMust
                                                                      where !permBOWL4.Contains(i)
                                                                      select i;

                                                        if (NotMatching != null && NotMatching.Count() > 0)
                                                            continue;

                                                        player = new List<PlayerDetail> { wk, bat1, bat2, bat3, bat4, all1, all2, bowl1, bowl2, bowl3, bowl4 };
                                                        Credits = player.Select(n => n.credits).Sum();
                                                        TeamACount = player.Where(item => item.team == this.TeamA).Count();
                                                        TeamBCount = player.Where(item => item.team == this.TeamB).Count();
                                                        ViceCaptainCount = player.Where(item => item.vc == true).Count();

                                                        if ((Credits <= 100) && (TeamACount <= 7 && TeamBCount <= 7) && (TeamACount + TeamBCount) == 11)
                                                        {
                                                            teams.Add(new TeamWithCombo(Combination.COMBO4, player, Credits));
                                                        }

                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion

                                    }
                                }
                            }

                            #endregion

                            #region "Combination5 - 4BAT,3ALL,3BOWL"

                            comboSelected = dgvCombination.Rows.Cast<DataGridViewRow>().Where(r => (r.Cells["checkBoxColumn"].Value != DBNull.Value && r.Cells["checkBoxColumn"].Value != null && r.Cells["dgvCombo"].Value.ToString() == Combination.COMBO5 && Convert.ToBoolean(r.Cells["checkBoxColumn"].Value) == true));

                            if (comboSelected != null && comboSelected.Count() > 0)
                            {
                                if (combBAT4 != null && combBAT4.Count() > 0)
                                {
                                    foreach (IList<int> permBAT4 in combBAT4)
                                    {
                                        var bat1 = lstPlayer.FirstOrDefault(i => i.id == permBAT4.ElementAt(0));
                                        var bat2 = lstPlayer.FirstOrDefault(i => i.id == permBAT4.ElementAt(1));
                                        var bat3 = lstPlayer.FirstOrDefault(i => i.id == permBAT4.ElementAt(2));
                                        var bat4 = lstPlayer.FirstOrDefault(i => i.id == permBAT4.ElementAt(3));

                                        NotMatching = from i in listBATMust
                                                      where !permBAT4.Contains(i)
                                                      select i;

                                        if (NotMatching != null && NotMatching.Count() > 0)
                                            continue;

                                        #region "3 ALL"
                                        if (combALL3 != null && combALL3.Count() > 0)
                                        {
                                            foreach (IList<int> permALL3 in combALL3)
                                            {
                                                var all1 = lstPlayer.FirstOrDefault(i => i.id == permALL3.ElementAt(0));
                                                var all2 = lstPlayer.FirstOrDefault(i => i.id == permALL3.ElementAt(1));
                                                var all3 = lstPlayer.FirstOrDefault(i => i.id == permALL3.ElementAt(2));

                                                NotMatching = from i in listALLMust
                                                              where !permALL3.Contains(i)
                                                              select i;

                                                if (NotMatching != null && NotMatching.Count() > 0)
                                                    continue;

                                                #region "3 BOWL"
                                                if (combBOWL3 != null && combBOWL3.Count() > 0)
                                                {
                                                    foreach (IList<int> permBOWL3 in combBOWL3)
                                                    {
                                                        var bowl1 = lstPlayer.FirstOrDefault(i => i.id == permBOWL3.ElementAt(0));
                                                        var bowl2 = lstPlayer.FirstOrDefault(i => i.id == permBOWL3.ElementAt(1));
                                                        var bowl3 = lstPlayer.FirstOrDefault(i => i.id == permBOWL3.ElementAt(2));

                                                        NotMatching = from i in listBOWLMust
                                                                      where !permBOWL3.Contains(i)
                                                                      select i;

                                                        if (NotMatching != null && NotMatching.Count() > 0)
                                                            continue;

                                                        player = new List<PlayerDetail> { wk, bat1, bat2, bat3, bat4, all1, all2, all3, bowl1, bowl2, bowl3 };
                                                        Credits = player.Select(n => n.credits).Sum();
                                                        TeamACount = player.Where(item => item.team == this.TeamA).Count();
                                                        TeamBCount = player.Where(item => item.team == this.TeamB).Count();
                                                        ViceCaptainCount = player.Where(item => item.vc == true).Count();

                                                        if ((Credits <= 100) && (TeamACount <= 7 && TeamBCount <= 7) && (TeamACount + TeamBCount) == 11)
                                                        {
                                                            teams.Add(new TeamWithCombo(Combination.COMBO5, player, Credits));
                                                        }

                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion

                                    }
                                }
                            }

                            #endregion

                            #region "Combination6 - 3BAT,2ALL,5BOWL"

                            comboSelected = dgvCombination.Rows.Cast<DataGridViewRow>().Where(r => (r.Cells["checkBoxColumn"].Value != DBNull.Value && r.Cells["checkBoxColumn"].Value != null && r.Cells["dgvCombo"].Value.ToString() == Combination.COMBO6 && Convert.ToBoolean(r.Cells["checkBoxColumn"].Value) == true));

                            if (comboSelected != null && comboSelected.Count() > 0)
                            {
                                if (combBAT3 != null && combBAT3.Count() > 0)
                                {
                                    foreach (IList<int> permBAT3 in combBAT3)
                                    {
                                        var bat1 = lstPlayer.FirstOrDefault(i => i.id == permBAT3.ElementAt(0));
                                        var bat2 = lstPlayer.FirstOrDefault(i => i.id == permBAT3.ElementAt(1));
                                        var bat3 = lstPlayer.FirstOrDefault(i => i.id == permBAT3.ElementAt(2));

                                        NotMatching = from i in listBATMust
                                                      where !permBAT3.Contains(i)
                                                      select i;

                                        if (NotMatching != null && NotMatching.Count() > 0)
                                            continue;

                                        #region "2 ALL"
                                        if (combALL2 != null && combALL2.Count() > 0)
                                        {
                                            foreach (IList<int> permALL2 in combALL2)
                                            {
                                                var all1 = lstPlayer.FirstOrDefault(i => i.id == permALL2.ElementAt(0));
                                                var all2 = lstPlayer.FirstOrDefault(i => i.id == permALL2.ElementAt(1));

                                                NotMatching = from i in listALLMust
                                                              where !permALL2.Contains(i)
                                                              select i;

                                                if (NotMatching != null && NotMatching.Count() > 0)
                                                    continue;

                                                #region "5 BOWL"
                                                if (combBOWL5 != null && combBOWL5.Count() > 0)
                                                {
                                                    foreach (IList<int> permBOWL5 in combBOWL5)
                                                    {
                                                        var bowl1 = lstPlayer.FirstOrDefault(i => i.id == permBOWL5.ElementAt(0));
                                                        var bowl2 = lstPlayer.FirstOrDefault(i => i.id == permBOWL5.ElementAt(1));
                                                        var bowl3 = lstPlayer.FirstOrDefault(i => i.id == permBOWL5.ElementAt(2));
                                                        var bowl4 = lstPlayer.FirstOrDefault(i => i.id == permBOWL5.ElementAt(3));
                                                        var bowl5 = lstPlayer.FirstOrDefault(i => i.id == permBOWL5.ElementAt(4));

                                                        NotMatching = from i in listBOWLMust
                                                                      where !permBOWL5.Contains(i)
                                                                      select i;

                                                        if (NotMatching != null && NotMatching.Count() > 0)
                                                            continue;

                                                        player = new List<PlayerDetail> { wk, bat1, bat2, bat3, all1, all2, bowl1, bowl2, bowl3, bowl4, bowl5 };
                                                        Credits = player.Select(n => n.credits).Sum();
                                                        TeamACount = player.Where(item => item.team == this.TeamA).Count();
                                                        TeamBCount = player.Where(item => item.team == this.TeamB).Count();
                                                        ViceCaptainCount = player.Where(item => item.vc == true).Count();

                                                        if ((Credits <= 100) && (TeamACount <= 7 && TeamBCount <= 7) && (TeamACount + TeamBCount) == 11)
                                                        {
                                                            teams.Add(new TeamWithCombo(Combination.COMBO6, player, Credits));
                                                        }

                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion

                                    }
                                }
                            }
                            
                            #endregion

                            #region "Combination7 - 3BAT,3ALL,4BOWL"

                            comboSelected = dgvCombination.Rows.Cast<DataGridViewRow>().Where(r => (r.Cells["checkBoxColumn"].Value != DBNull.Value && r.Cells["checkBoxColumn"].Value != null && r.Cells["dgvCombo"].Value.ToString() == Combination.COMBO7 && Convert.ToBoolean(r.Cells["checkBoxColumn"].Value) == true));

                            if (comboSelected != null && comboSelected.Count() > 0)
                            {
                                if (combBAT3 != null && combBAT3.Count() > 0)
                                {
                                    foreach (IList<int> permBAT3 in combBAT3)
                                    {
                                        var bat1 = lstPlayer.FirstOrDefault(i => i.id == permBAT3.ElementAt(0));
                                        var bat2 = lstPlayer.FirstOrDefault(i => i.id == permBAT3.ElementAt(1));
                                        var bat3 = lstPlayer.FirstOrDefault(i => i.id == permBAT3.ElementAt(2));

                                        NotMatching = from i in listBATMust
                                                      where !permBAT3.Contains(i)
                                                      select i;

                                        if (NotMatching != null && NotMatching.Count() > 0)
                                            continue;

                                        #region "3 ALL"
                                        if (combALL3 != null && combALL3.Count() > 0)
                                        {
                                            foreach (IList<int> permALL3 in combALL3)
                                            {
                                                var all1 = lstPlayer.FirstOrDefault(i => i.id == permALL3.ElementAt(0));
                                                var all2 = lstPlayer.FirstOrDefault(i => i.id == permALL3.ElementAt(1));
                                                var all3 = lstPlayer.FirstOrDefault(i => i.id == permALL3.ElementAt(2));

                                                NotMatching = from i in listALLMust
                                                              where !permALL3.Contains(i)
                                                              select i;

                                                if (NotMatching != null && NotMatching.Count() > 0)
                                                    continue;

                                                #region "4 BOWL"
                                                if (combBOWL4 != null && combBOWL4.Count() > 0)
                                                {
                                                    foreach (IList<int> permBOWL4 in combBOWL4)
                                                    {
                                                        var bowl1 = lstPlayer.FirstOrDefault(i => i.id == permBOWL4.ElementAt(0));
                                                        var bowl2 = lstPlayer.FirstOrDefault(i => i.id == permBOWL4.ElementAt(1));
                                                        var bowl3 = lstPlayer.FirstOrDefault(i => i.id == permBOWL4.ElementAt(2));
                                                        var bowl4 = lstPlayer.FirstOrDefault(i => i.id == permBOWL4.ElementAt(3));

                                                        NotMatching = from i in listBOWLMust
                                                                      where !permBOWL4.Contains(i)
                                                                      select i;

                                                        if (NotMatching != null && NotMatching.Count() > 0)
                                                            continue;

                                                        player = new List<PlayerDetail> { wk, bat1, bat2, bat3, all1, all2, all3, bowl1, bowl2, bowl3, bowl4 };
                                                        Credits = player.Select(n => n.credits).Sum();
                                                        TeamACount = player.Where(item => item.team == this.TeamA).Count();
                                                        TeamBCount = player.Where(item => item.team == this.TeamB).Count();
                                                        ViceCaptainCount = player.Where(item => item.vc == true).Count();

                                                        if ((Credits <= 100) && (TeamACount <= 7 && TeamBCount <= 7) && (TeamACount + TeamBCount) == 11)
                                                        {
                                                            teams.Add(new TeamWithCombo(Combination.COMBO7, player, Credits));
                                                        }

                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }

                            #endregion
                        }

                    }

                    if(dtCombinationType != null && dtCombinationType.Rows.Count > 0)
                    {
                        int iComboStart = 0;

                        iCurrentRow = iCurrentRow + 2;

                        xlWorkSheet.Cells[iCurrentRow, 1] = "Combo";
                        xlWorkSheet.Cells[iCurrentRow, 2] = "Team";
                        xlWorkSheet.Cells[iCurrentRow, 3] = "Unique Teams";
                        xlWorkSheet.Cells[iCurrentRow, 4] = "C & VC Teams";

                        range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "D" + iCurrentRow.ToString());
                        range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colSummary);
                        range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                        range = null;

                        comboSelected = dgvCombination.Rows.Cast<DataGridViewRow>().Where(r => (r.Cells["checkBoxColumn"].Value != DBNull.Value && r.Cells["checkBoxColumn"].Value != null && Convert.ToBoolean(r.Cells["checkBoxColumn"].Value) == true));

                        foreach (DataGridViewRow dr in comboSelected)
                        {
                            iCurrentRow++;

                            iComboStart = (iComboStart == 0 ? iCurrentRow : iComboStart);

                            xlWorkSheet.Cells[iCurrentRow, 1] = dr.Cells["dgvCombo"].Value.ToString();
                            xlWorkSheet.Cells[iCurrentRow, 2] = dr.Cells["dgvTeam"].Value.ToString();

                            dr.Cells["dgvRowID"].Value = iCurrentRow;
                            
                        }

                        int iComboEnd = iCurrentRow;

                        /* Border */
                        range = xlWorkSheet.Rows.get_Range("A" + iComboStart.ToString(), "D" + iCurrentRow.ToString());
                        range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                        range = null;

                        /* Column Alignment */
                        range = xlWorkSheet.Rows.get_Range("C" + iComboStart.ToString(), "D" + iCurrentRow.ToString());
                        range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        range = null;

                        iCurrentRow++;

                        xlWorkSheet.Cells[iCurrentRow, 2] = "Total";

                        range = xlWorkSheet.Rows.get_Range("C" + iCurrentRow.ToString(), "C" + iCurrentRow.ToString());
                        range.Formula = string.Format("=SUM(C{0}:C{1})", iComboStart.ToString(),iComboEnd.ToString());
                        range = null;

                        range = xlWorkSheet.Rows.get_Range("D" + iCurrentRow.ToString(), "D" + iCurrentRow.ToString());
                        range.Formula = string.Format("=SUM(D{0}:D{1})", iComboStart.ToString(), iComboEnd.ToString());
                        range = null;

                        /* Border */
                        range = xlWorkSheet.Rows.get_Range("B" + iCurrentRow.ToString(), "D" + iCurrentRow.ToString());
                        range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                        range.EntireRow.Font.Bold = true;
                        range = null;

                        /* Column Alignment */
                        range = xlWorkSheet.Rows.get_Range("C" + iComboStart.ToString(), "D" + iCurrentRow.ToString());
                        range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        range = null;

                    }

                    var query = teams.GroupBy(team => team.Combo )
                                    .Select(team => new { Combo = team.Key , Teams = team , Credits = Credits })
                                    .OrderBy(team => team.Combo);

                    int iTeamStart = 0;

                    if (chkUniqueTeams.Checked)
                    {
                        foreach (var group in query)
                        {
                            iTeamStart = 0;

                            iCurrentRow = iCurrentRow + 2;

                            #region "Headers"

                            if (group.Combo.Equals(Combination.COMBO1))
                            {
                                xlWorkSheet.Cells[iCurrentRow, 1] = string.Empty;
                                range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                range.Value2 = "5BAT,2ALL,3BOWL";
                                range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colSummary);
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                range.Merge(misValue);
                                range = null;

                                iCurrentRow++;

                                xlWorkSheet.Cells[iCurrentRow, 1] = "TeamNo";
                                xlWorkSheet.Cells[iCurrentRow, 2] = "WK";
                                xlWorkSheet.Cells[iCurrentRow, 3] = "BAT1";
                                xlWorkSheet.Cells[iCurrentRow, 4] = "BAT2";
                                xlWorkSheet.Cells[iCurrentRow, 5] = "BAT3";
                                xlWorkSheet.Cells[iCurrentRow, 6] = "BAT4";
                                xlWorkSheet.Cells[iCurrentRow, 7] = "BAT5";
                                xlWorkSheet.Cells[iCurrentRow, 8] = "ALL1";
                                xlWorkSheet.Cells[iCurrentRow, 9] = "ALL2";
                                xlWorkSheet.Cells[iCurrentRow, 10] = "BOWL1";
                                xlWorkSheet.Cells[iCurrentRow, 11] = "BOWL2";
                                xlWorkSheet.Cells[iCurrentRow, 12] = "BOWL3";
                                xlWorkSheet.Cells[iCurrentRow, 13] = "Credits";

                                range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                range.EntireColumn.AutoFit();
                                range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                                range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                range = null;
                            }
                            else if (group.Combo.Equals(Combination.COMBO2))
                            {
                                xlWorkSheet.Cells[iCurrentRow, 1] = string.Empty;
                                range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                range.Value2 = "5BAT,1ALL,4BOWL";
                                range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colSummary);
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                range.Merge(misValue);
                                range = null;

                                iCurrentRow++;

                                xlWorkSheet.Cells[iCurrentRow, 1] = "TeamNo";
                                xlWorkSheet.Cells[iCurrentRow, 2] = "WK";
                                xlWorkSheet.Cells[iCurrentRow, 3] = "BAT1";
                                xlWorkSheet.Cells[iCurrentRow, 4] = "BAT2";
                                xlWorkSheet.Cells[iCurrentRow, 5] = "BAT3";
                                xlWorkSheet.Cells[iCurrentRow, 6] = "BAT4";
                                xlWorkSheet.Cells[iCurrentRow, 7] = "BAT5";
                                xlWorkSheet.Cells[iCurrentRow, 8] = "ALL1";
                                xlWorkSheet.Cells[iCurrentRow, 9] = "BOWL1";
                                xlWorkSheet.Cells[iCurrentRow, 10] = "BOWL2";
                                xlWorkSheet.Cells[iCurrentRow, 11] = "BOWL3";
                                xlWorkSheet.Cells[iCurrentRow, 12] = "BOWL4";
                                xlWorkSheet.Cells[iCurrentRow, 13] = "Credits";

                                range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                range.EntireColumn.AutoFit();
                                range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                                range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                range = null;
                            }
                            else if (group.Combo.Equals(Combination.COMBO3))
                            {
                                xlWorkSheet.Cells[iCurrentRow, 1] = string.Empty;
                                range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                range.Value2 = "4BAT,1ALL,5BOWL";
                                range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colSummary);
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                range.Merge(misValue);
                                range = null;

                                iCurrentRow++;

                                xlWorkSheet.Cells[iCurrentRow, 1] = "TeamNo";
                                xlWorkSheet.Cells[iCurrentRow, 2] = "WK";
                                xlWorkSheet.Cells[iCurrentRow, 3] = "BAT1";
                                xlWorkSheet.Cells[iCurrentRow, 4] = "BAT2";
                                xlWorkSheet.Cells[iCurrentRow, 5] = "BAT3";
                                xlWorkSheet.Cells[iCurrentRow, 6] = "BAT4";
                                xlWorkSheet.Cells[iCurrentRow, 7] = "ALL1";
                                xlWorkSheet.Cells[iCurrentRow, 8] = "BOWL1";
                                xlWorkSheet.Cells[iCurrentRow, 9] = "BOWL2";
                                xlWorkSheet.Cells[iCurrentRow, 10] = "BOWL3";
                                xlWorkSheet.Cells[iCurrentRow, 11] = "BOWL4";
                                xlWorkSheet.Cells[iCurrentRow, 12] = "BOWL5";
                                xlWorkSheet.Cells[iCurrentRow, 13] = "Credits";

                                range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                range.EntireColumn.AutoFit();
                                range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                                range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                range = null;
                            }
                            else if (group.Combo.Equals(Combination.COMBO4))
                            {
                                xlWorkSheet.Cells[iCurrentRow, 1] = string.Empty;
                                range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                range.Value2 = "4BAT,2ALL,4BOWL";
                                range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colSummary);
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                range.Merge(misValue);
                                range = null;

                                iCurrentRow++;

                                xlWorkSheet.Cells[iCurrentRow, 1] = "TeamNo";
                                xlWorkSheet.Cells[iCurrentRow, 2] = "WK";
                                xlWorkSheet.Cells[iCurrentRow, 3] = "BAT1";
                                xlWorkSheet.Cells[iCurrentRow, 4] = "BAT2";
                                xlWorkSheet.Cells[iCurrentRow, 5] = "BAT3";
                                xlWorkSheet.Cells[iCurrentRow, 6] = "BAT4";
                                xlWorkSheet.Cells[iCurrentRow, 7] = "ALL1";
                                xlWorkSheet.Cells[iCurrentRow, 8] = "ALL2";
                                xlWorkSheet.Cells[iCurrentRow, 9] = "BOWL1";
                                xlWorkSheet.Cells[iCurrentRow, 10] = "BOWL2";
                                xlWorkSheet.Cells[iCurrentRow, 11] = "BOWL3";
                                xlWorkSheet.Cells[iCurrentRow, 12] = "BOWL4";
                                xlWorkSheet.Cells[iCurrentRow, 13] = "Credits";

                                range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                range.EntireColumn.AutoFit();
                                range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                                range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                range = null;
                            }
                            else if (group.Combo.Equals(Combination.COMBO5))
                            {
                                xlWorkSheet.Cells[iCurrentRow, 1] = string.Empty;
                                range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                range.Value2 = "4BAT,3ALL,3BOWL";
                                range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colSummary);
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                range.Merge(misValue);
                                range = null;

                                iCurrentRow++;

                                xlWorkSheet.Cells[iCurrentRow, 1] = "TeamNo";
                                xlWorkSheet.Cells[iCurrentRow, 2] = "WK";
                                xlWorkSheet.Cells[iCurrentRow, 3] = "BAT1";
                                xlWorkSheet.Cells[iCurrentRow, 4] = "BAT2";
                                xlWorkSheet.Cells[iCurrentRow, 5] = "BAT3";
                                xlWorkSheet.Cells[iCurrentRow, 6] = "BAT4";
                                xlWorkSheet.Cells[iCurrentRow, 7] = "ALL1";
                                xlWorkSheet.Cells[iCurrentRow, 8] = "ALL2";
                                xlWorkSheet.Cells[iCurrentRow, 9] = "ALL3";
                                xlWorkSheet.Cells[iCurrentRow, 10] = "BOWL1";
                                xlWorkSheet.Cells[iCurrentRow, 11] = "BOWL2";
                                xlWorkSheet.Cells[iCurrentRow, 12] = "BOWL3";
                                xlWorkSheet.Cells[iCurrentRow, 13] = "Credits";

                                range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                range.EntireColumn.AutoFit();
                                range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                                range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                range = null;
                            }
                            else if (group.Combo.Equals(Combination.COMBO6))
                            {
                                xlWorkSheet.Cells[iCurrentRow, 1] = string.Empty;
                                range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                range.Value2 = "3BAT,2ALL,5BOWL";
                                range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colSummary);
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                range.Merge(misValue);
                                range = null;

                                iCurrentRow++;

                                xlWorkSheet.Cells[iCurrentRow, 1] = "TeamNo";
                                xlWorkSheet.Cells[iCurrentRow, 2] = "WK";
                                xlWorkSheet.Cells[iCurrentRow, 3] = "BAT1";
                                xlWorkSheet.Cells[iCurrentRow, 4] = "BAT2";
                                xlWorkSheet.Cells[iCurrentRow, 5] = "BAT3";
                                xlWorkSheet.Cells[iCurrentRow, 6] = "ALL1";
                                xlWorkSheet.Cells[iCurrentRow, 7] = "ALL2";
                                xlWorkSheet.Cells[iCurrentRow, 8] = "BOWL1";
                                xlWorkSheet.Cells[iCurrentRow, 9] = "BOWL2";
                                xlWorkSheet.Cells[iCurrentRow, 10] = "BOWL3";
                                xlWorkSheet.Cells[iCurrentRow, 11] = "BOWL4";
                                xlWorkSheet.Cells[iCurrentRow, 12] = "BOWL5";
                                xlWorkSheet.Cells[iCurrentRow, 13] = "Credits";

                                range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                range.EntireColumn.AutoFit();
                                range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                                range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                range = null;
                            }
                            else if (group.Combo.Equals(Combination.COMBO7))
                            {
                                xlWorkSheet.Cells[iCurrentRow, 1] = string.Empty;
                                range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                range.Value2 = "3BAT,3ALL,4BOWL";
                                range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colSummary);
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                range.Merge(misValue);
                                range = null;

                                iCurrentRow++;

                                xlWorkSheet.Cells[iCurrentRow, 1] = "TeamNo";
                                xlWorkSheet.Cells[iCurrentRow, 2] = "WK";
                                xlWorkSheet.Cells[iCurrentRow, 3] = "BAT1";
                                xlWorkSheet.Cells[iCurrentRow, 4] = "BAT2";
                                xlWorkSheet.Cells[iCurrentRow, 5] = "BAT3";
                                xlWorkSheet.Cells[iCurrentRow, 6] = "ALL1";
                                xlWorkSheet.Cells[iCurrentRow, 7] = "ALL2";
                                xlWorkSheet.Cells[iCurrentRow, 8] = "ALL3";
                                xlWorkSheet.Cells[iCurrentRow, 9] = "BOWL1";
                                xlWorkSheet.Cells[iCurrentRow, 10] = "BOWL2";
                                xlWorkSheet.Cells[iCurrentRow, 11] = "BOWL3";
                                xlWorkSheet.Cells[iCurrentRow, 12] = "BOWL4";
                                xlWorkSheet.Cells[iCurrentRow, 13] = "Credits";

                                range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                range.EntireColumn.AutoFit();
                                range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                                range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                range = null;
                            }

                            #endregion

                            #region "Content - Unique Teams"

                            foreach (var obj in group.Teams)
                            {
                                try
                                {
                                    player = obj.team;

                                    iCurrentRow++;

                                    iTeamStart = (iTeamStart == 0 ? iCurrentRow : iTeamStart);

                                    xlWorkSheet.Cells[iCurrentRow, 1] = (iCurrentRow - iTeamStart) + 1;
                                    xlWorkSheet.Cells[iCurrentRow, 2] = player.ElementAt(0).name;
                                    xlWorkSheet.Cells[iCurrentRow, 3] = player.ElementAt(1).name;
                                    xlWorkSheet.Cells[iCurrentRow, 4] = player.ElementAt(2).name;
                                    xlWorkSheet.Cells[iCurrentRow, 5] = player.ElementAt(3).name;
                                    xlWorkSheet.Cells[iCurrentRow, 6] = player.ElementAt(4).name;
                                    xlWorkSheet.Cells[iCurrentRow, 7] = player.ElementAt(5).name;
                                    xlWorkSheet.Cells[iCurrentRow, 8] = player.ElementAt(6).name;
                                    xlWorkSheet.Cells[iCurrentRow, 9] = player.ElementAt(7).name;
                                    xlWorkSheet.Cells[iCurrentRow, 10] = player.ElementAt(8).name;
                                    xlWorkSheet.Cells[iCurrentRow, 11] = player.ElementAt(9).name;
                                    xlWorkSheet.Cells[iCurrentRow, 12] = player.ElementAt(10).name;
                                    xlWorkSheet.Cells[iCurrentRow, 13] = obj.Credits;

                                    int gridIndex = (dgvCombination.Rows.Cast<DataGridViewRow>()
                                                .Where(r => r.Cells["dgvCombo"].Value.ToString() == obj.Combo.ToString())
                                                .Select(r => r.Index)).First();

                                    int rowIndex = Convert.ToInt32(dgvCombination.Rows[gridIndex].Cells["dgvRowID"].Value);

                                    xlWorkSheet.Cells[rowIndex, 3] = (iCurrentRow - iTeamStart) + 1;

                                }
                                catch (Exception ex)
                                {
                                    ExceptionHandler.HandleException(ex.ToString());
                                }
                            }

                            /* Border */
                            range = xlWorkSheet.Rows.get_Range("A" + iTeamStart.ToString(), "M" + iCurrentRow.ToString());
                            range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                            range = null;

                            /* Column Alignment */
                            range = xlWorkSheet.Rows.get_Range("A" + iTeamStart.ToString(), "A" + iCurrentRow.ToString());
                            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            range = null;

                            #endregion

                        }
                    }

                    if (chkWithCVC.Checked)
                    {
                        
                        foreach (var group in query)
                        {
                            iTeamStart = 0;

                            foreach (var obj in group.Teams)
                            {
                                try
                                {
                                    player = obj.team;

                                    foreach (var c in player.Where(item => item.c == true))
                                    {
                                        foreach (var vc in player.Where(item => item.vc == true))
                                        {
                                            if (c.name.Equals(vc.name))
                                                continue;

                                            if(iTeamStart == 0)
                                            {
                                                #region "Headers"

                                                if (group.Combo.Equals(Combination.COMBO1))
                                                {
                                                    iCurrentRow = iCurrentRow + 2;

                                                    xlWorkSheet.Cells[iCurrentRow, 1] = string.Empty;
                                                    range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                                    range.Value2 = "5BAT,2ALL,3BOWL";
                                                    range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colSummary);
                                                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                                    range.Merge(misValue);
                                                    range = null;

                                                    iCurrentRow++;

                                                    xlWorkSheet.Cells[iCurrentRow, 1] = "TeamNo";
                                                    xlWorkSheet.Cells[iCurrentRow, 2] = "WK";
                                                    xlWorkSheet.Cells[iCurrentRow, 3] = "BAT1";
                                                    xlWorkSheet.Cells[iCurrentRow, 4] = "BAT2";
                                                    xlWorkSheet.Cells[iCurrentRow, 5] = "BAT3";
                                                    xlWorkSheet.Cells[iCurrentRow, 6] = "BAT4";
                                                    xlWorkSheet.Cells[iCurrentRow, 7] = "BAT5";
                                                    xlWorkSheet.Cells[iCurrentRow, 8] = "ALL1";
                                                    xlWorkSheet.Cells[iCurrentRow, 9] = "ALL2";
                                                    xlWorkSheet.Cells[iCurrentRow, 10] = "BOWL1";
                                                    xlWorkSheet.Cells[iCurrentRow, 11] = "BOWL2";
                                                    xlWorkSheet.Cells[iCurrentRow, 12] = "BOWL3";
                                                    xlWorkSheet.Cells[iCurrentRow, 13] = "Credits";

                                                    range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    range.EntireColumn.AutoFit();
                                                    range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                                                    range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                                    range = null;
                                                }
                                                else if (group.Combo.Equals(Combination.COMBO2))
                                                {
                                                    iCurrentRow = iCurrentRow + 2;

                                                    xlWorkSheet.Cells[iCurrentRow, 1] = string.Empty;
                                                    range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                                    range.Value2 = "5BAT,1ALL,4BOWL";                                                    
                                                    range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colSummary);
                                                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                                    range.Merge(misValue);
                                                    range = null;

                                                    iCurrentRow++;

                                                    xlWorkSheet.Cells[iCurrentRow, 1] = "TeamNo";
                                                    xlWorkSheet.Cells[iCurrentRow, 2] = "WK";
                                                    xlWorkSheet.Cells[iCurrentRow, 3] = "BAT1";
                                                    xlWorkSheet.Cells[iCurrentRow, 4] = "BAT2";
                                                    xlWorkSheet.Cells[iCurrentRow, 5] = "BAT3";
                                                    xlWorkSheet.Cells[iCurrentRow, 6] = "BAT4";
                                                    xlWorkSheet.Cells[iCurrentRow, 7] = "BAT5";
                                                    xlWorkSheet.Cells[iCurrentRow, 8] = "ALL1";
                                                    xlWorkSheet.Cells[iCurrentRow, 9] = "BOWL1";
                                                    xlWorkSheet.Cells[iCurrentRow, 10] = "BOWL2";
                                                    xlWorkSheet.Cells[iCurrentRow, 11] = "BOWL3";
                                                    xlWorkSheet.Cells[iCurrentRow, 12] = "BOWL4";
                                                    xlWorkSheet.Cells[iCurrentRow, 13] = "Credits";

                                                    range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    range.EntireColumn.AutoFit();
                                                    range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                                                    range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                                    range = null;
                                                }
                                                else if (group.Combo.Equals(Combination.COMBO3))
                                                {
                                                    iCurrentRow = iCurrentRow + 2;

                                                    xlWorkSheet.Cells[iCurrentRow, 1] = string.Empty;
                                                    range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                                    range.Value2 = "4BAT,1ALL,5BOWL";
                                                    range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colSummary);
                                                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                                    range.Merge(misValue);
                                                    range = null;

                                                    iCurrentRow++;

                                                    xlWorkSheet.Cells[iCurrentRow, 1] = "TeamNo";
                                                    xlWorkSheet.Cells[iCurrentRow, 2] = "WK";
                                                    xlWorkSheet.Cells[iCurrentRow, 3] = "BAT1";
                                                    xlWorkSheet.Cells[iCurrentRow, 4] = "BAT2";
                                                    xlWorkSheet.Cells[iCurrentRow, 5] = "BAT3";
                                                    xlWorkSheet.Cells[iCurrentRow, 6] = "BAT4";
                                                    xlWorkSheet.Cells[iCurrentRow, 7] = "ALL1";
                                                    xlWorkSheet.Cells[iCurrentRow, 8] = "BOWL1";
                                                    xlWorkSheet.Cells[iCurrentRow, 9] = "BOWL2";
                                                    xlWorkSheet.Cells[iCurrentRow, 10] = "BOWL3";
                                                    xlWorkSheet.Cells[iCurrentRow, 11] = "BOWL4";
                                                    xlWorkSheet.Cells[iCurrentRow, 12] = "BOWL5";
                                                    xlWorkSheet.Cells[iCurrentRow, 13] = "Credits";

                                                    range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    range.EntireColumn.AutoFit();
                                                    range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                                                    range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                                    range = null;
                                                }
                                                else if (group.Combo.Equals(Combination.COMBO4))
                                                {
                                                    iCurrentRow = iCurrentRow + 2;

                                                    xlWorkSheet.Cells[iCurrentRow, 1] = string.Empty;
                                                    range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                                    range.Value2 = "4BAT,2ALL,4BOWL";
                                                    range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colSummary);
                                                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                                    range.Merge(misValue);
                                                    range = null;

                                                    iCurrentRow++;

                                                    xlWorkSheet.Cells[iCurrentRow, 1] = "TeamNo";
                                                    xlWorkSheet.Cells[iCurrentRow, 2] = "WK";
                                                    xlWorkSheet.Cells[iCurrentRow, 3] = "BAT1";
                                                    xlWorkSheet.Cells[iCurrentRow, 4] = "BAT2";
                                                    xlWorkSheet.Cells[iCurrentRow, 5] = "BAT3";
                                                    xlWorkSheet.Cells[iCurrentRow, 6] = "BAT4";
                                                    xlWorkSheet.Cells[iCurrentRow, 7] = "ALL1";
                                                    xlWorkSheet.Cells[iCurrentRow, 8] = "ALL2";
                                                    xlWorkSheet.Cells[iCurrentRow, 9] = "BOWL1";
                                                    xlWorkSheet.Cells[iCurrentRow, 10] = "BOWL2";
                                                    xlWorkSheet.Cells[iCurrentRow, 11] = "BOWL3";
                                                    xlWorkSheet.Cells[iCurrentRow, 12] = "BOWL4";
                                                    xlWorkSheet.Cells[iCurrentRow, 13] = "Credits";

                                                    range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    range.EntireColumn.AutoFit();
                                                    range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                                                    range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                                    range = null;
                                                }
                                                else if (group.Combo.Equals(Combination.COMBO5))
                                                {
                                                    iCurrentRow = iCurrentRow + 2;

                                                    xlWorkSheet.Cells[iCurrentRow, 1] = string.Empty;
                                                    range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                                    range.Value2 = "4BAT,3ALL,3BOWL";
                                                    range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colSummary);
                                                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                                    range.Merge(misValue);
                                                    range = null;

                                                    iCurrentRow++;

                                                    xlWorkSheet.Cells[iCurrentRow, 1] = "TeamNo";
                                                    xlWorkSheet.Cells[iCurrentRow, 2] = "WK";
                                                    xlWorkSheet.Cells[iCurrentRow, 3] = "BAT1";
                                                    xlWorkSheet.Cells[iCurrentRow, 4] = "BAT2";
                                                    xlWorkSheet.Cells[iCurrentRow, 5] = "BAT3";
                                                    xlWorkSheet.Cells[iCurrentRow, 6] = "BAT4";
                                                    xlWorkSheet.Cells[iCurrentRow, 7] = "ALL1";
                                                    xlWorkSheet.Cells[iCurrentRow, 8] = "ALL2";
                                                    xlWorkSheet.Cells[iCurrentRow, 9] = "ALL3";
                                                    xlWorkSheet.Cells[iCurrentRow, 10] = "BOWL1";
                                                    xlWorkSheet.Cells[iCurrentRow, 11] = "BOWL2";
                                                    xlWorkSheet.Cells[iCurrentRow, 12] = "BOWL3";
                                                    xlWorkSheet.Cells[iCurrentRow, 13] = "Credits";

                                                    range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    range.EntireColumn.AutoFit();
                                                    range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                                                    range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                                    range = null;
                                                }
                                                else if (group.Combo.Equals(Combination.COMBO6))
                                                {
                                                    iCurrentRow = iCurrentRow + 2;

                                                    xlWorkSheet.Cells[iCurrentRow, 1] = string.Empty;
                                                    range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                                    range.Value2 = "3BAT,2ALL,5BOWL";
                                                    range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colSummary);
                                                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                                    range.Merge(misValue);
                                                    range = null;

                                                    iCurrentRow++;

                                                    xlWorkSheet.Cells[iCurrentRow, 1] = "TeamNo";
                                                    xlWorkSheet.Cells[iCurrentRow, 2] = "WK";
                                                    xlWorkSheet.Cells[iCurrentRow, 3] = "BAT1";
                                                    xlWorkSheet.Cells[iCurrentRow, 4] = "BAT2";
                                                    xlWorkSheet.Cells[iCurrentRow, 5] = "BAT3";
                                                    xlWorkSheet.Cells[iCurrentRow, 6] = "ALL1";
                                                    xlWorkSheet.Cells[iCurrentRow, 7] = "ALL2";
                                                    xlWorkSheet.Cells[iCurrentRow, 8] = "BOWL1";
                                                    xlWorkSheet.Cells[iCurrentRow, 9] = "BOWL2";
                                                    xlWorkSheet.Cells[iCurrentRow, 10] = "BOWL3";
                                                    xlWorkSheet.Cells[iCurrentRow, 11] = "BOWL4";
                                                    xlWorkSheet.Cells[iCurrentRow, 12] = "BOWL5";
                                                    xlWorkSheet.Cells[iCurrentRow, 13] = "Credits";

                                                    range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    range.EntireColumn.AutoFit();
                                                    range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                                                    range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                                    range = null;
                                                }
                                                else if (group.Combo.Equals(Combination.COMBO7))
                                                {
                                                    iCurrentRow = iCurrentRow + 2;

                                                    xlWorkSheet.Cells[iCurrentRow, 1] = string.Empty;
                                                    range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                                    range.Value2 = "3BAT,3ALL,4BOWL";
                                                    range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colSummary);
                                                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                                    range.Merge(misValue);
                                                    range = null;

                                                    iCurrentRow++;

                                                    xlWorkSheet.Cells[iCurrentRow, 1] = "TeamNo";
                                                    xlWorkSheet.Cells[iCurrentRow, 2] = "WK";
                                                    xlWorkSheet.Cells[iCurrentRow, 3] = "BAT1";
                                                    xlWorkSheet.Cells[iCurrentRow, 4] = "BAT2";
                                                    xlWorkSheet.Cells[iCurrentRow, 5] = "BAT3";
                                                    xlWorkSheet.Cells[iCurrentRow, 6] = "ALL1";
                                                    xlWorkSheet.Cells[iCurrentRow, 7] = "ALL2";
                                                    xlWorkSheet.Cells[iCurrentRow, 8] = "ALL3";
                                                    xlWorkSheet.Cells[iCurrentRow, 9] = "BOWL1";
                                                    xlWorkSheet.Cells[iCurrentRow, 10] = "BOWL2";
                                                    xlWorkSheet.Cells[iCurrentRow, 11] = "BOWL3";
                                                    xlWorkSheet.Cells[iCurrentRow, 12] = "BOWL4";
                                                    xlWorkSheet.Cells[iCurrentRow, 13] = "Credits";

                                                    range = xlWorkSheet.Rows.get_Range("A" + iCurrentRow.ToString(), "M" + iCurrentRow.ToString());
                                                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                                    range.EntireColumn.AutoFit();
                                                    range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                                                    range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                                    range = null;
                                                }

                                                #endregion
                                            }

                                            try
                                            {
                                                player = obj.team;

                                                iCurrentRow++;

                                                iTeamStart = (iTeamStart == 0 ? iCurrentRow : iTeamStart);

                                                xlWorkSheet.Cells[iCurrentRow, 1] = (iCurrentRow - iTeamStart) + 1; 
                                                xlWorkSheet.Cells[iCurrentRow, 2] = player.ElementAt(0).name;
                                                xlWorkSheet.Cells[iCurrentRow, 3] = player.ElementAt(1).name;
                                                xlWorkSheet.Cells[iCurrentRow, 4] = player.ElementAt(2).name;
                                                xlWorkSheet.Cells[iCurrentRow, 5] = player.ElementAt(3).name;
                                                xlWorkSheet.Cells[iCurrentRow, 6] = player.ElementAt(4).name;
                                                xlWorkSheet.Cells[iCurrentRow, 7] = player.ElementAt(5).name;
                                                xlWorkSheet.Cells[iCurrentRow, 8] = player.ElementAt(6).name;
                                                xlWorkSheet.Cells[iCurrentRow, 9] = player.ElementAt(7).name;
                                                xlWorkSheet.Cells[iCurrentRow, 10] = player.ElementAt(8).name;
                                                xlWorkSheet.Cells[iCurrentRow, 11] = player.ElementAt(9).name;
                                                xlWorkSheet.Cells[iCurrentRow, 12] = player.ElementAt(10).name;
                                                xlWorkSheet.Cells[iCurrentRow, 13] = obj.Credits;

                                                int gridIndex = (dgvCombination.Rows.Cast<DataGridViewRow>()
                                                .Where(r => r.Cells["dgvCombo"].Value.ToString() == obj.Combo.ToString())
                                                .Select(r => r.Index)).First();

                                                int rowIndex = Convert.ToInt32(dgvCombination.Rows[gridIndex].Cells["dgvRowID"].Value);

                                                xlWorkSheet.Cells[rowIndex, 4] = (iCurrentRow - iTeamStart) + 1;

                                                range = xlWorkSheet.Rows.get_Range("B" + iCurrentRow.ToString(), "L" + iCurrentRow.ToString());
                                                findRange = range.Find(c.name, misValue, Excel.XlFindLookIn.xlValues, misValue, misValue, Excel.XlSearchDirection.xlNext, false, false, misValue);
                                                findRange.Value2 = string.Format("{0}(c)", c.name);
                                                findRange.Interior.Color = System.Drawing.Color.LightGreen.ToArgb();
                                                range = null;
                                                findRange = null;

                                                range = xlWorkSheet.Rows.get_Range("B" + iCurrentRow.ToString(), "L" + iCurrentRow.ToString());
                                                findRange = range.Find(vc.name, misValue, Excel.XlFindLookIn.xlValues, misValue, misValue, Excel.XlSearchDirection.xlNext, false, false, misValue);
                                                findRange.Value2 = string.Format("{0}(vc)", vc.name);
                                                findRange.Interior.Color = System.Drawing.Color.LightBlue.ToArgb();
                                                range = null;
                                                findRange = null;

                                            }
                                            catch (Exception ex)
                                            {
                                                ExceptionHandler.HandleException(ex.ToString());
                                            }
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    ExceptionHandler.HandleException(ex.ToString());
                                }

                            }

                            if (iTeamStart > 0)
                            {
                                /* Border */
                                range = xlWorkSheet.Rows.get_Range("A" + iTeamStart.ToString(), "M" + iCurrentRow.ToString());
                                range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                range = null;

                                /* Column Alignment */
                                range = xlWorkSheet.Rows.get_Range("A" + iTeamStart.ToString(), "A" + iCurrentRow.ToString());
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                range = null;
                            }
                        }
                    }
                    
                    xlWorkSheet.Columns.EntireColumn.AutoFit();
                    
                    xlWorkBook.SaveAs(sExportFilePath, Excel.XlFileFormat.xlOpenXMLWorkbook, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                    xlWorkBook.Close(true, misValue, misValue);

                    e.Result = true;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex.ToString());
                }
                finally
                {
                    TryKillProcessByMainWindowHwnd(hWnd);
                }

                #endregion
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex.ToString());
            }
        }
        
        private void bgwGenerateTeam_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                tsProgress.Visible = false;
                tslblMessage.Visible = false;

                object result = e.Result;
                if (result != null && Convert.ToBoolean(result) == true)
                {
                    MessageBox.Show("Completed");
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex.ToString());
            }
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            try
            {
                if(new frmParse(UniqueID).ShowDialog() == DialogResult.OK)
                {
                    LoadPlayerDetail();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void LoadPlayerDetail()
        {
            try
            {
                DataTable dtPlayer = new DataTable();
                dtPlayer.Columns.Add("id", typeof(int));
                dtPlayer.Columns.Add("name", typeof(string));
                dtPlayer.Columns.Add("points", typeof(decimal));
                dtPlayer.Columns.Add("credits", typeof(decimal));
                dtPlayer.Columns.Add("player_type", typeof(string));
                dtPlayer.Columns.Add("team", typeof(string));
                DataRow drRow = null;

                var jsonObj = (dynamic)null;
                IEnumerable<dynamic> objResult = DBHandler.GetPlayerDetail(UniqueID);
                
                if (objResult != null && objResult.Count() > 0)
                {
                    foreach (var objData in objResult)
                    {
                        jsonObj = JsonConvert.DeserializeObject<RootObject>(objData.TeamData);
                    }

                    foreach (var obj in jsonObj.data.site.tour.match.players)
                    {
                        drRow = dtPlayer.NewRow();
                        drRow["id"] = obj.id;
                        drRow["name"] = obj.name;
                        drRow["points"] = obj.points;
                        drRow["credits"] = obj.credits;
                        drRow["player_type"] = obj.type.shortName;
                        drRow["team"] = obj.squad.shortName;

                        dtPlayer.Rows.Add(drRow);
                    }
                    
                    if (dtPlayer != null && dtPlayer.Rows.Count > 0)
                    {
                        dgvWK.AutoGenerateColumns = false;
                        DataView dvWK = new DataView(dtPlayer);
                        dvWK.RowFilter = "player_type = 'WK'";
                        dgvWK.DataSource = dvWK.ToTable();
                        this.dgvWK.Sort(this.dgvWK.Columns["WKPoints"], ListSortDirection.Descending);

                        dgvBAT.AutoGenerateColumns = false;
                        DataView dvBAT = new DataView(dtPlayer);
                        dvBAT.RowFilter = "player_type = 'BAT'";
                        dgvBAT.DataSource = dvBAT.ToTable();
                        this.dgvBAT.Sort(this.dgvBAT.Columns["BATPoints"], ListSortDirection.Descending);

                        dgvALL.AutoGenerateColumns = false;
                        DataView dvALL = new DataView(dtPlayer);
                        dvALL.RowFilter = "player_type = 'ALL'";
                        dgvALL.DataSource = dvALL.ToTable();
                        this.dgvALL.Sort(this.dgvALL.Columns["ALLPoints"], ListSortDirection.Descending);

                        dgvBOWL.AutoGenerateColumns = false;
                        DataView dvBOWL = new DataView(dtPlayer);
                        dvBOWL.RowFilter = "player_type = 'BOWL'";
                        dgvBOWL.DataSource = dvBOWL.ToTable();
                        this.dgvBOWL.Sort(this.dgvBOWL.Columns["BOWLPoints"], ListSortDirection.Descending);

                        rbTeamA.Text = this.TeamA;
                        rbTeamB.Text = this.TeamB;

                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void dgvWK_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvWK.CurrentCell is DataGridViewCheckBoxCell)
                    dgvWK.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            catch (Exception ex)
            {
                
            }
            
        }

        private void dgvWK_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dgvWK.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "WKSelected")
                {
                    if (Convert.ToBoolean(dgvWK.Rows[e.RowIndex].Cells["WKSelected"].Value))
                    {
                        dgvWK.Rows[e.RowIndex].Cells["WKMust"].ReadOnly = false;
                        dgvWK.Rows[e.RowIndex].Cells["WKVC"].ReadOnly = false;
                        dgvWK.Rows[e.RowIndex].Cells["WKC"].ReadOnly = false;
                        dgvWK.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                    }
                    else
                    {
                        dgvWK.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.White;

                        dgvWK.Rows[e.RowIndex].Cells["WKVC"].Value = false;
                        dgvWK.Rows[e.RowIndex].Cells["WKC"].Value = false;
                        dgvWK.Rows[e.RowIndex].Cells["WKMust"].Value = false;

                        dgvWK.Rows[e.RowIndex].Cells["WKVC"].ReadOnly = true;
                        dgvWK.Rows[e.RowIndex].Cells["WKC"].ReadOnly = true;
                        dgvWK.Rows[e.RowIndex].Cells["WKMust"].ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            
        }

        private void dgvBAT_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvBAT.CurrentCell is DataGridViewCheckBoxCell)
                    dgvBAT.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            catch (Exception ex)
            {

            }
        }

        private void dgvBAT_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dgvBAT.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "BATSelected")
                {
                    if (Convert.ToBoolean(dgvBAT.Rows[e.RowIndex].Cells["BATSelected"].Value))
                    {
                        dgvBAT.Rows[e.RowIndex].Cells["BATMust"].ReadOnly = false;
                        dgvBAT.Rows[e.RowIndex].Cells["BATVC"].ReadOnly = false;
                        dgvBAT.Rows[e.RowIndex].Cells["BATC"].ReadOnly = false;
                        dgvBAT.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                    }
                    else
                    {
                        dgvBAT.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.White;

                        dgvBAT.Rows[e.RowIndex].Cells["BATMust"].Value = false;
                        dgvBAT.Rows[e.RowIndex].Cells["BATVC"].Value = false;
                        dgvBAT.Rows[e.RowIndex].Cells["BATC"].Value = false;

                        dgvBAT.Rows[e.RowIndex].Cells["BATMust"].ReadOnly = true;
                        dgvBAT.Rows[e.RowIndex].Cells["BATVC"].ReadOnly = true;
                        dgvBAT.Rows[e.RowIndex].Cells["BATC"].ReadOnly = true;

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dgvALL_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvALL.CurrentCell is DataGridViewCheckBoxCell)
                    dgvALL.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            catch (Exception ex)
            {

            }
        }

        private void dgvALL_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dgvALL.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "ALLSelected")
                {
                    if (Convert.ToBoolean(dgvALL.Rows[e.RowIndex].Cells["ALLSelected"].Value))
                    {
                        dgvALL.Rows[e.RowIndex].Cells["ALLMust"].ReadOnly = false;
                        dgvALL.Rows[e.RowIndex].Cells["ALLVC"].ReadOnly = false;
                        dgvALL.Rows[e.RowIndex].Cells["ALLC"].ReadOnly = false;
                        dgvALL.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                    }
                    else
                    {
                        dgvALL.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.White;

                        dgvALL.Rows[e.RowIndex].Cells["ALLVC"].Value = false;
                        dgvALL.Rows[e.RowIndex].Cells["ALLC"].Value = false;
                        dgvALL.Rows[e.RowIndex].Cells["ALLMust"].Value = false;

                        dgvALL.Rows[e.RowIndex].Cells["ALLVC"].ReadOnly = true;
                        dgvALL.Rows[e.RowIndex].Cells["ALLC"].ReadOnly = true;
                        dgvALL.Rows[e.RowIndex].Cells["ALLMust"].ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dgvBOWL_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvBOWL.CurrentCell is DataGridViewCheckBoxCell)
                    dgvBOWL.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            catch (Exception ex)
            {

            }
        }

        private void dgvBOWL_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dgvBOWL.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name == "BOWLSelected")
                {
                    if (Convert.ToBoolean(dgvBOWL.Rows[e.RowIndex].Cells["BOWLSelected"].Value))
                    {
                        dgvBOWL.Rows[e.RowIndex].Cells["BOWLMust"].ReadOnly = false;
                        dgvBOWL.Rows[e.RowIndex].Cells["BOWLVC"].ReadOnly = false;
                        dgvBOWL.Rows[e.RowIndex].Cells["BOWLC"].ReadOnly = false;
                        dgvBOWL.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                    }
                    else
                    {
                        dgvBOWL.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.White;

                        dgvBOWL.Rows[e.RowIndex].Cells["BOWLVC"].Value = false;
                        dgvBOWL.Rows[e.RowIndex].Cells["BOWLC"].Value = false;
                        dgvBOWL.Rows[e.RowIndex].Cells["BOWLMust"].Value = false;

                        dgvBOWL.Rows[e.RowIndex].Cells["BOWLVC"].ReadOnly = true;
                        dgvBOWL.Rows[e.RowIndex].Cells["BOWLC"].ReadOnly = true;
                        dgvBOWL.Rows[e.RowIndex].Cells["BOWLMust"].ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dgvBAT_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvBAT.ClearSelection();
        }

        
    }
}
