using AOIClient.Internal;
using AOIClient.Modules;
using AOIClient.Modules.Handler;
using AOIClient.Net;
using Protocol.CAndS;

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
            this.Paint += Form1_Paint;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Pen blackPen = new Pen(Color.Black, 1);
            
            var maxLength = (GameManager.Instance.Map.MaxX + 1) * Map.CellSize;

            //°¡·Î
            for (int i=0; i<= GameManager.Instance.Map.MaxX + 1; ++i)
            {
                e.Graphics.DrawLine(blackPen, 0, Map.CellSize * i, maxLength, Map.CellSize * i);
            }
            for (int i = 0; i <= GameManager.Instance.Map.MaxY + 1; ++i)
            {
                e.Graphics.DrawLine(blackPen, Map.CellSize * i, 0, Map.CellSize * i, maxLength);
            }
            PlayerPaint(e.Graphics);
            OtherPlayerPaint(e.Graphics);
        }
        public void RefreshUI()
        {
            this.Refresh();
        }
        private void OtherPlayerPaint(Graphics graphics)
        {
            if (GameManager.Instance.UserPlayer == null)
            {
                return;
            }

            foreach(var item in GameManager.Instance.Players)
            {
                graphics.FillEllipse(new SolidBrush(Color.Green),
                new Rectangle(item.CellPos.X - 10,
                item.CellPos.Y - 10,
                20,
                20));

                graphics.DrawString(item.Nickname,
                new Font("Arial", 10),
                new SolidBrush(Color.Black),
                new PointF(item.CellPos.X - 10,
                item.CellPos.Y - 10));
            }
        }
        private void PlayerPaint(Graphics graphics)
        {
            if(GameManager.Instance.UserPlayer == null)
            {
                return;
            }
            graphics.FillEllipse(new SolidBrush(Color.Red),
                new Rectangle(GameManager.Instance.UserPlayer.CellPos.X-10, GameManager.Instance.UserPlayer.CellPos.Y-10, 20, 20));

            graphics.DrawString(GameManager.Instance.UserPlayer.Nickname,
                new Font("Arial", 10),
                new SolidBrush(Color.Black),
                new PointF(GameManager.Instance.UserPlayer.CellPos.X-10, GameManager.Instance.UserPlayer.CellPos.Y-10));
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == Keys.Left)
            {
                ClientModule.Instance.Send(Packet.MakePacket(CSProtocol.Move,
                    new Move()
                    {
                        X = -1,
                        Y = 0
                    }));
            }
            else if(keyData == Keys.Right)
            {
                ClientModule.Instance.Send(Packet.MakePacket(CSProtocol.Move,
                    new Move()
                    {
                        X = 1,
                        Y = 0
                    }));
            }
            else if(keyData == Keys.Down)
            {
                ClientModule.Instance.Send(Packet.MakePacket(CSProtocol.Move,
                    new Move()
                    {
                        X = 0,
                        Y = 1
                    }));
            }
            else if(keyData == Keys.Up)
            {
                ClientModule.Instance.Send(Packet.MakePacket(CSProtocol.Move,
                    new Move()
                    {
                        X = 0,
                        Y = -1
                    }));
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Init()
        {
            SCProtocolHandler.Init();
            ClientModule.Instance.Connect();
        }
        
    }
}