using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OperationXI
{
    public partial class frmTeam : Form
    {
        public frmTeam()
        {
            InitializeComponent();
        }

        private void frmTeam_Load(object sender, EventArgs e)
        {
            IEnumerable<dynamic> objTeam = DBHandler.GetTeams();
            if(objTeam != null)
            {
                DataTable dtTeam = objTeam.ToDataTable();

                if (dtTeam != null)
                {
                    DataRow drRow = dtTeam.NewRow();
                    drRow["TeamName"] = "-- Select --";
                    drRow["TeamID"] = "0";
                    dtTeam.Rows.InsertAt(drRow, 0);

                    cboTeam.DisplayMember = "TeamName";
                    cboTeam.ValueMember = "TeamID";
                    cboTeam.DataSource = dtTeam.DefaultView;
                }
                
            }
            
        }

        private void cboTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(cboTeam.SelectedIndex > 0)
                {
                    int TeamID = Convert.ToInt32(cboTeam.SelectedValue);

                    IEnumerable<dynamic> objPlayer = DBHandler.GetPlayers(TeamID);
                    if (objPlayer != null)
                    {
                        DataTable dtPlayer = objPlayer.ToDataTable();

                        if(dtPlayer!=null && dtPlayer.Rows.Count > 0)
                        {
                            dgvWK.AutoGenerateColumns = false;
                            DataView dvWK = new DataView(dtPlayer);
                            dvWK.RowFilter = "RoleCode = 'WK'";
                            dgvWK.DataSource = dvWK.ToTable();


                            dgvBAT.AutoGenerateColumns = false;
                            DataView dvBAT = new DataView(dtPlayer);
                            dvBAT.RowFilter = "RoleCode = 'BAT'";
                            dgvBAT.DataSource = dvBAT.ToTable();


                            dgvALL.AutoGenerateColumns = false;
                            DataView dvALL = new DataView(dtPlayer);
                            dvALL.RowFilter = "RoleCode = 'ALL'";
                            dgvALL.DataSource = dvALL.ToTable();


                            dgvBOWL.AutoGenerateColumns = false;
                            DataView dvBOWL = new DataView(dtPlayer);
                            dvBOWL.RowFilter = "RoleCode = 'BOWL'";
                            dgvBOWL.DataSource = dvBOWL.ToTable();
                        }
                        else
                        {
                            dgvWK.DataSource = null;
                            dgvBAT.DataSource = null;
                            dgvALL.DataSource = null;
                            dgvBOWL.DataSource = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                tabControlPlayer.TabPages["tabWK"].Text = string.Format("WK({0})",dgvWK.Rows.Count - 1);
                tabControlPlayer.TabPages["tabBAT"].Text = string.Format("BAT({0})", dgvBAT.Rows.Count - 1);
                tabControlPlayer.TabPages["tabALL"].Text = string.Format("ALL({0})", dgvALL.Rows.Count - 1);
                tabControlPlayer.TabPages["tabBOWL"].Text = string.Format("BOWL({0})", dgvBOWL.Rows.Count - 1);
            }
        }

        public void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b') && (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                }
                if (Char.IsControl(e.KeyChar))
                {
                    e.Handled = false;
                }
                else if (Char.IsNumber(e.KeyChar) || e.KeyChar == '.')
                {
                    TextBox tb = sender as TextBox;
                    int cursorPosLeft = tb.SelectionStart;
                    int cursorPosRight = tb.SelectionStart + tb.SelectionLength;
                    string result = tb.Text.Substring(0, cursorPosLeft) + e.KeyChar + tb.Text.Substring(cursorPosRight);
                    string[] parts = result.Split('.');
                    if (parts.Length > 1)
                    {
                        if (parts[1].Length > 2 || parts.Length > 2)
                        {
                            e.Handled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dgvWK_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (dgvWK.CurrentCell != null && dgvWK.CurrentCell.OwningColumn.Name == "dgvcWKCredits")
                {
                    System.Windows.Forms.TextBox txt = (System.Windows.Forms.TextBox)e.Control;
                    txt.KeyPress += new KeyPressEventHandler(txt_KeyPress);
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void dgvWK_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iPlayerID = ((dgvWK["dgvcWKPlayerID", e.RowIndex].Value != DBNull.Value && dgvWK["dgvcWKPlayerID", e.RowIndex].Value != null) ? Convert.ToInt32(dgvWK["dgvcWKPlayerID", e.RowIndex].Value) : 0);
                string sPlayerName = ((dgvWK["dgvcWKName", e.RowIndex].Value != DBNull.Value && dgvWK["dgvcWKName", e.RowIndex].Value != null) ? dgvWK["dgvcWKName", e.RowIndex].Value.ToString() : string.Empty);
                decimal dCredits = ((dgvWK["dgvcWKCredits", e.RowIndex].Value != DBNull.Value && dgvWK["dgvcWKCredits", e.RowIndex].Value != null) ? Convert.ToDecimal(dgvWK["dgvcWKCredits", e.RowIndex].Value) : 0);
                
                if (iPlayerID == 0 && !string.IsNullOrEmpty(sPlayerName) && dCredits > 0)
                {
                    int TeamID = Convert.ToInt32(cboTeam.SelectedValue);

                    if (MessageBox.Show("Are you sure you want to add this player?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        IEnumerable<dynamic> objData = DBHandler.AddPlayer(TeamID, sPlayerName, dCredits,"WK");
                    }
                }
                else if (iPlayerID > 0 && !string.IsNullOrEmpty(sPlayerName) && dCredits > 0)
                {
                    if (MessageBox.Show("Are you sure you want to update player details?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        IEnumerable<dynamic> objData = DBHandler.UpdatePlayerDetail(iPlayerID, sPlayerName, dCredits);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dgvBAT_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (dgvBAT.CurrentCell != null && dgvBAT.CurrentCell.OwningColumn.Name == "dgvcBATCredits")
                {
                    System.Windows.Forms.TextBox txt = (System.Windows.Forms.TextBox)e.Control;
                    txt.KeyPress += new KeyPressEventHandler(txt_KeyPress);
                }
                
            }
            catch (Exception ex)
            {
                
            }
        }

        private void dgvBAT_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iPlayerID = (( dgvBAT["dgvcBATPlayerID", e.RowIndex].Value != DBNull.Value && dgvBAT["dgvcBATPlayerID", e.RowIndex].Value != null ) ? Convert.ToInt32(dgvBAT["dgvcBATPlayerID", e.RowIndex].Value) : 0) ;
                string sPlayerName = ((dgvBAT["dgvcBATName", e.RowIndex].Value != DBNull.Value && dgvBAT["dgvcBATName", e.RowIndex].Value != null) ? dgvBAT["dgvcBATName", e.RowIndex].Value.ToString() : string.Empty);
                decimal dCredits = ((dgvBAT["dgvcBATCredits", e.RowIndex].Value != DBNull.Value && dgvBAT["dgvcBATCredits", e.RowIndex].Value != null) ? Convert.ToDecimal(dgvBAT["dgvcBATCredits", e.RowIndex].Value) : 0);
                
                if (iPlayerID == 0 && !string.IsNullOrEmpty(sPlayerName) && dCredits > 0)
                {
                    int TeamID = Convert.ToInt32(cboTeam.SelectedValue);

                    if (MessageBox.Show("Are you sure you want to add this player?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        IEnumerable<dynamic> objData = DBHandler.AddPlayer(TeamID, sPlayerName, dCredits,"BAT");
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void dgvALL_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (dgvALL.CurrentCell != null && dgvALL.CurrentCell.OwningColumn.Name == "dgvcALLCredits")
                {
                    System.Windows.Forms.TextBox txt = (System.Windows.Forms.TextBox)e.Control;
                    txt.KeyPress += new KeyPressEventHandler(txt_KeyPress);
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void dgvALL_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iPlayerID = ((dgvALL["dgvcALLPlayerID", e.RowIndex].Value != DBNull.Value && dgvALL["dgvcALLPlayerID", e.RowIndex].Value != null) ? Convert.ToInt32(dgvALL["dgvcALLPlayerID", e.RowIndex].Value) : 0);
                string sPlayerName = ((dgvALL["dgvcALLName", e.RowIndex].Value != DBNull.Value && dgvALL["dgvcALLName", e.RowIndex].Value != null) ? dgvALL["dgvcALLName", e.RowIndex].Value.ToString() : string.Empty);
                decimal dCredits = ((dgvALL["dgvcALLCredits", e.RowIndex].Value != DBNull.Value && dgvALL["dgvcALLCredits", e.RowIndex].Value != null) ? Convert.ToDecimal(dgvALL["dgvcALLCredits", e.RowIndex].Value) : 0);
                

                if (iPlayerID == 0 && !string.IsNullOrEmpty(sPlayerName) && dCredits > 0)
                {
                    int TeamID = Convert.ToInt32(cboTeam.SelectedValue);

                    if (MessageBox.Show("Are you sure you want to add this player?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        IEnumerable<dynamic> objData = DBHandler.AddPlayer(TeamID, sPlayerName, dCredits,"ALL");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dgvBOWL_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (dgvBOWL.CurrentCell != null && dgvBOWL.CurrentCell.OwningColumn.Name == "dgvcBOWLCredits")
                {
                    System.Windows.Forms.TextBox txt = (System.Windows.Forms.TextBox)e.Control;
                    txt.KeyPress += new KeyPressEventHandler(txt_KeyPress);
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void dgvBOWL_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iPlayerID = ((dgvBOWL["dgvcBOWLPlayerID", e.RowIndex].Value != DBNull.Value && dgvBOWL["dgvcBOWLPlayerID", e.RowIndex].Value != null) ? Convert.ToInt32(dgvBOWL["dgvcBOWLPlayerID", e.RowIndex].Value) : 0);
                string sPlayerName = ((dgvBOWL["dgvcBOWLName", e.RowIndex].Value != DBNull.Value && dgvBOWL["dgvcBOWLName", e.RowIndex].Value != null) ? dgvBOWL["dgvcBOWLName", e.RowIndex].Value.ToString() : string.Empty);
                decimal dCredits = ((dgvBOWL["dgvcBOWLCredits", e.RowIndex].Value != DBNull.Value && dgvBOWL["dgvcBOWLCredits", e.RowIndex].Value != null) ? Convert.ToDecimal(dgvBOWL["dgvcBOWLCredits", e.RowIndex].Value) : 0);
                

                if (iPlayerID == 0 && !string.IsNullOrEmpty(sPlayerName) && dCredits > 0)
                {
                    int TeamID = Convert.ToInt32(cboTeam.SelectedValue);

                    if (MessageBox.Show("Are you sure you want to add this player?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        IEnumerable<dynamic> objData = DBHandler.AddPlayer(TeamID, sPlayerName, dCredits,"BOWL");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
