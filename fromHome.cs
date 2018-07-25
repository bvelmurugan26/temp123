using Combinatorics.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace OperationXI
{
    public partial class frmHome : Form
    {
        DataTable dtTeams = new DataTable();
        DataTable dtPlayers = new DataTable();

        int iIndexSelected = 0;
        int iIndexTeamCode = 1;
        int iIndexPlayerName = 2;
        int iIndexCredits = 3;
        int iIndexMustPlayer = 4;
        int iIndexCaptain = 5;

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

        public static class TeamType
        {
            public static string KEY_PLAYERS = "KEY_PLAYERS";
            public static string VIP_PLAYERS = "VIP_PLAYERS";
        }

        public frmHome()
        {
            InitializeComponent();  
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            dgvWK.AutoGenerateColumns = false;

            LoadTeamType();
            GetPreLoadData();
        }

        private void LoadTeamType()
        {
            try
            {
                DataRow drRow = null;

                DataTable dtTeamType = new DataTable();
                dtTeamType.Columns.Add("TeamTypeID",typeof(int));
                dtTeamType.Columns.Add("TeamType", typeof(string));
                dtTeamType.Columns.Add("TeamTypeCode", typeof(string));

                drRow = dtTeamType.NewRow();
                drRow["TeamTypeID"] = 0;
                drRow["TeamType"] = "--Select--";
                drRow["TeamTypeCode"] = "SELECT";
                dtTeamType.Rows.Add(drRow);

                drRow = dtTeamType.NewRow();
                drRow["TeamTypeID"] = 1;
                drRow["TeamType"] = "KEY PLAYERS";
                drRow["TeamTypeCode"] = "KEY_PLAYERS";
                dtTeamType.Rows.Add(drRow);

                drRow = dtTeamType.NewRow();
                drRow["TeamTypeID"] = 2;
                drRow["TeamType"] = "VIP PLAYERS";
                drRow["TeamTypeCode"] = "VIP_PLAYERS";
                dtTeamType.Rows.Add(drRow);

                cboTeamTyoe.DisplayMember = "TeamType";
                cboTeamTyoe.ValueMember = "TeamTypeCode";
                cboTeamTyoe.DataSource = dtTeamType.DefaultView;


            }
            catch (Exception ex)
            {
                
            }
        }

        private void GetPreLoadData()
        {
            try
            {
                Tuple<IEnumerable<dynamic>, IEnumerable<dynamic>> objData = DBHandler.GetPreLoadData();
                if( objData != null)
                {
                    dtTeams = objData.Item1.ToDataTable();

                    if(dtTeams != null)
                    {
                        DataRow drRow = dtTeams.NewRow();
                        drRow["TeamName"] = "-- Select --";
                        drRow["TeamID"] = "0";
                        dtTeams.Rows.InsertAt(drRow, 0);

                        cboTeamA.DisplayMember = "TeamName";
                        cboTeamA.ValueMember = "TeamID";
                        cboTeamA.DataSource = dtTeams.DefaultView;
                    }

                    dtPlayers = objData.Item2.ToDataTable();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public void ShowMessage(string sMessage)
        {
            try
            {
                tslblMessage.Text = sMessage;
            }
            catch (Exception ex)
            {
                
            }
        }

        private Tuple<string,string,string> GetTeamAndCredits(string Role, string Tag)
        {
            try
            {
                string Player = string.Empty;
                string Team = string.Empty; 
                string Credits = string.Empty;

                if( Role.Equals("WK"))
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        var selected = from DataGridViewRow r in dgvWK.Rows
                                       where Convert.ToBoolean(r.Cells[iIndexSelected].Value) == true && r.Tag.ToString().Equals(Tag)
                                       select r;

                        foreach (var row in selected)
                        {
                            if (row.Tag.ToString().Equals(Tag))
                            {
                                Player = row.Cells[iIndexPlayerName].Value.ToString();
                                Team = row.Cells[iIndexTeamCode].Value.ToString();
                                Credits = row.Cells[iIndexCredits].Value.ToString();
                                break;
                            }
                        }
                    });
                }
                else if (Role.Equals("BAT"))
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        var selected = from DataGridViewRow r in dgvBAT.Rows
                                          where Convert.ToBoolean(r.Cells[iIndexSelected].Value) == true && r.Tag.ToString().Equals(Tag)
                                          select r;

                        foreach (var row in selected)
                        {
                            if(row.Tag.ToString().Equals(Tag))
                            {
                                Player = row.Cells[iIndexPlayerName].Value.ToString();
                                Team = row.Cells[iIndexTeamCode].Value.ToString();
                                Credits = row.Cells[iIndexCredits].Value.ToString();
                                break;
                            }
                        }
                    });
                    
                }
                else if (Role.Equals("ALL"))
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        var selected = from DataGridViewRow r in dgvALL.Rows
                                          where Convert.ToBoolean(r.Cells[iIndexSelected].Value) == true && r.Tag.ToString().Equals(Tag)
                                          select r;

                        foreach (var row in selected)
                        {
                            if (row.Tag.ToString().Equals(Tag))
                            {
                                Player = row.Cells[iIndexPlayerName].Value.ToString();
                                Team = row.Cells[iIndexTeamCode].Value.ToString();
                                Credits = row.Cells[iIndexCredits].Value.ToString();
                                break;
                            }
                        }
                    });

                }
                else if (Role.Equals("BOWL"))
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        var selected = from DataGridViewRow r in dgvBOWL.Rows
                                       where Convert.ToBoolean(r.Cells[iIndexSelected].Value) == true && r.Tag.ToString().Equals(Tag)
                                       select r;

                        foreach (var row in selected)
                        {
                            if (row.Tag.ToString().Equals(Tag))
                            {
                                Player = row.Cells[iIndexPlayerName].Value.ToString();
                                Team = row.Cells[iIndexTeamCode].Value.ToString();
                                Credits = row.Cells[iIndexCredits].Value.ToString();
                                break;
                            }
                        }
                    });

                }
                return Tuple.Create(Player,Team, Credits);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items, int count)
        {
            int i = 0;
            foreach (var item in items)
            {   
                if (count == 1)
                    yield return new T[] { item };
                else
                {
                    foreach (var result in GetPermutations(items.Skip(i + 1), count - 1))
                        yield return new T[] { item }.Concat(result);
                }

                ++i;
            }
        }

        private void cboTeamA_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboTeamA.SelectedIndex > 0 && (dtTeams != null && dtTeams.Rows.Count > 0))
                {
                    int TeamID = Convert.ToInt32(cboTeamA.SelectedValue);

                    DataView dvTeam = new DataView(dtTeams);
                    dvTeam.RowFilter = "TeamID <> " + TeamID + " ";

                    cboTeamB.DisplayMember = "TeamName";
                    cboTeamB.ValueMember = "TeamID";
                    cboTeamB.DataSource = dvTeam;

                    cboTeamB.SelectedIndex = 0;

                }
                else
                {
                    DataTable dtTemp = new DataTable();
                    dtTemp.Columns.Add("TeamName", typeof(string));
                    dtTemp.Columns.Add("TeamID", typeof(int));

                    DataRow drRow = dtTemp.NewRow();
                    drRow["TeamName"] = "-- Select --";
                    drRow["TeamID"] = "0";
                    dtTemp.Rows.InsertAt(drRow, 0);

                    cboTeamB.DisplayMember = "TeamName";
                    cboTeamB.ValueMember = "TeamID";
                    cboTeamB.DataSource = dtTemp;
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void cboTeamB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                if ((cboTeamA.SelectedIndex > 0 && cboTeamB.SelectedIndex > 0) && (dtPlayers != null && dtPlayers.Rows.Count > 0))
                {
                    DataView dvTeam = new DataView(dtPlayers);
                    int TeamAID = Convert.ToInt32(cboTeamA.SelectedValue);
                    int TeamBID = Convert.ToInt32(cboTeamB.SelectedValue);
                    dvTeam.RowFilter = "TeamID =" + TeamAID + " OR TeamID =" + TeamBID + " ";
                    DataTable dtTempPlayer = dvTeam.ToTable(true, "TeamCode","RoleCode", "PlayerName", "Credits");

                    foreach (DataRow drRow in dtTempPlayer.Rows)
                    {
                        if (drRow["RoleCode"].ToString().Equals("WK"))
                        {
                            DataGridViewRow row = (DataGridViewRow)dgvWK.RowTemplate.Clone();
                            row.CreateCells(dgvWK);
                            row.Cells[iIndexSelected].Value = false;
                            row.Cells[iIndexTeamCode].Value = drRow["TeamCode"].ToString();
                            row.Cells[iIndexPlayerName].Value = drRow["PlayerName"].ToString();
                            row.Cells[iIndexCredits].Value = drRow["Credits"].ToString();
                            row.Cells[iIndexMustPlayer].Value = false;
                            row.Tag = string.Format("{0}_{1}", drRow["TeamCode"].ToString(), drRow["PlayerName"].ToString());
                            dgvWK.Rows.Add(row);
                        }
                        else if (drRow["RoleCode"].ToString().Equals("BAT"))
                        {
                            DataGridViewRow row = (DataGridViewRow)dgvBAT.RowTemplate.Clone();
                            row.CreateCells(dgvBAT);
                            row.Cells[iIndexSelected].Value = false;
                            row.Cells[iIndexTeamCode].Value = drRow["TeamCode"].ToString();
                            row.Cells[iIndexPlayerName].Value = drRow["PlayerName"].ToString();
                            row.Cells[iIndexCredits].Value = drRow["Credits"].ToString();
                            row.Cells[iIndexMustPlayer].Value = false;
                            row.Tag = string.Format("{0}_{1}", drRow["TeamCode"].ToString(), drRow["PlayerName"].ToString());
                            dgvBAT.Rows.Add(row);
                        }
                        else if (drRow["RoleCode"].ToString().Equals("ALL"))
                        {
                            DataGridViewRow row = (DataGridViewRow)dgvALL.RowTemplate.Clone();
                            row.CreateCells(dgvALL);
                            row.Cells[iIndexSelected].Value = false;
                            row.Cells[iIndexTeamCode].Value = drRow["TeamCode"].ToString();
                            row.Cells[iIndexPlayerName].Value = drRow["PlayerName"].ToString();
                            row.Cells[iIndexCredits].Value = drRow["Credits"].ToString();
                            row.Cells[iIndexMustPlayer].Value = false;
                            row.Tag = string.Format("{0}_{1}", drRow["TeamCode"].ToString(), drRow["PlayerName"].ToString());
                            dgvALL.Rows.Add(row);
                        }
                        else if (drRow["RoleCode"].ToString().Equals("BOWL"))
                        {
                            DataGridViewRow row = (DataGridViewRow)dgvBOWL.RowTemplate.Clone();
                            row.CreateCells(dgvBOWL);
                            row.Cells[iIndexSelected].Value = false;
                            row.Cells[iIndexTeamCode].Value = drRow["TeamCode"].ToString();
                            row.Cells[iIndexPlayerName].Value = drRow["PlayerName"].ToString();
                            row.Cells[iIndexCredits].Value = drRow["Credits"].ToString();
                            row.Cells[iIndexMustPlayer].Value = false;
                            row.Tag = string.Format("{0}_{1}", drRow["TeamCode"].ToString(), drRow["PlayerName"].ToString());
                            dgvBOWL.Rows.Add(row); ;
                        }

                    }

                }
                
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        private void tsmiTeam_Click(object sender, EventArgs e)
        {
            try
            {
                new frmTeam().ShowDialog();
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                GetPreLoadData();
            }
            
        }

        private void dgvWK_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dgvWK_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                
                if (e.ColumnIndex == iIndexSelected && e.RowIndex >= 0)
                {
                    this.dgvWK.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    if ((bool)this.dgvWK.CurrentCell.Value == true)
                    {
                        this.dgvWK.Rows[e.RowIndex].Cells[iIndexMustPlayer].ReadOnly = false;
                        this.dgvWK.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        this.dgvWK.Rows[e.RowIndex].Cells[iIndexMustPlayer].Value = false;
                        this.dgvWK.Rows[e.RowIndex].Cells[iIndexMustPlayer].ReadOnly = true;
                        this.dgvWK.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    }

                    this.dgvWK.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
                
            }
            catch (Exception ex)
            {

            }
        }

        private void dgvBAT_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.ColumnIndex == iIndexSelected && e.RowIndex >= 0)
                {
                    this.dgvBAT.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    if ((bool)this.dgvBAT.CurrentCell.Value == true)
                    {
                        this.dgvBAT.Rows[e.RowIndex].Cells[iIndexMustPlayer].ReadOnly = false;
                        this.dgvBAT.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        this.dgvBAT.Rows[e.RowIndex].Cells[iIndexMustPlayer].Value = false;
                        this.dgvBAT.Rows[e.RowIndex].Cells[iIndexMustPlayer].ReadOnly = true;
                        this.dgvBAT.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    }

                    this.dgvBAT.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void dgvALL_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.ColumnIndex == iIndexSelected && e.RowIndex >= 0)
                {
                    this.dgvALL.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    if ((bool)this.dgvALL.CurrentCell.Value == true)
                    {
                        this.dgvALL.Rows[e.RowIndex].Cells[iIndexMustPlayer].ReadOnly = false;
                        this.dgvALL.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        this.dgvALL.Rows[e.RowIndex].Cells[iIndexMustPlayer].Value = false;
                        this.dgvALL.Rows[e.RowIndex].Cells[iIndexMustPlayer].ReadOnly = true;
                        this.dgvALL.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    }

                    this.dgvALL.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void dgvBOWL_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.ColumnIndex == iIndexSelected && e.RowIndex >= 0)
                {
                    this.dgvBOWL.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    if ((bool)this.dgvBOWL.CurrentCell.Value == true)
                    {
                        this.dgvBOWL.Rows[e.RowIndex].Cells[iIndexMustPlayer].ReadOnly = false;
                        this.dgvBOWL.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        this.dgvBOWL.Rows[e.RowIndex].Cells[iIndexMustPlayer].Value = false;
                        this.dgvBOWL.Rows[e.RowIndex].Cells[iIndexMustPlayer].ReadOnly = true;
                        this.dgvBOWL.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    }

                    this.dgvBOWL.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }

            }
            catch (Exception ex)
            {

            }
        }


        private void btnGenerateTeam_Click(object sender, EventArgs e)
        {
            try
            {

                #region "Common Validation"

                if (cboTeamA.SelectedIndex == 0)
                {
                    cboTeamA.Focus();
                    MessageBox.Show("Please choose TeamA");
                    return;
                }

                if (cboTeamB.SelectedIndex == 0)
                {
                    cboTeamB.Focus();
                    MessageBox.Show("Please choose TeamB");
                    return;
                }

                if (cboTeamTyoe.SelectedIndex == 0)
                {
                    cboTeamTyoe.Focus();
                    MessageBox.Show("Please choose Team Type");
                    return;
                }

                var selectedWK = from DataGridViewRow r in dgvWK.Rows
                                 where Convert.ToBoolean(r.Cells[iIndexSelected].Value) == true
                                 select r;

                if (selectedWK.Count() == 0)
                {
                    tabControlPlayer.SelectTab("tabWK");
                    MessageBox.Show("Every team needs atleast 1 Wicket-Keeper");
                    return;
                }

                var selectedBAT = from DataGridViewRow r in dgvBAT.Rows
                                  where Convert.ToBoolean(r.Cells[iIndexSelected].Value) == true
                                  select r;

                if (selectedBAT.Count() < 3)
                {
                    tabControlPlayer.SelectTab("tabBAT");
                    MessageBox.Show("Every team needs atleast 3 Batsman");
                    return;
                }

                var selectedALL = from DataGridViewRow r in dgvALL.Rows
                                  where Convert.ToBoolean(r.Cells[iIndexSelected].Value) == true
                                  select r;

                if (selectedALL.Count() < 1)
                {
                    tabControlPlayer.SelectTab("tabALL");
                    MessageBox.Show("Every team needs atleast 1 All-Rounder");
                    return;
                }

                var selectedBOWL = from DataGridViewRow r in dgvBOWL.Rows
                                   where Convert.ToBoolean(r.Cells[iIndexSelected].Value) == true
                                   select r;

                if (selectedBOWL.Count() < 3)
                {
                    tabControlPlayer.SelectTab("tabBOWL");
                    MessageBox.Show("Every team needs atleast 3 Bowlers");
                    return;
                }

                #endregion

                string sTeamType = cboTeamTyoe.SelectedValue.ToString();

                if (sTeamType.Equals(TeamType.KEY_PLAYERS) || sTeamType.Equals(TeamType.VIP_PLAYERS))
                {
                    var selectedWKC = from DataGridViewRow r in dgvWK.Rows
                                      where Convert.ToBoolean(r.Cells[iIndexCaptain].Value) == true
                                      select r;

                    var selectedBATC = from DataGridViewRow r in dgvBAT.Rows
                                       where Convert.ToBoolean(r.Cells[iIndexCaptain].Value) == true
                                       select r;

                    var selectedALLC = from DataGridViewRow r in dgvALL.Rows
                                       where Convert.ToBoolean(r.Cells[iIndexCaptain].Value) == true
                                       select r;

                    var selectedBOWLC = from DataGridViewRow r in dgvBOWL.Rows
                                        where Convert.ToBoolean(r.Cells[iIndexCaptain].Value) == true
                                        select r;

                    if ((selectedWKC.Count() + selectedBATC.Count() + selectedALLC.Count() + selectedBOWLC.Count()) == 0)
                    {
                        tabControlPlayer.SelectTab("tabBAT");
                        MessageBox.Show("Please choose captain(s) for your team..!");
                        return;
                    }
                }

                string TeamA = string.Empty;
                string TeamB = string.Empty;

                DataRow drRowTeamA = ((DataRowView)cboTeamA.SelectedItem).Row;
                TeamA = drRowTeamA["TeamCode"].ToString();

                DataRow drRowTeamB = ((DataRowView)cboTeamB.SelectedItem).Row;
                TeamB = drRowTeamB["TeamCode"].ToString();

                object oExportFilePath = null;
                SaveFileDialog objSFD = new SaveFileDialog();
                objSFD.FileName = string.Format("OperationXI({0}_{1})", TeamA, TeamB);
                objSFD.Filter = "Excel Documents (.xlsx)|*.xlsx";

                if (objSFD.ShowDialog() == DialogResult.OK)
                {
                    oExportFilePath = objSFD.FileName;
                }

                tsProgress.Visible = true;
                tslblMessage.Visible = true;
                tslblMessage.Text = string.Empty;
                tsProgress.Style = ProgressBarStyle.Marquee;

                object[] parameters = new object[] { oExportFilePath, TeamA, TeamB, sTeamType };
                ShowMessage("Generating team please wailt..");

                if (File.Exists(oExportFilePath.ToString()))
                {
                    File.Delete(oExportFilePath.ToString());
                }

                bgwGenerateTeam.RunWorkerAsync(parameters);

            }
            catch (Exception ex)
            {

            }
        }

        private void bgwGenerateTeam_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] parameters = e.Argument as object[];
                string sExportFilePath = parameters[0].ToString();
                string TeamA = parameters[1].ToString();
                string TeamB = parameters[2].ToString();
                string sTeamType = parameters[3].ToString();

                #region "Variables"

                Tuple<string, string, string> objData = null;

                List<string> lstWicketKeeper = new List<string>();
                List<string> lstBatsman = new List<string>();
                List<string> lstAllRounder = new List<string>();
                List<string> lstBowler = new List<string>();

                List<string> lstWicketKeeper_key = new List<string>();
                List<string> lstBatsman_key = new List<string>();
                List<string> lstAllRounder_key = new List<string>();
                List<string> lstBowler_key = new List<string>();

                string sCaptainTag = string.Empty;
                string sViceCaptainTag = string.Empty;

                string sCaptainName = string.Empty;
                string sViceCaptainName = string.Empty;

                string sCaptainRole = string.Empty;
                string sViceCaptainRole = string.Empty;

                string sWicketKeeper = string.Empty;
                string sWicketKeeper_Team = string.Empty;
                string sWicketKeeper_Credits = string.Empty;

                DataTable dtTeam = new DataTable();
                dtTeam.Columns.Add("C", typeof(string));
                dtTeam.Columns.Add("VC", typeof(string));
                dtTeam.Columns.Add("WK", typeof(string));
                dtTeam.Columns.Add("BAT1", typeof(string));
                dtTeam.Columns.Add("BAT2", typeof(string));
                dtTeam.Columns.Add("BAT3", typeof(string));
                dtTeam.Columns.Add("BAT4", typeof(string));
                dtTeam.Columns.Add("BAT5", typeof(string));
                dtTeam.Columns.Add("ALL1", typeof(string));
                dtTeam.Columns.Add("ALL2", typeof(string));
                dtTeam.Columns.Add("ALL3", typeof(string));
                dtTeam.Columns.Add("BOWL1", typeof(string));
                dtTeam.Columns.Add("BOWL2", typeof(string));
                dtTeam.Columns.Add("BOWL3", typeof(string));
                dtTeam.Columns.Add("BOWL4", typeof(string));
                dtTeam.Columns.Add("BOWL5", typeof(string));
                dtTeam.Columns.Add("WKT", typeof(string));
                dtTeam.Columns.Add("BAT1T", typeof(string));
                dtTeam.Columns.Add("BAT2T", typeof(string));
                dtTeam.Columns.Add("BAT3T", typeof(string));
                dtTeam.Columns.Add("BAT4T", typeof(string));
                dtTeam.Columns.Add("BAT5T", typeof(string));
                dtTeam.Columns.Add("ALL1T", typeof(string));
                dtTeam.Columns.Add("ALL2T", typeof(string));
                dtTeam.Columns.Add("ALL3T", typeof(string));
                dtTeam.Columns.Add("BOWL1T", typeof(string));
                dtTeam.Columns.Add("BOWL2T", typeof(string));
                dtTeam.Columns.Add("BOWL3T", typeof(string));
                dtTeam.Columns.Add("BOWL4T", typeof(string));
                dtTeam.Columns.Add("BOWL5T", typeof(string));
                dtTeam.Columns.Add("WKC", typeof(string));
                dtTeam.Columns.Add("BAT1C", typeof(string));
                dtTeam.Columns.Add("BAT2C", typeof(string));
                dtTeam.Columns.Add("BAT3C", typeof(string));
                dtTeam.Columns.Add("BAT4C", typeof(string));
                dtTeam.Columns.Add("BAT5C", typeof(string));
                dtTeam.Columns.Add("ALL1C", typeof(string));
                dtTeam.Columns.Add("ALL2C", typeof(string));
                dtTeam.Columns.Add("ALL3C", typeof(string));
                dtTeam.Columns.Add("BOWL1C", typeof(string));
                dtTeam.Columns.Add("BOWL2C", typeof(string));
                dtTeam.Columns.Add("BOWL3C", typeof(string));
                dtTeam.Columns.Add("BOWL4C", typeof(string));
                dtTeam.Columns.Add("BOWL5C", typeof(string));

                #endregion

                #region "Captain & Player"

                DataTable dtSelectedPlayer = new DataTable();
                dtSelectedPlayer.Columns.Add("PlayerID", typeof(int));
                dtSelectedPlayer.Columns.Add("Player", typeof(string));
                dtSelectedPlayer.Columns.Add("Role", typeof(string));
                dtSelectedPlayer.Columns.Add("Tag", typeof(string));
                dtSelectedPlayer.Columns.Add("IsCaptain", typeof(bool));

                this.Invoke((MethodInvoker)delegate
                {
                    #region "WK"

                    var selectedWK = from DataGridViewRow r in dgvWK.Rows
                                     where Convert.ToBoolean(r.Cells[iIndexSelected].Value) == true
                                     select r;

                    foreach (var row in selectedWK)
                    {
                        lstWicketKeeper.Add(row.Tag.ToString());

                        DataRow drRow = dtSelectedPlayer.NewRow();
                        drRow["PlayerID"] = dtSelectedPlayer.Rows.Count.ToString();
                        drRow["Player"] = row.Cells[iIndexPlayerName].Value.ToString();
                        drRow["Role"] = "WK";
                        drRow["Tag"] = row.Tag.ToString();
                        drRow["IsCaptain"] = Convert.ToBoolean(row.Cells[iIndexCaptain].Value);
                        dtSelectedPlayer.Rows.Add(drRow);

                        if (Convert.ToBoolean(row.Cells[iIndexMustPlayer].Value))
                        {
                            lstWicketKeeper_key.Add(row.Tag.ToString());
                        }

                        sWicketKeeper = row.Cells[iIndexPlayerName].Value.ToString();
                        sWicketKeeper_Team = row.Cells[iIndexTeamCode].Value.ToString();
                        sWicketKeeper_Credits = row.Cells[iIndexCredits].Value.ToString();

                    }

                    #endregion

                    #region "BAT"

                    var selectedBAT = from DataGridViewRow r in dgvBAT.Rows
                                      where Convert.ToBoolean(r.Cells[iIndexSelected].Value) == true
                                      select r;

                    foreach (var row in selectedBAT)
                    {
                        lstBatsman.Add(row.Tag.ToString());

                        DataRow drRow = dtSelectedPlayer.NewRow();
                        drRow["PlayerID"] = dtSelectedPlayer.Rows.Count.ToString();
                        drRow["Player"] = row.Cells[iIndexPlayerName].Value.ToString();
                        drRow["Role"] = "BAT";
                        drRow["Tag"] = row.Tag.ToString();
                        drRow["IsCaptain"] = Convert.ToBoolean(row.Cells[iIndexCaptain].Value);
                        dtSelectedPlayer.Rows.Add(drRow);

                        if (Convert.ToBoolean(row.Cells[iIndexMustPlayer].Value))
                        {
                            lstBatsman_key.Add(row.Tag.ToString());
                        }
                    }

                    #endregion

                    #region "ALL"

                    var selectedALL = from DataGridViewRow r in dgvALL.Rows
                                      where Convert.ToBoolean(r.Cells[iIndexSelected].Value) == true
                                      select r;

                    foreach (var row in selectedALL)
                    {
                        lstAllRounder.Add(row.Tag.ToString());

                        DataRow drRow = dtSelectedPlayer.NewRow();
                        drRow["PlayerID"] = dtSelectedPlayer.Rows.Count.ToString();
                        drRow["Player"] = row.Cells[iIndexPlayerName].Value.ToString();
                        drRow["Role"] = "ALL";
                        drRow["Tag"] = row.Tag.ToString();
                        drRow["IsCaptain"] = Convert.ToBoolean(row.Cells[iIndexCaptain].Value);
                        dtSelectedPlayer.Rows.Add(drRow);

                        if (Convert.ToBoolean(row.Cells[iIndexMustPlayer].Value))
                        {
                            lstAllRounder_key.Add(row.Tag.ToString());
                        }
                    }

                    #endregion

                    #region "BOWL"

                    var selectedBOWL = from DataGridViewRow r in dgvBOWL.Rows
                                       where Convert.ToBoolean(r.Cells[iIndexSelected].Value) == true
                                       select r;

                    foreach (var row in selectedBOWL)
                    {
                        lstBowler.Add(row.Tag.ToString());

                        DataRow drRow = dtSelectedPlayer.NewRow();
                        drRow["PlayerID"] = dtSelectedPlayer.Rows.Count.ToString();
                        drRow["Player"] = row.Cells[iIndexPlayerName].Value.ToString();
                        drRow["Role"] = "BOWL";
                        drRow["Tag"] = row.Tag.ToString();
                        drRow["IsCaptain"] = Convert.ToBoolean(row.Cells[iIndexCaptain].Value);
                        dtSelectedPlayer.Rows.Add(drRow);

                        if (Convert.ToBoolean(row.Cells[iIndexMustPlayer].Value))
                        {
                            lstBowler_key.Add(row.Tag.ToString());
                        }
                    }

                    #endregion

                });

                #endregion

                if (sTeamType.Equals(TeamType.KEY_PLAYERS))
                {
                    #region "Key Players"

                    DataView dvCaptain = new DataView(dtSelectedPlayer);
                    dvCaptain.RowFilter = "IsCaptain = true";
                    DataTable dtTempCaptain = dvCaptain.ToTable(true, "Player", "Tag", "Role");

                    if (lstWicketKeeper.Count >= 1)
                    {
                        Combinations<string> combi1WK = new Combinations<string>(lstWicketKeeper, 1);
                        foreach (IList<string> perm1WK in combi1WK)
                        {
                            #region "3 BAT"

                            if (lstBatsman.Count >= 3)
                            {
                                Combinations<string> combi3BAT = new Combinations<string>(lstBatsman, 3);
                                foreach (IList<string> perm3BAT in combi3BAT)
                                {
                                    if (lstAllRounder.Count >= 3)
                                    {
                                        #region "3 ALL"

                                        Combinations<string> combi3ALL = new Combinations<string>(lstAllRounder, 3);
                                        foreach (IList<string> perm3ALL in combi3ALL)
                                        {
                                            if (lstBowler.Count >= 4)
                                            {
                                                #region "4 BOWL"

                                                Combinations<string> combi4BOWL = new Combinations<string>(lstBowler, 4);
                                                foreach (IList<string> perm4BOWL in combi4BOWL)
                                                {

                                                    var NotMatchingBAT = from i in lstBatsman_key
                                                                         where !perm3BAT.Contains(i)
                                                                         select i;

                                                    var NotMatchingALL = from i in lstAllRounder_key
                                                                         where !perm3ALL.Contains(i)
                                                                         select i;

                                                    var NotMatchingBOWL = from i in lstBowler_key
                                                                          where !perm4BOWL.Contains(i)
                                                                          select i;

                                                    if (NotMatchingBAT.Count() == 0 && NotMatchingALL.Count() == 0 && NotMatchingBOWL.Count() == 0)
                                                    {
                                                        DataRow drTeam = dtTeam.NewRow();
                                                        drTeam["C"] = sCaptainName;
                                                        drTeam["VC"] = sViceCaptainName;

                                                        objData = GetTeamAndCredits("WK", perm1WK.ElementAt(0));
                                                        drTeam["WK"] = objData.Item1;
                                                        drTeam["WKT"] = objData.Item2;
                                                        drTeam["WKC"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm3BAT.ElementAt(0));
                                                        drTeam["BAT1"] = objData.Item1;
                                                        drTeam["BAT1T"] = objData.Item2;
                                                        drTeam["BAT1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm3BAT.ElementAt(1));
                                                        drTeam["BAT2"] = objData.Item1;
                                                        drTeam["BAT2T"] = objData.Item2;
                                                        drTeam["BAT2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm3BAT.ElementAt(2));
                                                        drTeam["BAT3"] = objData.Item1;
                                                        drTeam["BAT3T"] = objData.Item2;
                                                        drTeam["BAT3C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("ALL", perm3ALL.ElementAt(0));
                                                        drTeam["ALL1"] = objData.Item1;
                                                        drTeam["ALL1T"] = objData.Item2;
                                                        drTeam["ALL1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("ALL", perm3ALL.ElementAt(1));
                                                        drTeam["ALL2"] = objData.Item1;
                                                        drTeam["ALL2T"] = objData.Item2;
                                                        drTeam["ALL2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("ALL", perm3ALL.ElementAt(2));
                                                        drTeam["ALL3"] = objData.Item1;
                                                        drTeam["ALL3T"] = objData.Item2;
                                                        drTeam["ALL3C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(0));
                                                        drTeam["BOWL1"] = objData.Item1;
                                                        drTeam["BOWL1T"] = objData.Item2;
                                                        drTeam["BOWL1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(1));
                                                        drTeam["BOWL2"] = objData.Item1;
                                                        drTeam["BOWL2T"] = objData.Item2;
                                                        drTeam["BOWL2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(2));
                                                        drTeam["BOWL3"] = objData.Item1;
                                                        drTeam["BOWL3T"] = objData.Item2;
                                                        drTeam["BOWL3C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(3));
                                                        drTeam["BOWL4"] = objData.Item1;
                                                        drTeam["BOWL4T"] = objData.Item2;
                                                        drTeam["BOWL4C"] = objData.Item3;

                                                        dtTeam.Rows.Add(drTeam);
                                                    }
                                                }

                                                #endregion
                                            }

                                        }

                                        #endregion
                                    }

                                    if (lstAllRounder.Count >= 2)
                                    {
                                        #region "2 ALL"

                                        Combinations<string> combi2ALL = new Combinations<string>(lstAllRounder, 2);
                                        foreach (IList<string> perm2ALL in combi2ALL)
                                        {
                                            if (lstBowler.Count >= 5)
                                            {
                                                #region "5 BOWL"

                                                Combinations<string> combi5BOWL = new Combinations<string>(lstBowler, 5);
                                                foreach (IList<string> perm5BOWL in combi5BOWL)
                                                {
                                                    var NotMatchingBAT = from i in lstBatsman_key
                                                                         where !perm3BAT.Contains(i)
                                                                         select i;

                                                    var NotMatchingALL = from i in lstAllRounder_key
                                                                         where !perm2ALL.Contains(i)
                                                                         select i;

                                                    var NotMatchingBOWL = from i in lstBowler_key
                                                                          where !perm5BOWL.Contains(i)
                                                                          select i;

                                                    if (NotMatchingBAT.Count() == 0 && NotMatchingALL.Count() == 0 && NotMatchingBOWL.Count() == 0)
                                                    {
                                                        DataRow drTeam = dtTeam.NewRow();
                                                        drTeam["C"] = sCaptainName;
                                                        drTeam["VC"] = sViceCaptainName;

                                                        objData = GetTeamAndCredits("WK", perm1WK.ElementAt(0));
                                                        drTeam["WK"] = objData.Item1;
                                                        drTeam["WKT"] = objData.Item2;
                                                        drTeam["WKC"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm3BAT.ElementAt(0));
                                                        drTeam["BAT1"] = objData.Item1;
                                                        drTeam["BAT1T"] = objData.Item2;
                                                        drTeam["BAT1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm3BAT.ElementAt(1));
                                                        drTeam["BAT2"] = objData.Item1;
                                                        drTeam["BAT2T"] = objData.Item2;
                                                        drTeam["BAT2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm3BAT.ElementAt(2));
                                                        drTeam["BAT3"] = objData.Item1;
                                                        drTeam["BAT3T"] = objData.Item2;
                                                        drTeam["BAT3C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("ALL", perm2ALL.ElementAt(0));
                                                        drTeam["ALL1"] = objData.Item1;
                                                        drTeam["ALL1T"] = objData.Item2;
                                                        drTeam["ALL1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("ALL", perm2ALL.ElementAt(1));
                                                        drTeam["ALL2"] = objData.Item1;
                                                        drTeam["ALL2T"] = objData.Item2;
                                                        drTeam["ALL2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(0));
                                                        drTeam["BOWL1"] = objData.Item1;
                                                        drTeam["BOWL1T"] = objData.Item2;
                                                        drTeam["BOWL1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(1));
                                                        drTeam["BOWL2"] = objData.Item1;
                                                        drTeam["BOWL2T"] = objData.Item2;
                                                        drTeam["BOWL2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(2));
                                                        drTeam["BOWL3"] = objData.Item1;
                                                        drTeam["BOWL3T"] = objData.Item2;
                                                        drTeam["BOWL3C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(3));
                                                        drTeam["BOWL4"] = objData.Item1;
                                                        drTeam["BOWL4T"] = objData.Item2;
                                                        drTeam["BOWL4C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(4));
                                                        drTeam["BOWL5"] = objData.Item1;
                                                        drTeam["BOWL5T"] = objData.Item2;
                                                        drTeam["BOWL5C"] = objData.Item3;

                                                        dtTeam.Rows.Add(drTeam);
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

                            #region "4 BAT"

                            if (lstBatsman.Count >= 4)
                            {
                                Combinations<string> combi4BAT = new Combinations<string>(lstBatsman, 4);
                                foreach (IList<string> perm4BAT in combi4BAT)
                                {
                                    #region "3 ALL"

                                    if (lstAllRounder.Count >= 3)
                                    {
                                        Combinations<string> combi3ALL = new Combinations<string>(lstAllRounder, 3);
                                        foreach (IList<string> perm3ALL in combi3ALL)
                                        {
                                            if (lstBowler.Count >= 3)
                                            {
                                                #region "3 BOWL"
                                                Combinations<string> combi3BOWL = new Combinations<string>(lstBowler, 3);
                                                foreach (IList<string> perm3BOWL in combi3BOWL)
                                                {
                                                    var NotMatchingBAT = from i in lstBatsman_key
                                                                         where !perm4BAT.Contains(i)
                                                                         select i;

                                                    var NotMatchingALL = from i in lstAllRounder_key
                                                                         where !perm3ALL.Contains(i)
                                                                         select i;

                                                    var NotMatchingBOWL = from i in lstBowler_key
                                                                          where !perm3BOWL.Contains(i)
                                                                          select i;

                                                    if (NotMatchingBAT.Count() == 0 && NotMatchingALL.Count() == 0 && NotMatchingBOWL.Count() == 0)
                                                    {
                                                        DataRow drTeam = dtTeam.NewRow();
                                                        drTeam["C"] = sCaptainName;
                                                        drTeam["VC"] = sViceCaptainName;

                                                        objData = GetTeamAndCredits("WK", perm1WK.ElementAt(0));
                                                        drTeam["WK"] = objData.Item1;
                                                        drTeam["WKT"] = objData.Item2;
                                                        drTeam["WKC"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(0));
                                                        drTeam["BAT1"] = objData.Item1;
                                                        drTeam["BAT1T"] = objData.Item2;
                                                        drTeam["BAT1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(1));
                                                        drTeam["BAT2"] = objData.Item1;
                                                        drTeam["BAT2T"] = objData.Item2;
                                                        drTeam["BAT2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(2));
                                                        drTeam["BAT3"] = objData.Item1;
                                                        drTeam["BAT3T"] = objData.Item2;
                                                        drTeam["BAT3C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(3));
                                                        drTeam["BAT4"] = objData.Item1;
                                                        drTeam["BAT4T"] = objData.Item2;
                                                        drTeam["BAT4C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("ALL", perm3ALL.ElementAt(0));
                                                        drTeam["ALL1"] = objData.Item1;
                                                        drTeam["ALL1T"] = objData.Item2;
                                                        drTeam["ALL1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("ALL", perm3ALL.ElementAt(1));
                                                        drTeam["ALL2"] = objData.Item1;
                                                        drTeam["ALL2T"] = objData.Item2;
                                                        drTeam["ALL2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("ALL", perm3ALL.ElementAt(2));
                                                        drTeam["ALL3"] = objData.Item1;
                                                        drTeam["ALL3T"] = objData.Item2;
                                                        drTeam["ALL3C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm3BOWL.ElementAt(0));
                                                        drTeam["BOWL1"] = objData.Item1;
                                                        drTeam["BOWL1T"] = objData.Item2;
                                                        drTeam["BOWL1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm3BOWL.ElementAt(1));
                                                        drTeam["BOWL2"] = objData.Item1;
                                                        drTeam["BOWL2T"] = objData.Item2;
                                                        drTeam["BOWL2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm3BOWL.ElementAt(2));
                                                        drTeam["BOWL3"] = objData.Item1;
                                                        drTeam["BOWL3T"] = objData.Item2;
                                                        drTeam["BOWL3C"] = objData.Item3;

                                                        dtTeam.Rows.Add(drTeam);
                                                    }
                                                }

                                                #endregion
                                            }

                                        }
                                    }

                                    #endregion

                                    #region "2 ALL"

                                    if (lstAllRounder.Count >= 2)
                                    {
                                        Combinations<string> combi2ALL = new Combinations<string>(lstAllRounder, 2);
                                        foreach (IList<string> perm2ALL in combi2ALL)
                                        {
                                            if (lstBowler.Count >= 4)
                                            {
                                                #region "4 BOWL"

                                                Combinations<string> combi4BOWL = new Combinations<string>(lstBowler, 4);
                                                foreach (IList<string> perm4BOWL in combi4BOWL)
                                                {
                                                    var NotMatchingBAT = from i in lstBatsman_key
                                                                         where !perm4BAT.Contains(i)
                                                                         select i;

                                                    var NotMatchingALL = from i in lstAllRounder_key
                                                                         where !perm2ALL.Contains(i)
                                                                         select i;

                                                    var NotMatchingBOWL = from i in lstBowler_key
                                                                          where !perm4BOWL.Contains(i)
                                                                          select i;

                                                    if (NotMatchingBAT.Count() == 0 && NotMatchingALL.Count() == 0 && NotMatchingBOWL.Count() == 0)
                                                    {
                                                        DataRow drTeam = dtTeam.NewRow();
                                                        drTeam["C"] = sCaptainName;
                                                        drTeam["VC"] = sViceCaptainName;

                                                        objData = GetTeamAndCredits("WK", perm1WK.ElementAt(0));
                                                        drTeam["WK"] = objData.Item1;
                                                        drTeam["WKT"] = objData.Item2;
                                                        drTeam["WKC"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(0));
                                                        drTeam["BAT1"] = objData.Item1;
                                                        drTeam["BAT1T"] = objData.Item2;
                                                        drTeam["BAT1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(1));
                                                        drTeam["BAT2"] = objData.Item1;
                                                        drTeam["BAT2T"] = objData.Item2;
                                                        drTeam["BAT2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(2));
                                                        drTeam["BAT3"] = objData.Item1;
                                                        drTeam["BAT3T"] = objData.Item2;
                                                        drTeam["BAT3C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(3));
                                                        drTeam["BAT4"] = objData.Item1;
                                                        drTeam["BAT4T"] = objData.Item2;
                                                        drTeam["BAT4C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("ALL", perm2ALL.ElementAt(0));
                                                        drTeam["ALL1"] = objData.Item1;
                                                        drTeam["ALL1T"] = objData.Item2;
                                                        drTeam["ALL1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("ALL", perm2ALL.ElementAt(1));
                                                        drTeam["ALL2"] = objData.Item1;
                                                        drTeam["ALL2T"] = objData.Item2;
                                                        drTeam["ALL2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(0));
                                                        drTeam["BOWL1"] = objData.Item1;
                                                        drTeam["BOWL1T"] = objData.Item2;
                                                        drTeam["BOWL1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(1));
                                                        drTeam["BOWL2"] = objData.Item1;
                                                        drTeam["BOWL2T"] = objData.Item2;
                                                        drTeam["BOWL2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(2));
                                                        drTeam["BOWL3"] = objData.Item1;
                                                        drTeam["BOWL3T"] = objData.Item2;
                                                        drTeam["BOWL3C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(3));
                                                        drTeam["BOWL4"] = objData.Item1;
                                                        drTeam["BOWL4T"] = objData.Item2;
                                                        drTeam["BOWL4C"] = objData.Item3;

                                                        dtTeam.Rows.Add(drTeam);
                                                    }
                                                }

                                                #endregion
                                            }

                                        }
                                    }

                                    #endregion

                                    #region "1 ALL"

                                    if (lstAllRounder.Count >= 1)
                                    {
                                        Combinations<string> combi1ALL = new Combinations<string>(lstAllRounder, 1);
                                        foreach (IList<string> perm1ALL in combi1ALL)
                                        {
                                            if (lstBowler.Count >= 5)
                                            {
                                                #region "5 BOWL"

                                                Combinations<string> combi5BOWL = new Combinations<string>(lstBowler, 5);
                                                foreach (IList<string> perm5BOWL in combi5BOWL)
                                                {
                                                    var NotMatchingBAT = from i in lstBatsman_key
                                                                         where !perm4BAT.Contains(i)
                                                                         select i;

                                                    var NotMatchingALL = from i in lstAllRounder_key
                                                                         where !perm1ALL.Contains(i)
                                                                         select i;

                                                    var NotMatchingBOWL = from i in lstBowler_key
                                                                          where !perm5BOWL.Contains(i)
                                                                          select i;

                                                    if (NotMatchingBAT.Count() == 0 && NotMatchingALL.Count() == 0 && NotMatchingBOWL.Count() == 0)
                                                    {
                                                        DataRow drTeam = dtTeam.NewRow();
                                                        drTeam["C"] = sCaptainName;
                                                        drTeam["VC"] = sViceCaptainName;

                                                        objData = GetTeamAndCredits("WK", perm1WK.ElementAt(0));
                                                        drTeam["WK"] = objData.Item1;
                                                        drTeam["WKT"] = objData.Item2;
                                                        drTeam["WKC"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(0));
                                                        drTeam["BAT1"] = objData.Item1;
                                                        drTeam["BAT1T"] = objData.Item2;
                                                        drTeam["BAT1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(1));
                                                        drTeam["BAT2"] = objData.Item1;
                                                        drTeam["BAT2T"] = objData.Item2;
                                                        drTeam["BAT2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(2));
                                                        drTeam["BAT3"] = objData.Item1;
                                                        drTeam["BAT3T"] = objData.Item2;
                                                        drTeam["BAT3C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(3));
                                                        drTeam["BAT4"] = objData.Item1;
                                                        drTeam["BAT4T"] = objData.Item2;
                                                        drTeam["BAT4C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("ALL", perm1ALL.ElementAt(0));
                                                        drTeam["ALL1"] = objData.Item1;
                                                        drTeam["ALL1T"] = objData.Item2;
                                                        drTeam["ALL1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(0));
                                                        drTeam["BOWL1"] = objData.Item1;
                                                        drTeam["BOWL1T"] = objData.Item2;
                                                        drTeam["BOWL1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(1));
                                                        drTeam["BOWL2"] = objData.Item1;
                                                        drTeam["BOWL2T"] = objData.Item2;
                                                        drTeam["BOWL2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(2));
                                                        drTeam["BOWL3"] = objData.Item1;
                                                        drTeam["BOWL3T"] = objData.Item2;
                                                        drTeam["BOWL3C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(3));
                                                        drTeam["BOWL4"] = objData.Item1;
                                                        drTeam["BOWL4T"] = objData.Item2;
                                                        drTeam["BOWL4C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(4));
                                                        drTeam["BOWL5"] = objData.Item1;
                                                        drTeam["BOWL5T"] = objData.Item2;
                                                        drTeam["BOWL5C"] = objData.Item3;

                                                        dtTeam.Rows.Add(drTeam);
                                                    }
                                                }

                                                #endregion
                                            }

                                        }
                                    }

                                    #endregion
                                }
                            }


                            #endregion

                            #region "5 BAT"

                            if (lstBatsman.Count >= 5)
                            {
                                Combinations<string> combi5BAT = new Combinations<string>(lstBatsman, 5);
                                foreach (IList<string> perm5BAT in combi5BAT)
                                {
                                    #region "2 ALL"

                                    if (lstAllRounder.Count >= 2)
                                    {
                                        Combinations<string> combi2ALL = new Combinations<string>(lstAllRounder, 2);
                                        foreach (IList<string> perm2ALL in combi2ALL)
                                        {
                                            if (lstBowler.Count >= 3)
                                            {
                                                #region "3 BOWL"

                                                Combinations<string> combi3BOWL = new Combinations<string>(lstBowler, 3);
                                                foreach (IList<string> perm3BOWL in combi3BOWL)
                                                {
                                                    var NotMatchingBAT = from i in lstBatsman_key
                                                                         where !perm5BAT.Contains(i)
                                                                         select i;

                                                    var NotMatchingALL = from i in lstAllRounder_key
                                                                         where !perm2ALL.Contains(i)
                                                                         select i;

                                                    var NotMatchingBOWL = from i in lstBowler_key
                                                                          where !perm3BOWL.Contains(i)
                                                                          select i;

                                                    if (NotMatchingBAT.Count() == 0 && NotMatchingALL.Count() == 0 && NotMatchingBOWL.Count() == 0)
                                                    {
                                                        DataRow drTeam = dtTeam.NewRow();
                                                        drTeam["C"] = sCaptainName;
                                                        drTeam["VC"] = sViceCaptainName;

                                                        objData = GetTeamAndCredits("WK", perm1WK.ElementAt(0));
                                                        drTeam["WK"] = objData.Item1;
                                                        drTeam["WKT"] = objData.Item2;
                                                        drTeam["WKC"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(0));
                                                        drTeam["BAT1"] = objData.Item1;
                                                        drTeam["BAT1T"] = objData.Item2;
                                                        drTeam["BAT1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(1));
                                                        drTeam["BAT2"] = objData.Item1;
                                                        drTeam["BAT2T"] = objData.Item2;
                                                        drTeam["BAT2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(2));
                                                        drTeam["BAT3"] = objData.Item1;
                                                        drTeam["BAT3T"] = objData.Item2;
                                                        drTeam["BAT3C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(3));
                                                        drTeam["BAT4"] = objData.Item1;
                                                        drTeam["BAT4T"] = objData.Item2;
                                                        drTeam["BAT4C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(4));
                                                        drTeam["BAT5"] = objData.Item1;
                                                        drTeam["BAT5T"] = objData.Item2;
                                                        drTeam["BAT5C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("ALL", perm2ALL.ElementAt(0));
                                                        drTeam["ALL1"] = objData.Item1;
                                                        drTeam["ALL1T"] = objData.Item2;
                                                        drTeam["ALL1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("ALL", perm2ALL.ElementAt(1));
                                                        drTeam["ALL2"] = objData.Item1;
                                                        drTeam["ALL2T"] = objData.Item2;
                                                        drTeam["ALL2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm3BOWL.ElementAt(0));
                                                        drTeam["BOWL1"] = objData.Item1;
                                                        drTeam["BOWL1T"] = objData.Item2;
                                                        drTeam["BOWL1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm3BOWL.ElementAt(1));
                                                        drTeam["BOWL2"] = objData.Item1;
                                                        drTeam["BOWL2T"] = objData.Item2;
                                                        drTeam["BOWL2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm3BOWL.ElementAt(2));
                                                        drTeam["BOWL3"] = objData.Item1;
                                                        drTeam["BOWL3T"] = objData.Item2;
                                                        drTeam["BOWL3C"] = objData.Item3;

                                                        dtTeam.Rows.Add(drTeam);
                                                    }
                                                }

                                                #endregion
                                            }
                                        }
                                    }

                                    #endregion

                                    #region "1 ALL"

                                    if (lstAllRounder.Count >= 1)
                                    {
                                        Combinations<string> combi1ALL = new Combinations<string>(lstAllRounder, 1);
                                        foreach (IList<string> perm1ALL in combi1ALL)
                                        {
                                            if (lstBowler.Count >= 4)
                                            {
                                                #region "4 BOWL"

                                                Combinations<string> combi4BOWL = new Combinations<string>(lstBowler, 4);
                                                foreach (IList<string> perm4BOWL in combi4BOWL)
                                                {
                                                    var NotMatchingBAT = from i in lstBatsman_key
                                                                         where !perm5BAT.Contains(i)
                                                                         select i;

                                                    var NotMatchingALL = from i in lstAllRounder_key
                                                                         where !perm1ALL.Contains(i)
                                                                         select i;

                                                    var NotMatchingBOWL = from i in lstBowler_key
                                                                          where !perm4BOWL.Contains(i)
                                                                          select i;

                                                    if (NotMatchingBAT.Count() == 0 && NotMatchingALL.Count() == 0 && NotMatchingBOWL.Count() == 0)
                                                    {
                                                        DataRow drTeam = dtTeam.NewRow();
                                                        drTeam["C"] = sCaptainName;
                                                        drTeam["VC"] = sViceCaptainName;

                                                        objData = GetTeamAndCredits("WK", perm1WK.ElementAt(0));
                                                        drTeam["WK"] = objData.Item1;
                                                        drTeam["WKT"] = objData.Item2;
                                                        drTeam["WKC"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(0));
                                                        drTeam["BAT1"] = objData.Item1;
                                                        drTeam["BAT1T"] = objData.Item2;
                                                        drTeam["BAT1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(1));
                                                        drTeam["BAT2"] = objData.Item1;
                                                        drTeam["BAT2T"] = objData.Item2;
                                                        drTeam["BAT2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(2));
                                                        drTeam["BAT3"] = objData.Item1;
                                                        drTeam["BAT3T"] = objData.Item2;
                                                        drTeam["BAT3C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(3));
                                                        drTeam["BAT4"] = objData.Item1;
                                                        drTeam["BAT4T"] = objData.Item2;
                                                        drTeam["BAT4C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(4));
                                                        drTeam["BAT5"] = objData.Item1;
                                                        drTeam["BAT5T"] = objData.Item2;
                                                        drTeam["BAT5C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("ALL", perm1ALL.ElementAt(0));
                                                        drTeam["ALL1"] = objData.Item1;
                                                        drTeam["ALL1T"] = objData.Item2;
                                                        drTeam["ALL1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(0));
                                                        drTeam["BOWL1"] = objData.Item1;
                                                        drTeam["BOWL1T"] = objData.Item2;
                                                        drTeam["BOWL1C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(1));
                                                        drTeam["BOWL2"] = objData.Item1;
                                                        drTeam["BOWL2T"] = objData.Item2;
                                                        drTeam["BOWL2C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(2));
                                                        drTeam["BOWL3"] = objData.Item1;
                                                        drTeam["BOWL3T"] = objData.Item2;
                                                        drTeam["BOWL3C"] = objData.Item3;

                                                        objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(3));
                                                        drTeam["BOWL4"] = objData.Item1;
                                                        drTeam["BOWL4T"] = objData.Item2;
                                                        drTeam["BOWL4C"] = objData.Item3;

                                                        dtTeam.Rows.Add(drTeam);
                                                    }
                                                }

                                                #endregion
                                            }
                                        }
                                    }

                                    #endregion
                                }
                            }

                            #endregion
                        }
                    }

                    #region "Export to Excel"

                    if (dtTeam != null && dtTeam.Rows.Count > 0)
                    {
                        Tuple<IEnumerable<dynamic>> objTeam = null;

                        objTeam = DBHandler.GenerateTeam(TeamA, TeamB, dtTeam);

                        if (objTeam != null)
                        {
                            dtTeam = objTeam.Item1.ToDataTable();

                            if (dtTeam != null && dtTeam.Rows.Count > 0)
                            {
                                ShowMessage(string.Format("Exporting {0}", sExportFilePath));

                                #region "Excel object Declaration"

                                Excel.Application xlApp = null;
                                Excel.Workbook xlWorkBook = null;
                                Excel.Worksheet xlWorkSheet;
                                Excel.Range range = null;
                                object misValue = System.Reflection.Missing.Value;
                                int hWnd = 0;

                                int iRowStart = 0;

                                #endregion

                                #region "Excel object Initialization"

                                xlApp = new Excel.Application();
                                xlApp.Visible = false;
                                xlApp.DisplayAlerts = false;
                                hWnd = xlApp.Application.Hwnd;
                                xlWorkBook = xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                                xlApp.StandardFont = "Calibri";
                                xlApp.StandardFontSize = 11;

                                #endregion

                                #region "Excel Data"

                                try
                                {

                                    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                                    xlWorkSheet.Name = "KEY PLAYERS";

                                    #region "Excel Value - Start"

                                    int iRowCount = 1;
                                    iRowStart = iRowCount;

                                    xlWorkSheet.Cells[iRowCount, 1] = "TeamNo";
                                    xlWorkSheet.Cells[iRowCount, 2] = "WK";
                                    xlWorkSheet.Cells[iRowCount, 3] = "BAT1";
                                    xlWorkSheet.Cells[iRowCount, 4] = "BAT2";
                                    xlWorkSheet.Cells[iRowCount, 5] = "BAT3";
                                    xlWorkSheet.Cells[iRowCount, 6] = "BAT4";
                                    xlWorkSheet.Cells[iRowCount, 7] = "BAT5";
                                    xlWorkSheet.Cells[iRowCount, 8] = "ALL1";
                                    xlWorkSheet.Cells[iRowCount, 9] = "ALL2";
                                    xlWorkSheet.Cells[iRowCount, 10] = "ALL3";
                                    xlWorkSheet.Cells[iRowCount, 11] = "BOWL1";
                                    xlWorkSheet.Cells[iRowCount, 12] = "BOWL2";
                                    xlWorkSheet.Cells[iRowCount, 13] = "BOWL3";
                                    xlWorkSheet.Cells[iRowCount, 14] = "BOWL4";
                                    xlWorkSheet.Cells[iRowCount, 15] = "BOWL5";

                                    range = xlWorkSheet.Rows.get_Range("A" + iRowCount.ToString(), "O" + iRowCount.ToString());
                                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                    range.EntireColumn.AutoFit();
                                    System.Drawing.Color colorHeading = System.Drawing.ColorTranslator.FromHtml("#C5D9F1");
                                    range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                                    range = null;

                                    foreach (DataRow drRow in dtTeam.Rows)
                                    {
                                        iRowCount++;

                                        xlWorkSheet.Cells[iRowCount, 1] = drRow["TeamNo"].ToString();
                                        xlWorkSheet.Cells[iRowCount, 2] = drRow["WK"].ToString();
                                        xlWorkSheet.Cells[iRowCount, 3] = drRow["BAT1"].ToString();
                                        xlWorkSheet.Cells[iRowCount, 4] = drRow["BAT2"].ToString();
                                        xlWorkSheet.Cells[iRowCount, 5] = drRow["BAT3"].ToString();
                                        xlWorkSheet.Cells[iRowCount, 6] = drRow["BAT4"].ToString();
                                        xlWorkSheet.Cells[iRowCount, 7] = drRow["BAT5"].ToString();
                                        xlWorkSheet.Cells[iRowCount, 8] = drRow["ALL1"].ToString();
                                        xlWorkSheet.Cells[iRowCount, 9] = drRow["ALL2"].ToString();
                                        xlWorkSheet.Cells[iRowCount, 10] = drRow["ALL3"].ToString();
                                        xlWorkSheet.Cells[iRowCount, 11] = drRow["BOWL1"].ToString();
                                        xlWorkSheet.Cells[iRowCount, 12] = drRow["BOWL2"].ToString();
                                        xlWorkSheet.Cells[iRowCount, 13] = drRow["BOWL3"].ToString();
                                        xlWorkSheet.Cells[iRowCount, 14] = drRow["BOWL4"].ToString();
                                        xlWorkSheet.Cells[iRowCount, 15] = drRow["BOWL5"].ToString();

                                    }

                                    #endregion "Excel Value - End"

                                    int iRowEnd = iRowCount;

                                    #region "Border"

                                    range = xlWorkSheet.Rows.get_Range("A" + iRowStart.ToString(), "O" + iRowEnd.ToString());
                                    range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                    range = null;

                                    #endregion

                                    xlWorkSheet.Columns.EntireColumn.AutoFit();

                                    if (File.Exists(sExportFilePath))
                                    {
                                        File.Delete(sExportFilePath);
                                    }

                                    xlWorkBook.SaveAs(sExportFilePath, Excel.XlFileFormat.xlOpenXMLWorkbook, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                                    xlWorkBook.Close(true, misValue, misValue);
                                }
                                catch (Exception ex)
                                {

                                }
                                finally
                                {
                                    TryKillProcessByMainWindowHwnd(hWnd);
                                }

                                #endregion

                            }
                        }
                    }

                    #endregion

                    #endregion
                }
                else if (sTeamType.Equals(TeamType.VIP_PLAYERS))
                {
                    #region "Capatain all others as vice captain"

                    DataView dvCaptain = new DataView(dtSelectedPlayer);
                    dvCaptain.RowFilter = "IsCaptain = true";
                    DataTable dtTempCaptain = dvCaptain.ToTable(true, "Player", "Tag", "Role");

                    foreach (DataRow drRowCaptain in dtTempCaptain.Rows)
                    {
                        sCaptainName = drRowCaptain["Player"].ToString();
                        sCaptainTag = drRowCaptain["Tag"].ToString();
                        sCaptainRole = drRowCaptain["Role"].ToString();

                        foreach (DataRow drRowViceCaptain in dtSelectedPlayer.Rows)
                        {
                            sViceCaptainName = drRowViceCaptain["Player"].ToString();
                            sViceCaptainTag = drRowViceCaptain["Tag"].ToString();
                            sViceCaptainRole = drRowViceCaptain["Role"].ToString();

                            if ((!sCaptainTag.Equals(sViceCaptainTag) && !(sCaptainRole == "WK" && sViceCaptainRole == "WK")) || (dtSelectedPlayer.Rows.Count == 1))
                            {
                                #region "3 BAT"

                                if (lstBatsman.Count >= 3)
                                {
                                    Combinations<string> combi3BAT = new Combinations<string>(lstBatsman, 3);
                                    foreach (IList<string> perm3BAT in combi3BAT)
                                    {
                                        if (lstAllRounder.Count >= 3)
                                        {
                                            #region "3 ALL"

                                            Combinations<string> combi3ALL = new Combinations<string>(lstAllRounder, 3);
                                            foreach (IList<string> perm3ALL in combi3ALL)
                                            {
                                                if (lstBowler.Count >= 4)
                                                {
                                                    #region "4 BOWL"

                                                    Combinations<string> combi4BOWL = new Combinations<string>(lstBowler, 4);
                                                    foreach (IList<string> perm4BOWL in combi4BOWL)
                                                    {
                                                        if ((perm3BAT.Contains(sCaptainTag) || perm3ALL.Contains(sCaptainTag) || perm4BOWL.Contains(sCaptainTag)) && (perm3BAT.Contains(sViceCaptainTag) || perm3ALL.Contains(sViceCaptainTag) || perm4BOWL.Contains(sViceCaptainTag)))
                                                        {
                                                            DataRow drTeam = dtTeam.NewRow();
                                                            drTeam["C"] = sCaptainName;
                                                            drTeam["VC"] = sViceCaptainName;
                                                            drTeam["WK"] = sWicketKeeper;
                                                            drTeam["WKT"] = sWicketKeeper_Team;
                                                            drTeam["WKC"] = sWicketKeeper_Credits;

                                                            objData = GetTeamAndCredits("BAT", perm3BAT.ElementAt(0));
                                                            drTeam["BAT1"] = objData.Item1;
                                                            drTeam["BAT1T"] = objData.Item2;
                                                            drTeam["BAT1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm3BAT.ElementAt(1));
                                                            drTeam["BAT2"] = objData.Item1;
                                                            drTeam["BAT2T"] = objData.Item2;
                                                            drTeam["BAT2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm3BAT.ElementAt(2));
                                                            drTeam["BAT3"] = objData.Item1;
                                                            drTeam["BAT3T"] = objData.Item2;
                                                            drTeam["BAT3C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("ALL", perm3ALL.ElementAt(0));
                                                            drTeam["ALL1"] = objData.Item1;
                                                            drTeam["ALL1T"] = objData.Item2;
                                                            drTeam["ALL1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("ALL", perm3ALL.ElementAt(1));
                                                            drTeam["ALL2"] = objData.Item1;
                                                            drTeam["ALL2T"] = objData.Item2;
                                                            drTeam["ALL2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("ALL", perm3ALL.ElementAt(2));
                                                            drTeam["ALL3"] = objData.Item1;
                                                            drTeam["ALL3T"] = objData.Item2;
                                                            drTeam["ALL3C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(0));
                                                            drTeam["BOWL1"] = objData.Item1;
                                                            drTeam["BOWL1T"] = objData.Item2;
                                                            drTeam["BOWL1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(1));
                                                            drTeam["BOWL2"] = objData.Item1;
                                                            drTeam["BOWL2T"] = objData.Item2;
                                                            drTeam["BOWL2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(2));
                                                            drTeam["BOWL3"] = objData.Item1;
                                                            drTeam["BOWL3T"] = objData.Item2;
                                                            drTeam["BOWL3C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(3));
                                                            drTeam["BOWL4"] = objData.Item1;
                                                            drTeam["BOWL4T"] = objData.Item2;
                                                            drTeam["BOWL4C"] = objData.Item3;

                                                            dtTeam.Rows.Add(drTeam);
                                                        }
                                                    }

                                                    #endregion
                                                }

                                            }

                                            #endregion
                                        }

                                        if (lstAllRounder.Count >= 2)
                                        {
                                            #region "2 ALL"

                                            Combinations<string> combi2ALL = new Combinations<string>(lstAllRounder, 2);
                                            foreach (IList<string> perm2ALL in combi2ALL)
                                            {
                                                if (lstBowler.Count >= 5)
                                                {
                                                    #region "5 BOWL"

                                                    Combinations<string> combi5BOWL = new Combinations<string>(lstBowler, 5);
                                                    foreach (IList<string> perm5BOWL in combi5BOWL)
                                                    {
                                                        if ((perm3BAT.Contains(sCaptainTag) || perm2ALL.Contains(sCaptainTag) || perm5BOWL.Contains(sCaptainTag)) && (perm3BAT.Contains(sViceCaptainTag) || perm2ALL.Contains(sViceCaptainTag) || perm5BOWL.Contains(sViceCaptainTag)))
                                                        {
                                                            DataRow drTeam = dtTeam.NewRow();
                                                            drTeam["C"] = sCaptainName;
                                                            drTeam["VC"] = sViceCaptainName;
                                                            drTeam["WK"] = sWicketKeeper;
                                                            drTeam["WKT"] = sWicketKeeper_Team;
                                                            drTeam["WKC"] = sWicketKeeper_Credits;

                                                            objData = GetTeamAndCredits("BAT", perm3BAT.ElementAt(0));
                                                            drTeam["BAT1"] = objData.Item1;
                                                            drTeam["BAT1T"] = objData.Item2;
                                                            drTeam["BAT1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm3BAT.ElementAt(1));
                                                            drTeam["BAT2"] = objData.Item1;
                                                            drTeam["BAT2T"] = objData.Item2;
                                                            drTeam["BAT2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm3BAT.ElementAt(2));
                                                            drTeam["BAT3"] = objData.Item1;
                                                            drTeam["BAT3T"] = objData.Item2;
                                                            drTeam["BAT3C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("ALL", perm2ALL.ElementAt(0));
                                                            drTeam["ALL1"] = objData.Item1;
                                                            drTeam["ALL1T"] = objData.Item2;
                                                            drTeam["ALL1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("ALL", perm2ALL.ElementAt(1));
                                                            drTeam["ALL2"] = objData.Item1;
                                                            drTeam["ALL2T"] = objData.Item2;
                                                            drTeam["ALL2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(0));
                                                            drTeam["BOWL1"] = objData.Item1;
                                                            drTeam["BOWL1T"] = objData.Item2;
                                                            drTeam["BOWL1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(1));
                                                            drTeam["BOWL2"] = objData.Item1;
                                                            drTeam["BOWL2T"] = objData.Item2;
                                                            drTeam["BOWL2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(2));
                                                            drTeam["BOWL3"] = objData.Item1;
                                                            drTeam["BOWL3T"] = objData.Item2;
                                                            drTeam["BOWL3C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(3));
                                                            drTeam["BOWL4"] = objData.Item1;
                                                            drTeam["BOWL4T"] = objData.Item2;
                                                            drTeam["BOWL4C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(4));
                                                            drTeam["BOWL5"] = objData.Item1;
                                                            drTeam["BOWL5T"] = objData.Item2;
                                                            drTeam["BOWL5C"] = objData.Item3;

                                                            dtTeam.Rows.Add(drTeam);
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

                                #region "4 BAT"

                                if (lstBatsman.Count >= 4)
                                {
                                    Combinations<string> combi4BAT = new Combinations<string>(lstBatsman, 4);
                                    foreach (IList<string> perm4BAT in combi4BAT)
                                    {
                                        #region "3 ALL"

                                        if (lstAllRounder.Count >= 3)
                                        {
                                            Combinations<string> combi3ALL = new Combinations<string>(lstAllRounder, 3);
                                            foreach (IList<string> perm3ALL in combi3ALL)
                                            {
                                                if (lstBowler.Count >= 3)
                                                {
                                                    #region "3 BOWL"
                                                    Combinations<string> combi3BOWL = new Combinations<string>(lstBowler, 3);
                                                    foreach (IList<string> perm3BOWL in combi3BOWL)
                                                    {
                                                        if ((perm4BAT.Contains(sCaptainTag) || perm3ALL.Contains(sCaptainTag) || perm3BOWL.Contains(sCaptainTag)) && (perm4BAT.Contains(sViceCaptainTag) || perm3ALL.Contains(sViceCaptainTag) || perm3BOWL.Contains(sViceCaptainTag)))
                                                        {
                                                            DataRow drTeam = dtTeam.NewRow();
                                                            drTeam["C"] = sCaptainName;
                                                            drTeam["VC"] = sViceCaptainName;
                                                            drTeam["WK"] = sWicketKeeper;
                                                            drTeam["WKT"] = sWicketKeeper_Team;
                                                            drTeam["WKC"] = sWicketKeeper_Credits;

                                                            objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(0));
                                                            drTeam["BAT1"] = objData.Item1;
                                                            drTeam["BAT1T"] = objData.Item2;
                                                            drTeam["BAT1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(1));
                                                            drTeam["BAT2"] = objData.Item1;
                                                            drTeam["BAT2T"] = objData.Item2;
                                                            drTeam["BAT2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(2));
                                                            drTeam["BAT3"] = objData.Item1;
                                                            drTeam["BAT3T"] = objData.Item2;
                                                            drTeam["BAT3C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(3));
                                                            drTeam["BAT4"] = objData.Item1;
                                                            drTeam["BAT4T"] = objData.Item2;
                                                            drTeam["BAT4C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("ALL", perm3ALL.ElementAt(0));
                                                            drTeam["ALL1"] = objData.Item1;
                                                            drTeam["ALL1T"] = objData.Item2;
                                                            drTeam["ALL1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("ALL", perm3ALL.ElementAt(1));
                                                            drTeam["ALL2"] = objData.Item1;
                                                            drTeam["ALL2T"] = objData.Item2;
                                                            drTeam["ALL2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("ALL", perm3ALL.ElementAt(2));
                                                            drTeam["ALL3"] = objData.Item1;
                                                            drTeam["ALL3T"] = objData.Item2;
                                                            drTeam["ALL3C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm3BOWL.ElementAt(0));
                                                            drTeam["BOWL1"] = objData.Item1;
                                                            drTeam["BOWL1T"] = objData.Item2;
                                                            drTeam["BOWL1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm3BOWL.ElementAt(1));
                                                            drTeam["BOWL2"] = objData.Item1;
                                                            drTeam["BOWL2T"] = objData.Item2;
                                                            drTeam["BOWL2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm3BOWL.ElementAt(2));
                                                            drTeam["BOWL3"] = objData.Item1;
                                                            drTeam["BOWL3T"] = objData.Item2;
                                                            drTeam["BOWL3C"] = objData.Item3;

                                                            dtTeam.Rows.Add(drTeam);
                                                        }
                                                    }

                                                    #endregion
                                                }

                                            }
                                        }

                                        #endregion

                                        #region "2 ALL"

                                        if (lstAllRounder.Count >= 2)
                                        {
                                            Combinations<string> combi2ALL = new Combinations<string>(lstAllRounder, 2);
                                            foreach (IList<string> perm2ALL in combi2ALL)
                                            {
                                                if (lstBowler.Count >= 4)
                                                {
                                                    #region "4 BOWL"

                                                    Combinations<string> combi4BOWL = new Combinations<string>(lstBowler, 4);
                                                    foreach (IList<string> perm4BOWL in combi4BOWL)
                                                    {
                                                        if ((perm4BAT.Contains(sCaptainTag) || perm2ALL.Contains(sCaptainTag) || perm4BOWL.Contains(sCaptainTag)) && (perm4BAT.Contains(sViceCaptainTag) || perm2ALL.Contains(sViceCaptainTag) || perm4BOWL.Contains(sViceCaptainTag)))
                                                        {
                                                            DataRow drTeam = dtTeam.NewRow();
                                                            drTeam["C"] = sCaptainName;
                                                            drTeam["VC"] = sViceCaptainName;
                                                            drTeam["WK"] = sWicketKeeper;
                                                            drTeam["WKT"] = sWicketKeeper_Team;
                                                            drTeam["WKC"] = sWicketKeeper_Credits;

                                                            objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(0));
                                                            drTeam["BAT1"] = objData.Item1;
                                                            drTeam["BAT1T"] = objData.Item2;
                                                            drTeam["BAT1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(1));
                                                            drTeam["BAT2"] = objData.Item1;
                                                            drTeam["BAT2T"] = objData.Item2;
                                                            drTeam["BAT2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(2));
                                                            drTeam["BAT3"] = objData.Item1;
                                                            drTeam["BAT3T"] = objData.Item2;
                                                            drTeam["BAT3C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(3));
                                                            drTeam["BAT4"] = objData.Item1;
                                                            drTeam["BAT4T"] = objData.Item2;
                                                            drTeam["BAT4C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("ALL", perm2ALL.ElementAt(0));
                                                            drTeam["ALL1"] = objData.Item1;
                                                            drTeam["ALL1T"] = objData.Item2;
                                                            drTeam["ALL1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("ALL", perm2ALL.ElementAt(1));
                                                            drTeam["ALL2"] = objData.Item1;
                                                            drTeam["ALL2T"] = objData.Item2;
                                                            drTeam["ALL2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(0));
                                                            drTeam["BOWL1"] = objData.Item1;
                                                            drTeam["BOWL1T"] = objData.Item2;
                                                            drTeam["BOWL1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(1));
                                                            drTeam["BOWL2"] = objData.Item1;
                                                            drTeam["BOWL2T"] = objData.Item2;
                                                            drTeam["BOWL2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(2));
                                                            drTeam["BOWL3"] = objData.Item1;
                                                            drTeam["BOWL3T"] = objData.Item2;
                                                            drTeam["BOWL3C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(3));
                                                            drTeam["BOWL4"] = objData.Item1;
                                                            drTeam["BOWL4T"] = objData.Item2;
                                                            drTeam["BOWL4C"] = objData.Item3;

                                                            dtTeam.Rows.Add(drTeam);
                                                        }
                                                    }

                                                    #endregion
                                                }

                                            }
                                        }

                                        #endregion

                                        #region "1 ALL"

                                        if (lstAllRounder.Count >= 1)
                                        {
                                            Combinations<string> combi1ALL = new Combinations<string>(lstAllRounder, 1);
                                            foreach (IList<string> perm1ALL in combi1ALL)
                                            {
                                                if (lstBowler.Count >= 5)
                                                {
                                                    #region "5 BOWL"

                                                    Combinations<string> combi5BOWL = new Combinations<string>(lstBowler, 5);
                                                    foreach (IList<string> perm5BOWL in combi5BOWL)
                                                    {
                                                        if ((perm4BAT.Contains(sCaptainTag) || perm1ALL.Contains(sCaptainTag) || perm5BOWL.Contains(sCaptainTag)) && (perm4BAT.Contains(sViceCaptainTag) || perm1ALL.Contains(sViceCaptainTag) || perm5BOWL.Contains(sViceCaptainTag)))
                                                        {
                                                            DataRow drTeam = dtTeam.NewRow();
                                                            drTeam["C"] = sCaptainName;
                                                            drTeam["VC"] = sViceCaptainName;
                                                            drTeam["WK"] = sWicketKeeper;
                                                            drTeam["WKT"] = sWicketKeeper_Team;
                                                            drTeam["WKC"] = sWicketKeeper_Credits;

                                                            objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(0));
                                                            drTeam["BAT1"] = objData.Item1;
                                                            drTeam["BAT1T"] = objData.Item2;
                                                            drTeam["BAT1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(1));
                                                            drTeam["BAT2"] = objData.Item1;
                                                            drTeam["BAT2T"] = objData.Item2;
                                                            drTeam["BAT2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(2));
                                                            drTeam["BAT3"] = objData.Item1;
                                                            drTeam["BAT3T"] = objData.Item2;
                                                            drTeam["BAT3C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm4BAT.ElementAt(3));
                                                            drTeam["BAT4"] = objData.Item1;
                                                            drTeam["BAT4T"] = objData.Item2;
                                                            drTeam["BAT4C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("ALL", perm1ALL.ElementAt(0));
                                                            drTeam["ALL1"] = objData.Item1;
                                                            drTeam["ALL1T"] = objData.Item2;
                                                            drTeam["ALL1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(0));
                                                            drTeam["BOWL1"] = objData.Item1;
                                                            drTeam["BOWL1T"] = objData.Item2;
                                                            drTeam["BOWL1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(1));
                                                            drTeam["BOWL2"] = objData.Item1;
                                                            drTeam["BOWL2T"] = objData.Item2;
                                                            drTeam["BOWL2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(2));
                                                            drTeam["BOWL3"] = objData.Item1;
                                                            drTeam["BOWL3T"] = objData.Item2;
                                                            drTeam["BOWL3C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(3));
                                                            drTeam["BOWL4"] = objData.Item1;
                                                            drTeam["BOWL4T"] = objData.Item2;
                                                            drTeam["BOWL4C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm5BOWL.ElementAt(4));
                                                            drTeam["BOWL5"] = objData.Item1;
                                                            drTeam["BOWL5T"] = objData.Item2;
                                                            drTeam["BOWL5C"] = objData.Item3;

                                                            dtTeam.Rows.Add(drTeam);
                                                        }
                                                    }

                                                    #endregion
                                                }

                                            }
                                        }

                                        #endregion
                                    }
                                }


                                #endregion

                                #region "5 BAT"

                                if (lstBatsman.Count >= 5)
                                {
                                    Combinations<string> combi5BAT = new Combinations<string>(lstBatsman, 5);
                                    foreach (IList<string> perm5BAT in combi5BAT)
                                    {
                                        #region "2 ALL"

                                        if (lstAllRounder.Count >= 2)
                                        {
                                            Combinations<string> combi2ALL = new Combinations<string>(lstAllRounder, 2);
                                            foreach (IList<string> perm2ALL in combi2ALL)
                                            {
                                                if (lstBowler.Count >= 3)
                                                {
                                                    #region "3 BOWL"

                                                    Combinations<string> combi3BOWL = new Combinations<string>(lstBowler, 3);
                                                    foreach (IList<string> perm3BOWL in combi3BOWL)
                                                    {
                                                        if ((perm5BAT.Contains(sCaptainTag) || perm2ALL.Contains(sCaptainTag) || perm3BOWL.Contains(sCaptainTag)) && (perm5BAT.Contains(sViceCaptainTag) || perm2ALL.Contains(sViceCaptainTag) || perm3BOWL.Contains(sViceCaptainTag)))
                                                        {
                                                            DataRow drTeam = dtTeam.NewRow();
                                                            drTeam["C"] = sCaptainName;
                                                            drTeam["VC"] = sViceCaptainName;
                                                            drTeam["WK"] = sWicketKeeper;
                                                            drTeam["WKT"] = sWicketKeeper_Team;
                                                            drTeam["WKC"] = sWicketKeeper_Credits;

                                                            objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(0));
                                                            drTeam["BAT1"] = objData.Item1;
                                                            drTeam["BAT1T"] = objData.Item2;
                                                            drTeam["BAT1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(1));
                                                            drTeam["BAT2"] = objData.Item1;
                                                            drTeam["BAT2T"] = objData.Item2;
                                                            drTeam["BAT2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(2));
                                                            drTeam["BAT3"] = objData.Item1;
                                                            drTeam["BAT3T"] = objData.Item2;
                                                            drTeam["BAT3C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(3));
                                                            drTeam["BAT4"] = objData.Item1;
                                                            drTeam["BAT4T"] = objData.Item2;
                                                            drTeam["BAT4C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(4));
                                                            drTeam["BAT5"] = objData.Item1;
                                                            drTeam["BAT5T"] = objData.Item2;
                                                            drTeam["BAT5C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("ALL", perm2ALL.ElementAt(0));
                                                            drTeam["ALL1"] = objData.Item1;
                                                            drTeam["ALL1T"] = objData.Item2;
                                                            drTeam["ALL1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("ALL", perm2ALL.ElementAt(1));
                                                            drTeam["ALL2"] = objData.Item1;
                                                            drTeam["ALL2T"] = objData.Item2;
                                                            drTeam["ALL2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm3BOWL.ElementAt(0));
                                                            drTeam["BOWL1"] = objData.Item1;
                                                            drTeam["BOWL1T"] = objData.Item2;
                                                            drTeam["BOWL1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm3BOWL.ElementAt(1));
                                                            drTeam["BOWL2"] = objData.Item1;
                                                            drTeam["BOWL2T"] = objData.Item2;
                                                            drTeam["BOWL2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm3BOWL.ElementAt(2));
                                                            drTeam["BOWL3"] = objData.Item1;
                                                            drTeam["BOWL3T"] = objData.Item2;
                                                            drTeam["BOWL3C"] = objData.Item3;

                                                            dtTeam.Rows.Add(drTeam);
                                                        }
                                                    }

                                                    #endregion
                                                }
                                            }
                                        }

                                        #endregion

                                        #region "1 ALL"

                                        if (lstAllRounder.Count >= 1)
                                        {
                                            Combinations<string> combi1ALL = new Combinations<string>(lstAllRounder, 1);
                                            foreach (IList<string> perm1ALL in combi1ALL)
                                            {
                                                if (lstBowler.Count >= 4)
                                                {
                                                    #region "4 BOWL"

                                                    Combinations<string> combi4BOWL = new Combinations<string>(lstBowler, 4);
                                                    foreach (IList<string> perm4BOWL in combi4BOWL)
                                                    {
                                                        if ((perm5BAT.Contains(sCaptainTag) || perm1ALL.Contains(sCaptainTag) || perm4BOWL.Contains(sCaptainTag)) && (perm5BAT.Contains(sViceCaptainTag) || perm1ALL.Contains(sViceCaptainTag) || perm4BOWL.Contains(sViceCaptainTag)))
                                                        {
                                                            DataRow drTeam = dtTeam.NewRow();
                                                            drTeam["C"] = sCaptainName;
                                                            drTeam["VC"] = sViceCaptainName;
                                                            drTeam["WK"] = sWicketKeeper;
                                                            drTeam["WKT"] = sWicketKeeper_Team;
                                                            drTeam["WKC"] = sWicketKeeper_Credits;

                                                            objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(0));
                                                            drTeam["BAT1"] = objData.Item1;
                                                            drTeam["BAT1T"] = objData.Item2;
                                                            drTeam["BAT1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(1));
                                                            drTeam["BAT2"] = objData.Item1;
                                                            drTeam["BAT2T"] = objData.Item2;
                                                            drTeam["BAT2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(2));
                                                            drTeam["BAT3"] = objData.Item1;
                                                            drTeam["BAT3T"] = objData.Item2;
                                                            drTeam["BAT3C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(3));
                                                            drTeam["BAT4"] = objData.Item1;
                                                            drTeam["BAT4T"] = objData.Item2;
                                                            drTeam["BAT4C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BAT", perm5BAT.ElementAt(4));
                                                            drTeam["BAT5"] = objData.Item1;
                                                            drTeam["BAT5T"] = objData.Item2;
                                                            drTeam["BAT5C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("ALL", perm1ALL.ElementAt(0));
                                                            drTeam["ALL1"] = objData.Item1;
                                                            drTeam["ALL1T"] = objData.Item2;
                                                            drTeam["ALL1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(0));
                                                            drTeam["BOWL1"] = objData.Item1;
                                                            drTeam["BOWL1T"] = objData.Item2;
                                                            drTeam["BOWL1C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(1));
                                                            drTeam["BOWL2"] = objData.Item1;
                                                            drTeam["BOWL2T"] = objData.Item2;
                                                            drTeam["BOWL2C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(2));
                                                            drTeam["BOWL3"] = objData.Item1;
                                                            drTeam["BOWL3T"] = objData.Item2;
                                                            drTeam["BOWL3C"] = objData.Item3;

                                                            objData = GetTeamAndCredits("BOWL", perm4BOWL.ElementAt(3));
                                                            drTeam["BOWL4"] = objData.Item1;
                                                            drTeam["BOWL4T"] = objData.Item2;
                                                            drTeam["BOWL4C"] = objData.Item3;

                                                            dtTeam.Rows.Add(drTeam);
                                                        }
                                                    }

                                                    #endregion
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

                    #region "Export to Excel"

                    if (dtTeam != null && dtTeam.Rows.Count > 0)
                    {
                        Tuple<IEnumerable<dynamic>> objTeam = null;

                        objTeam = DBHandler.GenerateTeam(TeamA, TeamB, dtTeam);

                        if (objTeam != null)
                        {
                            dtTeam = objTeam.Item1.ToDataTable();

                            if (dtTeam != null && dtTeam.Rows.Count > 0)
                            {
                                ShowMessage(string.Format("Exporting {0}", sExportFilePath));

                                #region "Excel object Declaration"

                                Excel.Application xlApp = null;
                                Excel.Workbook xlWorkBook = null;
                                Excel.Worksheet xlWorkSheet;
                                Excel.Range range = null;
                                object misValue = System.Reflection.Missing.Value;
                                int hWnd = 0;

                                int iRowStart = 0;

                                #endregion

                                #region "Excel object Initialization"

                                xlApp = new Excel.Application();
                                xlApp.Visible = false;
                                xlApp.DisplayAlerts = false;
                                hWnd = xlApp.Application.Hwnd;
                                xlWorkBook = xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                                xlApp.StandardFont = "Calibri";
                                xlApp.StandardFontSize = 11;

                                #endregion

                                #region "Excel Data"

                                try
                                {

                                    DataView dvCaptainExcel = new DataView(dtTeam);
                                    DataTable dtTempCaptainExcel = dvCaptainExcel.ToTable(true, "C");

                                    foreach (DataRow drRowCap in dtTempCaptainExcel.Rows)
                                    {

                                        if (dtTempCaptainExcel.Rows.IndexOf(drRowCap) > 0)
                                        {
                                            xlWorkBook.Sheets.Add(After: xlWorkBook.Sheets[xlWorkBook.Sheets.Count]);
                                        }

                                        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(dtTempCaptainExcel.Rows.IndexOf(drRowCap) + 1);

                                        xlWorkSheet.Name = drRowCap["C"].ToString();

                                        #region "Excel Value - Start"

                                        int iRowCount = 1;
                                        iRowStart = iRowCount;

                                        xlWorkSheet.Cells[iRowCount, 1] = "TeamNo";
                                        xlWorkSheet.Cells[iRowCount, 2] = "VC";
                                        xlWorkSheet.Cells[iRowCount, 3] = "WK";
                                        xlWorkSheet.Cells[iRowCount, 4] = "BAT1";
                                        xlWorkSheet.Cells[iRowCount, 5] = "BAT2";
                                        xlWorkSheet.Cells[iRowCount, 6] = "BAT3";
                                        xlWorkSheet.Cells[iRowCount, 7] = "BAT4";
                                        xlWorkSheet.Cells[iRowCount, 8] = "BAT5";
                                        xlWorkSheet.Cells[iRowCount, 9] = "ALL1";
                                        xlWorkSheet.Cells[iRowCount, 10] = "ALL2";
                                        xlWorkSheet.Cells[iRowCount, 11] = "ALL3";
                                        xlWorkSheet.Cells[iRowCount, 12] = "BOWL1";
                                        xlWorkSheet.Cells[iRowCount, 13] = "BOWL2";
                                        xlWorkSheet.Cells[iRowCount, 14] = "BOWL3";
                                        xlWorkSheet.Cells[iRowCount, 15] = "BOWL4";
                                        xlWorkSheet.Cells[iRowCount, 16] = "BOWL5";

                                        range = xlWorkSheet.Rows.get_Range("A" + iRowCount.ToString(), "P" + iRowCount.ToString());
                                        range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                        range.EntireColumn.AutoFit();
                                        System.Drawing.Color colorHeading = System.Drawing.ColorTranslator.FromHtml("#C5D9F1");
                                        range.Interior.Color = System.Drawing.ColorTranslator.ToOle(colorHeading);
                                        range = null;

                                        DataView dvPlayer = new DataView(dtTeam);
                                        dvPlayer.RowFilter = "C = '" + drRowCap["C"].ToString() + "' ";

                                        foreach (DataRow drRow in dvPlayer.ToTable().Rows)
                                        {
                                            iRowCount++;

                                            xlWorkSheet.Cells[iRowCount, 1] = drRow["TeamNo"].ToString();
                                            xlWorkSheet.Cells[iRowCount, 2] = drRow["VC"].ToString();
                                            xlWorkSheet.Cells[iRowCount, 3] = drRow["WK"].ToString();
                                            xlWorkSheet.Cells[iRowCount, 4] = drRow["BAT1"].ToString();
                                            xlWorkSheet.Cells[iRowCount, 5] = drRow["BAT2"].ToString();
                                            xlWorkSheet.Cells[iRowCount, 6] = drRow["BAT3"].ToString();
                                            xlWorkSheet.Cells[iRowCount, 7] = drRow["BAT4"].ToString();
                                            xlWorkSheet.Cells[iRowCount, 8] = drRow["BAT5"].ToString();
                                            xlWorkSheet.Cells[iRowCount, 9] = drRow["ALL1"].ToString();
                                            xlWorkSheet.Cells[iRowCount, 10] = drRow["ALL2"].ToString();
                                            xlWorkSheet.Cells[iRowCount, 11] = drRow["ALL3"].ToString();
                                            xlWorkSheet.Cells[iRowCount, 12] = drRow["BOWL1"].ToString();
                                            xlWorkSheet.Cells[iRowCount, 13] = drRow["BOWL2"].ToString();
                                            xlWorkSheet.Cells[iRowCount, 14] = drRow["BOWL3"].ToString();
                                            xlWorkSheet.Cells[iRowCount, 15] = drRow["BOWL4"].ToString();
                                            xlWorkSheet.Cells[iRowCount, 16] = drRow["BOWL5"].ToString();

                                        }

                                        #endregion "Excel Value - End"

                                        int iRowEnd = iRowCount;

                                        #region "Border"

                                        range = xlWorkSheet.Rows.get_Range("A" + iRowStart.ToString(), "P" + iRowEnd.ToString());
                                        range.Borders.Color = System.Drawing.Color.Black.ToArgb();
                                        range = null;

                                        #endregion

                                        xlWorkSheet.Columns.EntireColumn.AutoFit();

                                    }

                                    if (File.Exists(sExportFilePath))
                                    {
                                        File.Delete(sExportFilePath);
                                    }

                                    xlWorkBook.SaveAs(sExportFilePath, Excel.XlFileFormat.xlOpenXMLWorkbook, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                                    xlWorkBook.Close(true, misValue, misValue);
                                }
                                catch (Exception ex)
                                {

                                }
                                finally
                                {
                                    TryKillProcessByMainWindowHwnd(hWnd);
                                }

                                #endregion

                            }
                        }
                    }

                    #endregion

                    #endregion
                }

                e.Result = true;
            }
            catch (Exception ex)
            {
                e.Result = false;
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
                    MessageBox.Show("Completed", "OperationXI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed!", "OperationXI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}
