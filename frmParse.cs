using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Billion
{
    public partial class frmParse : Form
    {

        int UniqueID = 0;


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

        public frmParse()
        {
            InitializeComponent();
        }

        public frmParse(int UniqueID)
        {
            InitializeComponent();
            this.UniqueID = UniqueID;
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            try
            {
                var jsonObj = JsonConvert.DeserializeObject<RootObject>(rtbContent.Text);
                foreach (var obj in jsonObj.data.site.tour.match.players)
                {
                    obj.name = obj.name.Replace("\'", "");
                }
                
                int iAffectedRows = DBHandler.SavePlayerDetail(UniqueID, JsonConvert.SerializeObject(jsonObj));

                if(iAffectedRows > 0)
                {
                    this.DialogResult = DialogResult.OK;
                }

            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
