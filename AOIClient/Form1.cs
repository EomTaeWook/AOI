using AOIClient.Modules;
using Kosher.Log;

namespace AOIClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
            this.KeyPreview = true;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == Keys.Left)
            {

            }
            else if(keyData == Keys.Right)
            {
                
            }
            else if(keyData == Keys.Down)
            {

            }
            else if(keyData == Keys.Up)
            {

            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Init()
        {
            ClientModule.Instance.Connect();
        }
        
    }
}